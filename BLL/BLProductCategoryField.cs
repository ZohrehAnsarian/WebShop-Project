using BLL.Base;
using BLL.SystemTools;

using Model;

using Repository.EF.Repository;

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;
using Model.ViewModels.ProductCategoryField;

namespace BLL
{
    public class BLProductCategoryField : BLBase
    {
        ProductCategoryFieldRepository productCategoryFieldRepository;
        ViewProductCategoryFieldRepository viewProductCategoryFieldRepository;
        public BLProductCategoryField(int currentLanguageId) : base(currentLanguageId)
        {
            productCategoryFieldRepository = UnitOfWork.GetRepository<ProductCategoryFieldRepository>();
            viewProductCategoryFieldRepository = UnitOfWork.GetRepository<ViewProductCategoryFieldRepository>();
        }
        public int GetProductCategoryFieldCountByProduct(int id)
        {
            return productCategoryFieldRepository.GetProductCategoryFieldCountByProduct(id);
        }
        public IEnumerable<int> GetProductCategoryFieldIds(IEnumerable<int> categoryFieldIdList)
        {
            return productCategoryFieldRepository.GetProductCategoryFieldIds(categoryFieldIdList);
        }
    }
}