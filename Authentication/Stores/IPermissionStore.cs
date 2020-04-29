using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapiCore.Interface;
using Authentication.Models;
namespace Authentication.Stores
{
    public interface IPermissionStore : Baseinterface<Permissionitem>
    {
        Task<bool> DeleteRangeAsync(List<string> id);

    }
}
