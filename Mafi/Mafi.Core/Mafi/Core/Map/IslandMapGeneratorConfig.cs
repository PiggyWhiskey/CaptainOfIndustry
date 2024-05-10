// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IslandMapGeneratorConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public sealed class IslandMapGeneratorConfig : IConfig
  {
    private bool m_islandMapAlreadyGenerated;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type IslandMapGeneratorType { get; private set; }

    public IslandMapGeneratorConfig(Type islandMapGenType)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetIslandMapGeneratorType(islandMapGenType);
    }

    public void SetIslandMapGeneratorType<T>() where T : IIslandMapGenerator
    {
      Assert.That<bool>(this.m_islandMapAlreadyGenerated).IsFalse("Island map was already generated.");
      this.IslandMapGeneratorType = typeof (T);
    }

    public void SetIslandMapGeneratorType(Type islandMapGenType)
    {
      Assert.That<bool>(this.m_islandMapAlreadyGenerated).IsFalse("Island map was already generated.");
      Assert.That<Type>(islandMapGenType).IsAssignableTo<IIslandMapGenerator>("Invalid map generator type.");
      this.IslandMapGeneratorType = islandMapGenType;
    }

    public void MarkIslandMapAlreadyGenerated() => this.m_islandMapAlreadyGenerated = true;

    public static void Serialize(IslandMapGeneratorConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IslandMapGeneratorConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IslandMapGeneratorConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<Type>(this.IslandMapGeneratorType);
      writer.WriteBool(this.m_islandMapAlreadyGenerated);
    }

    public static IslandMapGeneratorConfig Deserialize(BlobReader reader)
    {
      IslandMapGeneratorConfig mapGeneratorConfig;
      if (reader.TryStartClassDeserialization<IslandMapGeneratorConfig>(out mapGeneratorConfig))
        reader.EnqueueDataDeserialization((object) mapGeneratorConfig, IslandMapGeneratorConfig.s_deserializeDataDelayedAction);
      return mapGeneratorConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.IslandMapGeneratorType = reader.ReadGenericAs<Type>();
      this.m_islandMapAlreadyGenerated = reader.ReadBool();
    }

    static IslandMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IslandMapGeneratorConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IslandMapGeneratorConfig) obj).SerializeData(writer));
      IslandMapGeneratorConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IslandMapGeneratorConfig) obj).DeserializeData(reader));
    }
  }
}
