// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  /// <summary>
  /// Binds <see cref="T:Mafi.Core.Buildings.Cargo.Modules.CargoDepotModule" /> to <see cref="T:Mafi.Core.Buildings.Cargo.CargoDepot" /> and manages the amount of available ships.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class CargoDepotManager
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly Lyst<CargoDepot> m_depots;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private readonly Queueue<AvailableCargoShipData> m_repairedUnusedShips;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool HasShipOrDepot => this.m_depots.IsNotEmpty || this.AmountOfShipsDiscovered > 0;

    public int AmountOfShipsDiscovered { get; private set; }

    [DoNotSave(140, null)]
    private int AmountOfShipsRepairedOld { get; set; }

    public int AmountOfShipsInUse { get; private set; }

    public IIndexable<AvailableCargoShipData> RepairedUnusedShips
    {
      get => (IIndexable<AvailableCargoShipData>) this.m_repairedUnusedShips;
    }

    public CargoDepotManager(EntitiesManager entitiesManager, IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_depots = new Lyst<CargoDepot>();
      this.m_repairedUnusedShips = new Queueue<AvailableCargoShipData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      entitiesManager.StaticEntityAdded.Add<CargoDepotManager>(this, new Action<IStaticEntity>(this.entityAdded));
      entitiesManager.StaticEntityRemoved.Add<CargoDepotManager>(this, new Action<IStaticEntity>(this.entityRemoved));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      if (saveVersion < 140)
      {
        int num = this.AmountOfShipsRepairedOld - this.AmountOfShipsInUse;
        for (int index = 0; index < num; ++index)
          this.m_repairedUnusedShips.Enqueue(new AvailableCargoShipData());
        this.AmountOfShipsRepairedOld = 0;
      }
      else
      {
        if (saveVersion >= 152)
          return;
        int num;
        for (num = (this.m_repairedUnusedShips.Count + this.AmountOfShipsInUse - this.AmountOfShipsDiscovered).Min(this.m_repairedUnusedShips.Count); num > 0 && this.m_repairedUnusedShips.IsNotEmpty; --num)
          this.m_repairedUnusedShips.RemoveAt(this.m_repairedUnusedShips.Count - 1);
        if (num <= 0)
          return;
        this.AmountOfShipsDiscovered += num;
      }
    }

    /// <summary>Called by depot when trying to request a ship.</summary>
    public AvailableCargoShipData? GetShipSpawnDataForCargoDepot()
    {
      if (this.m_repairedUnusedShips.Count <= 0)
        return new AvailableCargoShipData?();
      ++this.AmountOfShipsInUse;
      return new AvailableCargoShipData?(this.m_repairedUnusedShips.Dequeue());
    }

    /// <summary>Called by a ship after its destroy.</summary>
    public void ReleaseShipFromDepot(CargoShip ship)
    {
      if (this.AmountOfShipsInUse <= 0)
      {
        Log.Error("Amount of ships in use can turn negative!");
      }
      else
      {
        --this.AmountOfShipsInUse;
        this.m_repairedUnusedShips.EnqueueFirst(new AvailableCargoShipData(ship));
      }
    }

    /// <summary>Called by a ship wreck after its discovery.</summary>
    public void ReportNewCargoShipFound(int count = 1)
    {
      if (count <= 0)
        return;
      this.AmountOfShipsDiscovered += count;
    }

    /// <summary>Called by a ship wreck after repair is done.</summary>
    public void ReportNewCargoShipRepaired(int count = 1)
    {
      if (count <= 0)
        return;
      for (int index = 0; index < count; ++index)
        this.m_repairedUnusedShips.Enqueue(new AvailableCargoShipData());
    }

    private void entityAdded(IStaticEntity entity)
    {
      if (!(entity is CargoDepot cargoDepot))
        return;
      this.m_depots.Add(cargoDepot);
    }

    private void entityRemoved(IStaticEntity entity)
    {
      if (!(entity is CargoDepot cargoDepot))
        return;
      this.m_depots.TryRemoveReplaceLast(cargoDepot);
    }

    /// <returns>
    /// Pair containing the <see cref="T:Mafi.Core.Buildings.Cargo.CargoDepot" /> the module belongs to and index of the slot where the module
    /// belongs, or null if the position (transform) fits no <see cref="T:Mafi.Core.Buildings.Cargo.CargoDepot" /> module position.
    /// </returns>
    public KeyValuePair<CargoDepot, int>? FindOwnerForModule(
      CargoDepotModuleProto proto,
      TileTransform transform)
    {
      Tile3i tile3i = proto.Layout.Transform(RelTile3i.Zero, transform);
      foreach (CargoDepot depot in this.m_depots)
      {
        if (!(depot.Transform.Rotation != transform.Rotation) && depot.Transform.IsReflected == transform.IsReflected)
        {
          ImmutableArray<CargoDepotProto.ModuleSlotPosition> moduleSlots = depot.Prototype.ModuleSlots;
          for (int index = 0; index < moduleSlots.Length; ++index)
          {
            if (moduleSlots[index].GetModuleOrigin(depot) == tile3i)
              return new KeyValuePair<CargoDepot, int>?(new KeyValuePair<CargoDepot, int>(depot, index));
          }
        }
      }
      return new KeyValuePair<CargoDepot, int>?();
    }

    public static void Serialize(CargoDepotManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.AmountOfShipsDiscovered);
      writer.WriteInt(this.AmountOfShipsInUse);
      Lyst<CargoDepot>.Serialize(this.m_depots, writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Queueue<AvailableCargoShipData>.Serialize(this.m_repairedUnusedShips, writer);
    }

    public static CargoDepotManager Deserialize(BlobReader reader)
    {
      CargoDepotManager cargoDepotManager;
      if (reader.TryStartClassDeserialization<CargoDepotManager>(out cargoDepotManager))
        reader.EnqueueDataDeserialization((object) cargoDepotManager, CargoDepotManager.s_deserializeDataDelayedAction);
      return cargoDepotManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AmountOfShipsDiscovered = reader.ReadInt();
      this.AmountOfShipsInUse = reader.ReadInt();
      if (reader.LoadedSaveVersion < 140)
        this.AmountOfShipsRepairedOld = reader.ReadInt();
      reader.SetField<CargoDepotManager>(this, "m_depots", (object) Lyst<CargoDepot>.Deserialize(reader));
      reader.SetField<CargoDepotManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<CargoDepotManager>(this, "m_repairedUnusedShips", reader.LoadedSaveVersion >= 140 ? (object) Queueue<AvailableCargoShipData>.Deserialize(reader) : (object) new Queueue<AvailableCargoShipData>());
      reader.RegisterInitAfterLoad<CargoDepotManager>(this, "initSelf", InitPriority.Normal);
    }

    static CargoDepotManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CargoDepotManager) obj).SerializeData(writer));
      CargoDepotManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CargoDepotManager) obj).DeserializeData(reader));
    }
  }
}
