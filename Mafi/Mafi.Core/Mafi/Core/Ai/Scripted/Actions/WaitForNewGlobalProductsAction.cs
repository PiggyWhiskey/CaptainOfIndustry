// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.WaitForNewGlobalProductsAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class WaitForNewGlobalProductsAction : IScriptedAiPlayerAction
  {
    private readonly ProductProto.ID m_productId;
    private readonly Quantity m_delta;
    private readonly Duration m_maxWait;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (WaitForNewGlobalProductsAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Wait for global delta {0} of {1}.", (object) this.m_delta, (object) this.m_productId);
      }
    }

    public WaitForNewGlobalProductsAction(
      ProductProto.ID productId,
      Quantity delta,
      Duration maxWait)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productId = productId;
      this.m_delta = delta.CheckPositive();
      this.m_maxWait = maxWait;
    }

    public static void Serialize(WaitForNewGlobalProductsAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaitForNewGlobalProductsAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaitForNewGlobalProductsAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Quantity.Serialize(this.m_delta, writer);
      Duration.Serialize(this.m_maxWait, writer);
      ProductProto.ID.Serialize(this.m_productId, writer);
    }

    public static WaitForNewGlobalProductsAction Deserialize(BlobReader reader)
    {
      WaitForNewGlobalProductsAction globalProductsAction;
      if (reader.TryStartClassDeserialization<WaitForNewGlobalProductsAction>(out globalProductsAction))
        reader.EnqueueDataDeserialization((object) globalProductsAction, WaitForNewGlobalProductsAction.s_deserializeDataDelayedAction);
      return globalProductsAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WaitForNewGlobalProductsAction>(this, "m_delta", (object) Quantity.Deserialize(reader));
      reader.SetField<WaitForNewGlobalProductsAction>(this, "m_maxWait", (object) Duration.Deserialize(reader));
      reader.SetField<WaitForNewGlobalProductsAction>(this, "m_productId", (object) ProductProto.ID.Deserialize(reader));
    }

    static WaitForNewGlobalProductsAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaitForNewGlobalProductsAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForNewGlobalProductsAction) obj).SerializeData(writer));
      WaitForNewGlobalProductsAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForNewGlobalProductsAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly WaitForNewGlobalProductsAction m_action;
      private readonly IProductsManager m_productsManager;
      private readonly ProtosDb m_protosDb;
      private Option<ProductProto> m_productProto;
      private QuantityLarge m_initialQuantity;
      private SimStep m_initialSimStep;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(
        WaitForNewGlobalProductsAction action,
        IProductsManager productsManager,
        ProtosDb protosDb)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_productsManager = productsManager;
        this.m_protosDb = protosDb;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (player.IsInstaBuildEnabled)
          return true;
        if (this.m_productProto.IsNone)
        {
          this.m_productProto = this.m_protosDb.Get<ProductProto>((Proto.ID) this.m_action.m_productId);
          if (this.m_productProto.IsNone)
          {
            Log.Error(string.Format("Failed to find product `{0}`.", (object) this.m_action.m_productId));
            return true;
          }
          this.m_initialSimStep = player.SimLoopEvents.CurrentStep;
          this.m_initialQuantity = this.m_productsManager.GetStatsFor(this.m_productProto.Value).GlobalQuantity;
          return false;
        }
        QuantityLarge quantityLarge = this.m_productsManager.GetStatsFor(this.m_productProto.Value).GlobalQuantity - this.m_initialQuantity;
        if (quantityLarge >= (QuantityLarge) this.m_action.m_delta)
          return true;
        Duration duration = (player.SimLoopEvents.CurrentStep.Value - this.m_initialSimStep.Value).Ticks();
        if (!(duration > this.m_action.m_maxWait))
          return false;
        Log.Error(string.Format("Failed to fulfill quantity delta {0} of {1}. ", (object) this.m_action.m_delta, (object) this.m_action.m_productId) + string.Format("Current quantity delta is {0} after {1} mins. ", (object) quantityLarge, (object) duration.Minutes.ToStringRounded()) + string.Format("Action: {0}/{1}, stage: {2}.", (object) player.CurrentActionIndex, (object) player.ActionsCount, (object) player.Stage));
        return true;
      }

      public static void Serialize(WaitForNewGlobalProductsAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WaitForNewGlobalProductsAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WaitForNewGlobalProductsAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        WaitForNewGlobalProductsAction.Serialize(this.m_action, writer);
        QuantityLarge.Serialize(this.m_initialQuantity, writer);
        SimStep.Serialize(this.m_initialSimStep, writer);
        Option<ProductProto>.Serialize(this.m_productProto, writer);
        writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      }

      public static WaitForNewGlobalProductsAction.Core Deserialize(BlobReader reader)
      {
        WaitForNewGlobalProductsAction.Core core;
        if (reader.TryStartClassDeserialization<WaitForNewGlobalProductsAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, WaitForNewGlobalProductsAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<WaitForNewGlobalProductsAction.Core>(this, "m_action", (object) WaitForNewGlobalProductsAction.Deserialize(reader));
        this.m_initialQuantity = QuantityLarge.Deserialize(reader);
        this.m_initialSimStep = SimStep.Deserialize(reader);
        this.m_productProto = Option<ProductProto>.Deserialize(reader);
        reader.SetField<WaitForNewGlobalProductsAction.Core>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
        reader.RegisterResolvedMember<WaitForNewGlobalProductsAction.Core>(this, "m_protosDb", typeof (ProtosDb), true);
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WaitForNewGlobalProductsAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitForNewGlobalProductsAction.Core) obj).SerializeData(writer));
        WaitForNewGlobalProductsAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitForNewGlobalProductsAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
