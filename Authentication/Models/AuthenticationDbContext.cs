using Microsoft.EntityFrameworkCore;

namespace Authentication.Models
{
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



    }
}
