using BLL;

using Model.UIControls.Tree;

using System.Linq;
using System.Web.Mvc;

using static Model.ApplicationDomainModels.ConstantObjects;


namespace WebShop.Controllers
{
    public class TreeController : BaseController
    {
        [HttpPost]
        [ActionName("lct")]
        public PartialViewResult LoadCategoryTree(VmTree vmTree)
        {

            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            TreeNode finaltree = blCategory.GetAllCategoryTree(vmTree.ParentId);

            return PartialView("_Tree", new VmTree()
            {
                Tree = finaltree,
                ContainerId = vmTree.ContainerId,
                ExpandedLevel = vmTree.ExpandedLevel,

                AddCallback = vmTree.AddCallback,
                EditCallback = vmTree.EditCallback,
                DeleteCallback = vmTree.DeleteCallback,
                SelectNodeCallback = vmTree.SelectNodeCallback,

                ReadOnly = vmTree.ReadOnly,

                ShowAdditionalDataInLeaf = vmTree.ShowAdditionalDataInLeaf,
                AdditionalDataRenderCallback = vmTree.AdditionalDataRenderCallback,
            });
        }

        [HttpPost]
        [ActionName("lft")]
        public PartialViewResult LoadFeatureTree(VmTree vmTree, int productId)
        {
            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);
            
            TreeNode finaltree = blProductFeature.GetProductFeaturesByProduct(vmTree.ParentId, productId);

            return PartialView("_Tree", new VmTree()
            {
                Tree = finaltree,
                ContainerId = vmTree.ContainerId,
                ExpandedLevel = vmTree.ExpandedLevel,

                AddCallback = vmTree.AddCallback,
                EditCallback = vmTree.EditCallback,
                DeleteCallback = vmTree.DeleteCallback,
                SelectNodeCallback = vmTree.SelectNodeCallback,

                ReadOnly = vmTree.ReadOnly,

                ShowAdditionalDataInLeaf = vmTree.ShowAdditionalDataInLeaf,
                AdditionalDataRenderCallback = vmTree.AdditionalDataRenderCallback,
            });
        }
    }
}
