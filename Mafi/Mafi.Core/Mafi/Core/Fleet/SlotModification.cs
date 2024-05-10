// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.SlotModification
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Fleet
{
  [GenerateSerializer(false, null, 0)]
  public struct SlotModification
  {
    public readonly Proto.ID SlotId;
    public readonly FleetEntityPartProto.ID? Part;

    public SlotModification(Proto.ID slotId, FleetEntityPartProto.ID? part)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SlotId = slotId;
      this.Part = part;
    }

    public static void Serialize(SlotModification value, BlobWriter writer)
    {
      Proto.ID.Serialize(value.SlotId, writer);
      writer.WriteNullableStruct<FleetEntityPartProto.ID>(value.Part);
    }

    public static SlotModification Deserialize(BlobReader reader)
    {
      return new SlotModification(Proto.ID.Deserialize(reader), reader.ReadNullableStruct<FleetEntityPartProto.ID>());
    }
  }
}
