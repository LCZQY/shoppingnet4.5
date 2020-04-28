using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication
{
    /// <summary>
    /// 验证用户名或者密码是否正确
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public ResourceOwnerPasswordValidator()
        {

        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            if (context.UserName == "aaa" && context.Password == "123")
            {
                context.Result = new GrantValidationResult(
                 subject: context.UserName,
                 authenticationMethod: "custom",
                 claims: GetUserClaims());
            }
            else
            {

                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user_password_error");
            }
        }

        //找出用户信息携带到token中
        private Claim[] GetUserClaims()
        {
            return new Claim[]
            {
                    new Claim(JwtClaimTypes.Id, Guid.NewGuid().ToString()),
                    new Claim(JwtClaimTypes.Name,"郑强勇"),
                    new Claim(JwtClaimTypes.PhoneNumber,"13167874692"),
                    new Claim(JwtClaimTypes.GivenName, "aaa"),
                    new Claim(JwtClaimTypes.FamilyName, "yyy"),
                    new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
                    new Claim(JwtClaimTypes.Role,"admin")
            };
        }
    }


}

