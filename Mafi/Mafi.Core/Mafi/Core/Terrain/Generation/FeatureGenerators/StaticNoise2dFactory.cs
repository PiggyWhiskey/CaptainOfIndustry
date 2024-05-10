// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.FeatureGenerators.StaticNoise2dFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation.FeatureGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class StaticNoise2dFactory : INoise2dFactory
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly INoise2D Noise;

    public static void Serialize(StaticNoise2dFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticNoise2dFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticNoise2dFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.Noise);
    }

    public static StaticNoise2dFactory Deserialize(BlobReader reader)
    {
      StaticNoise2dFactory staticNoise2dFactory;
      if (reader.TryStartClassDeserialization<StaticNoise2dFactory>(out staticNoise2dFactory))
        reader.EnqueueDataDeserialization((object) staticNoise2dFactory, StaticNoise2dFactory.s_deserializeDataDelayedAction);
      return staticNoise2dFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StaticNoise2dFactory>(this, "Noise", (object) reader.ReadGenericAs<INoise2D>());
    }

    public StaticNoise2dFactory(INoise2D noise)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Noise = noise;
    }

    public bool TryCreateNoise(
      IResolver resolver,
      IReadOnlyDictionary<string, object> extraArgs,
      out INoise2D result,
      out string error)
    {
      result = this.Noise;
      error = "";
      return true;
    }

    static StaticNoise2dFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticNoise2dFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticNoise2dFactory) obj).SerializeData(writer));
      StaticNoise2dFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticNoise2dFactory) obj).DeserializeData(reader));
    }
  }
}
