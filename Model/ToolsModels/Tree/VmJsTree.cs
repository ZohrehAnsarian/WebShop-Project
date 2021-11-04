using Model.Base;

namespace Model.ToolsModels.Tree
{
    public class VmJsTree : BaseViewModel
    {
        public string DataAction { get; set; }
        public string DataActionParameter { get; set; }
        public string DataController { get; set; }
        public string OperationAction { get; set; }
        public string OperationController { get; set; }
        public string HtmlElementId { get; set; }
        public string ExternalHtmlControlId { get; set; }
        public string OnItemSelected { get; set; }
        public string OnItemDeleted { get; set; }
        public string OnItemCreated { get; set; }
        public string SelectedItemHtmlControlId { get; set; }
        public string AdditionalData { get; set; }
        public bool ShowSearchBox { get; set; }
        public bool ShowLanguageList { get; set; }
        public string SearchText { get; set; }
        public bool OneClickExtend { get; set; }
        public bool SkipAutoEdit { get; set; }
        public bool HideRootNode { get; set; }
        public bool Sort { get; set; }
        public int RestrictLevelNumber { get; set; }
        public string GridElementIdToRefresh { get; set; }
        public string AdminLanguageElementId { get; set; }

    }
}