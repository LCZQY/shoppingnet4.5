using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth2IdentityServer.OAuth2
{
    /// <summary>
    /// 读取数据库登录身份验证
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        // private readonly IUserRepository _userRepo;//数据库仓储类
        private readonly IReferenceTokenStore _reference;
        private readonly IPersistedGrantStore _persisted;

        public ResourceOwnerPasswordValidator(/*IUserRepository userRepo,*/ IReferenceTokenStore reference, IPersistedGrantStore persisted)
        {
            //  _userRepo = userRepo;
            _reference = reference;
            _persisted = persisted;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.Request.GrantType == GrantType.AuthorizationCode)
            {
                string a = "";
            }
            else if (context.Request.GrantType == GrantType.ResourceOwnerPassword)
            {
                #region 获取所有请求参数：grant_type、client_Id、client_secret、username、password、eid、device_id
                var validatedRequestDictionary = context.Request.Raw.AllKeys.ToDictionary(s => s, s => context.Request.Raw[s]);
                string eid = validatedRequestDictionary["eid"];
                string device_id = validatedRequestDictionary["device_id"];
                #endregion
            }

            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            if (context.UserName == "admin" && context.Password == "123456")
            {
                string clientId = new Guid().ToString();
                await _reference.RemoveReferenceTokensAsync(context.UserName, clientId);
                await _persisted.RemoveAllAsync(context.UserName, clientId);

                context.Result = new GrantValidationResult(
                 subject: context.UserName,
                 authenticationMethod: "custom",
                 claims: GetUserClaims());
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims()
        {
            return new Claim[]
            {
                new Claim("UserId", 1.ToString()),
                new Claim(JwtClaimTypes.Name,"wjk"),
                new Claim(JwtClaimTypes.GivenName, "jaycewu"),
                new Claim(JwtClaimTypes.FamilyName, "yyy"),
                new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
                new Claim(JwtClaimTypes.Role,"admin")
            };
        }
    }
}
