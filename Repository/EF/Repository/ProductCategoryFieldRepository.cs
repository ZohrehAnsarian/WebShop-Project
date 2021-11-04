using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ProductCategoryFieldRepository : EFBaseRepository<ProductCategoryField>
    {
        public int GetProductCategoryFieldCountByProduct(int productId)
        {
            return Context.ProductCategoryFields.Where(p => p.ProductId == productId).Count();

        }
        public void CreateBatchProductCategoryField(IEnumerable<ProductCategoryField> productCategoryFieldList)
        {

            foreach (var item in productCategoryFieldList)
            {
                Add(item);
            }
        }
        public IEnumerable<int> GetProductCategoryFieldIds(IEnumerable<int> categoryFieldIdList)
        {
            return Context.ProductCategoryFields.Where(p => categoryFieldIdList.Contains(p.Id))
             .Select(d => d.Id).Distinct().ToArray();
        }
        public bool DeleteProductCategoryFieldByProduct(int productId)
        {
            var deleteableList = Context.ProductCategoryFields.Where(p => p.ProductId == productId);
            foreach (var deleteable in deleteableList)
            {
                Delete(deleteable);
            }
            return true;
        }
    }
}
