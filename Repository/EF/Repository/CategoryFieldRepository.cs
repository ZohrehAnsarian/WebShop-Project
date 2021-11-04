using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class CategoryFieldRepository : EFBaseRepository<CategoryField>
    {
        public IEnumerable<CategoryField> GetCategoryFieldsByCategoryId(int categoryId)
        {
            return (from s in Context.CategoryFields
                    where s.CategoryId == categoryId
                    select s).ToArray();

        }
        public int GetCategoryFieldsCountByCategoryId(int categoryId)
        {
            return (from s in Context.CategoryFields
                    where s.CategoryId == categoryId
                    select s).Count();

        }
        public void BatchAddCategoryField(List<CategoryField> categoryFieldList)
        {
            foreach (var item in categoryFieldList)
            {
                Add(item);
            }
        }

        public void UpdateCategoryField(CategoryField categoryField)
        {
            var oldCategoryField = (from s in Context.CategoryFields where s.Id == categoryField.Id select s).FirstOrDefault();
            oldCategoryField.Name = categoryField.Name;
            oldCategoryField.Priority = categoryField.Priority;
            oldCategoryField.CategoryId = categoryField.CategoryId;

            Update(oldCategoryField);
        }

        public bool DeleteCategoryField(int id)
        {
            var deleteable = Context.CategoryFields.Find(id);
            Delete(deleteable);
            return true;
        }

        public void BatchDeleteCategoryField(int[] ids)
        {
            var deletableCategoryField = (from s in Context.CategoryFields where ids.Contains(s.Id) select s).ToList();

            foreach (var item in deletableCategoryField)
            {
                Delete(item);
            }
        }
    }
}
