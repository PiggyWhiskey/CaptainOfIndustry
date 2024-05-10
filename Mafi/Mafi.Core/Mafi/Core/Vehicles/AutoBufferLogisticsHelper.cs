// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.AutoBufferLogisticsHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class AutoBufferLogisticsHelper : IEntityObserverForPorts, IEntityObserver
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IStaticEntity m_owner;
    private IInputBufferPriorityProvider m_priorityForInputs;
    private IOutputBufferPriorityProvider m_priorityForOutputs;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly Dict<ProductProto, Lyst<IProductBuffer>> m_inputBuffers;
    private readonly Dict<ProductProto, IProductBuffer> m_outputBuffers;

    public static void Serialize(AutoBufferLogisticsHelper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AutoBufferLogisticsHelper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AutoBufferLogisticsHelper.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt((int) this.LogisticsInputMode);
      writer.WriteInt((int) this.LogisticsOutputMode);
      Dict<ProductProto, Lyst<IProductBuffer>>.Serialize(this.m_inputBuffers, writer);
      Dict<ProductProto, IProductBuffer>.Serialize(this.m_outputBuffers, writer);
      writer.WriteGeneric<IStaticEntity>(this.m_owner);
      writer.WriteGeneric<IInputBufferPriorityProvider>(this.m_priorityForInputs);
      writer.WriteGeneric<IOutputBufferPriorityProvider>(this.m_priorityForOutputs);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
    }

    public static AutoBufferLogisticsHelper Deserialize(BlobReader reader)
    {
      AutoBufferLogisticsHelper bufferLogisticsHelper;
      if (reader.TryStartClassDeserialization<AutoBufferLogisticsHelper>(out bufferLogisticsHelper))
        reader.EnqueueDataDeserialization((object) bufferLogisticsHelper, AutoBufferLogisticsHelper.s_deserializeDataDelayedAction);
      return bufferLogisticsHelper;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.LogisticsInputMode = (EntityLogisticsMode) reader.ReadInt();
      this.LogisticsOutputMode = (EntityLogisticsMode) reader.ReadInt();
      reader.SetField<AutoBufferLogisticsHelper>(this, "m_inputBuffers", (object) Dict<ProductProto, Lyst<IProductBuffer>>.Deserialize(reader));
      reader.SetField<AutoBufferLogisticsHelper>(this, "m_outputBuffers", (object) Dict<ProductProto, IProductBuffer>.Deserialize(reader));
      reader.SetField<AutoBufferLogisticsHelper>(this, "m_owner", (object) reader.ReadGenericAs<IStaticEntity>());
      this.m_priorityForInputs = reader.ReadGenericAs<IInputBufferPriorityProvider>();
      this.m_priorityForOutputs = reader.ReadGenericAs<IOutputBufferPriorityProvider>();
      reader.SetField<AutoBufferLogisticsHelper>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
    }

    public EntityLogisticsMode LogisticsInputMode { get; private set; }

    public EntityLogisticsMode LogisticsOutputMode { get; private set; }

    public AutoBufferLogisticsHelper(
      IStaticEntity owner,
      IInputBufferPriorityProvider priorityForInputs,
      IOutputBufferPriorityProvider priorityForOutputs,
      IVehicleBuffersRegistry vehicleBuffersRegistry)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_inputBuffers = new Dict<ProductProto, Lyst<IProductBuffer>>();
      this.m_outputBuffers = new Dict<ProductProto, IProductBuffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_owner = owner;
      this.m_priorityForInputs = priorityForInputs;
      this.m_priorityForOutputs = priorityForOutputs;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      owner.AddObserver((IEntityObserver) this);
    }

    public void OnProductReceivedFromPort(IProductBuffer bufferReceived)
    {
      Lyst<IProductBuffer> lyst;
      if (this.LogisticsInputMode != EntityLogisticsMode.Auto || !this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer(bufferReceived) || !this.m_inputBuffers.TryGetValue(bufferReceived.Product, out lyst) || lyst.Count == 1)
        return;
      foreach (IProductBuffer buffer in lyst)
        this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer(buffer);
    }

    public void OnProductSentToPort(IProductBuffer bufferSent)
    {
      if (this.LogisticsOutputMode != EntityLogisticsMode.Auto)
        return;
      this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer(bufferSent);
    }

    public void AddInputBuffer(IProductBuffer buffer)
    {
      this.m_inputBuffers.GetOrAdd<ProductProto, Lyst<IProductBuffer>>(buffer.Product, (Func<ProductProto, Lyst<IProductBuffer>>) (_ => new Lyst<IProductBuffer>())).Add(buffer);
      if (this.LogisticsInputMode == EntityLogisticsMode.Off)
        return;
      this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert(this.m_owner, buffer, this.m_priorityForInputs);
    }

    public void TryAddInputBuffer(IProductBuffer buffer)
    {
      Lyst<IProductBuffer> lyst;
      if (this.m_inputBuffers.TryGetValue(buffer.Product, out lyst) && lyst.Contains(buffer))
        return;
      this.AddInputBuffer(buffer);
    }

    public void RemoveInputBuffer(IProductBuffer buffer)
    {
      Lyst<IProductBuffer> lyst;
      if (!this.m_inputBuffers.TryGetValue(buffer.Product, out lyst))
        return;
      if (!lyst.Remove(buffer))
        Assert.Fail(string.Format("Failed to remove buffer for product {0}", (object) buffer.Product));
      else
        this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer(buffer);
    }

    public void RemoveOutputBuffer(IProductBuffer buffer)
    {
      if (!this.m_outputBuffers.TryRemove(buffer.Product, out IProductBuffer _))
        Assert.Fail(string.Format("Failed to remove buffer for product {0}", (object) buffer.Product));
      else
        this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer(buffer);
    }

    public void TryRemoveInputBuffer(IProductBuffer buffer)
    {
      Lyst<IProductBuffer> lyst;
      if (!this.m_inputBuffers.TryGetValue(buffer.Product, out lyst) || !lyst.Remove(buffer))
        return;
      this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer(buffer);
    }

    public void TryRemoveOutputBuffer(IProductBuffer buffer)
    {
      if (!this.m_outputBuffers.TryRemove(buffer.Product, out IProductBuffer _))
        return;
      this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer(buffer);
    }

    public void AddOutputBuffer(IProductBuffer buffer)
    {
      this.m_outputBuffers.Add(buffer.Product, buffer);
      if (this.LogisticsOutputMode == EntityLogisticsMode.Off)
        return;
      this.m_vehicleBuffersRegistry.RegisterOutputBufferAndAssert(this.m_owner, buffer, this.m_priorityForOutputs);
    }

    public void ReRegisterOutputBufferIfAuto(IProductBuffer buffer)
    {
      if (this.LogisticsInputMode != EntityLogisticsMode.Auto)
        return;
      this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer(this.m_owner, buffer, this.m_priorityForOutputs);
    }

    public bool HasOutputBufferFor(ProductProto product)
    {
      return this.m_outputBuffers.ContainsKey(product);
    }

    public void TryAddOutputBuffer(IProductBuffer buffer)
    {
      if (this.m_outputBuffers.ContainsKey(buffer.Product))
        return;
      this.AddOutputBuffer(buffer);
    }

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      if (this.LogisticsInputMode == mode)
        return;
      this.LogisticsInputMode = mode;
      if (this.LogisticsInputMode == EntityLogisticsMode.Off)
      {
        foreach (Lyst<IProductBuffer> lyst in this.m_inputBuffers.Values)
        {
          foreach (IProductBuffer buffer in lyst)
            this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer(buffer);
        }
      }
      else
      {
        foreach (Lyst<IProductBuffer> lyst in this.m_inputBuffers.Values)
        {
          foreach (IProductBuffer buffer in lyst)
            this.m_vehicleBuffersRegistry.TryRegisterInputBuffer(this.m_owner, buffer, this.m_priorityForInputs);
        }
      }
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      if (this.LogisticsOutputMode == mode)
        return;
      this.LogisticsOutputMode = mode;
      if (this.LogisticsOutputMode == EntityLogisticsMode.Off)
      {
        foreach (IProductBuffer buffer in this.m_outputBuffers.Values)
          this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer(buffer);
      }
      else
      {
        foreach (IProductBuffer buffer in this.m_outputBuffers.Values)
          this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer(this.m_owner, buffer, this.m_priorityForOutputs);
      }
    }

    private void inputConnectionChanged(IoPort thisPort)
    {
      if (thisPort.IsConnected || this.LogisticsInputMode != EntityLogisticsMode.Auto)
        return;
      this.registerBuffersToLogistics(thisPort.ShapePrototype.AllowedProductType, thisPort.Type);
    }

    private void outputConnectionChanged(IoPort thisPort)
    {
      if (thisPort.IsConnected || this.LogisticsOutputMode != EntityLogisticsMode.Auto)
        return;
      this.registerBuffersToLogistics(thisPort.ShapePrototype.AllowedProductType, thisPort.Type);
    }

    private void registerBuffersToLogistics(ProductType productType, IoPortType portType)
    {
      if (portType == IoPortType.Output)
      {
        foreach (IProductBuffer buffer in this.m_outputBuffers.Values)
        {
          if (buffer.Product.Type == productType)
            this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer(this.m_owner, buffer, this.m_priorityForOutputs);
        }
      }
      else
      {
        foreach (Lyst<IProductBuffer> lyst in this.m_inputBuffers.Values)
        {
          foreach (IProductBuffer buffer in lyst)
          {
            if (buffer.Product.Type == productType)
              this.m_vehicleBuffersRegistry.TryRegisterInputBuffer(this.m_owner, buffer, this.m_priorityForInputs);
          }
        }
      }
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
    }

    void IEntityObserverForPorts.OnConnectionChanged(IoPort ourPort, IoPort otherPort)
    {
      if (ourPort.Type == IoPortType.Input)
      {
        this.inputConnectionChanged(ourPort);
      }
      else
      {
        if (ourPort.Type != IoPortType.Output)
          return;
        this.outputConnectionChanged(ourPort);
      }
    }

    static AutoBufferLogisticsHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AutoBufferLogisticsHelper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AutoBufferLogisticsHelper) obj).SerializeData(writer));
      AutoBufferLogisticsHelper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AutoBufferLogisticsHelper) obj).DeserializeData(reader));
    }
  }
}
