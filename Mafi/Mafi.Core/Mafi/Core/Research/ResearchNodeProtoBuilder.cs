// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchNodeProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.UnlockingTree;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Research
{
  public sealed class ResearchNodeProtoBuilder : IProtoBuilder
  {
    private readonly Dict<ResearchNodeProto.ID, ResearchCostsTpl> m_costsFromAttrs;

    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public ResearchNodeProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_costsFromAttrs = new Dict<ResearchNodeProto.ID, ResearchCostsTpl>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public ResearchNodeProtoBuilder.State Start(
      string name,
      ResearchNodeProto.ID nodeId,
      string translationComment = "title of a research node in the research tree")
    {
      return new ResearchNodeProtoBuilder.State(this, nodeId, name, translationComment);
    }

    internal void SetResearchCost(ResearchNodeProto.ID id, ResearchCostsTpl value)
    {
      this.m_costsFromAttrs[id] = value;
    }

    public class State : ProtoBuilderState<ResearchNodeProtoBuilder.State>
    {
      private readonly ResearchNodeProtoBuilder m_parentBuilder;
      private readonly ResearchNodeProto.ID m_protoId;
      private readonly Set<ResearchNodeProto> m_parents;
      private readonly Set<IResearchNodeUnlockingCondition> m_unlockConditions;
      private readonly Set<IUnlockNodeUnit> m_units;
      private readonly Lyst<KeyValuePair<Option<Proto>, string>> m_nodeIcons;
      private readonly Set<string> m_allIcons;
      private Vector2i m_gridPosition;
      private bool m_anyParentCanUnlock;
      private int? m_difficulty;

      public IReadOnlySet<IUnlockNodeUnit> Units => (IReadOnlySet<IUnlockNodeUnit>) this.m_units;

      public State(
        ResearchNodeProtoBuilder builder,
        ResearchNodeProto.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_parents = new Set<ResearchNodeProto>();
        this.m_unlockConditions = new Set<IResearchNodeUnlockingCondition>();
        this.m_units = new Set<IUnlockNodeUnit>();
        this.m_nodeIcons = new Lyst<KeyValuePair<Option<Proto>, string>>();
        this.m_allIcons = new Set<string>();
        this.m_gridPosition = Vector2i.Zero;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (Proto.ID) protoId, name, translationComment);
        this.m_parentBuilder = builder;
        this.m_protoId = protoId;
      }

      [MustUseReturnValue]
      public override ResearchNodeProtoBuilder.State Description(
        string description,
        string explanation = "description of a research node in the research tree")
      {
        return base.Description(description, explanation);
      }

      [MustUseReturnValue]
      public ResearchNodeProtoBuilder.State UseDescriptionFrom(Proto.ID protoId)
      {
        this.DescShort = this.m_parentBuilder.ProtosDb.GetOrThrow<Proto>(protoId).Strings.DescShort;
        return this;
      }

      /// <summary>Sets nodes as parents of the current one.</summary>
      public ResearchNodeProtoBuilder.State AddParents(params ResearchNodeProto[] parents)
      {
        foreach (ResearchNodeProto parent in parents)
        {
          if (!this.m_parents.Add(parent))
            Log.Warning(string.Format("Parent '{0}' of node '{1}' was duplicated, ignoring.", (object) parent, (object) this.Id));
        }
        return this;
      }

      public ResearchNodeProtoBuilder.State SetCosts(ResearchCostsTpl costs)
      {
        this.m_difficulty = !this.m_difficulty.HasValue ? new int?(costs.Difficulty.CheckNotNegative()) : throw new ProtoBuilderException(string.Format("Costs of Research node '{0}' was already set.", (object) this.Id));
        return this;
      }

      /// <summary>
      /// Adds proto that will be unlocked by the research node.
      /// </summary>
      public ResearchNodeProtoBuilder.State AddUnit(IUnlockNodeUnit unit)
      {
        this.m_units.AddAndAssertNew(unit);
        return this;
      }

      /// <summary>
      /// Adds icon to be displayed in the node visualization of the research tree.
      /// </summary>
      public ResearchNodeProtoBuilder.State AddIcon(Option<Proto> owner, string iconPath)
      {
        if (this.m_allIcons.Contains(iconPath))
          return this;
        this.m_allIcons.Add(iconPath);
        this.m_nodeIcons.Add(new KeyValuePair<Option<Proto>, string>(owner, iconPath));
        return this;
      }

      /// <summary>
      /// Adds proto that will be unlocked by the research node.
      /// </summary>
      public ResearchNodeProtoBuilder.State AddProductIcon(ProductProto.ID protoId)
      {
        ProductProto orThrow = this.Builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) protoId);
        if (this.m_allIcons.Contains(orThrow.Graphics.IconPath))
          return this;
        this.m_allIcons.Add(orThrow.Graphics.IconPath);
        this.m_nodeIcons.Add(new KeyValuePair<Option<Proto>, string>(orThrow.SomeOption<ProductProto>().As<Proto>(), orThrow.Graphics.IconPath));
        return this;
      }

      /// <summary>
      /// Adds proto that must be unlocked before this research is available.
      /// </summary>
      public ResearchNodeProtoBuilder.State AddRequiredProto(Proto.ID protoId)
      {
        this.m_unlockConditions.Add((IResearchNodeUnlockingCondition) new UnlockingConditionProtoRequired(this.Builder.ProtosDb.GetOrThrow<Proto>(protoId)));
        return this;
      }

      public ResearchNodeProtoBuilder.State AddRequirementForLifetimeProduction(
        ProductProto.ID productId,
        int quantityRequired)
      {
        this.m_unlockConditions.Add((IResearchNodeUnlockingCondition) new UnlockingConditionGlobalStats(this.m_parentBuilder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) productId), quantityRequired, (Func<ProductStats, QuantityLarge>) (stats => stats.CreatedByProduction.Lifetime), UnlockingConditionGlobalStats.LIFETIME_PRODUCTION));
        return this;
      }

      /// <summary>
      /// Sets that only one parent is required to unlock the current node. Otherwise all the parents have to be
      /// researched before.
      /// </summary>
      public ResearchNodeProtoBuilder.State SetAnyParentCanUnlock()
      {
        this.m_anyParentCanUnlock = true;
        return this;
      }

      public ResearchNodeProtoBuilder.State SetGridPosition(Vector2i gridPosition)
      {
        Assert.That<int>(this.m_gridPosition.X).IsNotNegative();
        Assert.That<int>(this.m_gridPosition.Y).IsNotNegative();
        this.m_gridPosition = gridPosition;
        return this;
      }

      public ResearchNodeProto BuildAndAdd()
      {
        ResearchNodeProto.Gfx graphics = new ResearchNodeProto.Gfx(this.m_nodeIcons.ToImmutableArray());
        ResearchCostsTpl costs;
        if (this.m_parentBuilder.m_costsFromAttrs.TryGetValue(this.m_protoId, out costs))
        {
          if (this.m_difficulty.HasValue)
            throw new ProtoBuilderException(string.Format("Cost of '{0}' was set to builder and using ", (object) this.m_protoId) + "ResearchCostsAttribute. This is not allowed, pick one.");
          this.SetCosts(costs);
        }
        return this.AddToDb<ResearchNodeProto>(new ResearchNodeProto(this.m_protoId, this.Strings, this.m_parents.ToImmutableArray<ResearchNodeProto>(), this.m_units.ToImmutableArray<IUnlockNodeUnit>(), this.m_unlockConditions.ToImmutableArray<IResearchNodeUnlockingCondition>(), this.ValueOrThrow<int>(this.m_difficulty, "Difficulty not provided!"), this.m_gridPosition, this.m_anyParentCanUnlock, graphics));
      }

      private Quantity roundQuantity(Quantity quantity)
      {
        if (quantity <= new Quantity(10))
          return quantity;
        return quantity <= new Quantity(100) ? new Quantity(((float) quantity.Value / 10f).RoundToInt() * 10) : new Quantity(((float) quantity.Value / 100f).RoundToInt() * 100);
      }
    }
  }
}
