// Decompiled with JetBrains decompiler
// Type: Mafi.Core.RandomProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  /// <summary>Provides random pseudo-random generators.</summary>
  [GenerateSerializer(false, null, 0)]
  [DependencyRegisteredManually("")]
  public sealed class RandomProvider
  {
    /// <summary>
    /// Generator prototype that is used for cloning. Its state is never used.
    /// </summary>
    private readonly ICoreRandom m_randomPrototype;
    /// <summary>
    /// Master seed that will be part of seed for all created generators.
    /// </summary>
    public readonly string MasterSeed;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public RandomProvider(string masterSeed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_randomPrototype = (ICoreRandom) new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 42UL, 69UL);
      this.MasterSeed = masterSeed;
    }

    public RandomProvider(ICoreRandom randomPrototype, string masterSeed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_randomPrototype = randomPrototype.CheckNotNull<ICoreRandom>();
      this.MasterSeed = masterSeed;
    }

    /// <summary>
    /// Returns new random generator that should be used only on sim thread (or during initialization) and does
    /// affect game state. Asserts are in place to verify this. Returned random generator is seeded by master seed
    /// and type of given object. Thus, multiple requests with the same object type will result in generators with
    /// identical state.
    /// </summary>
    public IRandom GetSimRandomFor(object obj, string extraSeed = "")
    {
      return this.getRandomFor(obj, extraSeed, RandomGeneratorType.SimOnly);
    }

    /// <summary>
    /// Returns new random generator that should be used outside of sim thread and should not affect game state.
    /// Asserts are in place to verify this. Returned random generator is seeded by master seed and type of given
    /// object. Thus, multiple requests with the same object type will result in generators with identical state.
    /// </summary>
    public IRandom GetNonSimRandomFor(object obj, string extraSeed = "")
    {
      return this.getRandomFor(obj, extraSeed, RandomGeneratorType.NonSim);
    }

    private IRandom getRandomFor(object obj, string extraSeed, RandomGeneratorType generatorType)
    {
      IRandom random = (IRandom) this.m_randomPrototype.Clone(generatorType);
      random.Seed(this.MasterSeed + extraSeed + obj.GetType().Name);
      for (int index = 0; index < 100; ++index)
      {
        long num = (long) random.NextUlong();
      }
      return random;
    }

    public static void Serialize(RandomProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RandomProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RandomProvider.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICoreRandom>(this.m_randomPrototype);
      writer.WriteString(this.MasterSeed);
    }

    public static RandomProvider Deserialize(BlobReader reader)
    {
      RandomProvider randomProvider;
      if (reader.TryStartClassDeserialization<RandomProvider>(out randomProvider))
        reader.EnqueueDataDeserialization((object) randomProvider, RandomProvider.s_deserializeDataDelayedAction);
      return randomProvider;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<RandomProvider>(this, "m_randomPrototype", (object) reader.ReadGenericAs<ICoreRandom>());
      reader.SetField<RandomProvider>(this, "MasterSeed", (object) reader.ReadString());
    }

    static RandomProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RandomProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RandomProvider) obj).SerializeData(writer));
      RandomProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RandomProvider) obj).DeserializeData(reader));
    }
  }
}
