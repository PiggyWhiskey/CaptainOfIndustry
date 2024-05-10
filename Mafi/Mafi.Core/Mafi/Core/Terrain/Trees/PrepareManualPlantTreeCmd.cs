// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.PrepareManualPlantTreeCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GenerateSerializer(false, null, 0)]
  public class PrepareManualPlantTreeCmd : InputCommand<bool>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Proto.ID ProtoId;
    public readonly TileTransform Transform;

    public static void Serialize(PrepareManualPlantTreeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PrepareManualPlantTreeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PrepareManualPlantTreeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Proto.ID.Serialize(this.ProtoId, writer);
      TileTransform.Serialize(this.Transform, writer);
    }

    public static PrepareManualPlantTreeCmd Deserialize(BlobReader reader)
    {
      PrepareManualPlantTreeCmd manualPlantTreeCmd;
      if (reader.TryStartClassDeserialization<PrepareManualPlantTreeCmd>(out manualPlantTreeCmd))
        reader.EnqueueDataDeserialization((object) manualPlantTreeCmd, PrepareManualPlantTreeCmd.s_deserializeDataDelayedAction);
      return manualPlantTreeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PrepareManualPlantTreeCmd>(this, "ProtoId", (object) Proto.ID.Deserialize(reader));
      reader.SetField<PrepareManualPlantTreeCmd>(this, "Transform", (object) TileTransform.Deserialize(reader));
    }

    public PrepareManualPlantTreeCmd(Proto.ID protoId, TileTransform transform)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.Transform = transform;
    }

    static PrepareManualPlantTreeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PrepareManualPlantTreeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      PrepareManualPlantTreeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
