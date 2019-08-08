﻿using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Common;

/// <summary>
/// 数据处理层
/// </summary>
namespace DAL
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class UserInfoDal
    {


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<Users> GetList()
        {
            string sql = "select UserId,UserName,Pwd,Nick,Email,DeliveryId from Users";
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Users> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<Users>();
                Users Users = null;
                foreach (DataRow row in da.Rows)
                {
                    Users = new Users();
                    LoadEntity(Users, row);
                    list.Add(Users);
                }
            }
            return list;
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<Users> GetList(int page, int index)
        {

            //string sql = $"select UserId,UserName,Pwd,Nick,Email,DeliveryId from Users limit  {((page - 1) * index)}, {index}";
            string sql = CreateSqlString.SelectSqlString(new Users { });
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Users> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<Users>();
                Users Users = null;
                foreach (DataRow row in da.Rows)
                {
                    Users = new Users();
                    LoadEntity(Users, row);
                    list.Add(Users);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取一条用户信息 By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Users GetDeail(int id)
        {
            string sql = "SELECT UserId,Username,Userpass,regtime,email FROM Users where id = @UserId";
            SqlParameter[] pars ={
                                      new SqlParameter("@UserId",SqlDbType.Int)
                                  };
            pars[0].Value = id;
            DataTable dt = SqlHelper.GetDataTable(sql, CommandType.Text, pars);
            Users instance = null;
            if (dt.Rows.Count > 0)
            {
                instance = new Users();
                LoadEntity(instance, dt.Rows[0]);
            }
            return instance;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="Users"></param>
        /// <returns></returns>
        public int AddUsers(Users Users)
        {
            string sql = "insert into Users(UserName,Pwd,Nick,Email,UserId,DeliveryId) values(@UsersName,@Pwd,@Nick,@Email,@UserId,@DeliveryId)";
            SqlParameter[] pars = {
                                new SqlParameter("@UsersName",SqlDbType.VarChar,32),
                                  new SqlParameter("@Pwd",SqlDbType.VarChar,32),
                                         new SqlParameter("@Nick",SqlDbType.VarChar,32),
                                    new SqlParameter("@Email",SqlDbType.VarChar,32),
                                        new SqlParameter("@UserId",SqlDbType.VarChar,32),
                                          new SqlParameter("@DeliveryId",SqlDbType.VarChar,32)
                                  };
            pars[0].Value = Users.UserName;
            pars[1].Value = Users.Pwd;
            pars[2].Value = Users.Nick;
            pars[3].Value = Users.Email;
            pars[4].Value = Users.UserId;
            pars[5].Value = Users.DeliveryId;
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="Users"></param>
        /// <returns></returns>
        public int UpdateUsers(Users Users)
        {
            string sql = "UPDATE Users SET UserName = @UserName,Pwd = @Pwd,Nick = @Nick,Email = @Email,DeliveryId = @DeliveryId WHERE UserId = @UserId";
            SqlParameter[] pars = {
                                       new SqlParameter("@UserName",SqlDbType.VarChar,50),
                                       new SqlParameter("@Pwd",SqlDbType.VarChar,50),
                                       new SqlParameter("@Nick",SqlDbType.VarChar),
                                       new SqlParameter("@Email",SqlDbType.VarChar,50),
                                       new SqlParameter("@UserId",SqlDbType.VarChar,36),
                                       new SqlParameter("@DeliveryId",SqlDbType.VarChar,36)
                                   };
            pars[0].Value = Users.UserName;
            pars[1].Value = Users.Pwd;
            pars[2].Value = Users.Nick;
            pars[3].Value = Users.Email;
            pars[4].Value = Users.UserId;
            pars[5].Value = Users.DeliveryId;
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteUsers(string id)
        {
            try
            {
                string sql = "DELETE FROM Users WHERE UserId = @UserId";
                SqlParameter[] pars ={
                                      new SqlParameter("@UserId",SqlDbType.VarChar,36)
                                  };
                pars[0].Value = id;
                return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  初始化实体
        /// </summary>
        /// <param name="Users"></param>
        /// <param name="row"></param>
        private void LoadEntity(Users Users, DataRow row)
        {
            Users.UserName = row["UserName"] != DBNull.Value ? row["UserName"].ToString() : string.Empty;
            Users.Pwd = row["Pwd"] != DBNull.Value ? row["Pwd"].ToString() : string.Empty;
            Users.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : string.Empty;
            Users.UserId = row["UserId"] != DBNull.Value ? row["UserId"].ToString() : string.Empty;
            Users.Nick = row["Nick"] != DBNull.Value ? row["Nick"].ToString() : string.Empty;
            Users.DeliveryId = row["DeliveryId"] != DBNull.Value ? row["DeliveryId"].ToString() : string.Empty;
        }

    }
}
