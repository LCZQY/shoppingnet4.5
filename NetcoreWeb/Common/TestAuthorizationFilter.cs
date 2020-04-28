using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore;

namespace ShoppingApi.Common
{
    public class TestAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return;
            }

            var attributeList = new List<object>();
            attributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            attributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            var authorizeAttributes = attributeList.OfType<TestAuthorizeAttribute>().ToList();
            var claims = context.HttpContext.User.Claims;
            
            // 从claims取出用户相关信息，到数据库中取得用户具备的权限码，与当前Controller或Action标识的权限码做比较<比较权限是否和定义的一致>            
            var userPermissions = "User_Edit";
            if (!authorizeAttributes.Any(s => s.Permission.Equals(userPermissions)))
            {
                throw new ZCustomizeException(ResponseCodeEnum.NoPermission,"暂无权限");
                //context.Result = new JsonResult("没有权限");
            }
            return;
        }


        /// <summary>
        /// 解释token权限的验证
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
        public class TestAuthorizeAttribute : AuthorizeAttribute
        {

            public string Permission { get; set; }

            public TestAuthorizeAttribute(string permission)
            {
                Permission = permission;
            }

        }
    }
}
