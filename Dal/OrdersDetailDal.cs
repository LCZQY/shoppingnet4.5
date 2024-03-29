﻿using System.Data.SqlClient;
using System.Collections.Generic;
using Common;
using System.Data;
using Model;
using System;

/// <summary>
/// 数据处理层
/// </summary>
namespace DAL
{

    public class OrdersDetailDal
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Detail> GetList()
        {
            string sql = CreateSqlString.SelectSqlString(new Detail { });
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Detail> list = new List<Detail> { };
            if (da.Rows.Count > 0)
            {
                list = new List<Detail>();
                Detail Detail = null;
                foreach (DataRow row in da.Rows)
                {
                    Detail = new Detail();
                    LoadEntity(Detail, row);
                    list.Add(Detail);
                }
            }
            return list;
        }


        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <returns></returns>
        public List<Detail> GetList(int page, int index)
        {

            string sql = CreateSqlString.SelectSqlString(new Detail { }) ;;//limit  {((page - 1) * index)}, {index}";
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Detail> list = new List<Detail> { };
            if (da.Rows.Count > 0)
            {
                list = new List<Detail>();
                Detail Detail = null;
                foreach (DataRow row in da.Rows)
                {
                    Detail = new Detail();
                    LoadEntity(Detail, row);
                    list.Add(Detail);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取一条用户信息 By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Detail GetDeail(int id)
        {
            string sql =  CreateSqlString.SelectSqlString(new Detail { })+" WHERE DetailId =@DetailId ";
            SqlParameter[] pars ={
                                      new SqlParameter("@DetailId",SqlDbType.Int)
                                  };
            pars[0].Value = id;
            DataTable dt = SqlHelper.GetDataTable(sql, CommandType.Text, pars);
            Detail instance = null;
            if (dt.Rows.Count > 0)
            {
                instance = new Detail();
                LoadEntity(instance, dt.Rows[0]);
            }
            return instance;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public int AddDetail(Detail Detail)
        {
            string sql = CreateSqlString.InsertSqlString(Detail);
            var pars = CreateSqlString.SqlServerParameterArray(Detail);
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public int UpdateDetail(Detail Detail)
        {
            string sql =  CreateSqlString.UpdateSqlString(Detail)     +" WHERE DetailId =@DetailId";
            var pars = CreateSqlString.SqlServerParameterArray(Detail);
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDetail(string id)
        {
            try
            {
                string sql =  CreateSqlString.DeleteSqlString(new Detail { }) + "WHERE DetailId = @DetailId";
                SqlParameter[] pars ={
                                      new SqlParameter("@DetailId",SqlDbType.VarChar,36)
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
        /// <param name="Detail"></param>
        /// <param name="row"></param>
        /// <param name="row"></param>
        private void LoadEntity(Detail Detail, DataRow row)
        {
            Detail.DetailId = row["DetailId"] != DBNull.Value ? row["DetailId"].ToString() : string.Empty;
            Detail.ProductId = row["ProductId"] != DBNull.Value ? row["ProductId"].ToString() : string.Empty;
            Detail.Quantity = Convert.ToInt32(row["Quantity"] != DBNull.Value ? row["Quantity"].ToString() : string.Empty);
            Detail.OrdersId = row["OrdersId"] != DBNull.Value ? row["OrdersId"].ToString() : string.Empty;
            Detail.States = Convert.ToInt32(row["States"] != DBNull.Value ? row["States"].ToString() : string.Empty);
            Detail.UserId = row["UserId"] != DBNull.Value ? row["UserId"].ToString() : string.Empty;
           
        }



    }
}
