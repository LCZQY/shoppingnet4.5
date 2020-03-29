using Model;
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
        public List<Customer> GetList()
        {
            string sql = "select UserId,UserName,Pwd,Nick,Email,DeliveryId from Customer";
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Customer> list = new List<Customer> { };
            if (da.Rows.Count > 0)
            {
                list = new List<Customer>();
                Customer Customer = null;
                foreach (DataRow row in da.Rows)
                {
                    Customer = new Customer();
                    LoadEntity(Customer, row);
                    list.Add(Customer);
                }
            }
            return list;
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetList(int page, int index)
        {

            //string sql = $"select UserId,UserName,Pwd,Nick,Email,DeliveryId from Customer limit  {((page - 1) * index)}, {index}";
            string sql = CreateSqlString.SelectSqlString(new Customer { });
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Customer> list = new List<Customer> { };
            if (da.Rows.Count > 0)
            {
                list = new List<Customer>();
                Customer Customer = null;
                foreach (DataRow row in da.Rows)
                {
                    Customer = new Customer();
                    LoadEntity(Customer, row);
                    list.Add(Customer);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取一条用户信息 By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetDeail(int id)
        {
            string sql = "SELECT UserId,Username,Userpass,regtime,email FROM Customer where id = @UserId";
            SqlParameter[] pars ={
                                      new SqlParameter("@UserId",SqlDbType.Int)
                                  };
            pars[0].Value = id;
            DataTable dt = SqlHelper.GetDataTable(sql, CommandType.Text, pars);
            Customer instance = null;
            if (dt.Rows.Count > 0)
            {
                instance = new Customer();
                LoadEntity(instance, dt.Rows[0]);
            }
            return instance;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        public int AddCustomer(Customer Customer)
        {
            string sql = "insert into Customer(UserName,Pwd,Nick,Email,UserId,DeliveryId) values(@CustomerName,@Pwd,@Nick,@Email,@UserId,@DeliveryId)";
            SqlParameter[] pars = {
                                new SqlParameter("@CustomerName",SqlDbType.VarChar,32),
                                  new SqlParameter("@Pwd",SqlDbType.VarChar,32),
                                         new SqlParameter("@Nick",SqlDbType.VarChar,32),
                                    new SqlParameter("@Email",SqlDbType.VarChar,32),
                                        new SqlParameter("@UserId",SqlDbType.VarChar,32),
                                          new SqlParameter("@DeliveryId",SqlDbType.VarChar,32)
                                  };
            pars[0].Value = Customer.UserName;
            pars[1].Value = Customer.Pwd;
            pars[2].Value = Customer.Nick;
            pars[3].Value = Customer.Email;
            pars[4].Value = Customer.UserId;
            pars[5].Value = Customer.DeliveryId;
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        public int UpdateCustomer(Customer Customer)
        {
            string sql = "UPDATE Customer SET UserName = @UserName,Pwd = @Pwd,Nick = @Nick,Email = @Email,DeliveryId = @DeliveryId WHERE UserId = @UserId";
            SqlParameter[] pars = {
                                       new SqlParameter("@UserName",SqlDbType.VarChar,50),
                                       new SqlParameter("@Pwd",SqlDbType.VarChar,50),
                                       new SqlParameter("@Nick",SqlDbType.VarChar),
                                       new SqlParameter("@Email",SqlDbType.VarChar,50),
                                       new SqlParameter("@UserId",SqlDbType.VarChar,36),
                                       new SqlParameter("@DeliveryId",SqlDbType.VarChar,36)
                                   };
            pars[0].Value = Customer.UserName;
            pars[1].Value = Customer.Pwd;
            pars[2].Value = Customer.Nick;
            pars[3].Value = Customer.Email;
            pars[4].Value = Customer.UserId;
            pars[5].Value = Customer.DeliveryId;
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCustomer(string id)
        {
            try
            {
                string sql = "DELETE FROM Customer WHERE UserId = @UserId";
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
        /// <param name="Customer"></param>
        /// <param name="row"></param>
        private void LoadEntity(Customer Customer, DataRow row)
        {
            Customer.UserName = row["UserName"] != DBNull.Value ? row["UserName"].ToString() : string.Empty;
            Customer.Pwd = row["Pwd"] != DBNull.Value ? row["Pwd"].ToString() : string.Empty;
            Customer.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : string.Empty;
            Customer.UserId = row["UserId"] != DBNull.Value ? row["UserId"].ToString() : string.Empty;
            Customer.Nick = row["Nick"] != DBNull.Value ? row["Nick"].ToString() : string.Empty;
            Customer.DeliveryId = row["DeliveryId"] != DBNull.Value ? row["DeliveryId"].ToString() : string.Empty;
        }

    }
}
