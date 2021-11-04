using AppClassLibrary;

using BLL.Base;

using CyberneticCode.Web.Mvc.Helpers;

using Model;
using Model.ViewModels.CategoryField;
using Model.ViewModels.Product;

using Newtonsoft.Json;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLProduct : BLBase
    {
        ProductRepository productRepository;
        ProductCategoryFieldRepository productCategoryFieldRepository;
        public int GetProductCountByCategoryId(int id)
        {
            return productRepository.GetProductCountByCategoryId(id);
        }
        public BLProduct(int currentLanguageId) : base(currentLanguageId)
        {
            productRepository = UnitOfWork.GetRepository<ProductRepository>();
            productCategoryFieldRepository = UnitOfWork.GetRepository<ProductCategoryFieldRepository>();
        }
        public IEnumerable<VmProduct> GetProductsByCategoryId(int categoryId, string searchText = "")
        {
            var productList = productRepository.GetProductsByCategoryId(categoryId, searchText).ToList();
            return (from p in productList
                    select new VmProduct
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CategoryId = p.CategoryId,
                        Description = p.Description,
                        IsPackage = p.IsPackage,
                        QuantityUnitId = p.QuantityUnitId,
                        QuantityUnitName = p.QuantityUnitName,
                        QuantityDetail = p.QuantityDetail,

                        ProductionDate = p.ProductionDate ?? DateTime.Now,
                        StringProductionDate = (p.ProductionDate == null) ? DateTime.Now.ToShortDateString() : p.ProductionDate.Value.ToShortDateString(),
                        PersianProductionDate = AppClassLibrary.AppToolBox.GetJalaliDateText(p.ProductionDate),

                        Date = p.Date,
                        StringDate = p.Date.ToShortDateString(),
                        PersianDate = AppClassLibrary.AppToolBox.GetJalaliDateText(p.Date),

                        CategoryName = p.CategoryName
                    }).ToArray();
        }
        public VmProduct GetProductById(int id)
        {
            var product = productRepository.GetProductById(id);
            BLCategory bLCategory = new BLCategory(CurrentLanguageId);

            var category = bLCategory.GetCategoriesWithFieldsById(product.CategoryId);

            VmProduct vmProduct = new VmProduct
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                PersianProductionDate = AppToolBox.GetJalaliDateText(DateTime.Now.Date),
                PersianDate = AppToolBox.GetJalaliDateText(DateTime.Now.Date),
                Date = product.Date,
                ProductionDate = product.ProductionDate,
                Description = product.Description,
                IsPackage = product.IsPackage,
                CategoryFieldDetailList = (from c in product.ProductCategoryFields
                                           select new VmCategoryFieldDetail
                                           {
                                               Id = c.CategoryFieldId,
                                               CategoryId = product.CategoryId,
                                               Value = c.Value,


                                           }).ToList(),
            };
            foreach (var item in vmProduct.CategoryFieldDetailList)
            {
                var result = category.CategoryFieldDetailList.First(c => c.Id == item.Id);
                item.Priority = result.Priority;
                item.Name = result.Name;

            }
            return vmProduct;
        }
        public int AddProduct(VmProduct product)
        {
            if (product.CurrentCultureName == "fa-IR")
            {
                product.ProductionDate = AppToolBox.GetDateFromJalaliText(product.PersianProductionDate);
            }

            var newProduct = new Product()
            {
                CategoryId = product.CategoryId,
                Name = product.Name,
                Date = DateTime.Now.Date,
                Description = product.Description,
                IsPackage = product.IsPackage,
                QuantityUnitId = product.QuantityUnitId,
                ProductionDate = product.ProductionDate,
                ProductCategoryFields = JsonConvert.DeserializeObject<List<ProductCategoryField>>(product.ClientProductCategoryFieldList)
            };

            productRepository.AddProduct(newProduct);

            if (UnitOfWork.Commit() == true)
            {

                BLProductFeature bLProductFeature = new BLProductFeature(CurrentLanguageId);

                bLProductFeature.GenerateProductFeatureTree(newProduct.Id, product.CategoryId);
            }
            return newProduct.Id;

        }
        public bool UpdateProduct(VmProduct product)
        {
            try
            {
                if (product.CurrentCultureName == "fa-IR")
                {
                    product.ProductionDate = AppToolBox.GetDateFromJalaliText(product.PersianProductionDate);
                }

                var updatedProduct = new Product()
                {
                    Id = product.Id,
                    CategoryId = product.CategoryId,
                    Name = product.Name,
                    Description = product.Description,
                    IsPackage = product.IsPackage,
                    QuantityUnitId = product.QuantityUnitId,
                    ProductionDate = product.ProductionDate,
                    ProductCategoryFields = JsonConvert.DeserializeObject<List<ProductCategoryField>>(product.ClientProductCategoryFieldList)
                };
                productCategoryFieldRepository.DeleteProductCategoryFieldByProduct(product.Id);
                productRepository.UpdateProduct(updatedProduct);
                UnitOfWork.Commit();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public bool DeleteProduct(int id, int categoryId)
        {
            bool result = true;
            try
            {
                var imageRepository = UnitOfWork.GetRepository<ImageRepository>();

                var productFeatureIds = imageRepository.DeleteImageByProductFeature(id);

                productRepository.DeleteProduct(id);

                if (UnitOfWork.Commit() == true)
                {

                    foreach (var productFeatureId in productFeatureIds)
                    {
                        UIHelper.DeleteFilesByPattern(
                            "/Resources/Uploaded/Product/" + categoryId + "/",
                            "*_" + productFeatureId.ToString().Replace("-", "_") + "_PID_*");
                    }

                }
            }
            catch
            {
                result = false;
            }

            return result;

        }
    }
}