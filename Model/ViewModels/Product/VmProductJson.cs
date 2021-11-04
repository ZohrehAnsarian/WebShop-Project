using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.Product
{
    public class VmProductJson
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool IsPackage { get; set; }
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public short QuantityUnitId { get; set; }
        public string QuantityUnitName { get; set; }
        public int? QuantityDetail { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string PersianProductionDate { get; set; }
    }
}
