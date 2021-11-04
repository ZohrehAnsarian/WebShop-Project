using Model.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.ProductCategoryField
{
    public class VmProductCategoryField : BaseViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryFieldId { get; set; }
        public int CategoryFieldValue { get; set; }
        public int CategoryId { get; set; }
        public string CategoryFieldName { get; set; }
        public int? Priority { get; set; }
        public string CategoryName { get; set; }
        public int? LanguageId { get; set; }
        public string ProductName { get; set; }
    }
}
