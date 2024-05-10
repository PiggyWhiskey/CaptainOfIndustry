// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.ClearRecipeProductsCmd
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
  public class ClearRecipeProductsCmd : InputCommand
  {
    public readonly EntityId MachineId;
    public readonly Proto.ID RecipeId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ClearRecipeProductsCmd(EntityId machineId, Proto.ID recipeId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MachineId = machineId;
      this.RecipeId = recipeId;
    }

    public static void Serialize(ClearRecipeProductsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ClearRecipeProductsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ClearRecipeProductsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.MachineId, writer);
      Proto.ID.Serialize(this.RecipeId, writer);
    }

    public static ClearRecipeProductsCmd Deserialize(BlobReader reader)
    {
      ClearRecipeProductsCmd recipeProductsCmd;
      if (reader.TryStartClassDeserialization<ClearRecipeProductsCmd>(out recipeProductsCmd))
        reader.EnqueueDataDeserialization((object) recipeProductsCmd, ClearRecipeProductsCmd.s_deserializeDataDelayedAction);
      return recipeProductsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ClearRecipeProductsCmd>(this, "MachineId", (object) EntityId.Deserialize(reader));
      reader.SetField<ClearRecipeProductsCmd>(this, "RecipeId", (object) Proto.ID.Deserialize(reader));
    }

    static ClearRecipeProductsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ClearRecipeProductsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ClearRecipeProductsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
