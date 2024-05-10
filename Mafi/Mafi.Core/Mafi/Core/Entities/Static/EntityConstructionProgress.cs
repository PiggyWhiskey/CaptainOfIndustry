// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.EntityConstructionProgress
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  public class EntityConstructionProgress : 
    ConstructionProgress,
    IEntityConstructionProgress,
    IConstructionProgress,
    IInputBufferPriorityProvider,
    IOutputBufferPriorityProvider
  {
    private bool m_isDestroyed;
    private EntityNotificator m_priorityNotif;
    private readonly GlobalPrioritiesManager m_prioritiesManager;
    private int m_standardPriorityOverride;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IStaticEntity Entity => this.Owner as IStaticEntity;

    public bool IsPriority { get; private set; }

    private int IncreasedPriority => 0;

    public EntityConstructionProgress(
      IStaticEntity owner,
      INotificationsManager notificationsManager,
      GlobalPrioritiesManager prioritiesManager,
      ImmutableArray<ProductBuffer> buffers,
      AssetValue totalCost,
      Duration durationPerProduct,
      Duration extraDuration,
      bool isDeconstruction = false,
      bool isUpgrade = false,
      Percent? currentStepsPerc = null,
      bool allowFreeRebuild = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_standardPriorityOverride = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntity) owner, buffers, totalCost, durationPerProduct, extraDuration, isDeconstruction, isUpgrade, currentStepsPerc, allowFreeRebuild);
      this.m_prioritiesManager = prioritiesManager;
      this.m_priorityNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.ConstructionPrioritized);
    }

    public override bool TryMakeStep()
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      return base.TryMakeStep();
    }

    public void RegisterBuffers(IVehicleBuffersRegistry buffersRegistry)
    {
      if (this.IsDeconstruction)
      {
        foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
          buffersRegistry.RegisterOutputBufferAndAssert(this.Entity, (IProductBuffer) constructionBuffer, (IOutputBufferPriorityProvider) this, true, true, true);
      }
      else
      {
        foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
          buffersRegistry.RegisterInputBufferAndAssert(this.Entity, (IProductBuffer) constructionBuffer, (IInputBufferPriorityProvider) this, true, allowDeliveryAtDistanceWhenBlocked: true);
      }
    }

    public void UnregisterBuffers(IVehicleBuffersRegistry buffersRegistry)
    {
      if (this.IsDeconstruction)
      {
        if (this.IsPaused)
        {
          foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
            buffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) constructionBuffer);
        }
        else
        {
          foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
            buffersRegistry.UnregisterOutputBufferAndAssert((IProductBuffer) constructionBuffer);
        }
      }
      else if (this.IsPaused)
      {
        foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
          buffersRegistry.TryUnregisterInputBuffer((IProductBuffer) constructionBuffer);
      }
      else
      {
        foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
          buffersRegistry.TryUnregisterInputBuffer((IProductBuffer) constructionBuffer);
      }
    }

    public void SetStandardPriorityOverride(int priority)
    {
      if (!GeneralPriorities.AssertAssignableRange(priority))
        return;
      this.m_standardPriorityOverride = priority;
    }

    public void SetPriority(bool isPriority)
    {
      this.IsPriority = isPriority;
      this.m_priorityNotif.NotifyIff(this.IsPriority, this.Owner);
    }

    public void Destroy()
    {
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      this.m_priorityNotif.Deactivate(this.Owner);
      this.m_isDestroyed = true;
    }

    private void clearBuffers(IAssetTransactionManager assetManager)
    {
      foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
      {
        assetManager.ClearBuffer((IProductBuffer) constructionBuffer);
        constructionBuffer.SetCapacity(Quantity.Zero);
      }
    }

    public AssetValue RemoveAssetValueFromBuffers()
    {
      AssetValueBuilder pooledInstance = AssetValueBuilder.GetPooledInstance();
      foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
      {
        pooledInstance.Add(constructionBuffer.Product, constructionBuffer.Quantity);
        constructionBuffer.Clear();
      }
      return pooledInstance.GetAssetValueAndReturnToPool();
    }

    public void ClearAndDestroyBuffers(
      IAssetTransactionManager assetManager,
      IVehicleBuffersRegistry vehicleRegistry)
    {
      this.UnregisterBuffers(vehicleRegistry);
      this.clearBuffers(assetManager);
      this.Destroy();
    }

    public int GetPriority()
    {
      return this.IsPriority ? this.IncreasedPriority : this.resolveStandardPriority();
    }

    private int resolveStandardPriority()
    {
      if (this.m_standardPriorityOverride != -1)
        return this.m_standardPriorityOverride;
      return !this.IsDeconstruction ? this.m_prioritiesManager.ConstructionPriority : this.m_prioritiesManager.DeconstructionPriority;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      Assert.That<bool>(this.IsDeconstruction).IsFalse();
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      if (this.IsPriority)
        return BufferStrategy.FullFillAtAnyCost(this.IncreasedPriority);
      int priority = this.resolveStandardPriority();
      if (this.CurrentSteps > 0 || pendingQuantity.IsPositive)
        priority = (priority - 1).Max(0);
      Quantity quantity = buffer.UsableCapacity - pendingQuantity;
      return new BufferStrategy(priority, new Quantity?(quantity));
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      Assert.That<bool>(this.IsDeconstruction).IsTrue();
      Assert.That<bool>(this.m_isDestroyed).IsFalse();
      if (this.IsPriority)
        return new BufferStrategy(this.IncreasedPriority, new Quantity?(request.Buffer.Quantity - request.PendingQuantity));
      int priority = this.resolveStandardPriority();
      if (this.CurrentSteps > this.ExtraSteps || request.PendingQuantity.IsPositive)
        priority = (priority - 1).Max(0);
      return new BufferStrategy(priority, new Quantity?(request.Buffer.Quantity - request.PendingQuantity));
    }

    public AssetValue FillBuffersWith(AssetValue value)
    {
      AssetValueBuilder pooledInstance = AssetValueBuilder.GetPooledInstance();
      foreach (ProductQuantity product in value.Products)
      {
        foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
        {
          if (!constructionBuffer.IsFull() && !((Proto) constructionBuffer.Product != (Proto) product.Product))
          {
            Quantity quantity = constructionBuffer.StoreAsMuchAs(product);
            Assert.That<Quantity>(quantity).IsLess(product.Quantity);
            if (quantity.IsPositive)
              pooledInstance.Add(product.Product, quantity);
          }
        }
      }
      return pooledInstance.GetAssetValueAndReturnToPool();
    }

    public bool AreBuffersFull()
    {
      foreach (ProductBuffer constructionBuffer in this.ConstructionBuffers)
      {
        if (constructionBuffer.IsNotFull)
          return false;
      }
      return true;
    }

    public static void Serialize(EntityConstructionProgress value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EntityConstructionProgress>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EntityConstructionProgress.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsPriority);
      writer.WriteBool(this.m_isDestroyed);
      GlobalPrioritiesManager.Serialize(this.m_prioritiesManager, writer);
      EntityNotificator.Serialize(this.m_priorityNotif, writer);
      writer.WriteInt(this.m_standardPriorityOverride);
    }

    public static EntityConstructionProgress Deserialize(BlobReader reader)
    {
      EntityConstructionProgress constructionProgress;
      if (reader.TryStartClassDeserialization<EntityConstructionProgress>(out constructionProgress))
        reader.EnqueueDataDeserialization((object) constructionProgress, EntityConstructionProgress.s_deserializeDataDelayedAction);
      return constructionProgress;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.IsPriority = reader.ReadBool();
      this.m_isDestroyed = reader.ReadBool();
      reader.SetField<EntityConstructionProgress>(this, "m_prioritiesManager", (object) GlobalPrioritiesManager.Deserialize(reader));
      this.m_priorityNotif = EntityNotificator.Deserialize(reader);
      this.m_standardPriorityOverride = reader.ReadInt();
    }

    static EntityConstructionProgress()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityConstructionProgress.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ConstructionProgress) obj).SerializeData(writer));
      EntityConstructionProgress.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ConstructionProgress) obj).DeserializeData(reader));
    }
  }
}
