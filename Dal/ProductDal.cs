﻿using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// 数据处理层
/// </summary>
namespace DAL
{

    /// <summary>
    /// 商品表
    /// </summary>
    public class ProductDal
    {

        //public List<ProductEx> ProductJoinImage()
        //{
        //    string sql 
        //}



        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Product> GetList()
        {
            string sql = CreateSqlString.SelectSqlString(new Product { });
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
            List<Product> list = new List<Product> { };
            if (da.Rows.Count > 0)
            {
                list = new List<Product>();
                Product Product = null;
                foreach (DataRow row in da.Rows)
                {
                    Product = new Product();
                    LoadEntity(Product, row);
                    list.Add(Product);
                }
            }
            return list;
        }


        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <returns></returns>
        public List<Product> GetList(int page, int index)
        {

            string sql = CreateSqlString.SelectSqlString(new Product { });//}limit  {((page - 1) * index)}, {index}";
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text);
                List<Product> list = new List<Product> { };
            if (da.Rows.Count > 0)
            {
                list = new List<Product>();
                Product Product = null;
                foreach (DataRow row in da.Rows)
                {
                    Product = new Product();
                    LoadEntity(Product, row);
                    list.Add(Product);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取一条用户信息 By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetDeail(int id)
        {
            string sql = CreateSqlString.SelectSqlString(new Product { }) + " WHERE ProductId =@ProductId ";
            SqlParameter[] pars ={
                                      new SqlParameter("@ProductId",SqlDbType.Int)
                                  };
            pars[0].Value = id;
            DataTable dt = SqlHelper.GetDataTable(sql, CommandType.Text, pars);
            Product instance = null;
            if (dt.Rows.Count > 0)
            {
                instance = new Product();
                LoadEntity(instance, dt.Rows[0]);
            }
            return instance;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public int AddProduct(Product Product)
        {
            //string sql = "insert into Product(ProductId,Title,CateId,MarketPrice,Price,Content,PostTime,Stock) values(@ProductId,@Title,@CateId,@MarketPrice,@Price,@Content,@PostTime,@Stock)";
            string sql = CreateSqlString.InsertSqlString(Product);
            var pars = CreateSqlString.SqlServerParameterArray(Product);
            //SqlParameter[] pars = {
            //                    new SqlParameter("@ProductId",SqlDbType.VarChar,32),
            //                      new SqlParameter("@Title",SqlDbType.VarChar,32),
            //                                  new SqlParameter("@CateId",SqlDbType.VarChar,32),
            //                             new SqlParameter("@MarketPrice",SqlDbType.Decimal),
            //                             new SqlParameter("@Price",SqlDbType.Decimal),
            //                               new SqlParameter("@Stock",SqlDbType.Int32),
            //                               new SqlParameter("@PostTime",SqlDbType.DateTime),
            //                               new SqlParameter("@Content",SqlDbType.Text),
            //                      };
            //pars[0].Value = Product.ProductId;
            //pars[1].Value = Product.Title;
            //pars[2].Value = Product.CateId;
            //pars[3].Value = Product.MarketPrice;
            //pars[4].Value = Product.Price;
            //pars[5].Value = Product.Content;
            //pars[5].Value = Product.PostTime;
            //pars[7].Value = Product.Stock;
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public int UpdateProduct(Product Product)
        {

            string sql = CreateSqlString.UpdateSqlString(Product) + " WHERE ProductId =@ProductId";
            var pars = CreateSqlString.SqlServerParameterArray(Product);
            //SqlParameter[] pars = {
            //                    new SqlParameter("@ProductId",SqlDbType.VarChar,32),
            //                      new SqlParameter("@Title",SqlDbType.VarChar,32),
            //                                  new SqlParameter("@CateId",SqlDbType.VarChar,32),
            //                             new SqlParameter("@MarketPrice",SqlDbType.Decimal),
            //                             new SqlParameter("@Price",SqlDbType.Decimal),
            //                               new SqlParameter("@Stock",SqlDbType.Int32),
            //                               new SqlParameter("@PostTime",SqlDbType.DateTime),
            //                               new SqlParameter("@Content",SqlDbType.Text),
            //                      };
            //pars[0].Value = Product.ProductId;
            //pars[1].Value = Product.Title;
            //pars[2].Value = Product.CateId;
            //pars[3].Value = Product.MarketPrice;
            //pars[4].Value = Product.Price;
            //pars[5].Value = Product.Content;
            //pars[5].Value = Product.PostTime;
            //pars[7].Value = Product.Stock;
            return SqlHelper.ExecuteNonquery(sql, CommandType.Text, pars);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteProduct(string id)
        {
            try
            {
                string sql = CreateSqlString.DeleteSqlString(new Product { }) + " WHERE ProductId = @ProductId";
                SqlParameter[] pars ={
                                      new SqlParameter("@ProductId",SqlDbType.VarChar,36)
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
        /// <param name="Product"></param>
        /// <param name="row"></param>
        /// <param name="row"></param>
        private void LoadEntity(Product Product, DataRow row)
        {
            Product.Content = row["Content"] != DBNull.Value ? row["Content"].ToString() : string.Empty;
            Product.CateId = row["CateId"] != DBNull.Value ? row["CateId"].ToString() : string.Empty;
            Product.MarketPrice = Convert.ToDecimal(row["MarketPrice"] != DBNull.Value ? row["MarketPrice"].ToString() : string.Empty);
            Product.Price = Convert.ToDecimal(row["Price"] != DBNull.Value ? row["Price"].ToString() : string.Empty);
            Product.PostTime = Convert.ToDateTime(row["PostTime"] != DBNull.Value ? row["PostTime"].ToString() : string.Empty);
            Product.ProductId = row["ProductId"] != DBNull.Value ? row["ProductId"].ToString() : string.Empty;
            Product.Title = row["Title"] != DBNull.Value ? row["Title"].ToString() : string.Empty;
            Product.Stock = Convert.ToInt32(row["Stock"] != DBNull.Value ? row["Stock"].ToString() : string.Empty);
            Product.Icon = row["Icon"] != DBNull.Value ? row["Icon"].ToString() : string.Empty;
        }



    }
}
