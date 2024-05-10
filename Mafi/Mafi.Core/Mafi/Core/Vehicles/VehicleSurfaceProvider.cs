// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleSurfaceProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles
{
  /// <summary>
  /// Vehicle surface provider that takes account terrain and static entities that have <see cref="P:Mafi.Core.Entities.Static.IStaticEntity.VehicleSurfaceHeights" /> set.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class VehicleSurfaceProvider : IVehicleSurfaceProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly ThicknessTilesF SURFACE_REL_HEIGHT;
    private readonly TerrainManager m_terrainManager;
    private readonly Dict<Tile2i, VehicleSurfaceProvider.SurfaceHeights> m_surfaceHeights;
    private readonly Event<Tile2i> m_onVehicleSurfaceChanged;

    public static void Serialize(VehicleSurfaceProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleSurfaceProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleSurfaceProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Event<Tile2i>.Serialize(this.m_onVehicleSurfaceChanged, writer);
      Dict<Tile2i, VehicleSurfaceProvider.SurfaceHeights>.Serialize(this.m_surfaceHeights, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
    }

    public static VehicleSurfaceProvider Deserialize(BlobReader reader)
    {
      VehicleSurfaceProvider vehicleSurfaceProvider;
      if (reader.TryStartClassDeserialization<VehicleSurfaceProvider>(out vehicleSurfaceProvider))
        reader.EnqueueDataDeserialization((object) vehicleSurfaceProvider, VehicleSurfaceProvider.s_deserializeDataDelayedAction);
      return vehicleSurfaceProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleSurfaceProvider>(this, "m_onVehicleSurfaceChanged", (object) Event<Tile2i>.Deserialize(reader));
      reader.SetField<VehicleSurfaceProvider>(this, "m_surfaceHeights", (object) Dict<Tile2i, VehicleSurfaceProvider.SurfaceHeights>.Deserialize(reader));
      reader.SetField<VehicleSurfaceProvider>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
    }

    public IReadOnlyDictionary<Tile2i, VehicleSurfaceProvider.SurfaceHeights> EntityHeights
    {
      get
      {
        return (IReadOnlyDictionary<Tile2i, VehicleSurfaceProvider.SurfaceHeights>) this.m_surfaceHeights;
      }
    }

    public IEvent<Tile2i> OnVehicleSurfaceChanged
    {
      get => (IEvent<Tile2i>) this.m_onVehicleSurfaceChanged;
    }

    public VehicleSurfaceProvider(
      TerrainManager terrainManager,
      IEntitiesManager entitiesManager,
      IConstructionManager constructionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_surfaceHeights = new Dict<Tile2i, VehicleSurfaceProvider.SurfaceHeights>();
      this.m_onVehicleSurfaceChanged = new Event<Tile2i>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      entitiesManager.StaticEntityAdded.Add<VehicleSurfaceProvider>(this, new Action<IStaticEntity>(this.entityAdded));
      entitiesManager.StaticEntityRemoved.Add<VehicleSurfaceProvider>(this, new Action<IStaticEntity>(this.entityRemoved));
      constructionManager.EntityConstructed.Add<VehicleSurfaceProvider>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<VehicleSurfaceProvider>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
    }

    public HeightTilesF? GetEntityVehicleSurfaceAt(Tile2i coord, out bool isAccessible)
    {
      VehicleSurfaceProvider.SurfaceHeights surfaceHeights;
      if (this.m_surfaceHeights.TryGetValue(coord, out surfaceHeights))
      {
        if (surfaceHeights.Height == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT)
        {
          isAccessible = false;
          return new HeightTilesF?(this.m_terrainManager.GetHeight(coord));
        }
        Assert.That<int>((int) surfaceHeights.Count).IsPositive();
        isAccessible = (int) surfaceHeights.Count == (int) surfaceHeights.ConstructedCount;
        return new HeightTilesF?(surfaceHeights.Height);
      }
      isAccessible = false;
      return new HeightTilesF?();
    }

    public HeightTilesF GetVehicleSurfaceAt(Tile2i coord, out bool isAccessible)
    {
      HeightTilesF? vehicleSurfaceAt = this.GetEntityVehicleSurfaceAt(coord, out isAccessible);
      if (vehicleSurfaceAt.HasValue)
        return vehicleSurfaceAt.Value;
      isAccessible = true;
      HeightTilesF height = this.m_terrainManager.GetHeight(coord);
      TileSurfaceData tileSurfaceData;
      return this.m_terrainManager.TryGetTileSurface(this.m_terrainManager.GetTileIndex(coord), out tileSurfaceData) ? (tileSurfaceData.Height + VehicleSurfaceProvider.SURFACE_REL_HEIGHT).Max(height) : height;
    }

    public HeightTilesF GetInterpolatedVehicleSurfaceAt(Tile2f coord)
    {
      bool isAccessible;
      HeightTilesF vehicleSurfaceAt1 = this.GetVehicleSurfaceAt(coord.Tile2i, out isAccessible);
      HeightTilesF vehicleSurfaceAt2 = this.GetVehicleSurfaceAt(coord.Tile2i.IncrementX, out isAccessible);
      HeightTilesF vehicleSurfaceAt3 = this.GetVehicleSurfaceAt(coord.Tile2i.IncrementY, out isAccessible);
      Tile2i tile2i = coord.Tile2i;
      tile2i = tile2i.IncrementX;
      HeightTilesF vehicleSurfaceAt4 = this.GetVehicleSurfaceAt(tile2i.IncrementY, out isAccessible);
      RelTile2f relTile2f = coord.FractionalPartNonNegative();
      return vehicleSurfaceAt1.Lerp(vehicleSurfaceAt3, relTile2f.Y.ToPercent()).Lerp(vehicleSurfaceAt2.Lerp(vehicleSurfaceAt4, relTile2f.Y.ToPercent()), relTile2f.X.ToPercent());
    }

    private void entityAdded(IStaticEntity entity)
    {
      if (entity.VehicleSurfaceHeights.IsEmpty)
        return;
      foreach (KeyValuePair<Tile2i, HeightTilesF> vehicleSurfaceHeight in entity.VehicleSurfaceHeights)
        this.addSurfaceHeight(vehicleSurfaceHeight.Key, vehicleSurfaceHeight.Value);
    }

    private void entityRemoved(IStaticEntity entity)
    {
      if (entity.VehicleSurfaceHeights.IsEmpty)
        return;
      foreach (KeyValuePair<Tile2i, HeightTilesF> vehicleSurfaceHeight in entity.VehicleSurfaceHeights)
        this.removeSurfaceHeight(vehicleSurfaceHeight.Key);
    }

    private void entityConstructed(IStaticEntity entity)
    {
      if (entity.VehicleSurfaceHeights.IsEmpty)
        return;
      foreach (KeyValuePair<Tile2i, HeightTilesF> vehicleSurfaceHeight in entity.VehicleSurfaceHeights)
      {
        VehicleSurfaceProvider.SurfaceHeights surfaceHeight = this.m_surfaceHeights[vehicleSurfaceHeight.Key];
        ++surfaceHeight.ConstructedCount;
        this.m_surfaceHeights[vehicleSurfaceHeight.Key] = surfaceHeight;
        Assert.That<int>((int) surfaceHeight.ConstructedCount).IsLessOrEqual((int) surfaceHeight.Count);
        if ((int) surfaceHeight.ConstructedCount == (int) surfaceHeight.Count)
          this.m_onVehicleSurfaceChanged.Invoke(vehicleSurfaceHeight.Key);
      }
    }

    private void entityDeconstructionStarted(IStaticEntity entity)
    {
      if (entity.VehicleSurfaceHeights.IsEmpty)
        return;
      foreach (KeyValuePair<Tile2i, HeightTilesF> vehicleSurfaceHeight in entity.VehicleSurfaceHeights)
      {
        VehicleSurfaceProvider.SurfaceHeights surfaceHeight = this.m_surfaceHeights[vehicleSurfaceHeight.Key];
        --surfaceHeight.ConstructedCount;
        this.m_surfaceHeights[vehicleSurfaceHeight.Key] = surfaceHeight;
        Assert.That<int>((int) surfaceHeight.ConstructedCount).IsNotNegative();
        if ((int) surfaceHeight.ConstructedCount + 1 == (int) surfaceHeight.Count)
          this.m_onVehicleSurfaceChanged.Invoke(vehicleSurfaceHeight.Key);
      }
    }

    private void addSurfaceHeight(Tile2i tile, HeightTilesF height)
    {
      VehicleSurfaceProvider.SurfaceHeights surfaceHeights;
      this.m_surfaceHeights.TryGetValue(tile, out surfaceHeights);
      if (surfaceHeights.Count <= (sbyte) 0)
      {
        surfaceHeights.Height = height;
        surfaceHeights.Count = (sbyte) 1;
        this.m_surfaceHeights[tile] = surfaceHeights;
        this.m_onVehicleSurfaceChanged.Invoke(tile);
      }
      else
      {
        Assert.That<HeightTilesF>(surfaceHeights.Height).IsEqualTo<Tile2i, HeightTilesF, HeightTilesF>(height, "Vehicle surface height conflict at {0}: {1} != {2}", tile, height, surfaceHeights.Height);
        ++surfaceHeights.Count;
        this.m_surfaceHeights[tile] = surfaceHeights;
      }
    }

    private void removeSurfaceHeight(Tile2i tile)
    {
      VehicleSurfaceProvider.SurfaceHeights surfaceHeights;
      if (!this.m_surfaceHeights.TryGetValue(tile, out surfaceHeights))
        Log.Error(string.Format("Failed to remove vehicle height at tile {0}.", (object) tile));
      else if (surfaceHeights.Count > (sbyte) 1)
      {
        --surfaceHeights.Count;
        this.m_surfaceHeights[tile] = surfaceHeights;
      }
      else
      {
        this.m_surfaceHeights.Remove(tile);
        this.m_onVehicleSurfaceChanged.Invoke(tile);
      }
    }

    static VehicleSurfaceProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleSurfaceProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleSurfaceProvider) obj).SerializeData(writer));
      VehicleSurfaceProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleSurfaceProvider) obj).DeserializeData(reader));
      VehicleSurfaceProvider.SURFACE_REL_HEIGHT = 0.025.TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public struct SurfaceHeights
    {
      public HeightTilesF Height;
      public sbyte Count;
      public sbyte ConstructedCount;

      public static void Serialize(VehicleSurfaceProvider.SurfaceHeights value, BlobWriter writer)
      {
        HeightTilesF.Serialize(value.Height, writer);
        writer.WriteSByte(value.Count);
        writer.WriteSByte(value.ConstructedCount);
      }

      public static VehicleSurfaceProvider.SurfaceHeights Deserialize(BlobReader reader)
      {
        return new VehicleSurfaceProvider.SurfaceHeights(HeightTilesF.Deserialize(reader), reader.ReadSByte(), reader.ReadSByte());
      }

      public SurfaceHeights(HeightTilesF height, sbyte count, sbyte constructedCount)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Height = height;
        this.Count = count;
        this.ConstructedCount = constructedCount;
      }
    }
  }
}
