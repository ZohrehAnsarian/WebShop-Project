using System;

namespace Model.ToolsModels.Tree
{
    public interface ITmNode
    {
        int Id { get; set; }
        int? ParentId { get; set; }
        bool IsDefault { get; set; }
        string Name { get; set; }
        string AdditionalData { get; set; }
        string NodeLanguageId { get; set; }
    }
    public interface ITmNodeGuid
    {
        Guid Id { get; set; }
        Guid? ParentId { get; set; }
        bool IsDefault { get; set; }
        string Name { get; set; }
        string AdditionalData { get; set; }
        string NodeLanguageId { get; set; }
        string Description { get; set; }
    }
}
