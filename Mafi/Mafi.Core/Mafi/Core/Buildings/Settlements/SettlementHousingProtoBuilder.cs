// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementHousingProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public sealed class SettlementHousingProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public SettlementHousingProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public SettlementHousingProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new SettlementHousingProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<SettlementHousingProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      public readonly Lyst<string> m_materialPaths;
      private int? m_maxOccupants;
      private Upoints? m_upointsCapacity;
      private Option<SettlementHousingModuleProto> m_nextTier;
      private readonly Lyst<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>> m_unityIncreases;
      private readonly Dict<PopNeedProto, Percent> m_needsIncreases;

      public State(SettlementHousingProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_materialPaths = new Lyst<string>();
        this.m_unityIncreases = new Lyst<KeyValuePair<ImmutableArray<PopNeedProto>, Percent>>();
        this.m_needsIncreases = new Dict<PopNeedProto, Percent>();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      [MustUseReturnValue]
      public SettlementHousingProtoBuilder.State SetCapacity(int maxOccupants)
      {
        this.m_maxOccupants = new int?(maxOccupants);
        return this;
      }

      [MustUseReturnValue]
      public SettlementHousingProtoBuilder.State SetUpointsCapacity(Upoints capacity)
      {
        this.m_upointsCapacity = new Upoints?(capacity);
        return this;
      }

      [MustUseReturnValue]
      public SettlementHousingProtoBuilder.State SetNextTier(SettlementHousingModuleProto nextTier)
      {
        this.m_nextTier = (Option<SettlementHousingModuleProto>) nextTier;
        return this;
      }

      [MustUseReturnValue]
      public SettlementHousingProtoBuilder.State AddUnityIncrease(
        Percent increase,
        params PopNeedProto[] needs)
      {
        this.m_unityIncreases.Add(new KeyValuePair<ImmutableArray<PopNeedProto>, Percent>(((ICollection<PopNeedProto>) needs).ToImmutableArray<PopNeedProto>(), increase));
        return this;
      }

      [MustUseReturnValue]
      public SettlementHousingProtoBuilder.State AddNeedIncrease(
        PopNeedProto need,
        Percent increase)
      {
        Assert.That<Percent>(increase).IsPositive();
        this.m_needsIncreases.AddAndAssertNew(need, increase);
        return this;
      }

      [MustUseReturnValue]
      public SettlementHousingProtoBuilder.State AddHouseMaterial(string materialPath)
      {
        this.m_materialPaths.Add(materialPath);
        return this;
      }

      public SettlementHousingModuleProto BuildAndAdd()
      {
        SettlementHousingModuleProto.Gfx graphics = string.IsNullOrEmpty(this.PrefabPath) ? SettlementHousingModuleProto.Gfx.Empty : new SettlementHousingModuleProto.Gfx(this.PrefabPath, this.m_materialPaths.ToImmutableArray(), this.PrefabOrigin, this.CustomIconPath, this.MaterialColor, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(this.VisualizedResourcesList), categories: new ImmutableArray<ToolbarCategoryProto>?(this.GetCategoriesOrThrow()));
        return this.AddToDb<SettlementHousingModuleProto>(new SettlementHousingModuleProto(this.m_id, this.Strings, this.LayoutOrThrow, this.Costs, this.ValueOrThrow<int>(this.m_maxOccupants, "Capacity not set!"), this.ValueOrThrow<Upoints>(this.m_upointsCapacity, "Unity capacity not set!"), this.m_unityIncreases.ToImmutableArray(), (IReadOnlyDictionary<PopNeedProto, Percent>) this.m_needsIncreases, this.m_nextTier, graphics));
      }
    }
  }
}
