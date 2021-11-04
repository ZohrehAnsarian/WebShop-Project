namespace Model.ViewModels.FeatureType
{
    public class VmFeatureTypeDetail
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public int FeatureTypeId { get; set; }
        public int? BaseFeatureTypeDetailId { get; set; }
        public bool Deletable { get; set; }
        public string RowState { get; set; }
        public bool IsLeaf { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public string Checked { get; set; }
    }
}
