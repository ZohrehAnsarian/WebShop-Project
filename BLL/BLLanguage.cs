using BLL.Base;
using Model.ViewModels;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLLanguage : BLBase
    {
        public Dictionary<string, string> GetDictionary(string cultureInfoCode)
        {
            var dictionary = new LanguageRepository();

            return dictionary.GetDictionary(cultureInfoCode);
        }
      
        public IEnumerable<VmLanguageDictionaryList> GetDictionaryList()
        {
            var dictionary = new LanguageRepository();

            return dictionary.GetDictionaryList();
        }
        public Dictionary<string, string> GetDictionary(int languageId)
        {
            var dictionary = new LanguageRepository();

            return dictionary.GetDictionary(languageId);
        }
        public List<VmActiveLanguage> GetActiveLanguages()
        {
            var activeLanguage = new LanguageRepository();

            return activeLanguage.GetActiveLanguages();
        }
        public void SetActiveLanguages(List<string> activeLanguages)
        {
            var activeLanguage = new LanguageRepository();

            activeLanguage.SetActiveLanguages(activeLanguages);
        }
        public string GetActiveLanguagesCommaSeparated(List<VmActiveLanguage> activeLanguageList)
        {
            var tesmpString = string.Empty;

            return string.Join(",", activeLanguageList.Select(l => l.CultureInfo).ToList<string>());
        }
    }
}
