using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Common
{
    /// <summary>
    /// 权限检查特性
    /// </summary>
    public class CheckPermissionAttribute : TypeFilterAttribute
    {
        public CheckPermissionAttribute() : base(typeof(CheckPermissionImpl))
        {

        }

        private class CheckPermissionImpl : IAsyncActionFilter
        {

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var _users = new AdminUser
                {
                         UserName = "过滤器新增的用户"
                };
                context.ActionArguments["User"] = _users;
                await next();
            }
        }    
    }
  
}
