using System.Data.SqlClient;
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

   /// <summary>
   /// 评价表
   /// </summary>
    public class AppraiseDal
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Appraise> GetList()
        {
            string sql = CreateSqlString.SelectSqlString(new Appraise { });
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Appraise> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<Appraise>();
                Appraise Appraise = null;
                foreach (DataRow row in da.Rows)
                {
                    Appraise = new Appraise();
                    LoadEntity(Appraise, row);
                    list.Add(Appraise);
                }
            }
            return list;
        }


        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <returns></returns>
        public List<Appraise> GetList(int page, int index)
        {

            string sql = $"{CreateSqlString.SelectSqlString(new Appraise { }) }limit  {((page - 1) * index)}, {index}";
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Appraise> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<Appraise>();
                Appraise Appraise = null;
                foreach (DataRow row in da.Rows)
                {
                    Appraise = new Appraise();
                    LoadEntity(Appraise, row);
                    list.Add(Appraise);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取一条用户信息 By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Appraise GetDeail(int id)
        {
            string sql = $"{ CreateSqlString.SelectSqlString(new Appraise { })} WHERE AppraiseId =@AppraiseId ";
            SqlParameter[] pars ={
                                      new SqlParameter("@AppraiseId",SqlDbType.Int)
                                  };
            pars[0].Value = id;
            DataTable dt = SqlHelper.GetDataTable(sql, CommandType.Text, pars);
            Appraise instance = null;
            if (dt.Rows.Count > 0)
            {
                instance = new Appraise();
                LoadEntity(instance, dt.Rows[0]);
            }
            return instance;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="Appraise"></param>
        /// <returns></returns>
        public int AddAppraise(Appraise Appraise)
        {
            string sql = CreateSqlString.InsertSqlString(Appraise);
            var pars = CreateSqlString.SqlServerParameterArray(Appraise);
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="Appraise"></param>
        /// <returns></returns>
        public int UpdateAppraise(Appraise Appraise)
        {
            string sql = $"{ CreateSqlString.UpdateSqlString(Appraise)    } WHERE AppraiseId =@AppraiseId";
            var pars = CreateSqlString.SqlServerParameterArray(Appraise);
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteAppraise(string id)
        {
            try
            {
                string sql = $"{ CreateSqlString.DeleteSqlString(new Appraise { }) }  WHERE AppraiseId = @AppraiseId";
                SqlParameter[] pars ={
                                      new SqlParameter("@AppraiseId",SqlDbType.VarChar,36)
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
        /// <param name="Appraise"></param>
        /// <param name="row"></param>
        /// <param name="row"></param>
        private void LoadEntity(Appraise Appraise, DataRow row)
        {
            Appraise.AppraiseId = row["AppraiseId"] != DBNull.Value ? row["AppraiseId"].ToString() : string.Empty;
            Appraise.RateTime = Convert.ToDateTime(row["RateTime"] != DBNull.Value ? row["RateTime"].ToString() : string.Empty);           
            Appraise.UserId = row["UserId"] != DBNull.Value ? row["UserId"].ToString() : string.Empty;            
            Appraise.ProductId = row["ProductId"] != DBNull.Value ? row["ProductId"].ToString() : string.Empty;
            Appraise.Grade = Convert.ToInt32(row["Grade"] != DBNull.Value ? row["Grade"].ToString() : string.Empty);
       
        }



    }
}
