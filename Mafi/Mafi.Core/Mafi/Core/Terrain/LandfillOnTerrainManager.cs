// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.LandfillOnTerrainManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.Terrain.Physics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class LandfillOnTerrainManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// Every month we reduce landfill by this quantity. This aims to reduce pressure in early game.
    /// </summary>
    private static readonly Quantity MONTHLY_QUANTITY_TO_IGNORE;
    private static readonly Percent HEALTH_PENALTY_PER_QUANTITY;
    private static readonly Percent MAX_HEALTH_PENALTY;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly TerrainMaterialProto m_landfillProto;
    private PartialQuantity m_landfillThisMonth;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_landfillPollutionMultiplier;

    public static void Serialize(LandfillOnTerrainManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LandfillOnTerrainManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LandfillOnTerrainManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IProperty<Percent>>(this.m_landfillPollutionMultiplier);
      writer.WriteGeneric<TerrainMaterialProto>(this.m_landfillProto);
      PartialQuantity.Serialize(this.m_landfillThisMonth, writer);
      PopsHealthManager.Serialize(this.m_popsHealthManager, writer);
      QuantitySumStats.Serialize(this.Stats, writer);
    }

    public static LandfillOnTerrainManager Deserialize(BlobReader reader)
    {
      LandfillOnTerrainManager onTerrainManager;
      if (reader.TryStartClassDeserialization<LandfillOnTerrainManager>(out onTerrainManager))
        reader.EnqueueDataDeserialization((object) onTerrainManager, LandfillOnTerrainManager.s_deserializeDataDelayedAction);
      return onTerrainManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<LandfillOnTerrainManager>(this, "m_landfillPollutionMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.SetField<LandfillOnTerrainManager>(this, "m_landfillProto", (object) reader.ReadGenericAs<TerrainMaterialProto>());
      this.m_landfillThisMonth = PartialQuantity.Deserialize(reader);
      reader.SetField<LandfillOnTerrainManager>(this, "m_popsHealthManager", (object) PopsHealthManager.Deserialize(reader));
      this.Stats = reader.LoadedSaveVersion >= 100 ? QuantitySumStats.Deserialize(reader) : (QuantitySumStats) null;
      reader.RegisterInitAfterLoad<LandfillOnTerrainManager>(this, "initSelf", InitPriority.Normal);
    }

    [NewInSaveVersion(100, null, null, null, null)]
    public QuantitySumStats Stats { get; private set; }

    public ProductProto LandfillProduct => (ProductProto) this.m_landfillProto.MinedProduct;

    public LandfillOnTerrainManager(
      ITerrainDisruptionSimulator disruptionSimulator,
      ICalendar calendar,
      IPropertiesDb propertiesDb,
      PopsHealthManager popsHealthManager,
      StatsManager statsManager,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_popsHealthManager = popsHealthManager;
      this.m_landfillProto = protosDb.GetOrThrow<TerrainMaterialProto>(IdsCore.TerrainMaterials.Landfill);
      this.m_landfillPollutionMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.LandfillPollutionMultiplier);
      this.Stats = new QuantitySumStats((Option<StatsManager>) statsManager);
      disruptionSimulator.GetMaterialRecoveryEvent(this.m_landfillProto).Add<LandfillOnTerrainManager>(this, new Action<MaterialConversionResult>(this.landfillRecovered));
      calendar.NewMonth.Add<LandfillOnTerrainManager>(this, new Action(this.onNewMonth));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion < 100)
        this.Stats = new QuantitySumStats((Option<StatsManager>) resolver.Resolve<StatsManager>());
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<LandfillOnTerrainManager>(this, "m_landfillPollutionMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.LandfillPollutionMultiplier));
    }

    private void landfillRecovered(MaterialConversionResult conversionResult)
    {
      if (conversionResult.IsEmpty || conversionResult.SourceMaterialThickness.IsNone)
        return;
      this.m_landfillThisMonth += this.m_landfillProto.ThicknessToQuantity(conversionResult.SourceMaterialThickness.Thickness);
    }

    private void onNewMonth()
    {
      this.Stats.Add((QuantityLarge) this.m_landfillThisMonth.ToQuantityRounded());
      PartialQuantity partialQuantity = this.m_landfillThisMonth.ScaledBy(this.m_landfillPollutionMultiplier.Value);
      this.m_landfillThisMonth = PartialQuantity.Zero;
      Percent reduction = ((partialQuantity.ToQuantityRounded() - LandfillOnTerrainManager.MONTHLY_QUANTITY_TO_IGNORE).Value * LandfillOnTerrainManager.HEALTH_PENALTY_PER_QUANTITY).Min(LandfillOnTerrainManager.MAX_HEALTH_PENALTY);
      if (!reduction.IsPositive)
        return;
      this.m_popsHealthManager.AddHealthDecrease(IdsCore.HealthPointsCategories.LandfillPollution, reduction);
    }

    static LandfillOnTerrainManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LandfillOnTerrainManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LandfillOnTerrainManager) obj).SerializeData(writer));
      LandfillOnTerrainManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LandfillOnTerrainManager) obj).DeserializeData(reader));
      LandfillOnTerrainManager.MONTHLY_QUANTITY_TO_IGNORE = 3.Quantity();
      LandfillOnTerrainManager.HEALTH_PENALTY_PER_QUANTITY = 0.27.Percent();
      LandfillOnTerrainManager.MAX_HEALTH_PENALTY = 40.Percent();
    }
  }
}
