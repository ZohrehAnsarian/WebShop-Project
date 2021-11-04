using Model;
using Model.ViewModels.Person;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewPersonInRoleRepository : EFBaseRepository<ViewPersonInRoleRepository>/*, IcountryRepository*/
    {
        public IEnumerable<ViewPersonInRole> GetAllpersons()
        {

            var personInRoleList = from person in Context.ViewPersonInRoles
                                   select person;

            return personInRoleList.ToArray();
        }

        public IEnumerable<ViewPersonInRole> GetUsersByRole(string roleId)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles
                                   where person.UserId == roleId
                                   select person;

            return personInRoleList.ToArray();
        }
        public ViewPersonInRole GetUsersById(string userId)
        {
            var personInRole = from person in Context.ViewPersonInRoles
                               where person.UserId == userId
                               select person;

            return personInRole.FirstOrDefault();
        }
        public ViewPersonInRole GetUsersByName(string userName)
        {
            var personInRole = from person in Context.ViewPersonInRoles
                               where person.UserName == userName
                               select person;

            return personInRole.FirstOrDefault();
        }
        public string[] GetEmailsByUserIds(string[] userIds)
        {
            var personInRole = from person in Context.ViewPersonInRoles
                               where userIds.Contains(person.UserId)
                               select person.Email;

            return personInRole.ToArray();
        }
        public IEnumerable<ViewPersonInRole> Select(VmPerson filterItem, int index, int count)
        {
            var personInRoleList = from person in Context.ViewPersonInRoles

                                   select person;

            if (filterItem.Email != null)
            {
                personInRoleList = personInRoleList.Where(t => t.Email.Contains(filterItem.Email));
            }

            if (filterItem.Name != null)
            {
                personInRoleList = personInRoleList.Where(t => t.Name.Contains(filterItem.Name));
            }


            if (filterItem.PhoneNumber != null)
            {
                personInRoleList = personInRoleList.Where(t => t.PhoneNumber.Contains(filterItem.PhoneNumber));
            }

            if (filterItem.Country != null)
            {
                personInRoleList = personInRoleList.Where(t => t.Country.Contains(filterItem.Country));
            }

            if (filterItem.RoleName != null)
            {
                personInRoleList = personInRoleList.Where(t => t.RoleName.Contains(filterItem.RoleName));
            }

            if (filterItem.UserAllowAcceptReject != null)
            {
                personInRoleList = personInRoleList.Where(t => t.AllowAcceptReject == filterItem.UserAllowAcceptReject);
            }

           
            return personInRoleList.OrderBy(t => t.FirstName).Skip(index).Take(count).ToList();

        }

    }
}
