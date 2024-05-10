// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.StartingLocationConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Logging;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public class StartingLocationConfig : IConfig, IMapStartInfoProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly int SetStartingLocationIndex;

    public static void Serialize(StartingLocationConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartingLocationConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartingLocationConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.SetStartingLocationIndex);
    }

    public static StartingLocationConfig Deserialize(BlobReader reader)
    {
      StartingLocationConfig startingLocationConfig;
      if (reader.TryStartClassDeserialization<StartingLocationConfig>(out startingLocationConfig))
        reader.EnqueueDataDeserialization((object) startingLocationConfig, StartingLocationConfig.s_deserializeDataDelayedAction);
      return startingLocationConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StartingLocationConfig>(this, "SetStartingLocationIndex", (object) reader.ReadInt());
    }

    public StartingLocationConfig(int setStartingLocationIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SetStartingLocationIndex = setStartingLocationIndex;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    int IMapStartInfoProvider.StartingLocationIndex => this.SetStartingLocationIndex;

    static StartingLocationConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StartingLocationConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartingLocationConfig) obj).SerializeData(writer));
      StartingLocationConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartingLocationConfig) obj).DeserializeData(reader));
    }
  }
}
