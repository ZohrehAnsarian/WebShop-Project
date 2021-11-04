using BLL;

using Model.ApplicationDomainModels;
using Model.UIControls.Tree;
using Model.ViewModels.PageContent;

using System.Collections.Generic;
using System.Linq;

namespace WebShop.AppDomainHelper
{
    public static class PreLoadData
    {
        public static Dictionary<string, string> LoadLanguage(string cultureInfoCode)
        {

            if (ConstantObjects.LanguageDictionaryList == null)
            {
                var blLanguage = new BLLanguage();

                ConstantObjects.LanguageDictionaryList = blLanguage.GetDictionaryList();
            }

            return ConstantObjects.LanguageDictionaryList.FirstOrDefault(d => d.CultureInfo == cultureInfoCode).LanguageDictionary;
        }
    }
}