using DAL;

using Model;
using Model.ViewModels;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class LanguageRepository : EFBaseRepository<AspNetUser>
    {
        public IEnumerable<VmLanguageDictionaryList> GetDictionaryList()
        {
            using (var context = new WebShopEntities())
            {
                var dictionaries = (from dict in context.Dictionaries
                                    join refWord in context.RefrenceWords on dict.RefrenceWordId equals refWord.Id
                                    select new
                                    {
                                        refWord.Word,
                                        dict.Value,
                                        dict.CultureInfoCode,

                                    }).ToList();

                List<VmLanguageDictionaryList> languageDictionaryList = new List<VmLanguageDictionaryList>();

                var cultureInfoCodeList = dictionaries.Select(c => c.CultureInfoCode).Distinct();

                foreach (var item in cultureInfoCodeList)
                {

                    var vmLanguageDictionaryList = new VmLanguageDictionaryList
                    {
                        CultureInfo = item,
                        LanguageDictionary = new Dictionary<string, string>(),
                    };

                    var dictionaryInfo = dictionaries.Where(d => d.CultureInfoCode == item).ToList();

                    foreach (var dictionary in dictionaryInfo)
                    {
                        if (vmLanguageDictionaryList.LanguageDictionary.ContainsKey(dictionary.Word) == false)
                        {
                            vmLanguageDictionaryList.LanguageDictionary.Add(dictionary.Word, dictionary.Value);
                        }
                    }

                    languageDictionaryList.Add(vmLanguageDictionaryList);

                }

                return languageDictionaryList;
            }
        }
        public Dictionary<string, string> GetDictionary(string cultureInfoCode)
        {
            using (var context = new WebShopEntities())
            {
                var dictionaries = (from dict in context.Dictionaries
                                    join refWord in context.RefrenceWords on dict.RefrenceWordId equals refWord.Id
                                    where dict.CultureInfoCode == cultureInfoCode
                                    select new { refWord.Word, dict.Value }).ToList();

                var dictionary = new Dictionary<string, string>();

                foreach (var item in dictionaries)
                {
                    try
                    {
                        dictionary.Add(item.Word, item.Value);
                    }
                    catch { }
                }

                return dictionary;
            }
        }
        public Dictionary<string, string> GetDictionary(int languageId)
        {

            using (var context = new WebShopEntities())
            {
                var cultureName = context.Languages.Where(l => l.Id == languageId).FirstOrDefault().CultureInfoCode;
                var dictionaries = (from dict in context.Dictionaries
                                    join refWord in context.RefrenceWords on dict.RefrenceWordId equals refWord.Id
                                    where dict.CultureInfoCode == cultureName
                                    select new { refWord.Word, dict.Value }).ToList();

                var dictionary = new Dictionary<string, string>();

                foreach (var item in dictionaries)
                {
                    try
                    {
                        dictionary.Add(item.Word, item.Value);
                    }
                    catch { }
                }

                return dictionary;
            }
        }
        public List<VmActiveLanguage> GetActiveLanguages()
        {
            using (var context = new WebShopEntities())
            {
                var activeLanguage = (from lang in context.Languages
                                      join c in context.Countries on lang.Country equals c.Name
                                      where lang.IsActive == true
                                      select new VmActiveLanguage()
                                      {
                                          Id = lang.Id,
                                          Name = lang.Language1,
                                          CultureInfo = lang.CultureInfoCode,
                                          FlagUrl = "/Resources/Images/flags/32x32/" + c.Iso + ".png",

                                      }).ToList();

                return activeLanguage;
            }
        }
        public void SetActiveLanguages(List<string> activeLanguages)
        {
            using (var context = new WebShopEntities())
            {
                var languages = from lang in context.Languages
                                select lang;

                foreach (var item in languages)
                {
                    if (activeLanguages.Contains(item.CultureInfoCode))
                    {
                        item.IsActive = true;
                    }
                    else
                    {
                        item.IsActive = false;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
