// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ElectricityConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [OnlyForSaveCompatibility(null)]
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public sealed class ElectricityConfig : IConfig, IElectricityConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static ElectricityConfig CreateDefault() => new ElectricityConfig();

    private bool CanLogisticsWorkOnLowPower { get; set; }

    public ElectricityConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCanLogisticsWorkOnLowPower\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public static void Serialize(ElectricityConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ElectricityConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ElectricityConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.CanLogisticsWorkOnLowPower);
    }

    public static ElectricityConfig Deserialize(BlobReader reader)
    {
      ElectricityConfig electricityConfig;
      if (reader.TryStartClassDeserialization<ElectricityConfig>(out electricityConfig))
        reader.EnqueueDataDeserialization((object) electricityConfig, ElectricityConfig.s_deserializeDataDelayedAction);
      return electricityConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.CanLogisticsWorkOnLowPower = reader.ReadBool();
    }

    static ElectricityConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ElectricityConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ElectricityConfig) obj).SerializeData(writer));
      ElectricityConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ElectricityConfig) obj).DeserializeData(reader));
    }
  }
}
