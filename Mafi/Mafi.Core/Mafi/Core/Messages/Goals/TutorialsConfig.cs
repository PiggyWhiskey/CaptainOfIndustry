// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.TutorialsConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class TutorialsConfig : IConfig
  {
    public readonly bool AreTutorialsEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static TutorialsConfig CreateConfig(bool areTutorialsEnabled)
    {
      return new TutorialsConfig(areTutorialsEnabled);
    }

    private TutorialsConfig(bool areTutorialsEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AreTutorialsEnabled = areTutorialsEnabled;
    }

    public static void Serialize(TutorialsConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TutorialsConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TutorialsConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AreTutorialsEnabled);
    }

    public static TutorialsConfig Deserialize(BlobReader reader)
    {
      TutorialsConfig tutorialsConfig;
      if (reader.TryStartClassDeserialization<TutorialsConfig>(out tutorialsConfig))
        reader.EnqueueDataDeserialization((object) tutorialsConfig, TutorialsConfig.s_deserializeDataDelayedAction);
      return tutorialsConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TutorialsConfig>(this, "AreTutorialsEnabled", (object) reader.ReadBool());
    }

    static TutorialsConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TutorialsConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TutorialsConfig) obj).SerializeData(writer));
      TutorialsConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TutorialsConfig) obj).DeserializeData(reader));
    }
  }
}
