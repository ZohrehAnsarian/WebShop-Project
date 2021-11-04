namespace Model.ViewModels.BaseFeatureType
{
    public class VmBaseFeatureTypeDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseFeatureTypeId { get; set; }
        public bool Deletable { get; set; }
        public string RowState { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
    }
}
