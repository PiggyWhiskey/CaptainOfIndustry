// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.SortingPlantSetBlockedProductAlertCmd
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
  public class SortingPlantSetBlockedProductAlertCmd : InputCommand
  {
    public readonly EntityId SortingPlantId;
    public readonly ProductProto.ID ProductId;
    public readonly bool IsAlertEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SortingPlantSetBlockedProductAlertCmd(
      EntityId sortingPlantId,
      ProductProto.ID productId,
      bool isAlertEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SortingPlantId = sortingPlantId;
      this.ProductId = productId;
      this.IsAlertEnabled = isAlertEnabled;
    }

    public static void Serialize(SortingPlantSetBlockedProductAlertCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SortingPlantSetBlockedProductAlertCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SortingPlantSetBlockedProductAlertCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsAlertEnabled);
      ProductProto.ID.Serialize(this.ProductId, writer);
      EntityId.Serialize(this.SortingPlantId, writer);
    }

    public static SortingPlantSetBlockedProductAlertCmd Deserialize(BlobReader reader)
    {
      SortingPlantSetBlockedProductAlertCmd blockedProductAlertCmd;
      if (reader.TryStartClassDeserialization<SortingPlantSetBlockedProductAlertCmd>(out blockedProductAlertCmd))
        reader.EnqueueDataDeserialization((object) blockedProductAlertCmd, SortingPlantSetBlockedProductAlertCmd.s_deserializeDataDelayedAction);
      return blockedProductAlertCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SortingPlantSetBlockedProductAlertCmd>(this, "IsAlertEnabled", (object) reader.ReadBool());
      reader.SetField<SortingPlantSetBlockedProductAlertCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<SortingPlantSetBlockedProductAlertCmd>(this, "SortingPlantId", (object) EntityId.Deserialize(reader));
    }

    static SortingPlantSetBlockedProductAlertCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SortingPlantSetBlockedProductAlertCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SortingPlantSetBlockedProductAlertCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
