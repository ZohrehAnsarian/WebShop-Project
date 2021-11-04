using Model.ViewModels.Product;
using System.Collections;
using System.Collections.Generic;

namespace Model.ViewModels.Category
{
    public class VmCategoryProducts
    {
        public int CategoryId { get; set; }
        public IEnumerable<VmProduct> Products { get; set; }
 
    }
}
