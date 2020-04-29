using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingApi.Common.Utils;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZapiCore;
using ZapiCore.Model;

namespace ShoppingApi.Common
{
    public class TestAuthorizationFilter : IAuthorizationFilter , IAsyncActionFilter
    {
        /// <summary>
        /// 在动作之前执行，给用户实体进行动态的赋值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ClaimsPrincipal user2 = context.HttpContext.User;
            string token = context.HttpContext.Request.Headers["authorization"].ToString().Replace("Bearer ", "");
            // string text = user2.FindFirst("http://schemas.microsoft.com/claims/authnmethodsreferences")?.Value;
            var user = new UserInfo 
            {
                Id = user2.FindFirst(JwtClaimTypes.Id)?.Value,
                UserName = user2.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value,
                PhoneNumber = user2.FindFirst(JwtClaimTypes.PhoneNumber)?.Value,
                TrueName = user2.FindFirst(JwtClaimTypes.Name)?.Value,
                Token = token
            };

            if (context.Controller is BaseController controller)
            {
                controller.User = user;
            }
            //if (!context.ActionArguments.ContainsKey("UserId"))
            //{
            //    context.ActionArguments.Add("UserId", user.Id);
            //}
            //if (context.ActionArguments.ContainsKey("User"))
            //{
            //    context.ActionArguments["User"] = user;
            //}
            await next();
        }

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
            if (context?.HttpContext?.User == null)
            {
                context.Result = new ContentResult
                {
                    Content = "用户未登录",
                    StatusCode = (int)ResponseCodeEnum.NotAllow
                };
                return;
            }

            var attributeList = new List<object>();
            attributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            attributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            var authorizeAttributes = attributeList.OfType<AuthorizationLocalAttribute>().ToList();
            // 从claims取出用户相关信息，到数据库中取得用户具备的权限码，与当前Controller或Action标识的权限码做比较<比较权限是否和定义的一致>  

            //是否检查权限
            if (authorizeAttributes.Select(y => y.IsCheck).FirstOrDefault())
            {

            }

            //if (!authorizeAttributes.Any(s =>s.Permission.Equals()))
            //{
            //    context.Result = new ContentResult
            //    {
            //        Content = "暂无权限",
            //        StatusCode = (int)ResponseCodeEnum.NoPermission
            //    };
            //    return;
            //}
            return;
        }


        /// <summary>
        /// 解释token权限的验证
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
        public class AuthorizationLocalAttribute : AuthorizeAttribute
        {
            /// <summary>
            /// 权限项名称
            /// </summary>
            public string Permission { get; set; }
            
            /// <summary>
            /// 是否检查权限
            /// </summary>
            public bool IsCheck { get; set; }

            public AuthorizationLocalAttribute(string permission,bool IsCheck = false)
            {
                Permission = permission;
            }

        }
    }
}
