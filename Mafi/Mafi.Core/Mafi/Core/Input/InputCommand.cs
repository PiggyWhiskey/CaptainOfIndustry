// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.InputCommand
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Input
{
  [GenerateSerializer(false, null, 0)]
  public class InputCommand : InputCommand<bool>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool AffectsSaveState => true;

    public void SetResultSuccess() => this.SetResultSuccess(true);

    public void SetResultError(string errorMessage) => this.SetResultError(false, errorMessage);

    public static void Serialize(InputCommand value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<InputCommand>(value))
        return;
      writer.EnqueueDataSerialization((object) value, InputCommand.s_serializeDataDelayedAction);
    }

    public static InputCommand Deserialize(BlobReader reader)
    {
      InputCommand inputCommand;
      if (reader.TryStartClassDeserialization<InputCommand>(out inputCommand))
        reader.EnqueueDataDeserialization((object) inputCommand, InputCommand.s_deserializeDataDelayedAction);
      return inputCommand;
    }

    public InputCommand()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static InputCommand()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      InputCommand.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      InputCommand.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
