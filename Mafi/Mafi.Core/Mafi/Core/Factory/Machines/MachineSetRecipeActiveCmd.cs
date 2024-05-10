// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.MachineSetRecipeActiveCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  [GenerateSerializer(false, null, 0)]
  public class MachineSetRecipeActiveCmd : InputCommand
  {
    public readonly EntityId MachineId;
    public readonly RecipeProto.ID RecipeId;
    public readonly bool EnableRecipe;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MachineSetRecipeActiveCmd(
      EntityId machineId,
      RecipeProto.ID recipeId,
      bool enableRecipe)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MachineId = machineId;
      this.RecipeId = recipeId;
      this.EnableRecipe = enableRecipe;
    }

    public static void Serialize(MachineSetRecipeActiveCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MachineSetRecipeActiveCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MachineSetRecipeActiveCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.EnableRecipe);
      EntityId.Serialize(this.MachineId, writer);
      RecipeProto.ID.Serialize(this.RecipeId, writer);
    }

    public static MachineSetRecipeActiveCmd Deserialize(BlobReader reader)
    {
      MachineSetRecipeActiveCmd setRecipeActiveCmd;
      if (reader.TryStartClassDeserialization<MachineSetRecipeActiveCmd>(out setRecipeActiveCmd))
        reader.EnqueueDataDeserialization((object) setRecipeActiveCmd, MachineSetRecipeActiveCmd.s_deserializeDataDelayedAction);
      return setRecipeActiveCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MachineSetRecipeActiveCmd>(this, "EnableRecipe", (object) reader.ReadBool());
      reader.SetField<MachineSetRecipeActiveCmd>(this, "MachineId", (object) EntityId.Deserialize(reader));
      reader.SetField<MachineSetRecipeActiveCmd>(this, "RecipeId", (object) RecipeProto.ID.Deserialize(reader));
    }

    static MachineSetRecipeActiveCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MachineSetRecipeActiveCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MachineSetRecipeActiveCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
