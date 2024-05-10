// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.ReorderRecipeCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  [GenerateSerializer(false, null, 0)]
  public class ReorderRecipeCmd : InputCommand
  {
    public readonly EntityId MachineId;
    public readonly Proto.ID RecipeId;
    public readonly int IndexDiff;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ReorderRecipeCmd(EntityId machineId, Proto.ID recipeId, int indexDiff)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MachineId = machineId;
      this.RecipeId = recipeId;
      this.IndexDiff = indexDiff;
    }

    public static void Serialize(ReorderRecipeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ReorderRecipeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ReorderRecipeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.IndexDiff);
      EntityId.Serialize(this.MachineId, writer);
      Proto.ID.Serialize(this.RecipeId, writer);
    }

    public static ReorderRecipeCmd Deserialize(BlobReader reader)
    {
      ReorderRecipeCmd reorderRecipeCmd;
      if (reader.TryStartClassDeserialization<ReorderRecipeCmd>(out reorderRecipeCmd))
        reader.EnqueueDataDeserialization((object) reorderRecipeCmd, ReorderRecipeCmd.s_deserializeDataDelayedAction);
      return reorderRecipeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ReorderRecipeCmd>(this, "IndexDiff", (object) reader.ReadInt());
      reader.SetField<ReorderRecipeCmd>(this, "MachineId", (object) EntityId.Deserialize(reader));
      reader.SetField<ReorderRecipeCmd>(this, "RecipeId", (object) Proto.ID.Deserialize(reader));
    }

    static ReorderRecipeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ReorderRecipeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ReorderRecipeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
