using Microsoft.EntityFrameworkCore;

namespace ShoppingApi.Models
{
   /// <summary>
   /// 业务库上下文
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
        public DbSet<Files> Files { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// 顾客【可以成为卖家】
        /// </summary>
        public DbSet<Customer> Customer { get; set; }






        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(b =>
            {
                b.ToTable("zqy_customer");
            });

            builder.Entity<Product>(b =>
            {
                b.ToTable("zqy_product");
            });

            builder.Entity<Files>(b =>
            {
                b.ToTable("zqy_files");
            });

            builder.Entity<News>(b =>
            {
                b.ToTable("zqy_news");
            });

            builder.Entity<Favorite>(b =>
            {
                b.ToTable("zqy_favorite");
            });

            builder.Entity<Appraise>(b =>
            {
                b.ToTable("zqy_appraise");
            });
        }
    }
}
