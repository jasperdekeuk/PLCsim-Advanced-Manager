
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;

public class BasePortModel : Blazor.Diagrams.Core.Models.PortModel
{
    public string RefId { get; set; } = string.Empty;
    public BasePortModel(NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(parent, alignment, position, size)
    {
    }

    public BasePortModel(string id, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(id, parent, alignment, position, size)
    {
    }
}