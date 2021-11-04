
using Model.Base;

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Model.ViewModels.CategoryField
{
    public class VmCategoryField : BaseViewModel
    {
        public int Id { get; set; }
        public List<VmCategoryFieldDetail> CategoryFieldDetailList { get; set; }
        public string CategoryFieldNames { get; set; }
        public string CategoryFieldPriorities { get; set; }
        public string CategoryFieldIds { get; set; }
        public string CategoryFieldDeletable { get; set; }
        public string JSONCategoryFieldDetail { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        
    }
}
