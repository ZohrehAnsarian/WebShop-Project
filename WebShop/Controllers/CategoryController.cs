using BLL;
using BLL.SystemTools;
using Model.UIControls.Tree;
using Model.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Controllers
{
    public class CategoryController : BaseController
    {

        [HttpPost]
        [ActionName("lt")]
        public PartialViewResult LoadTree(VmTree vmTree)
        {

            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            TreeNode finalTree = blCategory.GetAllCategoryTree(vmTree.ParentId);

            return PartialView("_Tree", new VmTree()
            {
                Tree = finalTree,
                ExpandedLevel = vmTree.ExpandedLevel,
                AddCallback = vmTree.AddCallback,
                EditCallback = vmTree.EditCallback,
                DeleteCallback = vmTree.DeleteCallback,
                SelectNodeCallback = vmTree.SelectNodeCallback
            });
        }

        [HttpPost]
        [ActionName("atn")]
        public ActionResult AddTreeNode(int parentId, string nodeName,bool isDefalut)
        {

            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            var id = blCategory.AddNewCategory(parentId, nodeName,isDefalut);
 
            var jsonResult = new
            {
                id,
                nodeName,
                message = "",
            };

            UpdateCategoriesConstantObject();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("utn")]
        public ActionResult EditTreeNode(int id, string nodeName, bool isDefalut)
        {

            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            var result = blCategory.UpdateCategory(id, nodeName,isDefalut);

            var jsonResult = new
            {
                id,
                nodeName,
                result,
                message = "",
            };

            UpdateCategoriesConstantObject();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("dtn")]
        public ActionResult DeleteTreeNode(int id)
        {

            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            var result = blCategory.DeleteCategory(id);

            var jsonResult = new
            {
                id,
                result,
                message = "",
            };

            UpdateCategoriesConstantObject();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCategoriesByParentId(int id)
        {
            var blCategory = new BLCategory(CurrentLanguageId);
            var categoryList = blCategory.GetCategoriesByParentId(id);

            return Json(categoryList, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [ActionName("cca")]
        public ActionResult CheckCategoryAssign(int categoryId)
        {
            var blFeatureType = new BLFeatureType(CurrentLanguageId);
            var count = blFeatureType.GetFeatureTypeCountByCategory(categoryId);

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCategoriesTreeData(string id, int? adminLanguageId = null)
        {
            BLCategory blCategory = null;

            if (adminLanguageId != null)
            {
                blCategory = new BLCategory(adminLanguageId.Value);
            }
            else
            {
                blCategory = new BLCategory(CurrentLanguageId);
            }

            var nodesList = blCategory.GetAllCategoryTree(id);

            return Json(nodesList, JsonRequestBehavior.AllowGet);
        }
         
    }
}