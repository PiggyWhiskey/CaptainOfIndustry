// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchStopCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  /// <summary>Command to stop the current research.</summary>
  [GenerateSerializer(false, null, 0)]
  public class ResearchStopCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ResearchStopCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public static void Serialize(ResearchStopCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchStopCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchStopCmd.s_serializeDataDelayedAction);
    }

    public static ResearchStopCmd Deserialize(BlobReader reader)
    {
      ResearchStopCmd researchStopCmd;
      if (reader.TryStartClassDeserialization<ResearchStopCmd>(out researchStopCmd))
        reader.EnqueueDataDeserialization((object) researchStopCmd, ResearchStopCmd.s_deserializeDataDelayedAction);
      return researchStopCmd;
    }

    static ResearchStopCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchStopCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ResearchStopCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
