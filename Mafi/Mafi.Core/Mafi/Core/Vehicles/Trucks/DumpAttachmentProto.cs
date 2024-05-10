// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.DumpAttachmentProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks
{
  /// <summary>
  /// Attachment that represent a dump bed. This proto mainly defines that cargo of such attachment should be
  /// visualized using the defined prefab.
  /// </summary>
  public class DumpAttachmentProto : AttachmentProto
  {
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly DumpAttachmentProto.Gfx Graphics;

    public DumpAttachmentProto(Proto.ID id, DumpAttachmentProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (Predicate<ProductProto>) (x => x.Type == LooseProductProto.ProductType), (AttachmentProto.Gfx) graphics, true);
      this.Graphics = graphics.CheckNotNull<DumpAttachmentProto.Gfx>();
    }

    public new class Gfx : AttachmentProto.Gfx
    {
      public static readonly DumpAttachmentProto.Gfx Empty;
      public readonly string SmoothPileObjectPath;
      public readonly string RoughPileObjectPath;
      public readonly LoosePileTextureParams PileTextureParams;
      /// <summary>
      /// Offset (local position) within paren when cargo is nearly empty. Offset is linearly interpolated between
      /// empty and full states based on cargo amount. This value is in Unity coordinate space (meters).
      /// </summary>
      public readonly Vector3f OffsetEmpty;
      /// <summary>
      /// Offset (local position) within paren when cargo is full. Offset is linearly interpolated between empty
      /// and full states based on cargo amount. This value is in Unity coordinate space (meters).
      /// </summary>
      public readonly Vector3f OffsetFull;
      /// <summary>
      /// When set, pile will be operated using animation instead of offsets.
      /// </summary>
      public readonly Option<string> AnimationStateName;

      public Gfx(
        string prefabPath,
        string smoothPileObjectPath,
        string roughPileObjectPath,
        LoosePileTextureParams pileTextureParams,
        string animationStateName)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath);
        this.AnimationStateName = (Option<string>) animationStateName.CheckNotNullOrEmpty();
        this.SmoothPileObjectPath = smoothPileObjectPath;
        this.RoughPileObjectPath = roughPileObjectPath;
        this.PileTextureParams = pileTextureParams;
      }

      public Gfx(
        string prefabPath,
        string smoothPileObjectPath,
        string roughPileObjectPath,
        LoosePileTextureParams pileTextureParams,
        Vector3f offsetEmpty,
        Vector3f offsetFull)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath);
        this.PileTextureParams = pileTextureParams;
        this.SmoothPileObjectPath = smoothPileObjectPath;
        this.RoughPileObjectPath = roughPileObjectPath;
        this.OffsetEmpty = offsetEmpty;
        this.OffsetFull = offsetFull;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        DumpAttachmentProto.Gfx.Empty = new DumpAttachmentProto.Gfx("EMPTY", "EMPTY", "EMPTY", LoosePileTextureParams.Default, Vector3f.Zero, Vector3f.Zero);
      }
    }
  }
}
