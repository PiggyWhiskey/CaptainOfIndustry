// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.ExcavatorTogglePreferredProductCmd
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
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class ExcavatorTogglePreferredProductCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId ExcavatorId;
    public readonly ProductProto.ID? ProductToMine;

    public static void Serialize(ExcavatorTogglePreferredProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ExcavatorTogglePreferredProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ExcavatorTogglePreferredProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ExcavatorId, writer);
      writer.WriteNullableStruct<ProductProto.ID>(this.ProductToMine);
    }

    public static ExcavatorTogglePreferredProductCmd Deserialize(BlobReader reader)
    {
      ExcavatorTogglePreferredProductCmd preferredProductCmd;
      if (reader.TryStartClassDeserialization<ExcavatorTogglePreferredProductCmd>(out preferredProductCmd))
        reader.EnqueueDataDeserialization((object) preferredProductCmd, ExcavatorTogglePreferredProductCmd.s_deserializeDataDelayedAction);
      return preferredProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ExcavatorTogglePreferredProductCmd>(this, "ExcavatorId", (object) EntityId.Deserialize(reader));
      reader.SetField<ExcavatorTogglePreferredProductCmd>(this, "ProductToMine", (object) reader.ReadNullableStruct<ProductProto.ID>());
    }

    public ExcavatorTogglePreferredProductCmd(EntityId excavatorId, ProductProto.ID? productToMine)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ExcavatorId = excavatorId;
      this.ProductToMine = productToMine;
    }

    static ExcavatorTogglePreferredProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ExcavatorTogglePreferredProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ExcavatorTogglePreferredProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
