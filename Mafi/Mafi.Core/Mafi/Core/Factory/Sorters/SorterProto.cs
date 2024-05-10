// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Sorters.SorterProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Factory.Sorters
{
  [DebuggerDisplay("SorterProto: {Id}")]
  public class SorterProto : 
    LayoutEntityProto,
    IProtoWithPowerConsumption,
    ILayoutEntityProtoWithElevation
  {
    /// <summary>
    /// Filter for <see cref="P:Mafi.Core.Factory.Sorters.SorterProto.AssignableProducts" />.
    /// </summary>
    private readonly Func<ProductProto, bool> m_productsFilter;

    public override Type EntityType => typeof (Sorter);

    /// <summary>
    /// Products that can be assigned (in the UI) to the special sorter port.
    /// </summary>
    public IReadOnlySet<ProductProto> AssignableProducts { get; private set; }

    /// <summary>If true, can be supported by pillars.</summary>
    public bool CanBeElevated { get; }

    /// <summary>
    /// If true, pillars can pass through this entity to support a higher entity.
    /// </summary>
    public bool CanPillarsPassThrough => true;

    /// <summary>Electricity consumed by the sorter.</summary>
    public Electricity ElectricityConsumed { get; }

    public SorterProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      Func<ProductProto, bool> productsFilter,
      EntityCosts costs,
      Electricity requiredPower,
      bool canBeElevated,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.m_productsFilter = productsFilter.CheckNotNull<Func<ProductProto, bool>>();
      this.ElectricityConsumed = requiredPower.CheckNotNegative();
      this.CanBeElevated = canBeElevated;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      this.AssignableProducts = (IReadOnlySet<ProductProto>) new Set<ProductProto>(protosDb.Filter<ProductProto>(this.m_productsFilter));
    }
  }
}
