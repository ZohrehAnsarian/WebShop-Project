using Model.Base;
using Model.ToolsModels.Tree;

namespace Model.ViewModels.Category
{
    public class VmCategoryTree : BaseViewModel, ITmNode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string AdditionalData { get; set; }
        public int? LanguageId { get; set; }
        public string NodeLanguageId { get; set; }
        public bool IsDefault { get; set; }
    }
}