using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingApi.Managers;
using System.Security.Claims;
using ZapiCore.Model;

namespace ShoppingApi.Common
{
    /// <summary>
    /// 基础控制器
    /// </summary>

    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class BaseController : Controller
    {

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <value></value>
        public new UserInfo User { get; internal set; }

     
        /// <summary>
        /// Gets the System.Security.Claims.ClaimsPrincipal for user associated with the executing action.
        /// </summary>
        /// <value></value>
        public ClaimsPrincipal LocalUser { get { return base.User; } }


    }
}
