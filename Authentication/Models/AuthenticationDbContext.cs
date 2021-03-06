﻿using Microsoft.EntityFrameworkCore;

namespace Authentication.Models
{
    /// <summary>
    /// 直接连接到mycat的数据库，相当于桥梁的作用，自动识别SQL分配到主库或者从库
    /// </summary>
    public class AuthenticationDbContext : DbContext
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {

        }      

        /// <summary>
        /// 角色-权限表
        /// </summary>
        public DbSet<Role_Permissionitem> Role_Permissions { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<Role> Role { get; set; }

        /// <summary>
        /// 用户角色表
        /// </summary>
        public DbSet<User_Role> User_Role { get; set; }

        /// <summary>
        /// 权限表
        /// </summary>
        public DbSet<Permissionitem> Permissionitem { get; set; }

        /// <summary>
        /// 权限扩展表
        /// </summary>
        public DbSet<Permissionitem_expansion> Permissionitem_Expansion { get; set; }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);      
            builder.Entity<Permissionitem_expansion>(b =>
            {
                b.ToTable("zqy_permissionitem_expansion");
                b.HasKey(k => new { k.Id });
            });

            builder.Entity<Permissionitem>(b =>
            {
                b.ToTable("zqy_permissionitem");
                b.HasKey(k => new { k.Id });
            });
            builder.Entity<User_Role>(b =>
            {
                b.ToTable("zqy_user_role");
                b.HasKey(k => new { k.Id });
            });

            builder.Entity<Role_Permissionitem>(b =>
            {
                b.ToTable("zqy_role_permissionitem");
                b.HasKey(k => new { k.Id });
            });

            builder.Entity<Role>(b =>
            {
                b.ToTable("zqy_role");
                b.HasKey(k => new { k.Id });
            });
            builder.Entity<User>(b =>
            {
                b.ToTable("zqy_user");
                b.HasKey(k => new { k.Id });
            });

        }
    }
}
