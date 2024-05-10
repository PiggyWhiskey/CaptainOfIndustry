// Decompiled with JetBrains decompiler
// Type: Mafi.Core.RandomSeedConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  [GenerateSerializer(false, null, 0)]
  public sealed class RandomSeedConfig : IConfig
  {
    public const string DEFAULT_SEED = "MaFi seed";
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Random seed for this game. This string's hash will be used to seed pseudo-random number generator.
    /// </summary>
    public string MasterRandomSeed { get; private set; }

    public RandomSeedConfig(string masterRandomSeed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MasterRandomSeed = masterRandomSeed;
    }

    public static void Serialize(RandomSeedConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RandomSeedConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RandomSeedConfig.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer) => writer.WriteString(this.MasterRandomSeed);

    public static RandomSeedConfig Deserialize(BlobReader reader)
    {
      RandomSeedConfig randomSeedConfig;
      if (reader.TryStartClassDeserialization<RandomSeedConfig>(out randomSeedConfig))
        reader.EnqueueDataDeserialization((object) randomSeedConfig, RandomSeedConfig.s_deserializeDataDelayedAction);
      return randomSeedConfig;
    }

    private void DeserializeData(BlobReader reader) => this.MasterRandomSeed = reader.ReadString();

    static RandomSeedConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RandomSeedConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RandomSeedConfig) obj).SerializeData(writer));
      RandomSeedConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RandomSeedConfig) obj).DeserializeData(reader));
    }
  }
}
