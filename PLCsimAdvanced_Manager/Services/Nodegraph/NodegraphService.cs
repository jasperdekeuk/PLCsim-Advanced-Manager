using System.Text.Json;
using System.Text.Json.Serialization;
using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using MatBlazor;
using PLCsimAdvanced_Manager.Services.Nodegraph;
using PLCsimAdvanced_Manager.Services.Nodegraph.BasicLib.BoolInverse;
using PLCsimAdvanced_Manager.Services.Nodegraph.InputNode;
using PLCsimAdvanced_Manager.Services.Nodegraph.OutputNode;
using PLCsimAdvanced_Manager.Services.Nodegraph.PortModel;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Services.Nodegraph;

public class NodegraphService
{
    public BlazorDiagram Diagram { get; set; } = new BlazorDiagram(new BlazorDiagramOptions()
    {
        AllowMultiSelection = true,
        Zoom =
        {
            Enabled = true,
        },
        Links =
        {
            DefaultRouter = new NormalRouter(),
            DefaultPathGenerator = new SmoothPathGenerator()
        },
    });

    public IInstance SelectedInstance;
    private List<BaseNodeModel> sorted = new List<BaseNodeModel>();
    private HashSet<string> visited = new HashSet<string>();
    private SDataValueByName[] inputnodes;
    private SDataValueByName[] outputnodes;
    private Timer _timer;
    public event EventHandler OnSimulationStarted;
    public event EventHandler OnSimulationStopped;
    public bool IsSimulationRunning;


    public NodegraphService(string plcInstanceName)
    {
        Diagram.RegisterComponent<InputNodeModel<bool>, InputNodeWidget<bool>>();
        Diagram.RegisterComponent<InputNodeModel<byte>, InputNodeWidget<byte>>();
        Diagram.RegisterComponent<InputNodeModel<short>, InputNodeWidget<short>>();
        Diagram.RegisterComponent<InputNodeModel<int>, InputNodeWidget<int>>();
        Diagram.RegisterComponent<InputNodeModel<long>, InputNodeWidget<long>>();
        Diagram.RegisterComponent<InputNodeModel<ushort>, InputNodeWidget<ushort>>();
        Diagram.RegisterComponent<InputNodeModel<uint>, InputNodeWidget<uint>>();
        Diagram.RegisterComponent<InputNodeModel<ulong>, InputNodeWidget<ulong>>();
        Diagram.RegisterComponent<InputNodeModel<float>, InputNodeWidget<float>>();
        Diagram.RegisterComponent<InputNodeModel<double>, InputNodeWidget<double>>();

        Diagram.RegisterComponent<OutputNodeModel<bool>, OutputNodeWidget<bool>>();
        Diagram.RegisterComponent<OutputNodeModel<byte>, OutputNodeWidget<byte>>();
        Diagram.RegisterComponent<OutputNodeModel<short>, OutputNodeWidget<short>>();
        Diagram.RegisterComponent<OutputNodeModel<int>, OutputNodeWidget<int>>();
        Diagram.RegisterComponent<OutputNodeModel<long>, OutputNodeWidget<long>>();
        Diagram.RegisterComponent<OutputNodeModel<ushort>, OutputNodeWidget<ushort>>();
        Diagram.RegisterComponent<OutputNodeModel<uint>, OutputNodeWidget<uint>>();
        Diagram.RegisterComponent<OutputNodeModel<ulong>, OutputNodeWidget<ulong>>();
        Diagram.RegisterComponent<OutputNodeModel<float>, OutputNodeWidget<float>>();
        Diagram.RegisterComponent<OutputNodeModel<double>, OutputNodeWidget<double>>();

        Diagram.RegisterComponent<BoolInverseNodeModel, BoolInverseNodeWidget>();

        SelectedInstance = SimulationRuntimeManager.CreateInterface(plcInstanceName);

        _timer = new Timer(_ => GraphExecution(), null, Timeout.Infinite, 10);
    }

    private void graphCompilation()
    {
        // remove nodes that have no links
        Diagram.Nodes.Where(e => !e.PortLinks.Any()).ToList().ForEach(e => Diagram.Nodes.Remove(e));

        // Get PLC output~nodegraph input nodes
        var nodegraphInputNodes = Diagram.Nodes
            .Where(node => node is InputNodeModel)
            .ToList();
        var outputDataValueByName = SelectedInstance.TagInfos
            .Where(e => (e.Area == EArea.Output || e.Area == EArea.DataBlock) &&
                        e.PrimitiveDataType != EPrimitiveDataType.Struct)
            .Select(e => new SDataValueByName
                { Name = e.Name, DataValue = new SDataValue { Type = e.PrimitiveDataType } })
            .ToArray();

        inputnodes = nodegraphInputNodes
            .Select(node => outputDataValueByName.FirstOrDefault(data => data.Name == node.Title))
            .ToArray();

        // PLC input~nodegraph output nodes
        var nodegraphOutputNodes = Diagram.Nodes
            .Where(node => node is OutputNodeModel)
            .ToList();
        var inputDataValueByName = SelectedInstance.TagInfos
            .Where(e => (e.Area == EArea.Input || e.Area == EArea.DataBlock) &&
                        e.PrimitiveDataType != EPrimitiveDataType.Struct)
            .Select(e => new SDataValueByName
                { Name = e.Name, DataValue = new SDataValue { Type = e.PrimitiveDataType } })
            .ToArray();
        outputnodes = nodegraphOutputNodes
            .Select(node => inputDataValueByName.FirstOrDefault(data => data.Name == node.Title))
            .ToArray();

        foreach (var node in nodegraphInputNodes)
        {
            visited.Add(node.Id);
        }

        foreach (var node in Diagram.Nodes)
        {
            Visit(node as BaseNodeModel);
        }
    }

