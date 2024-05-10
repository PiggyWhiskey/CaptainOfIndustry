// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.TerraformerRemoveCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Input
{
  [GenerateSerializer(false, null, 0)]
  [Serializable]
  public class TerraformerRemoveCmd : InputCommand
  {
    public readonly TilesRectSelection Selection;
    public readonly HeightTilesF TargetHeight;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public TerraformerRemoveCmd(TilesRectSelection selection, HeightTilesF targetHeight)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Selection = selection;
      this.TargetHeight = targetHeight;
    }

    public static void Serialize(TerraformerRemoveCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerraformerRemoveCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerraformerRemoveCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TilesRectSelection.Serialize(this.Selection, writer);
      HeightTilesF.Serialize(this.TargetHeight, writer);
    }

    public static TerraformerRemoveCmd Deserialize(BlobReader reader)
    {
      TerraformerRemoveCmd terraformerRemoveCmd;
      if (reader.TryStartClassDeserialization<TerraformerRemoveCmd>(out terraformerRemoveCmd))
        reader.EnqueueDataDeserialization((object) terraformerRemoveCmd, TerraformerRemoveCmd.s_deserializeDataDelayedAction);
      return terraformerRemoveCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<TerraformerRemoveCmd>(this, "Selection", (object) TilesRectSelection.Deserialize(reader));
      reader.SetField<TerraformerRemoveCmd>(this, "TargetHeight", (object) HeightTilesF.Deserialize(reader));
    }

    static TerraformerRemoveCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerraformerRemoveCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      TerraformerRemoveCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
