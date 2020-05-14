using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    /// <summary>
    /// 写入数据库上下文
    /// </summary>
    public class WriteAuthenticationDbContext : AuthenticationDbContext
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public WriteAuthenticationDbContext(DbContextOptions<WriteAuthenticationDbContext> options) : base(options)
        {

        }
    }
}
