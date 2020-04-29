using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.Model;
using Authentication.Stores;
using System.Linq;

namespace Authentication
{

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public readonly IUserStore  _userStore;
        public ResourceOwnerPasswordValidator(IUserStore userStore)
        {
            _userStore = userStore;
        }

        /// <summary>
        /// 验证当前用户是否和数据中一致
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            var user = await _userStore.IQueryableListAsync().Where
               (item => item.UserName == context.UserName && item.Password == context.Password && !item.IsDeleted).SingleOrDefaultAsync();
            if (user != null)
            {
                context.Result = new GrantValidationResult(
                 subject: context.UserName,
                 authenticationMethod: "custom",
                 claims: new Claim[] {
                    new Claim(JwtClaimTypes.Id, user.Id),
                    new Claim(JwtClaimTypes.Name,user.TrueName),
                    new Claim(JwtClaimTypes.PhoneNumber , user.PhoneNumber),
                    new Claim(JwtClaimTypes.GivenName, user.UserName),
                   // new Claim(JwtClaimTypes.FamilyName, "yyy"),
                   // new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
                   // new Claim(JwtClaimTypes.Role,"admin")
                 });
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user_password_error");
            }
        }


    }


}

