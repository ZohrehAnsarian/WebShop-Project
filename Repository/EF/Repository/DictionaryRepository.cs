using DAL;

using Model;
using Model.ViewModels;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class DictionaryRepository : EFBaseRepository<Dictionary>
    {
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
 
    }
}
