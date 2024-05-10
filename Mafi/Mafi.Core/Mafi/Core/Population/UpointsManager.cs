// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.UpointsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Population
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [DebuggerDisplay("Unity: quantity={Quantity}")]
  [GenerateSerializer(false, null, 0)]
  public class UpointsManager : IUpointsManager
  {
    public static readonly Upoints UNITY_BASE_CAP;
    public readonly VirtualProductProto UnityProto;
    private readonly IProductsManager m_productsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly ProtosDb m_protosDb;
    private readonly INotificationsManager m_notificationsManager;
    private readonly UpointsManager.UpointsBuffer m_buffer;
    private Upoints m_freeMonthlyUnity;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_quickActionCostMult;
    private readonly Lyst<UnityConsumer> m_sortedConsumers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Upoints DiffForLastMonth { get; private set; }

    public Upoints PossibleDiffForLastMonth { get; private set; }

    public Upoints DiffForLastMonthWithOneTimeActions { get; private set; }

    public Quantity Quantity => this.m_buffer.Quantity;

    public Upoints TotalUnityCap { get; private set; }

    public UpointsStats Stats { get; private set; }

    public Percent QuickActionCostMultiplier => this.m_quickActionCostMult.Value;

    public UpointsManager(
      [ProtoDep("Product_Virtual_Upoints")] VirtualProductProto unityProto,
      IProductsManager productsManager,
      PopsHealthManager popsHealthManager,
      ProtosDb protosDb,
      IPropertiesDb propsDb,
      IAssetTransactionManager assetManager,
      ICalendar calendar,
      INotificationsManager notificationsManager,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CTotalUnityCap\u003Ek__BackingField = UpointsManager.UNITY_BASE_CAP;
      this.m_freeMonthlyUnity = Upoints.Zero;
      this.m_sortedConsumers = new Lyst<UnityConsumer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.UnityProto = unityProto;
      this.m_productsManager = productsManager;
      this.m_popsHealthManager = popsHealthManager;
      this.m_protosDb = protosDb;
      this.m_notificationsManager = notificationsManager;
      this.Stats = new UpointsStats(protosDb, statsManager);
      calendar.NewMonthStart.Add<UpointsManager>(this, new Action(this.onNewMonthStart));
      calendar.NewMonthEnd.Add<UpointsManager>(this, new Action(this.onNewMonthEnd));
      this.m_quickActionCostMult = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.QuickActionsUnityCostMultiplier);
      this.m_buffer = new UpointsManager.UpointsBuffer(this.TotalUnityCap.GetQuantityRounded(), (ProductProto) unityProto, this.m_productsManager);
      assetManager.AddGlobalOutput((IProductBuffer) this.m_buffer, 10, (Option<IEntity>) Option.None);
      assetManager.AddGlobalInput((IProductBuffer) this.m_buffer, 10, (Option<IEntity>) Option.None);
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<UpointsManager>(this, "m_quickActionCostMult", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.QuickActionsUnityCostMultiplier));
    }

    internal void Cheat_addUnityMonthly(Upoints unity) => this.m_freeMonthlyUnity += unity;

    internal void Cheat_addUnityOnce(Upoints unity)
    {
      this.GenerateUnity(IdsCore.UpointsCategories.FreeUnity, unity, new Upoints?(), new LocStr?());
    }

    internal void Test_DisableUpointsFromHealth()
    {
    }

    public void AddConsumer(UnityConsumer consumer)
    {
      Mafi.Assert.That<bool>(consumer.IsDestroyed).IsFalse();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_sortedConsumers.PriorityListInsertSorted<UnityConsumer>(consumer, UpointsManager.\u003C\u003EO.\u003C0\u003E__priorityProvider ?? (UpointsManager.\u003C\u003EO.\u003C0\u003E__priorityProvider = new Func<UnityConsumer, int>(priorityProvider)));

      static int priorityProvider(UnityConsumer c) => c.Priority;
    }

    public void RemoveConsumer(UnityConsumer consumer)
    {
      this.m_sortedConsumers.RemoveAndAssert(consumer);
    }

    private void onNewMonthStart()
    {
      this.Stats.ClearOnNewMonthStart();
      this.DiffForLastMonth = Upoints.Zero;
      this.PossibleDiffForLastMonth = Upoints.Zero;
      this.DiffForLastMonthWithOneTimeActions = Upoints.Zero;
      if (this.m_freeMonthlyUnity.IsPositive)
      {
        this.GenerateUnity(IdsCore.UpointsCategories.FreeUnity, this.m_freeMonthlyUnity, new Upoints?(), new LocStr?());
      }
      else
      {
        if (!this.m_freeMonthlyUnity.IsNegative)
          return;
        this.ConsumeAsMuchAs(IdsCore.UpointsCategories.FreeUnity, -this.m_freeMonthlyUnity, new Option<IEntity>(), new LocStr?());
      }
    }

    /// <summary>
    /// Any unity generation or edict run on onNewMonth. So by doing this in end we make
    /// sure that consumers come last.
    /// </summary>
    private void onNewMonthEnd()
    {
      if (this.m_popsHealthManager.UpointsForHealthLastMonth.IsNegative)
        this.ConsumeAsMuchAs(IdsCore.UpointsCategories.Health, -this.m_popsHealthManager.UpointsForHealthLastMonth, new Option<IEntity>(), new LocStr?());
      else
        this.GenerateUnity(IdsCore.UpointsCategories.Health, this.m_popsHealthManager.UpointsForHealthLastMonth, new Upoints?(), new LocStr?());
      foreach (UnityConsumer sortedConsumer in this.m_sortedConsumers)
        sortedConsumer.RechargeOnNewMonth();
    }

    public void ChangeUnityCap(Upoints unityDiff)
    {
      if (this.TotalUnityCap + unityDiff < UpointsManager.UNITY_BASE_CAP)
      {
        Mafi.Assert.Fail(string.Format("Cannot add {0} to current cap of {1}!", (object) unityDiff, (object) this.TotalUnityCap));
      }
      else
      {
        this.TotalUnityCap += unityDiff;
        Quantity quantity = this.m_buffer.Quantity - this.TotalUnityCap.GetQuantityRounded();
        if (quantity.IsPositive)
        {
          this.m_buffer.RemoveExactly(quantity);
          this.m_productsManager.ProductDestroyed(this.UnityProto.WithQuantity(quantity), DestroyReason.Wasted);
        }
        this.m_buffer.SetCapacity(this.TotalUnityCap.GetQuantityRounded());
      }
    }

    public void AddInitialUnity(Upoints unity)
    {
      Quantity quantity = unity.GetQuantityRounded() - this.m_buffer.StoreAsMuchAs(unity.GetQuantityRounded());
      if (!quantity.IsPositive)
        return;
      this.m_productsManager.ProductCreated((ProductProto) this.UnityProto, quantity, CreateReason.InitialResource);
    }

    public void GenerateUnity(
      Proto.ID categoryId,
      Upoints generated,
      Upoints? max = null,
      LocStr? extraTitle = null)
    {
      if (generated.IsNegative)
      {
        Mafi.Assert.That<Upoints>(generated).IsNotNegative();
      }
      else
      {
        UpointsCategoryProto proto;
        if (!this.m_protosDb.TryGetProto<UpointsCategoryProto>(categoryId, out proto))
        {
          Mafi.Assert.Fail(string.Format("Could not find upoints category {0}", (object) categoryId));
        }
        else
        {
          this.PossibleDiffForLastMonth += generated;
          Quantity quantity = generated.GetQuantityRounded() - this.m_buffer.StoreAsMuchAs(generated.GetQuantityRounded());
          if (quantity.IsPositive)
          {
            this.m_productsManager.ProductCreated((ProductProto) this.UnityProto, quantity, CreateReason.General);
            this.DiffForLastMonth += quantity.Upoints();
          }
          if (!generated.IsPositive && !max.HasValue)
            return;
          this.Stats.AddNewProduction(proto, generated, max.GetValueOrDefault(generated), extraTitle);
        }
      }
    }

    public void ConsumeExactly(
      Proto.ID categoryId,
      Upoints unity,
      Option<IEntity> consumer = default (Option<IEntity>),
      LocStr? extraTitle = null)
    {
      Mafi.Assert.That<Upoints>(this.ConsumeAsMuchAs(categoryId, unity, consumer, extraTitle)).IsNear(unity);
    }

    /// <summary>Returns how much was removed.</summary>
    public Upoints ConsumeAsMuchAs(
      Proto.ID categoryId,
      Upoints unity,
      Option<IEntity> consumer = default (Option<IEntity>),
      LocStr? extraTitle = null)
    {
      Mafi.Assert.That<Upoints>(unity).IsNotNegative();
      UpointsCategoryProto proto;
      if (!this.m_protosDb.TryGetProto<UpointsCategoryProto>(categoryId, out proto))
      {
        Mafi.Log.Error(string.Format("Could not find upoints category {0}", (object) categoryId));
        return Upoints.Zero;
      }
      Quantity quantity = this.m_buffer.RemoveAsMuchAs(unity.GetQuantityRounded());
      this.m_productsManager.ProductDestroyed((ProductProto) this.UnityProto, quantity, DestroyReason.General);
      Upoints consumed = quantity.Upoints();
      this.DiffForLastMonthWithOneTimeActions -= consumed;
      if (!proto.IsOneTimeAction)
      {
        this.PossibleDiffForLastMonth -= unity;
        this.DiffForLastMonth -= consumed;
      }
      this.Stats.AddNewDemand(proto, consumed, unity, consumer, extraTitle);
      return quantity.Upoints();
    }

    public bool CanConsume(Upoints unity) => this.Quantity >= unity.GetQuantityRounded();

    public bool TryConsume(
      Proto.ID categoryId,
      Upoints unity,
      Option<IEntity> consumer = default (Option<IEntity>),
      LocStr? extraTitle = null)
    {
      if (this.CanConsume(unity))
      {
        this.ConsumeExactly(categoryId, unity, consumer, extraTitle);
        return true;
      }
      UpointsCategoryProto proto;
      if (!this.m_protosDb.TryGetProto<UpointsCategoryProto>(categoryId, out proto))
      {
        Mafi.Assert.Fail(string.Format("Could not find upoints category {0}", (object) categoryId));
        return false;
      }
      this.Stats.AddNewDemand(proto, Upoints.Zero, unity, consumer, extraTitle);
      this.m_notificationsManager.NotifyOnce(IdsCore.Notifications.NotEnoughUpoints);
      return false;
    }

    public static void Serialize(UpointsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpointsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpointsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Upoints.Serialize(this.DiffForLastMonth, writer);
      Upoints.Serialize(this.DiffForLastMonthWithOneTimeActions, writer);
      UpointsManager.UpointsBuffer.Serialize(this.m_buffer, writer);
      Upoints.Serialize(this.m_freeMonthlyUnity, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_quickActionCostMult);
      Lyst<UnityConsumer>.Serialize(this.m_sortedConsumers, writer);
      Upoints.Serialize(this.PossibleDiffForLastMonth, writer);
      UpointsStats.Serialize(this.Stats, writer);
      Upoints.Serialize(this.TotalUnityCap, writer);
      writer.WriteGeneric<VirtualProductProto>(this.UnityProto);
    }

    public static UpointsManager Deserialize(BlobReader reader)
    {
      UpointsManager upointsManager;
      if (reader.TryStartClassDeserialization<UpointsManager>(out upointsManager))
        reader.EnqueueDataDeserialization((object) upointsManager, UpointsManager.s_deserializeDataDelayedAction);
      return upointsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.DiffForLastMonth = Upoints.Deserialize(reader);
      this.DiffForLastMonthWithOneTimeActions = Upoints.Deserialize(reader);
      reader.SetField<UpointsManager>(this, "m_buffer", (object) UpointsManager.UpointsBuffer.Deserialize(reader));
      this.m_freeMonthlyUnity = Upoints.Deserialize(reader);
      reader.SetField<UpointsManager>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<UpointsManager>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      reader.SetField<UpointsManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.RegisterResolvedMember<UpointsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<UpointsManager>(this, "m_quickActionCostMult", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<UpointsManager>(this, "m_sortedConsumers", (object) Lyst<UnityConsumer>.Deserialize(reader));
      this.PossibleDiffForLastMonth = Upoints.Deserialize(reader);
      this.Stats = UpointsStats.Deserialize(reader);
      this.TotalUnityCap = Upoints.Deserialize(reader);
      reader.SetField<UpointsManager>(this, "UnityProto", (object) reader.ReadGenericAs<VirtualProductProto>());
      reader.RegisterInitAfterLoad<UpointsManager>(this, "initSelf", InitPriority.Normal);
    }

    static UpointsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpointsManager.UNITY_BASE_CAP = 20.Upoints();
      UpointsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpointsManager) obj).SerializeData(writer));
      UpointsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpointsManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class UpointsBuffer : ProductBuffer
    {
      private readonly ProductStats m_productStats;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public UpointsBuffer(
        Quantity capacity,
        ProductProto product,
        IProductsManager productsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product);
        this.m_productStats = productsManager.GetStatsFor(product);
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        this.m_productStats.StoredAvailableQuantityChange(diff);
      }

      public static void Serialize(UpointsManager.UpointsBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<UpointsManager.UpointsBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, UpointsManager.UpointsBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        ProductStats.Serialize(this.m_productStats, writer);
      }

      public static UpointsManager.UpointsBuffer Deserialize(BlobReader reader)
      {
        UpointsManager.UpointsBuffer upointsBuffer;
        if (reader.TryStartClassDeserialization<UpointsManager.UpointsBuffer>(out upointsBuffer))
          reader.EnqueueDataDeserialization((object) upointsBuffer, UpointsManager.UpointsBuffer.s_deserializeDataDelayedAction);
        return upointsBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<UpointsManager.UpointsBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
      }

      static UpointsBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        UpointsManager.UpointsBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        UpointsManager.UpointsBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
