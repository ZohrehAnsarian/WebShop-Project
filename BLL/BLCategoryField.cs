using BLL.Base;
using Model;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.ViewModels.CategoryField;
using Newtonsoft.Json;

namespace BLL
{
    public class BLCategoryField : BLBase
    {
        CategoryFieldRepository categoryFieldRepository;

        public BLCategoryField(int currentLanguageId) : base(currentLanguageId)
        {
            categoryFieldRepository = UnitOfWork.GetRepository<CategoryFieldRepository>();
        }

        public VmCategoryField GetCategoryFieldsByCategoryId(int categoryId)
        {
            var categoryFieldList = categoryFieldRepository.GetCategoryFieldsByCategoryId(categoryId);
            if (categoryFieldList.Count() > 0)
            {
                var categoryField = categoryFieldList.First();

                var categoryFieldWithDetail = new VmCategoryField
                {

                    Id = categoryField.Id,
                    CategoryId = categoryField.CategoryId,
                    CategoryName = categoryField.Name,

                    CategoryFieldDetailList = (from d in categoryFieldList
                                               select new VmCategoryFieldDetail
                                               {
                                                   Id = d.Id,
                                                   Name = d.Name,
                                                   CategoryId = d.CategoryId,
                                                   Priority = d.Priority
                                               }).OrderBy(d => d.Priority).ToList()
                };

                var categoryFieldDetailIds = categoryFieldWithDetail.CategoryFieldDetailList.Select(d => d.Id).ToArray();

                categoryFieldWithDetail.CategoryFieldIds = string.Join(",", categoryFieldDetailIds);

                categoryFieldWithDetail.CategoryFieldPriorities =
                    string.Join(",", categoryFieldWithDetail.CategoryFieldDetailList.Select(d => d.Priority));

                categoryFieldWithDetail.CategoryFieldNames =
                    string.Join(",", categoryFieldWithDetail.CategoryFieldDetailList.Select(d => d.Name));


                List<int> categoryFieldDetailIdList = new List<int>();

                foreach (var item in categoryFieldDetailIds)
                {
                    categoryFieldDetailIdList.Add(item);
                }

                BLProductCategoryField blProductCategoryField = new BLProductCategoryField(CurrentLanguageId);

                var usedCategoryFieldDetailIds = blProductCategoryField.GetProductCategoryFieldIds(categoryFieldDetailIdList);

                foreach (var item in categoryFieldWithDetail.CategoryFieldDetailList)
                {
                    if (usedCategoryFieldDetailIds.Contains(item.Id) == true)
                    {
                        item.Deletable = false;
                    }
                    else
                    {
                        item.Deletable = true;
                    }

                }

                var Deletable = categoryFieldWithDetail.CategoryFieldDetailList.Select(d => d.Deletable).ToArray();

                categoryFieldWithDetail.CategoryFieldDeletable = JsonConvert.SerializeObject(Deletable);
                return categoryFieldWithDetail;

            }

            var blCategory = new BLCategory(CurrentLanguageId);
            var category = blCategory.GetCategoriesById(categoryId);

            var emptyCategoryField = new VmCategoryField
            {

                CategoryId = category.Id,
                CategoryName = category.Name,
                CategoryFieldDeletable = "[]",
            };
            return emptyCategoryField;
        }
        public int GetCategoryFieldsCountByCategoryId(int categoryId)
        {
            return categoryFieldRepository.GetCategoryFieldsCountByCategoryId(categoryId);
        }

        public bool AddCategoryField(VmCategoryField categoryField)
        {
            try
            {
                var categoryFieldNames = categoryField.CategoryFieldNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var categoryFieldPriorities = categoryField.CategoryFieldPriorities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var categoryFieldList = new List<CategoryField>();

                for (var i = 0; i < categoryFieldNames.Length; i++)
                {
                    categoryFieldList.Add(
                        new CategoryField
                        {
                            CategoryId = categoryField.CategoryId,
                            Name = categoryFieldNames[i],
                            Priority = int.Parse(categoryFieldPriorities[i]),
                        });
                }

                categoryFieldRepository.BatchAddCategoryField(categoryFieldList);
                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EditCategoryField(VmCategoryField categoryField)
        {
            try
            {
                var clientCategoryField = JsonConvert.DeserializeObject<List<VmCategoryFieldDetail>>(categoryField.JSONCategoryFieldDetail);

                var updatableCategoryField = categoryFieldRepository.GetCategoryFieldsByCategoryId(categoryField.CategoryId);

                var addedList = (from a in clientCategoryField
                                 where a.RowState == "added"
                                 select new CategoryField
                                 {
                                     CategoryId = categoryField.CategoryId,
                                     Name = a.Name,
                                     Priority = a.Priority,
                                 }).ToList();

                if (addedList.Count() > 0)
                {
                    categoryFieldRepository.BatchAddCategoryField(addedList);
                }

                var editableList = (from a in clientCategoryField
                                    where a.RowState == "edited"
                                    select new CategoryField
                                    {
                                        Id = a.Id,
                                        CategoryId = categoryField.CategoryId,
                                        Name = a.Name,
                                        Priority = a.Priority,
                                    }).ToList();

                if (editableList.Count() > 0)
                {
                    foreach (var item in editableList)
                    {
                        var edited = updatableCategoryField.First(d => d.Id == item.Id);
                        edited.Name = item.Name;
                        edited.Priority = item.Priority;
                        categoryFieldRepository.UpdateCategoryField(edited);
                    }
                }

                var deletedIds = clientCategoryField.Where(d => d.RowState == "deleted").Select(d => d.Id);

                categoryFieldRepository.BatchDeleteCategoryField(deletedIds.ToArray());

                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteCategoryField(int id)
        {
            try
            {
                var blProductCategoryField = new BLProductCategoryField(CurrentLanguageId);

                if (blProductCategoryField.GetProductCategoryFieldCountByProduct(id) == 0)
                {
                    categoryFieldRepository.DeleteCategoryField(id);

                    return UnitOfWork.Commit();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}