    private void Visit(BaseNodeModel node)
    {
        if (visited.Contains(node.Id))
            return;

        visited.Add(node.Id);
        var inputPorts = node.Ports.OfType<InputPortModel>().ToList();
        foreach (var dependency in inputPorts)
        {
            var sourceNode = Diagram.Links[0].Source as SinglePortAnchor;
            Visit(sourceNode.Port.Parent as BaseNodeModel);
        }

        sorted.Add(node);
    }

    private void GraphExecution()
    {
        // 1. read input nodes
        // 2. execute graph
        // 3. write output nodes

        SelectedInstance.ReadSignals(ref inputnodes);
        foreach (var inputnode in inputnodes)
        {
            var node = Diagram.Nodes.FirstOrDefault(e => e.Title == inputnode.Name);
            if (node is BaseNodeModel baseNode)
            {
                if (baseNode is InputNodeModel<bool> boolNode)
                {
                    boolNode.OutputPort.Value = inputnode.DataValue.Bool;
                }
                else if (baseNode is InputNodeModel<byte> byteNode)
                {
                    byteNode.OutputPort.Value = inputnode.DataValue.UInt8;
                }
                else if (baseNode is InputNodeModel<short> shortNode)
                {
                    shortNode.OutputPort.Value = inputnode.DataValue.Int16;
                }
                else if (baseNode is InputNodeModel<int> intNode)
                {
                    intNode.OutputPort.Value = inputnode.DataValue.Int32;
                }
                else if (baseNode is InputNodeModel<long> longNode)
                {
                    longNode.OutputPort.Value = inputnode.DataValue.Int64;
                }
                else if (baseNode is InputNodeModel<ushort> ushortNode)
                {
                    ushortNode.OutputPort.Value = inputnode.DataValue.UInt16;
                }
                else if (baseNode is InputNodeModel<uint> uintNode)
                {
                    uintNode.OutputPort.Value = inputnode.DataValue.UInt32;
                }
                else if (baseNode is InputNodeModel<ulong> ulongNode)
                {
                    ulongNode.OutputPort.Value = inputnode.DataValue.UInt64;
                }
                else if (baseNode is InputNodeModel<float> floatNode)
                {
                    floatNode.OutputPort.Value = inputnode.DataValue.Float;
                }
                else if (baseNode is InputNodeModel<double> doubleNode)
                {
                    doubleNode.OutputPort.Value = inputnode.DataValue.Double;
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported type: {baseNode.GetType()}");
                }
            }
        }

        foreach (var node in sorted)
        {
            var baseNode = node as BaseNodeModel;
            baseNode.Calculate();
            // node. calculate values
        }

        for (int i = 0; i < outputnodes.Length; i++)
        {
            var node = Diagram.Nodes.FirstOrDefault(e => e.Title == outputnodes[i].Name);
            if (node is BaseNodeModel baseNode)
            {
                if (baseNode is OutputNodeModel<bool> boolNode)
                {
                    outputnodes[i].DataValue.Bool = boolNode.Value;
                }
                else if (baseNode is OutputNodeModel<byte> byteNode)
                {
                    outputnodes[i].DataValue.UInt8 = byteNode.Value;
                }
                else if (baseNode is OutputNodeModel<short> shortNode)
                {
                    outputnodes[i].DataValue.Int16 = shortNode.Value;
                }
                else if (baseNode is OutputNodeModel<int> intNode)
                {
                    outputnodes[i].DataValue.Int32 = intNode.Value;
                }
                else if (baseNode is OutputNodeModel<long> longNode)
                {
                    outputnodes[i].DataValue.Int64 = longNode.Value;
                }
                else if (baseNode is OutputNodeModel<ushort> ushortNode)
                {
                    outputnodes[i].DataValue.UInt16 = ushortNode.Value;
                }
                else if (baseNode is OutputNodeModel<uint> uintNode)
                {
                    outputnodes[i].DataValue.UInt32 = uintNode.Value;
                }
                else if (baseNode is OutputNodeModel<ulong> ulongNode)
                {
                    outputnodes[i].DataValue.UInt64 = ulongNode.Value;
                }
                else if (baseNode is OutputNodeModel<float> floatNode)
                {
                    outputnodes[i].DataValue.Float = floatNode.Value;
                }
                else if (baseNode is OutputNodeModel<double> doubleNode)
                {
                    outputnodes[i].DataValue.Double = doubleNode.Value;
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported type: {baseNode.GetType()}");
                }
            }
        }

        SelectedInstance.WriteSignals(ref outputnodes);
    }

