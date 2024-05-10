// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToActivateRecipe
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToActivateRecipe : Goal
  {
    private readonly GoalToActivateRecipe.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private int m_currentCount;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToActivateRecipe(
      GoalToActivateRecipe.Proto goalProto,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int num = 0;
      foreach (KeyValuePair<MachineProto.ID, RecipeProto.ID> keyValuePair in this.m_goalProto.MachineRecipeToActivate)
      {
        KeyValuePair<MachineProto.ID, RecipeProto.ID> pair = keyValuePair;
        num += this.m_entitiesManager.GetCountOf<Machine>((Predicate<Machine>) (x => x.IsConstructed && x.Prototype.Id == pair.Key && x.RecipesAssigned.Any<RecipeProto>((Predicate<RecipeProto>) (r => r.Id == pair.Value))));
      }
      if (num != this.m_currentCount)
      {
        this.m_currentCount = num;
        this.updateTitle();
      }
      return this.m_currentCount >= this.m_goalProto.EntitiesCount;
    }

    private void updateTitle()
    {
      if (this.m_goalProto.EntitiesCount > 1)
        this.Title = this.m_goalProto.Title.Value + string.Format(" ({0} / {1}) ", (object) this.m_currentCount, (object) this.m_goalProto.EntitiesCount);
      else
        this.Title = this.m_goalProto.Title.Value;
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToActivateRecipe value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToActivateRecipe>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToActivateRecipe.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_currentCount);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToActivateRecipe.Proto>(this.m_goalProto);
    }

    public static GoalToActivateRecipe Deserialize(BlobReader reader)
    {
      GoalToActivateRecipe toActivateRecipe;
      if (reader.TryStartClassDeserialization<GoalToActivateRecipe>(out toActivateRecipe))
        reader.EnqueueDataDeserialization((object) toActivateRecipe, GoalToActivateRecipe.s_deserializeDataDelayedAction);
      return toActivateRecipe;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentCount = reader.ReadInt();
      reader.SetField<GoalToActivateRecipe>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToActivateRecipe>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToActivateRecipe.Proto>());
    }

    static GoalToActivateRecipe()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToActivateRecipe.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToActivateRecipe.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public ImmutableArray<KeyValuePair<MachineProto.ID, RecipeProto.ID>> MachineRecipeToActivate;
      public readonly LocStrFormatted Title;
      public readonly int EntitiesCount;

      public override Type Implementation => typeof (GoalToActivateRecipe);

      public Proto(
        string id,
        LocStrFormatted title,
        MachineProto.ID machineProtoId,
        RecipeProto.ID recipeProtoId,
        int entitiesCount = 1,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip, tutorialUnlock);
        this.Title = title;
        this.EntitiesCount = entitiesCount;
        this.MachineRecipeToActivate = ImmutableArray.Create<KeyValuePair<MachineProto.ID, RecipeProto.ID>>(Make.Kvp<MachineProto.ID, RecipeProto.ID>(machineProtoId, recipeProtoId));
      }

      public Proto(
        string id,
        LocStrFormatted title,
        KeyValuePair<MachineProto.ID, RecipeProto.ID> first,
        KeyValuePair<MachineProto.ID, RecipeProto.ID> second,
        int entitiesCount = 1,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip, tutorialUnlock);
        this.Title = title;
        this.EntitiesCount = entitiesCount;
        this.MachineRecipeToActivate = ImmutableArray.Create<KeyValuePair<MachineProto.ID, RecipeProto.ID>>(first, second);
      }
    }
  }
}
