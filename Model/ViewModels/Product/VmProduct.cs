
using Model.Base;
using Model.ViewModels.CategoryField;

using System;
using System.Collections.Generic;

namespace Model.ViewModels.Product
{
    public class VmProduct : BaseViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public List<VmCategoryFieldDetail> CategoryFieldDetailList { get; set; }
        public string ClientProductCategoryFieldList { get; set; }

        public bool IsPackage { get; set; }
        public string Name { get; set; }
        public int QuantityUnitId { get; set; }
        public string QuantityUnitName { get; set; }
        public int? QuantityDetail { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string StringDate { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string StringProductionDate { get; set; }
        public string PersianProductionDate { get; set; }
        public string CategoryName { get; set; }
        public string PersianDate { get; set; }
        public int[] FeatureTypeIds { get; set; }
        public string CategoryFieldNames { get; set; }
        public string CategoryFieldIds { get; set; }
        public string CategoryFieldValues { get; set; }
    }
}
