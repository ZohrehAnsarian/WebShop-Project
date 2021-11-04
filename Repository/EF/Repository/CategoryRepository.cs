using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class CategoryRepository : EFBaseRepository<Category>
    {
        public IEnumerable<Category> GetAllCategories()
        {
            var CategoryList = from s in Context.Categories.AsNoTracking()
                               where s.ParentId != null
                               select s;

            return CategoryList.OrderBy(A => A.Name).ToArray();
        }
        public IEnumerable<Category> GetAllCategoryTree()
        {
            var CategoryList = from s in Context.Categories.AsNoTracking() select s;

            return CategoryList.OrderBy(A => A.Name).ToArray();
        }

        public IEnumerable<Category> GetCategoryByParentId(int parentId)
        {
            var CategoryList = from s in Context.Categories.AsNoTracking() where s.ParentId == parentId select s;

            return CategoryList.OrderBy(A => A.Name).ToArray();
        }
        public int GetCategoryIdByParentId(int parentId)
        {
            var CategoryList = from s in Context.Categories.AsNoTracking() where s.ParentId == parentId select s;

            return CategoryList.FirstOrDefault().Id;
        }
        public Category GetCategoriesById(int id)
        {
            var category = Context.Categories.Find(id);

            return category;
        }
        public Category GetCategoriesWithFieldsById(int id)
        {
            var category = Context.Categories.AsNoTracking().Include("CategoryFields").Where(c => c.Id == id).FirstOrDefault();

            return category;
        }
        public Category GetServiceBylanguage(int languageId, int parentId)
        {
            var category = Context.Categories.FirstOrDefault(s => s.ParentId == parentId);

            return category;
        }
        public void AddNewCategory(Category category)
        {
            Add(category);
        }

        public void UpdateCategory(Category category)
        {
            var oldCategory = (from s in Context.Categories.AsNoTracking() where s.Id == category.Id select s).FirstOrDefault();
            oldCategory.Name = category.Name;
            oldCategory.IsDefault = category.IsDefault;

            Update(oldCategory);

        }

        public bool DeleteCategory(int id)
        {
            var deleteable = Context.Categories.Find(id);
            Delete(deleteable);
            return true;

        }
    }
}
