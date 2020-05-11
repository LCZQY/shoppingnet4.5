using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Common
{
    public class FileUpload
    {


        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        /// <summary>
        /// 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        public static void Transport(string src, string dst, string fileName)
        {

            FileStream inFileStream = new FileStream(src, FileMode.Open);
            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }
            dst = dst + fileName;

            if (!File.Exists(dst))
            {
                FileStream outFileStream = new FileStream(dst, FileMode.Create, FileAccess.Write);


                byte[] buf = new byte[inFileStream.Length];

                int byteCount;

                while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
                {

                    outFileStream.Write(buf, 0, byteCount);

                }

                inFileStream.Flush();

                inFileStream.Close();

                outFileStream.Flush();

                outFileStream.Close();
            }
        }

        /// <summary>
        /// 从远程服务器下载文件到本地
        /// </summary>
        /// <param name="src">下载到本地后的文件路径，包含文件的扩展名</param>
        /// <param name="dst">远程服务器路径（共享文件夹路径）</param>
        /// <param name="fileName">远程服务器（共享文件夹）中的文件名称，包含扩展名</param>
        public static void TransportRemoteToLocal(string src, string dst, string fileName)   //src：下载到本地后的文件路径  dst：远程服务器路径 fileName:远程服务器dst路径下的文件名
        {
            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }
            dst = dst + fileName;
            FileStream inFileStream = new FileStream(dst, FileMode.Open);    //远程服务器文件  此处假定远程服务器共享文件夹下确实包含本文件，否则程序报错

            FileStream outFileStream = new FileStream(src, FileMode.OpenOrCreate);   //从远程服务器下载到本地的文件

            byte[] buf = new byte[inFileStream.Length];

            int byteCount;

            while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
            {

                outFileStream.Write(buf, 0, byteCount);

            }

            inFileStream.Flush();

            inFileStream.Close();

            outFileStream.Flush();

            outFileStream.Close();

        }



    }
}
