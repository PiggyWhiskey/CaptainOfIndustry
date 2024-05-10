﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.RemoveManualPlantTreeCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GenerateSerializer(false, null, 0)]
  public class RemoveManualPlantTreeCmd : InputCommand<bool>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly TreeId Id;

    public static void Serialize(RemoveManualPlantTreeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveManualPlantTreeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveManualPlantTreeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TreeId.Serialize(this.Id, writer);
    }

    public static RemoveManualPlantTreeCmd Deserialize(BlobReader reader)
    {
      RemoveManualPlantTreeCmd manualPlantTreeCmd;
      if (reader.TryStartClassDeserialization<RemoveManualPlantTreeCmd>(out manualPlantTreeCmd))
        reader.EnqueueDataDeserialization((object) manualPlantTreeCmd, RemoveManualPlantTreeCmd.s_deserializeDataDelayedAction);
      return manualPlantTreeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveManualPlantTreeCmd>(this, "Id", (object) TreeId.Deserialize(reader));
    }

    public RemoveManualPlantTreeCmd(TreeId treeId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = treeId;
    }

    static RemoveManualPlantTreeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveManualPlantTreeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RemoveManualPlantTreeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
