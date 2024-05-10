// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.TankAttachmentProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks
{
  /// <summary>
  /// Attachment that represent a flat bed. This proto mainly defines that cargo of such attachment should be
  /// visualized using the defined prefab.
  /// </summary>
  public class TankAttachmentProto : AttachmentProto
  {
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly TankAttachmentProto.Gfx Graphics;

    public TankAttachmentProto(
      Proto.ID id,
      Predicate<ProductProto> eligibleProductsFilter,
      TankAttachmentProto.Gfx graphics,
      bool keepOnEvenIfNotNeeded)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, eligibleProductsFilter, (AttachmentProto.Gfx) graphics, keepOnEvenIfNotNeeded);
      this.Graphics = graphics;
    }

    public new class Gfx : AttachmentProto.Gfx
    {
      public readonly string IconChildName;
      public readonly string BodyChildName;
      public readonly ColorRgba DefaultColor;
      public readonly ColorRgba DefaultAccentColor;
      public static readonly TankAttachmentProto.Gfx Empty;

      public Gfx(
        string prefabPath,
        string iconChildName,
        string bodyChildName,
        ColorRgba defaultColor,
        ColorRgba defaultAccentColor,
        string customIconPath = null,
        Func<ProductProto, ColorRgba> colorsMap = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, colorsMap);
        this.IconChildName = iconChildName;
        this.BodyChildName = bodyChildName;
        this.DefaultColor = defaultColor;
        this.DefaultAccentColor = defaultAccentColor;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TankAttachmentProto.Gfx.Empty = new TankAttachmentProto.Gfx("EMPTY", "EMPTY", "EMPTY", new ColorRgba(), new ColorRgba());
      }
    }
  }
}
