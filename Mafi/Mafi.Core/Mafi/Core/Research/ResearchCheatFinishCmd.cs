// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchCheatFinishCmd
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
  /// <summary>Command to finish current research - cheat.</summary>
  [GenerateSerializer(false, null, 0)]
  public class ResearchCheatFinishCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ResearchCheatFinishCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public static void Serialize(ResearchCheatFinishCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchCheatFinishCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchCheatFinishCmd.s_serializeDataDelayedAction);
    }

    public static ResearchCheatFinishCmd Deserialize(BlobReader reader)
    {
      ResearchCheatFinishCmd researchCheatFinishCmd;
      if (reader.TryStartClassDeserialization<ResearchCheatFinishCmd>(out researchCheatFinishCmd))
        reader.EnqueueDataDeserialization((object) researchCheatFinishCmd, ResearchCheatFinishCmd.s_deserializeDataDelayedAction);
      return researchCheatFinishCmd;
    }

    static ResearchCheatFinishCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchCheatFinishCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ResearchCheatFinishCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
