// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.AttachmentProto
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
  /// <summary>Attachment for a vehicle.</summary>
  public class AttachmentProto : Proto
  {
    /// <summary>
    /// Filter for products that the vehicles needs to work with to have this attachment.
    /// </summary>
    public readonly Predicate<ProductProto> EligibleProductsFilter;
    /// <summary>
    /// Whether the attachment should be kept on the vehicle even if it is not needed anymore. Otherwise the
    /// attachments is removed. E.g. after truck dumps stuff on the terrain we don't want the dump bed to disappear.
    /// </summary>
    public readonly bool KeepOnEvenIfNotNeeded;
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly AttachmentProto.Gfx Graphics;

    public AttachmentProto(
      Proto.ID id,
      Predicate<ProductProto> eligibleProductsFilter,
      AttachmentProto.Gfx graphics,
      bool keepOnEvenIfNotNeeded)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.EligibleProductsFilter = eligibleProductsFilter;
      this.KeepOnEvenIfNotNeeded = keepOnEvenIfNotNeeded;
      this.Graphics = graphics;
      graphics.Initialize(this);
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly AttachmentProto.Gfx Empty;
      /// <summary>3D model of this entity.</summary>
      public readonly string PrefabPath;
      /// <summary>
      /// Maps product to attachment's color. Color of attachment should not be changed if <see cref="F:Mafi.ColorRgba.Empty" /> is returned.
      /// </summary>
      public readonly Func<ProductProto, ColorRgba> ColorsMap;
      /// <summary>
      /// Whether custom icon path was set. Otherwise, icon path is automatically generated.
      /// </summary>
      public readonly bool IconIsCustom;

      /// <summary>
      /// Path for icon that shows a truck with this attachment.
      /// </summary>
      /// <remarks>This path is valid only after <see cref="M:Mafi.Core.Vehicles.Trucks.AttachmentProto.Gfx.Initialize(Mafi.Core.Vehicles.Trucks.AttachmentProto)" /> was called.</remarks>
      public string IconPath { get; private set; }

      public Gfx(string prefabPath, string customIconPath = null, Func<ProductProto, ColorRgba> colorsMap = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PrefabPath = prefabPath;
        this.IconPath = customIconPath;
        this.IconIsCustom = customIconPath != null;
        this.ColorsMap = colorsMap ?? (Func<ProductProto, ColorRgba>) (x => ColorRgba.Empty);
      }

      internal virtual void Initialize(AttachmentProto proto)
      {
        Assert.That<AttachmentProto.Gfx>(proto.Graphics).IsEqualTo<AttachmentProto.Gfx>(this);
        if (this.IconIsCustom)
          return;
        Assert.That<string>(this.IconPath).IsNull<string>();
        this.IconPath = string.Format("{0}/Vehicle/{1}.png", (object) "Assets/Unity/Generated/Icons", (object) proto.Id);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        AttachmentProto.Gfx.Empty = new AttachmentProto.Gfx("EMPTY", "EMPTY");
      }
    }
  }
}
