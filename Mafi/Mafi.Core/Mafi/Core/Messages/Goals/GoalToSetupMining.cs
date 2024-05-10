// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToSetupMining
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToSetupMining : Goal
  {
    private readonly GoalToSetupMining.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private readonly TerrainManager m_terrainManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToSetupMining(
      GoalToSetupMining.Proto goalProto,
      EntitiesManager entitiesManager,
      TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.m_terrainManager = terrainManager;
      this.Title = goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      TerrainManager terrainManager = this.m_terrainManager;
      foreach (MineTower mineTower in this.m_entitiesManager.GetAllEntitiesOfType<MineTower>((Predicate<MineTower>) (x => x.AssignedExcavatorsTotal >= 1 && x.AssignedTrucksTotal >= 2)))
      {
        int num = 0;
        foreach (TerrainDesignation managedDesignation in (IEnumerable<TerrainDesignation>) mineTower.ManagedDesignations)
        {
          ++num;
          if (num % 2 == 0)
          {
            TileMaterialLayers layersRawData = terrainManager.GetLayersRawData(terrainManager.GetTileIndex(managedDesignation.CenterTileCoord));
            if (layersRawData.Count >= 1)
            {
              TerrainMaterialSlimId slimId = layersRawData.First.SlimId;
              if ((Mafi.Core.Prototypes.Proto) slimId.ToFull(terrainManager).MinedProduct == (Mafi.Core.Prototypes.Proto) this.m_goalProto.LooseProductProto)
                return true;
              if (layersRawData.Count >= 2 && !(layersRawData.First.Thickness >= ThicknessTilesF.One))
              {
                slimId = layersRawData.Second.SlimId;
                if ((Mafi.Core.Prototypes.Proto) slimId.ToFull(terrainManager).MinedProduct == (Mafi.Core.Prototypes.Proto) this.m_goalProto.LooseProductProto)
                  return true;
                if (layersRawData.Count >= 3 && !(layersRawData.First.Thickness + layersRawData.Second.Thickness >= ThicknessTilesF.One))
                {
                  slimId = layersRawData.Third.SlimId;
                  if ((Mafi.Core.Prototypes.Proto) slimId.ToFull(terrainManager).MinedProduct == (Mafi.Core.Prototypes.Proto) this.m_goalProto.LooseProductProto)
                    return true;
                }
              }
            }
          }
        }
      }
      return false;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToSetupMining value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToSetupMining>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToSetupMining.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToSetupMining.Proto>(this.m_goalProto);
      TerrainManager.Serialize(this.m_terrainManager, writer);
    }

    public static GoalToSetupMining Deserialize(BlobReader reader)
    {
      GoalToSetupMining goalToSetupMining;
      if (reader.TryStartClassDeserialization<GoalToSetupMining>(out goalToSetupMining))
        reader.EnqueueDataDeserialization((object) goalToSetupMining, GoalToSetupMining.s_deserializeDataDelayedAction);
      return goalToSetupMining;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToSetupMining>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToSetupMining>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToSetupMining.Proto>());
      reader.SetField<GoalToSetupMining>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
    }

    static GoalToSetupMining()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToSetupMining.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToSetupMining.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      private static readonly LocStr4 TITLE_FOR_TOWER;
      public readonly LooseProductProto LooseProductProto;
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToSetupMining);

      public Proto(
        string id,
        LooseProductProto looseProduct,
        MineTowerProto towerProto,
        ExcavatorProto excavatorProto,
        TruckProto truckProto,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex);
        this.LooseProductProto = looseProduct;
        this.Title = GoalToSetupMining.Proto.TITLE_FOR_TOWER.Format(string.Format("<bc>{0}</bc>", (object) towerProto.Strings.Name), string.Format("<bc>{0}</bc>", (object) looseProduct.Strings.Name), string.Format("<bc>{0}</bc>", (object) excavatorProto.Strings.Name), string.Format("<bc>{0} (2x)</bc>", (object) truckProto.Strings.Name));
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToSetupMining.Proto.TITLE_FOR_TOWER = Loc.Str4("Goal__ForTower", "Build a {0} near {1} deposit, assign {2} and {3} to it and set up mining designations.", "text for a goal, example with replacements: 'Build a Mine control tower near iron ore deposit, assign Excavator and Pickup to it and set up mining designations.'");
      }
    }
  }
}
