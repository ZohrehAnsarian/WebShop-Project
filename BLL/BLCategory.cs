using BLL.Base;
using BLL.SystemTools;

using CyberneticCode.Web.Mvc.Helpers;

using Model;
using Model.UIControls.Tree;
using Model.ViewModels.Category;
using Model.ViewModels.CategoryField;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLCategory : BLBase
    {
        CategoryRepository categoryRepository;
        FeatureTypeRepository featureTypeRepository;
        FeatureTypeDetailRepository featureTypeDetailRepository;

        public BLCategory(int currentLanguageId) : base(currentLanguageId)
        {
            categoryRepository = UnitOfWork.GetRepository<CategoryRepository>();
            featureTypeRepository = UnitOfWork.GetRepository<FeatureTypeRepository>();
            featureTypeDetailRepository = UnitOfWork.GetRepository<FeatureTypeDetailRepository>();
        }
        public IEnumerable<int?> GetParentIds(int categoryId)
        {
            SpHandlerRepository spHandlerRepository = new SpHandlerRepository();
            return spHandlerRepository.SpGetParentsOfCategory(categoryId);
        }
        public IEnumerable<int?> GetChildIds(int categoryId)
        {
            SpHandlerRepository spHandlerRepository = new SpHandlerRepository();
            return spHandlerRepository.SpGetChildrenOfCategory(categoryId);
        }
        public IEnumerable<VmCategory> GetAllCategories()
        {
            IEnumerable<Category> categoryList = categoryRepository.GetAllCategories();

            IEnumerable<VmCategory> VmCategoryList = from s in categoryList
                                                     select new VmCategory
                                                     {
                                                         Id = s.Id,
                                                         Name = s.Name,

                                                         IsDefault = s.IsDefault,
                                                     };

            return VmCategoryList;
        }
        public IEnumerable<VmCategoryTree> GetAllMenuCategories()
        {
            IEnumerable<Category> categoryList = categoryRepository.GetAllCategoryTree();

            IEnumerable<VmCategoryTree> VmCategoryList = from s in categoryList
                                                         select new VmCategoryTree
                                                         {
                                                             Id = s.Id,
                                                             ParentId = s.ParentId,
                                                             Name = s.Name,


                                                             IsDefault = s.IsDefault,
                                                         };

            return VmCategoryList;
        }
        public TreeNode GetAllCategoryTree(string parentId)
        {
            try
            {
                IEnumerable<Category> categoryList = categoryRepository.GetAllCategories();

                IEnumerable<VmCategoryTree> VmCategoryList = from s in categoryList
                                                             select new VmCategoryTree
                                                             {
                                                                 Id = s.Id,
                                                                 ParentId = s.ParentId,
                                                                 Name = s.Name,


                                                                 IsDefault = s.IsDefault,
                                                             };

                BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

                if (VmCategoryList != null && VmCategoryList.Count() > 0)
                {
                    return blTreeModelTools.GetTreeModel(VmCategoryList, parentId);

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<VmCategory> GetCategoriesByParentId(int parentId)
        {
            IEnumerable<Category> allCategoryList = categoryRepository.GetAllCategories();
            IEnumerable<Category> categoryList = categoryRepository.GetCategoryByParentId(parentId);

            IEnumerable<VmCategory> VmCategoryList = from s in categoryList
                                                     select new VmCategory
                                                     {
                                                         Id = s.Id,
                                                         ParentId = s.ParentId,
                                                         Name = s.Name,
                                                         IsDefault = s.IsDefault,
                                                         InnerCategoryList = (from ins in allCategoryList
                                                                              where ins.ParentId == s.Id
                                                                              select new VmCategory
                                                                              {
                                                                                  Id = ins.Id,
                                                                                  Name = ins.Name,
                                                                                  ParentId = ins.ParentId,

                                                                                  IsDefault = s.IsDefault,

                                                                              }).ToList()
                                                     };

            return VmCategoryList.ToArray();
        }
        public int GetCategoryIdByParentId(int parentId)
        {
            return categoryRepository.GetCategoryIdByParentId(parentId);
        }
        public VmCategory GetCategoriesById(int id)
        {
            Category category = categoryRepository.GetCategoriesById(id);

            VmCategory vmCategory = new VmCategory
            {
                Id = category.Id,
                Name = category.Name,
                IsDefault = category.IsDefault,
            };

            return vmCategory;
        }
        public VmCategory GetCategoriesWithFieldsById(int id)
        {
            Category category = categoryRepository.GetCategoriesWithFieldsById(id);

            VmCategory vmCategory = new VmCategory
            {
                Id = category.Id,
                Name = category.Name,
                IsDefault = category.IsDefault,
                CategoryFieldDetailList = (from c in category.CategoryFields
                                           select new VmCategoryFieldDetail
                                           {
                                               Id = c.Id,
                                               CategoryId = c.CategoryId,
                                               Name = c.Name,
                                               Priority = c.Priority,
                                               Value = ""
                                           }).ToList()
            };

            return vmCategory;
        }
        public VmCategory GetServiceByLanguage(int languageId, int parentId)
        {
            Category category = categoryRepository.GetServiceBylanguage(languageId, parentId);

            VmCategory vmCategory = new VmCategory
            {
                Id = category.Id,
                Name = category.Name,

                IsDefault = category.IsDefault,
            };

            return vmCategory;
        }
        public int AddNewCategory(int parentId, string nodeName, bool isDefault = false)
        {
            //var result = UIHelper.TranslateText(nodeName);

            Category newCategory = new Category()
            {
                ParentId = parentId,
                Name = nodeName,
                IsDefault = isDefault,
            };

            var blFeatureType = new BLFeatureType(CurrentLanguageId);

            var parentFeatureTypeList = blFeatureType.GetAssignedFeatureTypeByCategoryId(parentId);

            if (parentFeatureTypeList.Count() == 0)
            {
                return -1;
            }

            var featureTpeList = new List<FeatureType>();

            foreach (var item in parentFeatureTypeList)
            {
                featureTpeList.Add(new FeatureType
                {
                    BaseFeatureTypeId = item.BaseFeatureTypeId.Value,
                    Priority = item.Priority,
                    FeatureTypeDetails = (from d in item.FeatureTypeDetailList
                                          select new FeatureTypeDetail
                                          {
                                              BaseFeatureTypeDetailId = d.BaseFeatureTypeDetailId.Value,
                                              Priority = d.Priority,

                                          }).ToList()
                });
            }

            newCategory.FeatureTypes = featureTpeList;

            categoryRepository.AddNewCategory(newCategory);

            UnitOfWork.Commit();


            return newCategory.Id;
        }
        public bool UpdateCategory(int id, string nodeName, bool isDefault = false)
        {
            try
            {
                Category updateableCategory = new Category()
                {
                    Id = id,
                    Name = nodeName,
                    IsDefault = isDefault,
                };

                categoryRepository.UpdateCategory(updateableCategory);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteCategory(int id)
        {
            bool result = true;
            try
            {
                var blProduct = new BLProduct(CurrentLanguageId);
                var blCategoryField = new BLCategoryField(CurrentLanguageId);
                var blFeatureType = new BLFeatureType(CurrentLanguageId);

                if (
                    blProduct.GetProductCountByCategoryId(id) == 0 &&
                    blCategoryField.GetCategoryFieldsCountByCategoryId(id) == 0
                    )
                {
                    var featureType = new BLFeatureType(CurrentLanguageId).GetAssignedFeatureTypeByCategoryId(id);

                    var featureTypeIds = (from f in featureType select f.Id).ToList();

                    featureTypeDetailRepository.DeleteFeatureTypeDetailByParentIds(featureTypeIds.ToArray());
                    featureTypeRepository.DeleteFeatureTypeByIds(featureTypeIds.ToArray());

                    categoryRepository.DeleteCategory(id);

                    return UnitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }

        public List<VmCategory> ConvertCategoryTreeToList(TreeNode tree)
        {
            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

            var result = blTreeModelTools.ComputePaths(tree, n => n.children).ToList();

            var categoryNodeList = new List<VmCategory>();

            foreach (var item in result)
            {

                foreach (var subItem in item)
                {
                    categoryNodeList.Add(new VmCategory
                    {
                        Id = int.Parse(subItem.id),
                        Path = subItem.path
                    });
                }

            }

            return categoryNodeList;
        }
 
    }
}