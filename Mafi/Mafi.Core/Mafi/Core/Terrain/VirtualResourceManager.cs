// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.VirtualResourceManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Resources;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class VirtualResourceManager : IVirtualResourceManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [NewInSaveVersion(141, null, null, null, null)]
    [OnlyForSaveCompatibility(null)]
    private readonly Option<IWorldRegionMap> m_worldRegionMap;
    [NewInSaveVersion(141, null, null, typeof (GameDifficultyConfig), null)]
    private readonly GameDifficultyConfig m_difficultyConfig;
    private ImmutableArray<IVirtualTerrainResource> m_virtualResources;
    /// <summary>
    /// Cache of IVirtualTerrainResource grouped by VirtualResourceProductProto.
    /// </summary>
    private Dict<VirtualResourceProductProto, ImmutableArray<IVirtualTerrainResource>> m_virtualResourcesMap;

    public static void Serialize(VirtualResourceManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VirtualResourceManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VirtualResourceManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      GameDifficultyConfig.Serialize(this.m_difficultyConfig, writer);
      ImmutableArray<IVirtualTerrainResource>.Serialize(this.m_virtualResources, writer);
      Dict<VirtualResourceProductProto, ImmutableArray<IVirtualTerrainResource>>.Serialize(this.m_virtualResourcesMap, writer);
      Option<IWorldRegionMap>.Serialize(this.m_worldRegionMap, writer);
    }

    public static VirtualResourceManager Deserialize(BlobReader reader)
    {
      VirtualResourceManager virtualResourceManager;
      if (reader.TryStartClassDeserialization<VirtualResourceManager>(out virtualResourceManager))
        reader.EnqueueDataDeserialization((object) virtualResourceManager, VirtualResourceManager.s_deserializeDataDelayedAction);
      return virtualResourceManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VirtualResourceManager>(this, "m_difficultyConfig", reader.LoadedSaveVersion >= 141 ? (object) GameDifficultyConfig.Deserialize(reader) : (object) (GameDifficultyConfig) null);
      if (reader.LoadedSaveVersion < 141)
        reader.RegisterResolvedMember<VirtualResourceManager>(this, "m_difficultyConfig", typeof (GameDifficultyConfig), true);
      this.m_virtualResources = ImmutableArray<IVirtualTerrainResource>.Deserialize(reader);
      this.m_virtualResourcesMap = Dict<VirtualResourceProductProto, ImmutableArray<IVirtualTerrainResource>>.Deserialize(reader);
      reader.SetField<VirtualResourceManager>(this, "m_worldRegionMap", (object) (reader.LoadedSaveVersion >= 141 ? Option<IWorldRegionMap>.Deserialize(reader) : new Option<IWorldRegionMap>()));
    }

    public VirtualResourceManager(
      IGameLoopEvents gameLoopEvents,
      IWorldRegionMap worldRegionMap,
      GameDifficultyConfig difficultyConfig)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_worldRegionMap = Option.Create<IWorldRegionMap>(worldRegionMap);
      this.m_difficultyConfig = difficultyConfig;
      gameLoopEvents.RegisterNewGameCreated((object) this, new Action(this.InitializeResources));
    }

    /// <remarks>
    /// Virtual resources field has to be initialized after the map is created, the is why it no longer is
    /// initialized in the constructor, because the constructor may be invoked before the map is initialized.
    /// </remarks>
    public void InitializeResources()
    {
      if (this.m_worldRegionMap.IsNone)
      {
        Log.Warning("m_worldRegionMap not set, this should not happen!");
      }
      else
      {
        this.m_virtualResources = this.m_worldRegionMap.Value.VirtualResourcesGenerators.AsEnumerable().Where<IVirtualTerrainResourceGenerator>((Func<IVirtualTerrainResourceGenerator, bool>) (x => !x.IsDisabled)).SelectMany<IVirtualTerrainResourceGenerator, IVirtualTerrainResource>((Func<IVirtualTerrainResourceGenerator, IEnumerable<IVirtualTerrainResource>>) (x => x.GenerateResources().AsEnumerable())).ToImmutableArray<IVirtualTerrainResource>();
        this.m_virtualResourcesMap = this.m_virtualResources.AsEnumerable().GroupBy<IVirtualTerrainResource, VirtualResourceProductProto>((Func<IVirtualTerrainResource, VirtualResourceProductProto>) (x => x.Product)).ToDict<IGrouping<VirtualResourceProductProto, IVirtualTerrainResource>, VirtualResourceProductProto, ImmutableArray<IVirtualTerrainResource>>((Func<IGrouping<VirtualResourceProductProto, IVirtualTerrainResource>, VirtualResourceProductProto>) (x => x.Key), (Func<IGrouping<VirtualResourceProductProto, IVirtualTerrainResource>, ImmutableArray<IVirtualTerrainResource>>) (x => x.ToImmutableArray<IVirtualTerrainResource>()));
        foreach (IVirtualTerrainResource virtualResource in this.m_virtualResources)
        {
          if (virtualResource is IVirtualTerrainResourceFriend terrainResourceFriend && virtualResource.Product.Id != IdsCore.Products.CleanWater)
            terrainResourceFriend.InitializeScale(this.m_difficultyConfig.ExtraStartingMaterialMult);
        }
      }
    }

    public ImmutableArray<IVirtualTerrainResource> GetAllResourcesFor(
      VirtualResourceProductProto product)
    {
      ImmutableArray<IVirtualTerrainResource> immutableArray;
      return this.m_virtualResourcesMap.TryGetValue(product, out immutableArray) ? immutableArray : ImmutableArray<IVirtualTerrainResource>.Empty;
    }

    public void AddAvailableTileResourcesToLyst(
      Tile2i position,
      Lyst<ProductVirtualResource> resources)
    {
      foreach (KeyValuePair<VirtualResourceProductProto, ImmutableArray<IVirtualTerrainResource>> virtualResources in this.m_virtualResourcesMap)
      {
        VirtualResourceProductProto key = virtualResources.Key;
        ThicknessTilesF zero = ThicknessTilesF.Zero;
        foreach (IVirtualTerrainResource virtualTerrainResource in virtualResources.Value)
          zero += virtualTerrainResource.GetApproxThicknessAt(position);
        if (zero.IsPositive)
          resources.Add(new ProductVirtualResource(key, zero));
      }
    }

    public ImmutableArray<IVirtualTerrainResource> RetrieveResourcesAt(
      ProductProto product,
      Tile2i position)
    {
      return this.m_virtualResources.Where((Func<IVirtualTerrainResource, bool>) (x => (Proto) x.Product.Product == (Proto) product && x.IsAt(position))).ToImmutableArray<IVirtualTerrainResource>();
    }

    public ImmutableArray<IVirtualTerrainResource> RetrieveAllResourcesAt(Tile2i position)
    {
      return this.m_virtualResources.Where((Func<IVirtualTerrainResource, bool>) (x => x.IsAt(position))).ToImmutableArray<IVirtualTerrainResource>();
    }

    static VirtualResourceManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VirtualResourceManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VirtualResourceManager) obj).SerializeData(writer));
      VirtualResourceManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VirtualResourceManager) obj).DeserializeData(reader));
    }
  }
}
