using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class CountryRepository : EFBaseRepository<Country>
    {
        public IEnumerable<Country> GetAllCountries()
        {
            var CountryList = from country in Context.Countries
                              select country;

            return CountryList.OrderBy(A => A.Name).ToArray();
        }

        public int GetCountryPhoneCode(int id)
        {
            return Context.Countries.Find(id).PhoneCode;
        }
    }
}
