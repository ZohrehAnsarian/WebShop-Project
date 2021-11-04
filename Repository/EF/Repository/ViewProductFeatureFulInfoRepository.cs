using LinqKit;

using Model;
using Model.ViewModels.Product;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.EF.Repository
{
    public class ViewProductFeatureFullInfoRepository : EFBaseRepository<ViewProductFeatureFullInfo>
    {
        public IEnumerable<ViewProductFeatureFullInfo> EntityList { get; set; }
        public int Count(Func<ViewProductFeatureFullInfo, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewProductFeatureFullInfo> GetProductFeatureFulInfoes(int index, int count)
        {
            var categoryFieldList = from categoryField in Context.ViewProductFeatureFullInfoes
                                    select categoryField;

            return categoryFieldList.OrderBy(A => A.Priority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductFeatureFullInfo> GetProductFeatureFulInfoes(Expression<Func<ViewProductFeatureFullInfo, bool>> predicate, int index, int count)
        {
            var productFeatureList = (from productFeature in Context.ViewProductFeatureFullInfoes
                                      select productFeature).Where(predicate);

            return productFeatureList.OrderBy(p=>p.FeatureTypeDetailPriority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductFeatureFullInfo> GetAllViewProductFeatureFulInfoes()
        {
            var viewProductFeatureFullInfoList = from p in Context.ViewProductFeatureFullInfoes
                                                select p;

            return viewProductFeatureFullInfoList;
        }
        public ViewProductFeatureFullInfo GetViewProductFeatureFullInfoById(Guid id)
        {
            return Context.ViewProductFeatureFullInfoes.Find(id);
        }
        public int GetViewProductFeatureFullInfoCountByFeatureTypeDetail(int id)
        {
            return Context.ViewProductFeatureFullInfoes.Where(p => p.FeatureTypeDetailId == id).Count();

        }
        public IEnumerable<ViewProductFeatureFullInfo> GetViewProductFeatureFullInfoesByProduct(int productId)
        {
            return (from p in Context.ViewProductFeatureFullInfoes
                    where p.ProductId == productId
                    select p).ToArray();
        }
        public IEnumerable<Guid> GetViewShopProductsTreeParentIds(int?[] categoryIds)
        {
            return (from p in Context.ViewProductFeatureFullInfoes
                    where categoryIds.Contains(p.CategoryId) && p.ParentId == Guid.Empty
                    select p.Id).ToArray();
        }

    }
}
