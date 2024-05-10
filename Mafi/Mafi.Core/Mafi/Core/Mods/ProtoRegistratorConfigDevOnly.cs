// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.ProtoRegistratorConfigDevOnly
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Mods
{
  [GenerateSerializer(false, null, 0)]
  public sealed class ProtoRegistratorConfigDevOnly : IConfig
  {
    public readonly bool DisableAllProtoCosts;
    public readonly bool DisableVehicleFuelConsumption;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ProtoRegistratorConfigDevOnly(
      bool disableAllProtoCosts,
      bool disableVehicleFuelConsumption)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DisableAllProtoCosts = disableAllProtoCosts;
      this.DisableVehicleFuelConsumption = disableVehicleFuelConsumption;
    }

    public static void Serialize(ProtoRegistratorConfigDevOnly value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ProtoRegistratorConfigDevOnly>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ProtoRegistratorConfigDevOnly.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.DisableAllProtoCosts);
      writer.WriteBool(this.DisableVehicleFuelConsumption);
    }

    public static ProtoRegistratorConfigDevOnly Deserialize(BlobReader reader)
    {
      ProtoRegistratorConfigDevOnly registratorConfigDevOnly;
      if (reader.TryStartClassDeserialization<ProtoRegistratorConfigDevOnly>(out registratorConfigDevOnly))
        reader.EnqueueDataDeserialization((object) registratorConfigDevOnly, ProtoRegistratorConfigDevOnly.s_deserializeDataDelayedAction);
      return registratorConfigDevOnly;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<ProtoRegistratorConfigDevOnly>(this, "DisableAllProtoCosts", (object) reader.ReadBool());
      reader.SetField<ProtoRegistratorConfigDevOnly>(this, "DisableVehicleFuelConsumption", (object) reader.ReadBool());
    }

    static ProtoRegistratorConfigDevOnly()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProtoRegistratorConfigDevOnly.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProtoRegistratorConfigDevOnly) obj).SerializeData(writer));
      ProtoRegistratorConfigDevOnly.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProtoRegistratorConfigDevOnly) obj).DeserializeData(reader));
    }
  }
}
