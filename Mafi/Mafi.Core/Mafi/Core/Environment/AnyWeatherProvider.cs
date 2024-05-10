// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.AnyWeatherProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Environment
{
  [GenerateSerializer(false, null, 0)]
  public class AnyWeatherProvider : IWeatherProvider
  {
    private readonly ImmutableArray<WeatherProto> m_yearlyWeather;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AnyWeatherProvider(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      WeatherProto[] array = protosDb.All<WeatherProto>().ToArray<WeatherProto>();
      ImmutableArrayBuilder<WeatherProto> immutableArrayBuilder = new ImmutableArrayBuilder<WeatherProto>(24);
      for (int i = 0; i < 24; ++i)
        immutableArrayBuilder[i] = array[i % array.Length];
      this.m_yearlyWeather = immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public ImmutableArray<WeatherProto> GetWeatherForYear(
      int year,
      Option<WeatherProto> previousMonthWeather)
    {
      return this.m_yearlyWeather;
    }

    public static void Serialize(AnyWeatherProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnyWeatherProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnyWeatherProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<WeatherProto>.Serialize(this.m_yearlyWeather, writer);
    }

    public static AnyWeatherProvider Deserialize(BlobReader reader)
    {
      AnyWeatherProvider anyWeatherProvider;
      if (reader.TryStartClassDeserialization<AnyWeatherProvider>(out anyWeatherProvider))
        reader.EnqueueDataDeserialization((object) anyWeatherProvider, AnyWeatherProvider.s_deserializeDataDelayedAction);
      return anyWeatherProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<AnyWeatherProvider>(this, "m_yearlyWeather", (object) ImmutableArray<WeatherProto>.Deserialize(reader));
    }

    static AnyWeatherProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnyWeatherProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AnyWeatherProvider) obj).SerializeData(writer));
      AnyWeatherProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AnyWeatherProvider) obj).DeserializeData(reader));
    }
  }
}
