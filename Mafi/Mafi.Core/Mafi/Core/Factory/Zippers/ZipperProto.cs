// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipperProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  public class ZipperProto : 
    LayoutEntityProto,
    IProtoWithPowerConsumption,
    ILayoutEntityProtoWithElevation
  {
    /// <summary>
    /// Shape of zipper ports. All ports of zipper have the same shape.
    /// </summary>
    public readonly IoPortShapeProto PortsShape;

    public override Type EntityType => typeof (Zipper);

    /// <summary>
    /// Electricity consumed by the zipper. Can be zero to turn off power consumption.
    /// </summary>
    public Electricity ElectricityConsumed { get; }

    /// <summary>If true, can be supported by pillars.</summary>
    public bool CanBeElevated { get; }

    /// <summary>
    /// If true, pillars can pass through this entity to support a higher entity.
    /// </summary>
    public bool CanPillarsPassThrough => true;

    public ZipperProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity electricityConsumed,
      bool canBeElevated,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      ImmutableArray<IoPortTemplate> immutableArray = !this.Ports.IsEmpty ? this.Ports : throw new ProtoBuilderException(string.Format("Zipper '{0}' has no ports.", (object) id));
      this.PortsShape = immutableArray[0].Shape;
      this.ElectricityConsumed = electricityConsumed;
      this.CanBeElevated = canBeElevated;
      immutableArray = this.Ports;
      if (immutableArray.Any((Func<IoPortTemplate, bool>) (p => (Proto) p.Shape != (Proto) this.PortsShape)))
        throw new ProtoBuilderException(string.Format("Some ports of zipper '{0}' incompatible ports, ", (object) id) + "all port shapes must have the same shape.");
    }
  }
}
