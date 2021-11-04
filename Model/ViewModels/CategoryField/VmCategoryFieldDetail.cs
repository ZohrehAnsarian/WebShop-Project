namespace Model.ViewModels.CategoryField
{
    public class VmCategoryFieldDetail
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int CategoryId { get; set; }
        public bool Deletable { get; set; }
        public string RowState { get; set; }
    }
}
