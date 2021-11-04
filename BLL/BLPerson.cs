using BLL.Base;

using Model;
using Model.ViewModels.Person;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLPerson : BLBase
    {
        public bool CreateSiteInfo(string url)
        {
            try
            {

                var SiteInfoRepository = UnitOfWork.GetRepository<SiteInfoRepository>();

                SiteInfoRepository.CreateSiteInfo(
                        new SiteInfo
                        {
                            SiteName = url + " - " + DateTime.Now
                        });

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreatePerson(VmPerson vmPerson)
        {
            try
            {
                if (PersonIsExistByUserId(vmPerson.UserId) == false)
                {
                    var personRepository = UnitOfWork.GetRepository<PersonRepository>();

                    personRepository.CreatePerson(new Person { UserId = vmPerson.UserId });

                    UnitOfWork.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool PersonIsExistByUserId(string userId)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonRepository>();

                return personRepository.PersonIsExistByUserId(userId);

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string[] GetEmailsByUserIds(string[] userIds)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

                return personInRoleRepository.GetEmailsByUserIds(userIds);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<VmPerson> GetUsersByFilter(VmPerson filterItem = null)
        {
            var viewPersonInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

            var usersList = viewPersonInRoleRepository.Select(filterItem, 0, int.MaxValue);

            var personInRoleList = (from person in usersList
                                    select new VmPerson
                                    {
                                        Id = person.Id,
                                        RoleId = person.RoleId,
                                        UserId = person.UserId,
                                        ProfilePictureUrl = person.ProfilePictureUrl ?? "",
                                        FirstName = person.FirstName ?? "",
                                        LastName = person.LastName ?? "",
                                        Name = person.Name,
                                        RoleName = person.RoleName ?? "",
                                        Email = person.Email,
                                        PhoneNumber = person.PhoneNumber ?? "",
                                        StreetLine1 = person.StreetLine1 ?? "",
                                        StreetLine2 = person.StreetLine2 ?? "",
                                        Country = person.Country,
                                        UserAllowAcceptReject = person.AllowAcceptReject,
                                        UserName = person.UserName
                                    }).ToList();

            return personInRoleList.OrderBy(p => p.LastName);

        }

        public VmPerson GetPersonByUserId(string userId)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

                var person = personInRoleRepository.GetUsersById(userId);
                var vwPerson = new VmPerson
                {
                    Id = person.Id,
                    RoleId = person.RoleId,
                    UserId = person.UserId,
                    ProfilePictureUrl = person.ProfilePictureUrl,
                    RoleName = person.RoleName,
                    Country = person.Country,
                    CountryId = person.CountryId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Email = person.Email,
                    PhoneNumber = person.PhoneNumber,
                    StreetLine1 = person.StreetLine1,
                    StreetLine2 = person.StreetLine2,
                    City = person.City,
                    State = person.State,
                    ZipCode = person.ZipCode,
                    AcademicInfoNames = (string.IsNullOrEmpty(person.AcademicInfoNames)) ? "," : person.AcademicInfoNames,
                    AcademicInfoValues = person.AcademicInfoValues ?? "",
                    Name = person.Name,
                };

                return vwPerson;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public VmPerson GetUsersByName(string userName)
        {
            try
            {
                var personInRoleRepository = UnitOfWork.GetRepository<ViewPersonInRoleRepository>();

                var person = personInRoleRepository.GetUsersByName(userName);
                var vwPerson = new VmPerson
                {
                    Id = person.Id,
                    RoleId = person.RoleId,
                    UserId = person.UserId,
                    ProfilePictureUrl = person.ProfilePictureUrl,
                    RoleName = person.RoleName,
                    Country = person.Country,
                    CountryId = person.CountryId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Email = person.Email,
                    PhoneNumber = person.PhoneNumber,
                    StreetLine1 = person.StreetLine1,
                    StreetLine2 = person.StreetLine2,
                    City = person.City,
                    State = person.State,
                    ZipCode = person.ZipCode,
                    AcademicInfoNames = (string.IsNullOrEmpty(person.AcademicInfoNames)) ? "," : person.AcademicInfoNames,
                    AcademicInfoValues = person.AcademicInfoValues ?? "",
                    Name = person.Name,
                    FulName = person.FirstName + " " + person.LastName,
                };

                return vwPerson;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool UpdatePerson(VmPerson vmPerson)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonRepository>();
                personRepository.UpdatePerson(
                    new Person
                    {
                        Id = vmPerson.Id,
                        UserId = vmPerson.UserId,
                        FirstName = vmPerson.FirstName,
                        LastName = vmPerson.LastName,
                        StreetLine1 = vmPerson.StreetLine1,
                        StreetLine2 = vmPerson.StreetLine2,
                        City = vmPerson.City,
                        State = vmPerson.State,
                        ZipCode = vmPerson.ZipCode,
                        AcademicInfoNames = vmPerson.AcademicInfoNames,
                        AcademicInfoValues = vmPerson.AcademicInfoValues,
                        CountryId = vmPerson.CountryId,
                    });

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UploadProfileImage(string userId, string profilePictureUrl)
        {
            try
            {
                var personRepository = UnitOfWork.GetRepository<PersonRepository>();
                personRepository.UpdateProfileImage(userId, profilePictureUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}