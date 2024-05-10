// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.TerraformerDepositCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Input
{
  [GenerateSerializer(false, null, 0)]
  [Serializable]
  public class TerraformerDepositCmd : InputCommand
  {
    public readonly Proto.ID MaterialId;
    public readonly TilesRectSelection Selection;
    public readonly HeightTilesF Height;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public TerraformerDepositCmd(
      TerrainMaterialProto material,
      TilesRectSelection selection,
      HeightTilesF targetHeight)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaterialId = material.Id;
      this.Selection = selection;
      this.Height = targetHeight;
    }

    public static void Serialize(TerraformerDepositCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerraformerDepositCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerraformerDepositCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      HeightTilesF.Serialize(this.Height, writer);
      Proto.ID.Serialize(this.MaterialId, writer);
      TilesRectSelection.Serialize(this.Selection, writer);
    }

    public static TerraformerDepositCmd Deserialize(BlobReader reader)
    {
      TerraformerDepositCmd terraformerDepositCmd;
      if (reader.TryStartClassDeserialization<TerraformerDepositCmd>(out terraformerDepositCmd))
        reader.EnqueueDataDeserialization((object) terraformerDepositCmd, TerraformerDepositCmd.s_deserializeDataDelayedAction);
      return terraformerDepositCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<TerraformerDepositCmd>(this, "Height", (object) HeightTilesF.Deserialize(reader));
      reader.SetField<TerraformerDepositCmd>(this, "MaterialId", (object) Proto.ID.Deserialize(reader));
      reader.SetField<TerraformerDepositCmd>(this, "Selection", (object) TilesRectSelection.Deserialize(reader));
    }

    static TerraformerDepositCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerraformerDepositCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      TerraformerDepositCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
