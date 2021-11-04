using Model.ViewModels;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;
using BLL.Base;
using Model.ToolsModels.DropDownList;
using Model.ApplicationDomainModels;

namespace BLL
{
    public class BLCountry : BLBase
    {
        public IEnumerable<VmCountry> GetCountries()
        {
            var countryRepository = UnitOfWork.GetRepository<CountryRepository>();

            var countryList = countryRepository.GetAllCountries();

            var vmCountryList = from country in countryList
                                orderby country.Name
                                select new VmCountry
                                {
                                    Id = country.Id,
                                    Code = country.NumCode.ToString(),
                                    Name = country.Name,
                                    PhoneCode = country.PhoneCode,
                                    FlagUrl = "/Resources/Images/flags/32x32/" + country.Iso + ".png",
                                };

            return vmCountryList;
        }

        public IEnumerable<VmSelectListItem> GetCountrySelectListItem(int index, int count, byte CountryType = 0)
        {
            var countryRepository = UnitOfWork.GetRepository<CountryRepository>();

            var countryList = countryRepository.GetAllCountries();

            var vmSelectListItem = (from Country in countryList
                                    select new VmSelectListItem
                                    {
                                        Value = Country.Id.ToString(),
                                        Text = Country.Name,
                                    });

            return vmSelectListItem;
        }

    }
}
