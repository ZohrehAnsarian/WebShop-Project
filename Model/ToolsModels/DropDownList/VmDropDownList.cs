using Model.Base;

namespace Model.ToolsModels.DropDownList
{
    public class VmDropDownList : BaseViewModel
    {
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public string OptionLabel { get; set; }
        public string ActiveItemValue { get; set; }
        public string SelectedItems{ get; set; }
        public string HtmlElementId { get; set; }
        public string OnItemSelected { get; set; }
        public string QueryStringParameters { get; set; }

    }
}
