using DAL;

using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;
using System;

namespace Repository.EF.Repository
{
    public class SpHandlerRepository
    {
        public WebShopEntities Context = new WebShopEntities();

        public IEnumerable<int?> SpGetParentsOfCategory(int categoryId)
        {
            var parentIdList = Context.SpGetParentsOfCategory(categoryId).AsEnumerable();

            return parentIdList.ToArray();
        }

        public IEnumerable<int?> SpGetChildrenOfCategory(int categoryId)
        {
            var childIdList = Context.SpGetChildrenOfCategory(categoryId).AsEnumerable();

            return childIdList.ToArray();
        }

        public List<ViewShopProduct> GetProductFeaturesByFilter(
            out string groupedIcons, int firstIconPriority,
            string featureDetailCombinationList, string delimiter,
            string parentIdList, decimal? minPrice, decimal? maxPrice)
        {
            ObjectParameter groupedIconsObjectParameter = new ObjectParameter("groupedIconList", typeof(string));

            var viewShopProductList = Context.SpGetProductFeaturesByFilter(
              groupedIconsObjectParameter,
              firstIconPriority,
              featureDetailCombinationList,
              delimiter,
              parentIdList,
              minPrice,
              maxPrice);

            var result = viewShopProductList.Select(v => new ViewShopProduct
            {
                CategoryId = v.CategoryId,
                CategoryName = v.CategoryName,
                Description = v.Description,
                ExpiryDate = v.ExpiryDate,
                FeatureTypeDetailId = v.FeatureTypeDetailId,
                BaseFeatureTypeDetailId = v.BaseFeatureTypeDetailId,
                FeatureTypeDetailName = v.FeatureTypeDetailName,
                FeatureTypeDetailPriority = v.FeatureTypeDetailPriority,
                FeatureTypeId = v.FeatureTypeId,
                FeatureTypeName = v.FeatureTypeName,
                FeatureTypePriority = v.FeatureTypePriority,
                IconUrl = v.IconUrl,
                Id = v.Id,
                ImagePriority = v.ImagePriority,
                ImageTitle = v.ImageTitle,
                ImageUrl = v.ImageUrl,
                LinkUrl = v.LinkUrl,
                ParentId = v.ParentId,
                Price = v.Price,
                Priority = v.Priority,
                ProductDescription = v.ProductDescription,
                ProductId = v.ProductId,
                ProductionDate = v.ProductionDate,
                ProductName = v.ProductName,
                Quantity = v.Quantity,
                Showcase = v.Showcase
            }).ToList();
            groupedIcons = groupedIconsObjectParameter.Value?.ToString();

            return result;
        }

        public List<ViewShopProduct> GetProductFeaturesByProductFilter(
             string featureDetailCombination, string delimiter,
             string parentId, decimal? minPrice, decimal? maxPrice)
        {

            var viewShopProductList = Context.SpGetProductFeaturesByProductFilter(
              featureDetailCombination,
              delimiter,
              parentId,
              minPrice,
              maxPrice);

            var result = viewShopProductList.Select(v => new ViewShopProduct
            {
                CategoryId = v.CategoryId,
                CategoryName = v.CategoryName,
                Description = v.Description,
                ExpiryDate = v.ExpiryDate,
                FeatureTypeDetailId = v.FeatureTypeDetailId,
                BaseFeatureTypeDetailId = v.BaseFeatureTypeDetailId,
                FeatureTypeDetailName = v.FeatureTypeDetailName,
                FeatureTypeDetailPriority = v.FeatureTypeDetailPriority,
                FeatureTypeId = v.FeatureTypeId,
                FeatureTypeName = v.FeatureTypeName,
                FeatureTypePriority = v.FeatureTypePriority,
                IconUrl = v.IconUrl,
                Id = v.Id,
                ImagePriority = v.ImagePriority,
                ImageTitle = v.ImageTitle,
                ImageUrl = v.ImageUrl,
                LinkUrl = v.LinkUrl,
                ParentId = v.ParentId,
                Price = v.Price,
                Priority = v.Priority,
                ProductDescription = v.ProductDescription,
                ProductId = v.ProductId,
                ProductionDate = v.ProductionDate,
                ProductName = v.ProductName,
                Quantity = v.Quantity,
                Showcase = v.Showcase
            }).ToList();

            return result;
        }

        public ViewShopProduct GetOrderProductFeature(
         string featureDetailCombinationList,
         string delimiter,
         string parentIdList)
        {

            var viewShopProduct = Context.SpGetOrderProductFeature(
              featureDetailCombinationList,
              delimiter,
              parentIdList);

            var result = viewShopProduct.Select(v => new ViewShopProduct
            {
                CategoryId = v.CategoryId,
                CategoryName = v.CategoryName,
                Description = v.Description,
                ExpiryDate = v.ExpiryDate,
                FeatureTypeDetailId = v.FeatureTypeDetailId,
                BaseFeatureTypeDetailId = v.BaseFeatureTypeDetailId,
                FeatureTypeDetailName = v.FeatureTypeDetailName,
                FeatureTypeDetailPriority = v.FeatureTypeDetailPriority,
                FeatureTypeId = v.FeatureTypeId,
                FeatureTypeName = v.FeatureTypeName,
                FeatureTypePriority = v.FeatureTypePriority,
                IconUrl = v.IconUrl,
                Id = v.Id,
                ImagePriority = v.ImagePriority,
                ImageTitle = v.ImageTitle,
                ImageUrl = v.ImageUrl,
                LinkUrl = v.LinkUrl,
                ParentId = v.ParentId,
                Price = v.Price,
                Priority = v.Priority,
                ProductDescription = v.ProductDescription,
                ProductId = v.ProductId,
                ProductionDate = v.ProductionDate,
                ProductName = v.ProductName,
                Quantity = v.Quantity,
                Showcase = v.Showcase
            }).FirstOrDefault();

            return result;
        }

        public List<ViewFeatureTypeDetail> SpGetAllProductFeatureFeatureTypeDetail(Guid productFeatureId)
        {
            var featureTypeDetailList = Context.SpGetAllProductFeatureFeatureTypeDetail(productFeatureId);
            var result = featureTypeDetailList.Select(f => new ViewFeatureTypeDetail { Id = f.FeatureTypeDetailId.Value, Name = f.Name }).ToList();
            return result;
        }
    }
}