    private void LockDiagram()
    {
        Diagram.Nodes.ToList().ForEach(node => node.Locked = true);
        Diagram.Links.ToList().ForEach(link => link.Locked = true);
    }

    private void UnlockDiagram()
    {
        Diagram.Nodes.ToList().ForEach(node => node.Locked = false);
        Diagram.Links.ToList().ForEach(link => link.Locked = false);
    }

    public void ExecuteSimulation()
    {
        graphCompilation();
        if (Diagram.Nodes.Count == 0)
        {
            return;
        }


        LockDiagram();
        PlcStartSetup();
        _timer.Change(0, 100);
        IsSimulationRunning = true;
        OnSimulationStarted?.Invoke(this, EventArgs.Empty);
    }

    public void StopSimulation()
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        UnlockDiagram();
        IsSimulationRunning = false;
        OnSimulationStopped?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private async void PlcStartSetup()
    {
        if (SelectedInstance.OperatingState == EOperatingState.Off)
        {
            SelectedInstance.PowerOn();
            SelectedInstance.Run();
        }
        else if (SelectedInstance.OperatingState == EOperatingState.Stop)
        {
            SelectedInstance.Run();
        }
    }

    public string GraphToJson()
    {
        var serializableDiagram = new SerializableDiagram
        {
            Nodes = Diagram.Nodes.Select(ToSerializableNode).ToList(),
            Links = Diagram.Links.Select(ToSerializableLink).ToList()
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        return JsonSerializer.Serialize(serializableDiagram, options);
    }

    public void JsonToGraph(string json)
    {
        if(json is "" or null)
            return;
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        var serializableDiagram = JsonSerializer.Deserialize<SerializableDiagram>(json, options);
        if (serializableDiagram != null)
        {
            Diagram.Nodes.Clear();
            Diagram.Links.Clear();
            foreach (var node in serializableDiagram.Nodes)
            {
                var type = Type.GetType(node.Type);
                if (type == null)
                    continue;

                var instance = Activator.CreateInstance(type, node.Position) as NodeModel;
                if (instance == null)
                    continue;

                // instance.Id = node.Id;
                instance.Title = node.Title;
                for (int i = 0; i < instance.Ports.Count; i++)
                {
                    var port = instance.Ports[i] as BasePortModel;
                    if (port == null)
                        continue;
                    port.RefId = node.Ports[i].Id;
                }
                Diagram.Nodes.Add(instance);
            }

            foreach (var link in serializableDiagram.Links)
            {
                var parent = serializableDiagram.Nodes;
                var source = Diagram.Nodes.FirstOrDefault(e => e.Ports.Any(p => ((BasePortModel)p).RefId == link.SourceId));
                var target = Diagram.Nodes.FirstOrDefault(e => e.Ports.Any(p => ((BasePortModel)p).RefId == link.TargetId));
                if (source == null || target == null)
                    continue;

                Diagram.Links.Add(new LinkModel(source.Ports.First(p => ((BasePortModel)p).RefId == link.SourceId), target.Ports.First(p => ((BasePortModel)p).RefId == link.TargetId)));
            }
        }
    }

    public SerializedNode ToSerializableNode(NodeModel node)
    {
        return new SerializedNode
        {
            Id = node.Id,
            Type = node.GetType().FullName,
            Position = node.Position,
            Title = node.Title,
            Ports = node.Ports.Select(port => new SerializedPort
            {
                Id = port.Id,
                Type = port.GetType().FullName
            }).ToList()
        };
    }

    public SerializedLink ToSerializableLink(BaseLinkModel link)
    {
        return new SerializedLink
        {
            SourceId = ((SinglePortAnchor)link.Source).Port.Id,
            TargetId = ((SinglePortAnchor)link.Target).Port.Id
        };
    }
}

public class SerializedNode
{
    public string Id { get; set; }
    public string Type { get; set; }
    public Point Position { get; set; }
    public string Title { get; set; }
    public List<SerializedPort> Ports { get; set; }
}

public class SerializedPort
{
    public string Id { get; set; }
    public string Type { get; set; }
}

public class SerializedLink
{
    public string SourceId { get; set; }
    public string TargetId { get; set; }
}

public class SerializableDiagram
{
    public List<SerializedNode> Nodes { get; set; }
    public List<SerializedLink> Links { get; set; }
}