// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToReachProductStatsValue
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToReachProductStatsValue : Goal
  {
    private readonly GoalToReachProductStatsValue.Proto m_goalProto;
    private readonly IProductsManager m_productsManager;
    private QuantityLarge m_currentValue;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToReachProductStatsValue(
      GoalToReachProductStatsValue.Proto goalProto,
      IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_productsManager = productsManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      QuantityLarge quantityLarge = this.m_goalProto.StatsSelector(this.m_productsManager.GetStatsFor(this.m_goalProto.ProtoToTrack));
      if (this.m_currentValue != quantityLarge)
      {
        this.m_currentValue = quantityLarge;
        this.updateTitle();
      }
      return this.m_currentValue >= (QuantityLarge) this.m_goalProto.MinQuantityRequired;
    }

    private void updateTitle()
    {
      if (this.m_goalProto.HideCount)
        this.Title = this.m_goalProto.GetTitle(this.m_goalProto.ProtoToTrack.Strings.Name).Value;
      else
        this.Title = this.m_goalProto.GetTitle(this.m_goalProto.ProtoToTrack.Strings.Name).Value + string.Format(" ({0} / {1})", (object) this.m_currentValue, (object) this.m_goalProto.MinQuantityRequired);
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToReachProductStatsValue value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToReachProductStatsValue>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToReachProductStatsValue.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      QuantityLarge.Serialize(this.m_currentValue, writer);
      writer.WriteGeneric<GoalToReachProductStatsValue.Proto>(this.m_goalProto);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
    }

    public static GoalToReachProductStatsValue Deserialize(BlobReader reader)
    {
      GoalToReachProductStatsValue productStatsValue;
      if (reader.TryStartClassDeserialization<GoalToReachProductStatsValue>(out productStatsValue))
        reader.EnqueueDataDeserialization((object) productStatsValue, GoalToReachProductStatsValue.s_deserializeDataDelayedAction);
      return productStatsValue;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentValue = QuantityLarge.Deserialize(reader);
      reader.SetField<GoalToReachProductStatsValue>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToReachProductStatsValue.Proto>());
      reader.SetField<GoalToReachProductStatsValue>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
    }

    static GoalToReachProductStatsValue()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToReachProductStatsValue.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToReachProductStatsValue.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      private static readonly LocStr1 TITLE_MINE;
      private static readonly LocStr1 TITLE_PRODUCE;
      private static readonly LocStr1 TITLE_START_PRODUCING;
      private static readonly LocStr1 TITLE_PROCESS;
      private static readonly LocStr1 TITLE_DUMP;
      private static readonly LocStr1 TITLE_DUMP_LIQUID;
      public readonly ProductProto ProtoToTrack;
      public readonly Quantity MinQuantityRequired;
      public readonly bool HideCount;
      public readonly Func<ProductStats, QuantityLarge> StatsSelector;
      private readonly Func<string, LocStrFormatted> m_formatFunc;

      public override Type Implementation => typeof (GoalToReachProductStatsValue);

      public Proto(
        string id,
        ProductProto protoToTrack,
        int quantityRequired,
        Func<string, LocStrFormatted> formatFunc,
        Func<ProductStats, QuantityLarge> statsSelector = null,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        bool hideCount = false,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip, tutorialUnlock);
        this.ProtoToTrack = protoToTrack;
        this.MinQuantityRequired = quantityRequired.Quantity();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.StatsSelector = statsSelector ?? GoalToReachProductStatsValue.Proto.\u003C\u003EO.\u003C0\u003E__producedInFactory ?? (GoalToReachProductStatsValue.Proto.\u003C\u003EO.\u003C0\u003E__producedInFactory = new Func<ProductStats, QuantityLarge>(GoalToReachProductStatsValue.Proto.producedInFactory));
        this.m_formatFunc = formatFunc;
        this.HideCount = hideCount;
      }

      private static QuantityLarge producedInFactory(ProductStats stats)
      {
        return stats.CreatedByProduction.Lifetime;
      }

      public static LocStrFormatted MineTitle(string productName)
      {
        return GoalToReachProductStatsValue.Proto.TITLE_MINE.Format("<bc>" + productName + "</bc>");
      }

      public static LocStrFormatted ProduceTitle(string productName)
      {
        return GoalToReachProductStatsValue.Proto.TITLE_PRODUCE.Format("<bc>" + productName + "</bc>");
      }

      public static LocStrFormatted StartProducingTitle(string productName)
      {
        return GoalToReachProductStatsValue.Proto.TITLE_START_PRODUCING.Format("<bc>" + productName + "</bc>");
      }

      public static LocStrFormatted ProcessTitle(string productName)
      {
        return GoalToReachProductStatsValue.Proto.TITLE_PROCESS.Format("<bc>" + productName + "</bc>");
      }

      public static LocStrFormatted DumpTitle(string productName)
      {
        return GoalToReachProductStatsValue.Proto.TITLE_DUMP.Format("<bc>" + productName + "</bc>");
      }

      public static LocStrFormatted DumpLiquidTitle(string productName)
      {
        return GoalToReachProductStatsValue.Proto.TITLE_DUMP_LIQUID.Format("<bc>" + productName + "</bc>");
      }

      public LocStrFormatted GetTitle(LocStr productName)
      {
        return this.m_formatFunc(productName.TranslatedString);
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToReachProductStatsValue.Proto.TITLE_MINE = Loc.Str1("Goal__Mine", "Mine {0}", "text for a goal, {0} replaced with title of a product");
        GoalToReachProductStatsValue.Proto.TITLE_PRODUCE = Loc.Str1("Goal__Produce", "Produce {0}", "text for a goal, {0} replaced with title of a product");
        GoalToReachProductStatsValue.Proto.TITLE_START_PRODUCING = Loc.Str1("Goal__StartProducing", "Start producing {0}", "text for a goal, {0} replaced with title of a product");
        GoalToReachProductStatsValue.Proto.TITLE_PROCESS = Loc.Str1("Goal__Process", "Process {0}", "text for a goal, {0} replaced with title of a product");
        GoalToReachProductStatsValue.Proto.TITLE_DUMP = Loc.Str1("Goal__Dump", "Dump {0}", "text for a goal to dump a loose product (on the ground), {0} replaced with title of a product");
        GoalToReachProductStatsValue.Proto.TITLE_DUMP_LIQUID = Loc.Str1("Goal__DumpLiquid", "Dump {0}", "text for a goal to dump a liquid product (into ocean), {0} replaced with title of a product");
      }
    }
  }
}
