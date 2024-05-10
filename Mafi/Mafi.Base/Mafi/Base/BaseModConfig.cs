// Decompiled with JetBrains decompiler
// Type: Mafi.Base.BaseModConfig
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class BaseModConfig : IModConfig, IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Whether a test factory will be built on the game start.
    /// </summary>
    public bool BuildStartingFactory { get; set; }

    [OnlyForSaveCompatibility(null)]
    public TerrainGenerator TerrainGenerator { get; set; }

    public string MapRandomSeed { get; set; }

    public bool DisableWorldMapGenerator { get; set; }

    public static void Serialize(BaseModConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BaseModConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BaseModConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.BuildStartingFactory);
      writer.WriteBool(this.DisableWorldMapGenerator);
      writer.WriteString(this.MapRandomSeed);
      writer.WriteInt((int) this.TerrainGenerator);
    }

    public static BaseModConfig Deserialize(BlobReader reader)
    {
      BaseModConfig baseModConfig;
      if (reader.TryStartClassDeserialization<BaseModConfig>(out baseModConfig))
        reader.EnqueueDataDeserialization((object) baseModConfig, BaseModConfig.s_deserializeDataDelayedAction);
      return baseModConfig;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.BuildStartingFactory = reader.ReadBool();
      this.DisableWorldMapGenerator = reader.ReadBool();
      this.MapRandomSeed = reader.ReadString();
      this.TerrainGenerator = (TerrainGenerator) reader.ReadInt();
    }

    public BaseModConfig()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBuildStartingFactory\u003Ek__BackingField = true;
      // ISSUE: reference to a compiler-generated field
      this.\u003CMapRandomSeed\u003Ek__BackingField = "MaFi random";
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BaseModConfig()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      BaseModConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BaseModConfig) obj).SerializeData(writer));
      BaseModConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BaseModConfig) obj).DeserializeData(reader));
    }
  }
}
