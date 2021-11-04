using Model;
using Repository.Core;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewProductCategoryFieldRepository : EFBaseRepository<ViewProductCategoryField>, IModelPaged<ViewProductCategoryField>
    {
        public IEnumerable<ViewProductCategoryField> EntityList { get; set; }
        public int Count(Func<ViewProductCategoryField, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewProductCategoryField> Select(int index, int count)
        {
            var categoryFieldList = from categoryField in Context.ViewProductCategoryFields
                                   select categoryField;

            return categoryFieldList.OrderBy(A => A.Priority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductCategoryField> Select(Func<ViewProductCategoryField, bool> predicate, int index, int count)
        {
            var categoryFieldList = (from categoryField in Context.ViewProductCategoryFields
                                    select categoryField).Where(predicate);

            return categoryFieldList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductCategoryField> GetCategoryFieldsByName(string categoryFieldName, bool like)
        {

            var categoryFieldList = from categoryField in Context.ViewProductCategoryFields
                                   select categoryField;

            if (like)
            {
                categoryFieldList = categoryFieldList.Where(j => j.CategoryFieldName.Contains(categoryFieldName));
            }
            else
            {
                categoryFieldList = categoryFieldList.Where(j => j.CategoryFieldName == categoryFieldName);

            }
            return categoryFieldList.ToArray();
        }
        public IEnumerable<ViewProductCategoryField> Select(ViewProductCategoryField filterItem, int index, int count)
        {
            var categoryFieldList = from categoryField in Context.ViewProductCategoryFields
                                   select categoryField;


            if (filterItem.CategoryId != 0)
            {
                categoryFieldList = categoryFieldList.Where(j => filterItem.CategoryId == j.CategoryId);
            }

            if (filterItem.CategoryFieldName != null)
            {
                categoryFieldList = categoryFieldList.Where(j => j.CategoryFieldName.Contains(filterItem.CategoryFieldName));
            }

            if (filterItem.Priority != null)
            {
                categoryFieldList = categoryFieldList.Where(j => j.Priority == filterItem.Priority);
            }

            return categoryFieldList.OrderBy(j => j.CategoryFieldName).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductCategoryField> GetCategoryFieldsByCategory(int categoryId)
        {
            var categoryFieldList = from categoryField in Context.ViewProductCategoryFields
                                   where categoryField.CategoryId == categoryId
                                   select categoryField;


            return categoryFieldList.ToArray();
        }
        public int GetCategoryFieldsCount(int categoryId)
        {
            return (from categoryField in Context.ViewProductCategoryFields
                    where categoryField.CategoryId == categoryId
                    select categoryField).Count();



        }
        public IEnumerable<ViewProductCategoryField> GetCategoryFieldsByCategoryIds(int[] categoryIds)
        {
            var categoryFieldList = from categoryField in Context.ViewProductCategoryFields
                                   where categoryIds.Contains(categoryField.CategoryId)
                                   select categoryField;


            return categoryFieldList.ToArray();
        }
        public IEnumerable<ViewProductCategoryField> GetAllCategoryFields()
        {
            var categoryFieldList = from categoryField in Context.ViewProductCategoryFields
                                   select categoryField;

            return categoryFieldList.ToArray();
        }
    }
}
