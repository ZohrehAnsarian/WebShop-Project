using Model;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.ProductImage;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ProductFeatureRepository : EFBaseRepository<ProductFeature>
    {
        public IEnumerable<ProductFeature> GetAllProductFeatures()
        {
            var productFeatureList = from p in Context.ProductFeatures
                                     select p;

            return productFeatureList;
        }
        public VmProductFeature GetProductFeatureById(Guid id)
        {
            var productFeatureImageList = (from p in Context.ProductFeatures
                                           where p.Id == id
                                           join i in Context.Images on p.Id equals i.ProductFeatureId into sr
                                           from image in sr.DefaultIfEmpty()
                                           select new
                                           {
                                               Id = p.Id,
                                               Description = p.Description,
                                               IconUrl = p.IconUrl,
                                               ParentId = p.ParentId.Value,
                                               Price = p.Price,
                                               Priority = p.Priority,
                                               ProductId = p.ProductId,
                                               Quantity = p.Quantity,
                                               Showcase = p.Showcase,
                                               ImageId = image.Id != null ? image.Id : 0,
                                               ImageUrl = image.ImageUrl,
                                               LinkUrl = image.LinkUrl,
                                               imagePriority = image.Priority != null ? image.Priority : 0,
                                               ProductFeaturetId = image.ProductFeatureId != null ? image.ProductFeatureId.Value : Guid.Empty,
                                               Title = image.Title,

                                           }).ToList();


            var productFeature = from pfi in productFeatureImageList
                                 group pfi
                                 by new
                                 {
                                     pfi.Id,
                                     pfi.ParentId,
                                     pfi.Description,
                                     pfi.IconUrl,
                                     pfi.Price,
                                     pfi.Priority,
                                     pfi.ProductId,
                                     pfi.Quantity,
                                     pfi.Showcase,
                                 }
                                 into g
                                 select new VmProductFeature
                                 {
                                     Id = g.Key.Id,
                                     Description = g.Key.Description,
                                     IconUrl = g.Key.IconUrl,
                                     ParentId = g.Key.ParentId,
                                     Price = g.Key.Price,
                                     Priority = g.Key.Priority,
                                     ProductId = g.Key.ProductId,
                                     Quantity = g.Key.Quantity,
                                     Showcase = g.Key.Showcase,
                                     Images = (from i in g.ToList()
                                               select new VmImage
                                               {
                                                   Id = i.ImageId,
                                                   ImageUrl = i.ImageUrl,
                                                   LinkUrl = i.LinkUrl,
                                                   Priority = i.imagePriority,
                                                   ProducFeaturetId = i.ProductFeaturetId,
                                                   Title = i.Title,

                                               }).ToList()
                                 };

            if (productFeature.Count() > 0)
            {
                return productFeature.First();
            }
            return null;
        }
        public int GetProductFeatureCountByFeatureTypeDetail(int id)
        {
            return Context.ProductFeatures.Where(p => p.FeatureTypeDetailId == id).Count();

        }
        public int GetNoneEmptyProductFeatureCountByFeatureTypeDetail(int id)
        {
            return Context.ProductFeatures.Where(p => p.FeatureTypeDetailId == id && p.Price != null).Count();

        }
        public IEnumerable<int?> GetFeatureTypeDetailIds(IEnumerable<int?> featureTypeDetailIds)
        {
            return Context.ProductFeatures.Where(p => featureTypeDetailIds.Contains(p.FeatureTypeDetailId))
                .Select(d => d.FeatureTypeDetailId).Distinct().ToArray();

        }
        public void AddProductFeature(ProductFeature productFeature)
        {
            Add(productFeature);
        }

        public void CreateBatchProductFeature(IEnumerable<ProductFeature> productFeatureList)
        {

            foreach (var item in productFeatureList)
            {
                Add(item);
            }
        }

        public IEnumerable<ProductFeature> GetProductFeaturesByProduct(int productId)
        {
            return (from p in Context.ProductFeatures
                    where p.ProductId == productId
                    select p).ToArray();
        }

        public void UpdateProductFeature(Guid id, bool? showcase, decimal? price, int? quantity, string iconUrl, string description)
        {
            var oldProductFeature = (from s in Context.ProductFeatures where s.Id == id select s).FirstOrDefault();

            oldProductFeature.Showcase = showcase;
            oldProductFeature.Price = price;
            oldProductFeature.Quantity = quantity;
            oldProductFeature.IconUrl = iconUrl;
            oldProductFeature.Description = description;

            Update(oldProductFeature);
        }

        public void ClearProductFeatureData(Guid id)
        {
            var oldProductFeature = (from s in Context.ProductFeatures where s.Id == id select s).FirstOrDefault();

            oldProductFeature.Price = null;
            oldProductFeature.Quantity = null;
            oldProductFeature.IconUrl = null;
            oldProductFeature.Showcase = null;
            oldProductFeature.Description = null;

            Update(oldProductFeature);

        }
        public bool DeleteProductFeatureByProduct(int productId)
        {
            var deleteableList = Context.ProductFeatures.Where(p => p.ProductId == productId);
            foreach (var deleteable in deleteableList)
            {
                Delete(deleteable);
            }
            return true;
        }


    }
}
