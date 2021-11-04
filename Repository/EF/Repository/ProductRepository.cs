using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ProductRepository : EFBaseRepository<Product>
    {
        public IEnumerable<Product> GetAllProducts()
        {
            var productList = from p in Context.Products
                              orderby p.Name
                              select p;

            return productList;
        }
        public IEnumerable<ViewProduct> GetProductsByCategoryId(int categoryId, string searchText = "")
        {

            var productList = from p in Context.ViewProducts
                              where p.CategoryId == categoryId &&
                              (
                                 p.Name.Contains(searchText)
                              || p.Description.Contains(searchText)
                              )
                              orderby p.Name
                              select p;
            return productList;
        }
        public Product GetProductById(int id)
        {
            var product = Context.Products.Include(p => p.ProductCategoryFields).Where(p => p.Id == id);
            if (product.Count() > 0)
            {
                return product.First();
            }
            return null;
        }
        public int GetProductCountByCategoryId(int id)
        {
            return Context.Products.Where(p => p.CategoryId == id).Count();

        }
        public void AddProduct(Product product)
        {
            Add(product);
        }
        public void UpdateProduct(Product product)
        {
            var oldProduct = (from s in Context.Products where s.Id == product.Id select s).FirstOrDefault();

            oldProduct.Name = product.Name;
            oldProduct.CategoryId = product.CategoryId;
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.IsPackage = product.IsPackage;

            oldProduct.QuantityUnitId = product.QuantityUnitId;
            oldProduct.ProductionDate = product.ProductionDate;
            oldProduct.ProductCategoryFields = product.ProductCategoryFields;

            Update(oldProduct);
        }
        public bool DeleteProduct(int id)
        {
            var deleteable = Context.Products.Find(id);
            Delete(deleteable);
            return true;
        }
    }
}
