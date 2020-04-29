using Microsoft.EntityFrameworkCore;

/// <summary>
/// 数据模型层
/// </summary>
namespace ShoppingApi.Models
{
    /// <summary>
    /// 商品评价表
    /// </summary>
    public class ShoppingDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(b => {
                b.ToTable("Customer");
            });

        }

        ///// <summary>
        ///// 权限管理【考虑新建一个服务，独立出去】
        ///// </summary>
        //public DbSet<Models.use>  Users { get; set; }

        /// <summary>
        /// 商品评价
        /// </summary>
        public DbSet<Appraise> Appraise { get; set; }

        /// <summary>
        /// 商品类别    
        /// </summary>
        public DbSet<Category> Category { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>

        public DbSet<Delivery> Delivery { get; set; }

        /// <summary>
        /// 收藏
        /// </summary>
        public DbSet<Favorite> Favorite { get; set; }

        /// <summary>
        /// 促销资讯表
        /// </summary>
        public DbSet<News> News { get; set; }

        /// <summary>
        /// 订单表
        /// </summary>
        public DbSet<Orders> Orders { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public DbSet<Files>  Files { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// 顾客【可以成为卖家】
        /// </summary>
        public DbSet<Customer> Customer { get; set; }
    }

}
