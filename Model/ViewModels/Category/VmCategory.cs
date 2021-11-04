using Model.ViewModels.CategoryField;

using System.Collections.Generic;

namespace Model.ViewModels.Category
{
    public class VmCategory
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public List<VmCategory> InnerCategoryList { get; set; }
        public int? LanguageId { get; set; }
        public int ProductCount { get; set; }
        public List<VmCategoryFieldDetail> CategoryFieldDetailList { get; set; }
        public bool IsDefault { get; set; }
        public string Path { get; set; }
    }
}
