// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.MechanicalPower.ShaftManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.MechanicalPower
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class ShaftManager : IShaftManager, IVirtualBufferProvider
  {
    public static readonly MechPower MAX_SHAFT_THROUGHPUT;
    private readonly IProductsManager m_productsManager;
    private readonly IoPortShapeProto m_shaftShapeProto;
    private readonly VirtualProductProto m_mechPowerProto;
    private readonly Lyst<Shaft> m_shafts;
    private readonly Dict<IEntityWithPorts, Shaft> m_entityToShaft;
    private readonly Shaft m_zeroCapacityShaft;
    private readonly Dict<IEntityWithPorts, ShaftManager.Buffer> m_buffers;
    public readonly MechPowerAvgStats StoredMechPower;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ImmutableArray<ProductProto> ProvidedProducts { get; private set; }

    public VirtualProductProto MechPowerProto => this.m_mechPowerProto;

    public int ShaftsCount => this.m_shafts.Count;

    /// <summary>
    /// This shaft is connected to entities that are not connected to any other entities via shafts.
    /// </summary>
    public IShaft DefaultZeroCapacityShaft => (IShaft) this.m_zeroCapacityShaft;

    public ShaftManager(
      ISimLoopEvents simLoopEvents,
      IoPortsManager portsManager,
      IProductsManager productsManager,
      IEntitiesManager entitiesManager,
      IConstructionManager constructionManager,
      [ProtoDep("IoPortShape_Shaft")] IoPortShapeProto shaftShapeProto,
      [ProtoDep("Product_Virtual_MechPower")] VirtualProductProto mechPowerProto,
      StatsManager statsManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_shafts = new Lyst<Shaft>();
      this.m_entityToShaft = new Dict<IEntityWithPorts, Shaft>();
      this.m_buffers = new Dict<IEntityWithPorts, ShaftManager.Buffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
      this.m_shaftShapeProto = shaftShapeProto;
      this.m_mechPowerProto = mechPowerProto;
      this.m_zeroCapacityShaft = new Shaft((ProductProto) mechPowerProto, Quantity.Zero);
      this.ProvidedProducts = ImmutableArray.Create<ProductProto>((ProductProto) mechPowerProto);
      this.StoredMechPower = new MechPowerAvgStats((Option<StatsManager>) statsManager);
      simLoopEvents.UpdateStart.Add<ShaftManager>(this, new Action(this.updateStart));
      portsManager.PortConnectionChanged.Add<ShaftManager>(this, new Action<IoPort, IoPort>(this.portConnectionChanged));
      entitiesManager.StaticEntityRemoved.Add<ShaftManager>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
      constructionManager.EntityConstructed.Add<ShaftManager>(this, new Action<IStaticEntity>(this.updateShaftTotalInertia));
      constructionManager.EntityStartedDeconstruction.Add<ShaftManager>(this, new Action<IStaticEntity>(this.updateShaftTotalInertia));
      calendar.NewDay.Add<ShaftManager>(this, new Action(this.onNewDay));
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      ShaftManager.Buffer buffer;
      if (!(entity is IEntityWithPorts key) || !this.m_buffers.TryGetValue(key, out buffer))
        return;
      buffer.Destroy();
      this.m_buffers.Remove(key);
    }

    private void updateShaftTotalInertia(IStaticEntity entity)
    {
      if (!(entity is IEntityWithPorts entity1))
        return;
      this.m_productsManager.ProductDestroyed((ProductProto) this.m_mechPowerProto, this.getCurrentShaftFor((IStaticEntity) entity1).UpdateTotalInertia().Quantity, DestroyReason.Wasted);
    }

    private void onNewDay()
    {
      this.StoredMechPower.Set(new MechPower(this.m_shafts.Sum<Shaft>((Func<Shaft, int>) (x => x.InertiaBuffer.Quantity.Value / 20))));
    }

    public Option<IProductBuffer> GetBuffer(ProductProto product, Option<IEntity> entity)
    {
      return (Proto) product != (Proto) this.m_mechPowerProto || !(entity.ValueOrNull is IEntityWithPorts valueOrNull) ? Option<IProductBuffer>.None : Option.Some<IProductBuffer>((IProductBuffer) this.GetOrCreateShaftBufferFor(valueOrNull, this.computeMaxOutputCapacity(valueOrNull)));
    }

    private MechPower computeMaxOutputCapacity(IEntityWithPorts entity)
    {
      if (!(entity is Machine machine))
        return MechPower.Zero;
      Lyst<RecipeProto> lyst = machine.RecipesAssigned.Where<RecipeProto>((Func<RecipeProto, bool>) (r => r.AllInputs.Contains((Predicate<RecipeInput>) (x => (Proto) x.Product == (Proto) this.m_mechPowerProto)))).ToLyst<RecipeProto>();
      if (lyst.IsEmpty)
        return MechPower.Zero;
      Assert.That<Lyst<RecipeProto>>(lyst).HasLength<RecipeProto>(1);
      RecipeProduct recipeProduct = (RecipeProduct) lyst[0].AllInputs.FirstOrDefault((Func<RecipeInput, bool>) (x => (Proto) x.Product == (Proto) this.m_mechPowerProto));
      Assert.That<Quantity>(recipeProduct.Quantity).IsPositive();
      return MechPower.FromQuantity(recipeProduct.Quantity);
    }

    public IShaft GetCurrentShaftFor(IStaticEntity entity)
    {
      return (IShaft) this.getCurrentShaftFor(entity);
    }

    public IShaftBuffer GetOrCreateShaftBufferFor(IEntityWithPorts entity, MechPower maxOutput)
    {
      ShaftManager.Buffer shaftBufferFor;
      if (!this.m_buffers.TryGetValue(entity, out shaftBufferFor))
      {
        shaftBufferFor = new ShaftManager.Buffer(entity, this, maxOutput);
        this.m_buffers[entity] = shaftBufferFor;
      }
      Assert.That<Quantity>(shaftBufferFor.MaxOutput).IsEqualTo<IEntityWithPorts>(maxOutput.Quantity, "Shaft buffer max output not matching previous requests on entity {0}.", entity);
      return (IShaftBuffer) shaftBufferFor;
    }

    private Shaft getCurrentShaftFor(IStaticEntity entity)
    {
      Shaft shaft;
      return !(entity is IEntityWithPorts key) || !this.m_entityToShaft.TryGetValue(key, out shaft) ? this.m_zeroCapacityShaft : shaft;
    }

    private void updateStart()
    {
      MechPower zero = MechPower.Zero;
      foreach (Shaft shaft in this.m_shafts)
        zero += shaft.UpdateStart();
      this.m_productsManager.ProductDestroyed((ProductProto) this.m_mechPowerProto, zero.Quantity, DestroyReason.Wasted);
    }

    private void destroyShaft(Shaft shaft)
    {
      this.m_shafts.RemoveAndAssert(shaft);
      shaft.Destroy(this.m_productsManager);
    }

    private void portConnectionChanged(IoPort port, IoPort otherPort)
    {
      if ((Proto) port.ShapePrototype != (Proto) this.m_shaftShapeProto)
        return;
      IEntityWithPorts ownerEntity1 = port.OwnerEntity;
      IEntityWithPorts ownerEntity2 = otherPort.OwnerEntity;
      if (port.IsConnected)
      {
        Shaft shaft1;
        if (this.m_entityToShaft.TryGetValue(ownerEntity1, out shaft1))
        {
          Shaft shaft2;
          if (this.m_entityToShaft.TryGetValue(ownerEntity2, out shaft2))
          {
            if (shaft1 == shaft2)
              return;
            this.mergeShafts(ownerEntity1, ownerEntity2);
          }
          else
            this.addToShaft(ownerEntity2, shaft1);
        }
        else
        {
          Shaft shaft3;
          if (this.m_entityToShaft.TryGetValue(ownerEntity2, out shaft3))
            this.addToShaft(ownerEntity1, shaft3);
          else
            this.createNewShaft(ownerEntity1, ownerEntity2);
        }
      }
      else
      {
        Shaft shaft4;
        if (this.m_entityToShaft.TryGetValue(ownerEntity1, out shaft4))
        {
          Shaft shaft5;
          if (!this.m_entityToShaft.TryGetValue(ownerEntity2, out shaft5) || shaft4 != shaft5)
            return;
          this.splitShaft(ownerEntity1, ownerEntity2);
        }
        else
          this.m_entityToShaft.TryGetValue(ownerEntity2, out Shaft _);
      }
    }

    private Shaft createNewShaft()
    {
      Shaft newShaft = new Shaft((ProductProto) this.m_mechPowerProto, ShaftManager.MAX_SHAFT_THROUGHPUT.Quantity);
      this.m_shafts.Add(newShaft);
      return newShaft;
    }

    private void createNewShaft(IEntityWithPorts e1, IEntityWithPorts e2)
    {
      Assert.That<IEntityWithPorts>(e1).IsNotEqualTo<IEntityWithPorts>(e2);
      Assert.That<Dict<IEntityWithPorts, Shaft>>(this.m_entityToShaft).NotContainsKey<IEntityWithPorts, Shaft>(e1);
      Assert.That<Dict<IEntityWithPorts, Shaft>>(this.m_entityToShaft).NotContainsKey<IEntityWithPorts, Shaft>(e2);
      Shaft newShaft = this.createNewShaft();
      this.m_entityToShaft.Add(e1, newShaft);
      this.m_entityToShaft.Add(e2, newShaft);
      newShaft.AddEntity(e1);
      newShaft.AddEntity(e2);
      newShaft.UpdateTotalInertia();
      Assert.That<int>(this.m_entityToShaft.Count).IsEqualTo(this.m_shafts.Sum<Shaft>((Func<Shaft, int>) (x => x.EntitiesCount)));
    }

    private void addToShaft(IEntityWithPorts entity, Shaft shaft)
    {
      Assert.That<Dict<IEntityWithPorts, Shaft>>(this.m_entityToShaft).NotContainsKey<IEntityWithPorts, Shaft>(entity);
      this.m_entityToShaft.Add(entity, shaft);
      shaft.AddEntity(entity);
      shaft.UpdateTotalInertia();
      Assert.That<int>(this.m_entityToShaft.Count).IsEqualTo(this.m_shafts.Sum<Shaft>((Func<Shaft, int>) (x => x.EntitiesCount)));
    }

    private void splitShaft(IEntityWithPorts entity, IEntityWithPorts otherEntity)
    {
      Assert.That<Shaft>(this.m_entityToShaft[entity]).IsEqualTo<Shaft>(this.m_entityToShaft[otherEntity]);
      Shaft shaft = this.m_entityToShaft[entity];
      Set<IEntityWithPorts> set1 = new Set<IEntityWithPorts>()
      {
        entity
      };
      this.getAllConnectedEntitiesViaShaft(entity, set1);
      Set<IEntityWithPorts> set2 = new Set<IEntityWithPorts>()
      {
        otherEntity
      };
      this.getAllConnectedEntitiesViaShaft(otherEntity, set2);
      Assert.That<int>(set1.Count + set2.Count).IsEqualTo(shaft.EntitiesCount);
      Lyst<Shaft> newShafts = new Lyst<Shaft>(2);
      tryCreateShaftFrom(set1);
      tryCreateShaftFrom(set2);
      if (newShafts.IsNotEmpty)
        this.m_productsManager.ProductDestroyed((ProductProto) this.m_mechPowerProto, shaft.TransferStateTo(newShafts), DestroyReason.Wasted);
      this.destroyShaft(shaft);
      Assert.That<int>(this.m_entityToShaft.Count).IsEqualTo(this.m_shafts.Sum<Shaft>((Func<Shaft, int>) (x => x.EntitiesCount)));

      void tryCreateShaftFrom(Set<IEntityWithPorts> newEntities)
      {
        if (newEntities.Count == 1)
        {
          this.m_entityToShaft.Remove(newEntities.First<IEntityWithPorts>());
        }
        else
        {
          Shaft newShaft = this.createNewShaft();
          newShaft.AddEntities((IEnumerable<IEntityWithPorts>) newEntities);
          foreach (IEntityWithPorts newEntity in newEntities)
            this.m_entityToShaft[newEntity] = newShaft;
          newShaft.UpdateTotalInertia();
          newShafts.Add(newShaft);
        }
      }
    }

    /// <summary>
    /// This assumes that each entity with shaft ports connect to all other with shaft ports.
    /// </summary>
    private void getAllConnectedEntitiesViaShaft(
      IEntityWithPorts entity,
      Set<IEntityWithPorts> outputEntities)
    {
      foreach (IoPort port in entity.Ports)
      {
        if (!port.IsNotConnected && !((Proto) port.ShapePrototype != (Proto) this.m_shaftShapeProto))
        {
          IEntityWithPorts ownerEntity = port.ConnectedPort.Value.OwnerEntity;
          if (outputEntities.Add(ownerEntity))
            this.getAllConnectedEntitiesViaShaft(ownerEntity, outputEntities);
        }
      }
    }

    private void mergeShafts(IEntityWithPorts entity, IEntityWithPorts otherEntity)
    {
      Assert.That<Shaft>(this.m_entityToShaft[entity]).IsNotEqualTo<Shaft>(this.m_entityToShaft[otherEntity]);
      Shaft shaft1 = this.m_entityToShaft[entity];
      Shaft shaft2 = this.m_entityToShaft[otherEntity];
      Shaft newShaft;
      Shaft shaft3;
      if (shaft1.EntitiesCount >= shaft2.EntitiesCount)
      {
        newShaft = shaft1;
        shaft3 = shaft2;
      }
      else
      {
        newShaft = shaft2;
        shaft3 = shaft1;
      }
      int entitiesCount = newShaft.EntitiesCount;
      newShaft.AddEntities((IEnumerable<IEntityWithPorts>) shaft3.ConnectedEntities);
      newShaft.UpdateTotalInertia();
      this.m_productsManager.ProductDestroyed((ProductProto) this.m_mechPowerProto, shaft3.TransferStateTo(newShaft), DestroyReason.Wasted);
      Assert.That<int>(newShaft.EntitiesCount).IsEqualTo(entitiesCount + shaft3.EntitiesCount);
      foreach (IEntityWithPorts connectedEntity in (IEnumerable<IEntityWithPorts>) shaft3.ConnectedEntities)
        this.m_entityToShaft[connectedEntity] = newShaft;
      this.destroyShaft(shaft3);
      Assert.That<int>(this.m_entityToShaft.Count).IsEqualTo(this.m_shafts.Sum<Shaft>((Func<Shaft, int>) (x => x.EntitiesCount)));
    }

    public static void Serialize(ShaftManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ShaftManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ShaftManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<IEntityWithPorts, ShaftManager.Buffer>.Serialize(this.m_buffers, writer);
      Dict<IEntityWithPorts, Shaft>.Serialize(this.m_entityToShaft, writer);
      writer.WriteGeneric<VirtualProductProto>(this.m_mechPowerProto);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Lyst<Shaft>.Serialize(this.m_shafts, writer);
      writer.WriteGeneric<IoPortShapeProto>(this.m_shaftShapeProto);
      Shaft.Serialize(this.m_zeroCapacityShaft, writer);
      ImmutableArray<ProductProto>.Serialize(this.ProvidedProducts, writer);
      MechPowerAvgStats.Serialize(this.StoredMechPower, writer);
    }

    public static ShaftManager Deserialize(BlobReader reader)
    {
      ShaftManager shaftManager;
      if (reader.TryStartClassDeserialization<ShaftManager>(out shaftManager))
        reader.EnqueueDataDeserialization((object) shaftManager, ShaftManager.s_deserializeDataDelayedAction);
      return shaftManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ShaftManager>(this, "m_buffers", (object) Dict<IEntityWithPorts, ShaftManager.Buffer>.Deserialize(reader));
      reader.SetField<ShaftManager>(this, "m_entityToShaft", (object) Dict<IEntityWithPorts, Shaft>.Deserialize(reader));
      reader.SetField<ShaftManager>(this, "m_mechPowerProto", (object) reader.ReadGenericAs<VirtualProductProto>());
      reader.SetField<ShaftManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<ShaftManager>(this, "m_shafts", (object) Lyst<Shaft>.Deserialize(reader));
      reader.SetField<ShaftManager>(this, "m_shaftShapeProto", (object) reader.ReadGenericAs<IoPortShapeProto>());
      reader.SetField<ShaftManager>(this, "m_zeroCapacityShaft", (object) Shaft.Deserialize(reader));
      this.ProvidedProducts = ImmutableArray<ProductProto>.Deserialize(reader);
      reader.SetField<ShaftManager>(this, "StoredMechPower", (object) MechPowerAvgStats.Deserialize(reader));
    }

    static ShaftManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ShaftManager.MAX_SHAFT_THROUGHPUT = MechPower.FromMw(72);
      ShaftManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ShaftManager) obj).SerializeData(writer));
      ShaftManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ShaftManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Buffer : IShaftBuffer, IProductBuffer, IProductBufferReadOnly
    {
      private Option<IEntityWithPorts> m_owningEntity;
      private ShaftManager m_shaftManager;
      public readonly Quantity MaxOutput;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public IShaft Shaft => (IShaft) this.CurrentShaft;

      ProductProto IProductBufferReadOnly.Product
      {
        get => (ProductProto) this.m_shaftManager.m_mechPowerProto;
      }

      Quantity IProductBufferReadOnly.UsableCapacity => this.CurrentShaft.AvailableCapacity;

      Quantity IProductBufferReadOnly.Capacity => this.CurrentShaft.TotalStoreCapacity;

      Quantity IProductBufferReadOnly.Quantity
      {
        get => this.CurrentShaft.GetAvailableQuantity(this.MaxOutput);
      }

      public bool IsDestroyed => this.m_owningEntity.IsNone;

      public Shaft CurrentShaft
      {
        get
        {
          return this.m_shaftManager.getCurrentShaftFor((IStaticEntity) this.m_owningEntity.ValueOrNull);
        }
      }

      public Buffer(IEntityWithPorts owningEntity, ShaftManager shaftManager, MechPower maxOutput)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_owningEntity = Option<IEntityWithPorts>.Some(owningEntity);
        this.m_shaftManager = shaftManager;
        this.MaxOutput = maxOutput.Quantity;
      }

      public Quantity StoreAsMuchAs(Quantity quantity)
      {
        Assert.That<bool>(this.IsDestroyed).IsFalse();
        return this.CurrentShaft.StoreAsMuchAs(quantity);
      }

      public Quantity RemoveAsMuchAs(Quantity quantity)
      {
        Assert.That<bool>(this.IsDestroyed).IsFalse();
        Assert.That<Quantity>(quantity).IsLessOrEqual(this.MaxOutput, "Trying to remove more from shaft than its max.");
        return this.CurrentShaft.RemoveAsMuchAs(quantity, this.MaxOutput);
      }

      public MechPower RemoveAsMuchAs(MechPower mechPower)
      {
        return MechPower.FromQuantity(this.RemoveAsMuchAs(mechPower.Quantity));
      }

      public MechPower GetRemoveAmount(MechPower maxMechPower)
      {
        Assert.That<bool>(this.IsDestroyed).IsFalse();
        return MechPower.FromQuantity(this.CurrentShaft.GetRemoveAmount(maxMechPower.Quantity, this.MaxOutput));
      }

      public void Destroy() => this.m_owningEntity = Option<IEntityWithPorts>.None;

      public static void Serialize(ShaftManager.Buffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ShaftManager.Buffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ShaftManager.Buffer.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Option<IEntityWithPorts>.Serialize(this.m_owningEntity, writer);
        ShaftManager.Serialize(this.m_shaftManager, writer);
        Quantity.Serialize(this.MaxOutput, writer);
      }

      public static ShaftManager.Buffer Deserialize(BlobReader reader)
      {
        ShaftManager.Buffer buffer;
        if (reader.TryStartClassDeserialization<ShaftManager.Buffer>(out buffer))
          reader.EnqueueDataDeserialization((object) buffer, ShaftManager.Buffer.s_deserializeDataDelayedAction);
        return buffer;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.m_owningEntity = Option<IEntityWithPorts>.Deserialize(reader);
        this.m_shaftManager = ShaftManager.Deserialize(reader);
        reader.SetField<ShaftManager.Buffer>(this, "MaxOutput", (object) Quantity.Deserialize(reader));
      }

      static Buffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ShaftManager.Buffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ShaftManager.Buffer) obj).SerializeData(writer));
        ShaftManager.Buffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ShaftManager.Buffer) obj).DeserializeData(reader));
      }
    }
  }
}
