// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchStartCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  /// <summary>
  /// Command to start a research of the given node from the research tree.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class ResearchStartCmd : InputCommand
  {
    public readonly ResearchNodeProto.ID NodeId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ResearchStartCmd(ResearchNodeProto node)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(node.Id);
    }

    public ResearchStartCmd(ResearchNodeProto.ID nodeId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.NodeId = nodeId;
    }

    public static void Serialize(ResearchStartCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchStartCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchStartCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ResearchNodeProto.ID.Serialize(this.NodeId, writer);
    }

    public static ResearchStartCmd Deserialize(BlobReader reader)
    {
      ResearchStartCmd researchStartCmd;
      if (reader.TryStartClassDeserialization<ResearchStartCmd>(out researchStartCmd))
        reader.EnqueueDataDeserialization((object) researchStartCmd, ResearchStartCmd.s_deserializeDataDelayedAction);
      return researchStartCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ResearchStartCmd>(this, "NodeId", (object) ResearchNodeProto.ID.Deserialize(reader));
    }

    static ResearchStartCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchStartCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ResearchStartCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
