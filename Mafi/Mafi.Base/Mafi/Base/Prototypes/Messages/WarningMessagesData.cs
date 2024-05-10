// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Messages.WarningMessagesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Maintenance;
using Mafi.Core.Messages;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Messages
{
  internal class WarningMessagesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      LocStr locStr1 = Loc.Str("WarningPrefix", "Warning:", "");
      MessageGroupProto orThrow = prototypesDb.GetOrThrow<MessageGroupProto>(Ids.MessageGroups.Warnings);
      LocStr locStr2 = Loc.Str("WarningNoWorkersNoBeacon__part1", "You have <b>no more free workers</b> on your island. Everyone is already busy working. Some buildings or vehicles will not be able to operate.", "");
      LocStr1 locStr1_1 = Loc.Str1("WarningNoWorkersNoBeacon__part2", "In order to get more people, you should prioritize researching and building a {0}. It will help to attract more refugees to your island and you will not only get more workers but also some extra materials from them. The effectiveness of the beacon will decline over time. So you should prioritize repairing the <bc>ship</bc> to bring more people and resources to the island.", "{0} = Beacon");
      LocStr locStr3 = Loc.Str("WarningNoWorkersNoBeacon__part3", "In the meantime, you can pause some buildings that you need the least to free up some workers.", "");
      string content1 = string.Format("<i>{0}</i> {1}\n\n{2}\n\n{3}", (object) locStr1.TranslatedString, (object) locStr2.TranslatedString, (object) locStr1_1.Format(Ids.Buildings.Beacon.TranslatedName(prototypesDb).MakeBold()), (object) locStr3.TranslatedString);
      MessageProto messageProto1 = prototypesDb.Add<MessageProto>(new MessageProto(Ids.Messages.WarningNoWorkersNoBeacon, "No workers!", content1, InGameMessageType.Warning, group: orThrow));
      prototypesDb.Add<WarningMessagesData.NoWorkersNoBeaconTriggerProto>(new WarningMessagesData.NoWorkersNoBeaconTriggerProto(messageProto1));
      LocStr locStr4 = Loc.Str("WarningLowMaintenanceNoDepot__part1", "Some entities are <b>running out of maintenance</b> and they may start breaking down soon.", "");
      LocStr1 locStr1_2 = Loc.Str1("WarningLowMaintenanceNoDepot__part2", "In order to get more maintenance, you should prioritize researching and building a {0}. Once operational, you will need to supply it with products and it will start maintaining all your machines and vehicles.", "{0} = Maintenance depot");
      LocStr locStr5 = Loc.Str("WarningLowMaintenanceNoDepot__part3", "In the meantime, you can spend Unity for <bc>quick-repair</bc> action to fix machines or vehicles that are needed the most.", "");
      string content2 = string.Format("<i>{0}</i> {1}\n\n{2}\n\n{3}", (object) locStr1.TranslatedString, (object) locStr4.TranslatedString, (object) locStr1_2.Format(Ids.Buildings.MaintenanceDepotT1.TranslatedName(prototypesDb).MakeBold()), (object) locStr5.TranslatedString);
      MessageProto messageProto2 = prototypesDb.Add<MessageProto>(new MessageProto(Ids.Messages.WarningLowMaintenanceNoDepot, "Low maintenance!", content2, InGameMessageType.Warning, group: orThrow));
      prototypesDb.Add<WarningMessagesData.LowMaintenanceNoDepotTriggerProto>(new WarningMessagesData.LowMaintenanceNoDepotTriggerProto(messageProto2));
      LocStr locStr6 = Loc.Str("WarningLowDiesel__part1", "Global supply of <b>diesel is critically low</b>! You are at high risk of running out of diesel. When all your diesel supplies are depleted, all vehicles and diesel generators will stop working. Without working logistics and electricity generation your economy will halt.", "");
      LocStr1 locStr1_3 = Loc.Str1("WarningLowDiesel__part2", "Make sure that you have sufficient diesel production and that you are extracting enough crude oil. You can <bc>boost</bc> an {0} using Unity which will not only speed it up but also it <b>makes it work without the need for electricity</b>.", "{0} = Oil Pump");
      string content3 = string.Format("<i>{0}</i> {1}\n\n{2}", (object) locStr1.TranslatedString, (object) locStr6.TranslatedString, (object) locStr1_3.Format(Ids.Machines.OilPump.TranslatedName(prototypesDb)));
      MessageProto messageProto3 = prototypesDb.Add<MessageProto>(new MessageProto(Ids.Messages.WarningLowDiesel, "Low diesel!", content3, InGameMessageType.Warning, group: orThrow));
      prototypesDb.Add<MessageTriggerGlobalProductLowProto>(new MessageTriggerGlobalProductLowProto(messageProto3, Ids.Products.Diesel, (QuantityLarge) 80.Quantity()));
    }

    public WarningMessagesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [GenerateSerializer(false, null, 0)]
    private class LowMaintenanceNoDepotTrigger : MessageTrigger
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      private readonly MaintenanceManager m_maintenanceManager;
      private readonly IConstructionManager m_constructionManager;

      public static void Serialize(
        WarningMessagesData.LowMaintenanceNoDepotTrigger value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WarningMessagesData.LowMaintenanceNoDepotTrigger>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WarningMessagesData.LowMaintenanceNoDepotTrigger.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
        MaintenanceManager.Serialize(this.m_maintenanceManager, writer);
      }

      public static WarningMessagesData.LowMaintenanceNoDepotTrigger Deserialize(BlobReader reader)
      {
        WarningMessagesData.LowMaintenanceNoDepotTrigger maintenanceNoDepotTrigger;
        if (reader.TryStartClassDeserialization<WarningMessagesData.LowMaintenanceNoDepotTrigger>(out maintenanceNoDepotTrigger))
          reader.EnqueueDataDeserialization((object) maintenanceNoDepotTrigger, WarningMessagesData.LowMaintenanceNoDepotTrigger.s_deserializeDataDelayedAction);
        return maintenanceNoDepotTrigger;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<WarningMessagesData.LowMaintenanceNoDepotTrigger>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
        reader.SetField<WarningMessagesData.LowMaintenanceNoDepotTrigger>(this, "m_maintenanceManager", (object) MaintenanceManager.Deserialize(reader));
      }

      public LowMaintenanceNoDepotTrigger(
        WarningMessagesData.LowMaintenanceNoDepotTriggerProto triggerProto,
        ISimLoopEvents simLoopEvents,
        MessagesManager messageManager,
        MaintenanceManager maintenanceManager,
        IConstructionManager constructionManager)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
        this.m_maintenanceManager = maintenanceManager;
        this.m_constructionManager = constructionManager;
        maintenanceManager.NotEnoughMaintenanceThisMonth.Add<WarningMessagesData.LowMaintenanceNoDepotTrigger>(this, new Action<VirtualProductProto>(this.notEnoughMaintenanceThisMonth));
        constructionManager.EntityConstructed.Add<WarningMessagesData.LowMaintenanceNoDepotTrigger>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      }

      private void notEnoughMaintenanceThisMonth(VirtualProductProto product)
      {
        this.DeliverMessage(10.Seconds());
      }

      private void onEntityConstructed(IStaticEntity entity)
      {
        if (!(entity.Prototype.Id == Ids.Buildings.MaintenanceDepotT1))
          return;
        this.OnDestroy();
      }

      protected override void OnDestroy()
      {
        this.m_maintenanceManager.NotEnoughMaintenanceThisMonth.Remove<WarningMessagesData.LowMaintenanceNoDepotTrigger>(this, new Action<VirtualProductProto>(this.notEnoughMaintenanceThisMonth));
        this.m_constructionManager.EntityConstructed.Remove<WarningMessagesData.LowMaintenanceNoDepotTrigger>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      }

      static LowMaintenanceNoDepotTrigger()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        WarningMessagesData.LowMaintenanceNoDepotTrigger.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
        WarningMessagesData.LowMaintenanceNoDepotTrigger.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private class NoWorkersNoBeaconTrigger : MessageTrigger
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      private readonly IWorkersManager m_workersManager;
      private readonly IConstructionManager m_constructionManager;

      public static void Serialize(
        WarningMessagesData.NoWorkersNoBeaconTrigger value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WarningMessagesData.NoWorkersNoBeaconTrigger>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WarningMessagesData.NoWorkersNoBeaconTrigger.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
        writer.WriteGeneric<IWorkersManager>(this.m_workersManager);
      }

      public static WarningMessagesData.NoWorkersNoBeaconTrigger Deserialize(BlobReader reader)
      {
        WarningMessagesData.NoWorkersNoBeaconTrigger workersNoBeaconTrigger;
        if (reader.TryStartClassDeserialization<WarningMessagesData.NoWorkersNoBeaconTrigger>(out workersNoBeaconTrigger))
          reader.EnqueueDataDeserialization((object) workersNoBeaconTrigger, WarningMessagesData.NoWorkersNoBeaconTrigger.s_deserializeDataDelayedAction);
        return workersNoBeaconTrigger;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<WarningMessagesData.NoWorkersNoBeaconTrigger>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
        reader.SetField<WarningMessagesData.NoWorkersNoBeaconTrigger>(this, "m_workersManager", (object) reader.ReadGenericAs<IWorkersManager>());
      }

      public NoWorkersNoBeaconTrigger(
        WarningMessagesData.NoWorkersNoBeaconTriggerProto triggerProto,
        ISimLoopEvents simLoopEvents,
        MessagesManager messageManager,
        IWorkersManager workersManager,
        IConstructionManager constructionManager)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
        this.m_workersManager = workersManager;
        this.m_constructionManager = constructionManager;
        workersManager.WorkersAmountChanged.Add<WarningMessagesData.NoWorkersNoBeaconTrigger>(this, new Action<int>(this.workersAmountChanged));
        constructionManager.EntityConstructed.Add<WarningMessagesData.NoWorkersNoBeaconTrigger>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      }

      private void workersAmountChanged(int availableAmount)
      {
        if (availableAmount >= 0)
          return;
        this.DeliverMessage(10.Seconds());
      }

      private void onEntityConstructed(IStaticEntity entity)
      {
        if (!(entity.Prototype.Id == Ids.Buildings.Beacon))
          return;
        this.OnDestroy();
      }

      protected override void OnDestroy()
      {
        this.m_workersManager.WorkersAmountChanged.Remove<WarningMessagesData.NoWorkersNoBeaconTrigger>(this, new Action<int>(this.workersAmountChanged));
        this.m_constructionManager.EntityConstructed.Remove<WarningMessagesData.NoWorkersNoBeaconTrigger>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      }

      static NoWorkersNoBeaconTrigger()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        WarningMessagesData.NoWorkersNoBeaconTrigger.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
        WarningMessagesData.NoWorkersNoBeaconTrigger.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
      }
    }

    private class NoWorkersNoBeaconTriggerProto : MessageTriggerProto
    {
      public override Type Implementation => typeof (WarningMessagesData.NoWorkersNoBeaconTrigger);

      public NoWorkersNoBeaconTriggerProto(MessageProto messageProto)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector(messageProto);
      }
    }

    private class LowMaintenanceNoDepotTriggerProto : MessageTriggerProto
    {
      public override Type Implementation
      {
        get => typeof (WarningMessagesData.LowMaintenanceNoDepotTrigger);
      }

      public LowMaintenanceNoDepotTriggerProto(MessageProto messageProto)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector(messageProto);
      }
    }
  }
}
