using System.Collections.Generic;

namespace Model.UIControls.Tree
{
    public class TreeNode
    {
        public string id { set; get; } // نام این خواص باید با مستندات هماهنگ باشد
        public List<int> subIdList { set; get; }
        public string text { set; get; }
        public string icon { set; get; }
        public string additionalData { set; get; }
        public string LanguageId { set; get; }
        public TreeNodeState state { set; get; }
        public List<TreeNode> children { set; get; }
        public TreeNodeLiAttributes li_attr { set; get; }
        public TreeNodeAAttributes a_attr { set; get; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsRoot { get; set; }
        public string path { get; set; }
        public string description { get; set; }

        public TreeNode()
        {

            state = new TreeNodeState();
            children = new List<TreeNode>();
            li_attr = new TreeNodeLiAttributes();
            a_attr = new TreeNodeAAttributes();
        }
    }

    public class TreeNodeAAttributes
    {
        // به هر تعداد و نام اختیاری می‌توان خاصیت تعریف کرد
        public string href { set; get; }
        public bool IsFirstNode { set; get; }
    }

    public class TreeNodeLiAttributes
    {
        // به هر تعداد و نام اختیاری می‌توان خاصیت تعریف کرد
        public string data { set; get; }
    }

    public class TreeNodeState
    {
        public bool opened { set; get; }
        public bool disabled { set; get; }
        public bool selected { set; get; }

        public TreeNodeState()
        {
            opened = true;
        }
    }

    public enum JsTreeOperation
    {
        DeleteNode,
        CreateNode,
        RenameNode,
        MoveNode,
        CopyNode
    }

    public class JsTreeOperationData
    {
        public JsTreeOperation Operation { set; get; }
        public string Id { set; get; }
        public string ParentId { set; get; }
        public string OriginalId { set; get; }
        public string Text { set; get; }
        public string Position { set; get; }
        public string Href { set; get; }
        public string AdditionalData { set; get; }
        public int? LanguageId { get; set; }
    }

}

