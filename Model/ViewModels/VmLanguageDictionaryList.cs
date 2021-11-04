using System.Collections.Generic;

namespace Model.ViewModels
{
    public class VmLanguageDictionaryList
    {
        public string CultureInfo;
        public Dictionary<string, string> LanguageDictionary { get; set; }
    }
}
