// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.PinToggleCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  [GenerateSerializer(false, null, 0)]
  public class PinToggleCmd : InputCommand
  {
    public readonly ProductProto.ID ProductToToggle;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PinToggleCmd(ProductProto.ID productToToggle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProductToToggle = productToToggle;
    }

    public static void Serialize(PinToggleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PinToggleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PinToggleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductToToggle, writer);
    }

    public static PinToggleCmd Deserialize(BlobReader reader)
    {
      PinToggleCmd pinToggleCmd;
      if (reader.TryStartClassDeserialization<PinToggleCmd>(out pinToggleCmd))
        reader.EnqueueDataDeserialization((object) pinToggleCmd, PinToggleCmd.s_deserializeDataDelayedAction);
      return pinToggleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<PinToggleCmd>(this, "ProductToToggle", (object) ProductProto.ID.Deserialize(reader));
    }

    static PinToggleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PinToggleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      PinToggleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
