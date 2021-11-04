using Repository.EF.Base;
using System.Linq;
using Model;

namespace Repository.EF.Repository
{
    public class PersonRepository : EFBaseRepository<Person>
    {
        public void CreatePerson(Person person)
        {
            Add(person);
        }

        public Person GetPersonById(int id)
        {
            var person = Context.People.SingleOrDefault(a => a.Id == id);

            return person;
        }
        public Person GetPersonByUserId(string userId)
        {
            var person = Context.People.SingleOrDefault(a => a.UserId == userId);

            return person;
        }
        public bool PersonIsExistByUserId(string userId)
        {
            return Context.People.Any(a => a.UserId == userId);
        }
        public void UpdateProfileImage(string userId, string profilePictureUrl)
        {
            var oldPerson = (from s in Context.People where s.UserId == userId select s).FirstOrDefault();

            oldPerson.ProfilePictureUrl = profilePictureUrl;

            Update(oldPerson);
        }
      
        public void UpdatePerson(Person person)
        {
            var oldPerson = Context.People.Find(person.Id);

            oldPerson.UserId = person.UserId;
            oldPerson.FirstName = person.FirstName;
            oldPerson.LastName = person.LastName;
            oldPerson.StreetLine1 = person.StreetLine1;
            oldPerson.StreetLine2 = person.StreetLine2;
            oldPerson.City = person.City;
            oldPerson.State = person.State;
            oldPerson.ZipCode = person.ZipCode;
            oldPerson.AcademicInfoNames = person.AcademicInfoNames;
            oldPerson.AcademicInfoValues = person.AcademicInfoValues;
            oldPerson.CountryId = person.CountryId;

            Update(oldPerson);
        }
         
    }
}
