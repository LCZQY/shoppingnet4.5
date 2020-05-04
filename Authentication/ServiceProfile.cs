using AutoMapper;
using Authentication.Dto.Request;
using Authentication.Dto.Response;
using Authentication.Models;

namespace Authentication
{

    /// <summary>
    /// autoMapper
    /// </summary>
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            #region request
            CreateMap<PermissionEditRequest, Permissionitem>();
            CreateMap<Permissionitem, PermissionEditRequest>();

            CreateMap<UserEditRequest, User>();
            CreateMap<User, UserEditRequest>();

            CreateMap<RoleEditRequest, Role>();
            CreateMap<Role, RoleEditRequest>();

            #endregion

            #region Resposne
            CreateMap<UserListResponse, User>();
            CreateMap<User, UserListResponse>();


            CreateMap<RoleListResponse, Role>();
            CreateMap<Role, RoleListResponse>();
            #endregion
        }
    }
}
