using System;

namespace Model.ViewModels.ProductImage
{
    public class VmImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Priority { get; set; }
        public string LinkUrl { get; set; }
        public Guid ProducFeaturetId { get; set; }
    }
}
