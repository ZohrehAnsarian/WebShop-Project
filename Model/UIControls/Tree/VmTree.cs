using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UIControls.Tree
{
    public class VmTree : BaseViewModel
    {
        public TreeNode Tree { get; set; }
        public string ParentId { get; set; }
        public string RouteAddress { get; set; }
        public string ContainerId { get; set; }
        public int ExpandedLevel { get; set; }
        public string AddCallback { get; set; }
        public string EditCallback { get; set; }
        public string DeleteCallback { get; set; }
        public string SelectNodeCallback { get; set; }
        public bool ShowAdditionalDataInLeaf { get; set; }
        public string AdditionalDataRenderCallback { get; set; }
    }
}
