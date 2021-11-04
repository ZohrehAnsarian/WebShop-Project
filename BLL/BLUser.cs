using BLL.Base;
using Model.ApplicationDomainModels;
using Model.ViewModels.User;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLUser : BLBase
    {
        public VmUserList GetAllUsers()
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            var aspNetUserList = userRepository.GetAllUsers();
            var userList = from user in aspNetUserList
                           orderby user.Email
                           select new VmUserFullInfo
                           {
                               Id = user.Id,
                               UserName = user.UserName,
                               Email = user.Email,
                               Roles = from role in user.AspNetRoles select role.Name,
                               RegisterDate = user.RegisterDate.Value,
                               AllowAcceptReject = user.AllowAcceptReject
                           };

            var vmUserList = new VmUserList
            {
                Users = userList.ToArray()
            };

            return vmUserList;
        }
        public int GetUserCount()
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            var aspNetUserCount = userRepository.GetUserCount();
             
            return aspNetUserCount;
        }
        public VmUserList GetUserByFiler(string searchText)
        {
            var userRepository = UnitOfWork.GetRepository<UserRepository>();
            var aspNetUserList = userRepository.GetUserByFiler(searchText);
            var userList = from user in aspNetUserList
                           orderby user.Email
                           select new VmUserFullInfo
                           {
                               Id = user.Id,
                               UserName = user.UserName,
                               Email = user.Email,
                               Roles = from role in user.AspNetRoles select role.Name,
                               RegisterDate = user.RegisterDate.Value,
                               AllowAcceptReject = user.AllowAcceptReject
                           };

            var vmUserList = new VmUserList
            {
                Users = userList.ToArray()
            };

            return vmUserList;
        }
      
        public IEnumerable<SmUserRoles> GetAllUserRoles()
        {
            var viewUserRoleRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();

            var userRoleList = viewUserRoleRepository.GetAllUserRoles();

            var smUserRoleList = from userRoles in userRoleList
                                 select new SmUserRoles
                                 {
                                     Id = userRoles.Id,
                                     UserId = userRoles.UserId,
                                     UserName = userRoles.UserName,
                                     RoleName = userRoles.RoleName,
                                 };

            return smUserRoleList;
        }
        public IEnumerable<SmUserRoles> GetUsersByRole(string roleId)
        {
            var viewUserRoleRepository = UnitOfWork.GetRepository<ViewUserRoleRepository>();

            var userRoleList = viewUserRoleRepository.GetUsersByRole(roleId);

            var smUserRoleList = from userRoles in userRoleList
                                 select new SmUserRoles
                                 {
                                     Id = userRoles.Id,
                                     UserId = userRoles.UserId,
                                     UserName = userRoles.UserName,
                                     RoleName = userRoles.RoleName,
                                 };

            return smUserRoleList;

        }
    }
}
