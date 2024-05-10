// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.SetProductPortCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.OreSorting
{
  [GenerateSerializer(false, null, 0)]
  public class SetProductPortCmd : InputCommand
  {
    public readonly EntityId SortingPlantId;
    public readonly ProductProto.ID ProductId;
    public readonly int PortIndex;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetProductPortCmd(
      OreSortingPlant sortingPlant,
      ProductProto productProto,
      int portIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(sortingPlant.Id, productProto.Id, portIndex);
    }

    private SetProductPortCmd(EntityId sortingPlantId, ProductProto.ID productId, int portIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SortingPlantId = sortingPlantId;
      this.ProductId = productId;
      this.PortIndex = portIndex;
    }

    public static void Serialize(SetProductPortCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetProductPortCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetProductPortCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.PortIndex);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.SortingPlantId, writer);
    }

    public static SetProductPortCmd Deserialize(BlobReader reader)
    {
      SetProductPortCmd setProductPortCmd;
      if (reader.TryStartClassDeserialization<SetProductPortCmd>(out setProductPortCmd))
        reader.EnqueueDataDeserialization((object) setProductPortCmd, SetProductPortCmd.s_deserializeDataDelayedAction);
      return setProductPortCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetProductPortCmd>(this, "PortIndex", (object) reader.ReadInt());
      reader.SetField<SetProductPortCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<SetProductPortCmd>(this, "SortingPlantId", (object) EntityId.Deserialize(reader));
    }

    static SetProductPortCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetProductPortCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetProductPortCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
