using Authentication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZapiCore.Interface;
namespace Authentication.Stores
{
    public interface IUserRoleStore : Baseinterface<User_Role>
    {
        Task<bool> DeleteRangeAsync(List<string> id);
    }
}
