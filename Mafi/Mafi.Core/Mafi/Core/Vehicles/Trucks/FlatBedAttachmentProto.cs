// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.FlatBedAttachmentProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Numerics;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks
{
  /// <summary>
  /// Attachment that represent a flat bed. This proto mainly defines that cargo of such attachment should be
  /// visualized using the defined prefab.
  /// </summary>
  public class FlatBedAttachmentProto : AttachmentProto
  {
    public FlatBedAttachmentProto(
      Proto.ID id,
      Predicate<ProductProto> eligibleProductsFilter,
      FlatBedAttachmentProto.Gfx graphics,
      bool keepOnEvenIfNotNeeded)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, eligibleProductsFilter, (AttachmentProto.Gfx) graphics, keepOnEvenIfNotNeeded);
    }

    public new class Gfx : AttachmentProto.Gfx
    {
      public static readonly FlatBedAttachmentProto.Gfx Empty;
      /// <summary>The maximum amount of products we can render</summary>
      public int maxProductRenderCapacity;
      /// <summary>
      /// The offsets to render the products. We expect to render 3 products at each position
      /// </summary>
      public ImmutableArray<Vector3> productRenderOffsets;
      private readonly int Level;

      public Gfx(
        int level,
        string prefabPath,
        string customIconPath = null,
        Func<ProductProto, ColorRgba> colorsMap = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, colorsMap);
        this.Level = level;
      }

      internal override void Initialize(AttachmentProto proto)
      {
        Assert.That<AttachmentProto.Gfx>(proto.Graphics).IsEqualTo<AttachmentProto.Gfx>((AttachmentProto.Gfx) this);
        if (this.Level == 1)
        {
          float y = 0.95f;
          this.maxProductRenderCapacity = 9;
          this.productRenderOffsets = ImmutableArray.Create<Vector3>(new Vector3(-0.45f, y, 0.0f), new Vector3(-1.575f, y, 0.0f), new Vector3(-2.7f, y, 0.0f));
        }
        else if (this.Level == 2)
        {
          float y = 1.38f;
          this.maxProductRenderCapacity = 24;
          this.productRenderOffsets = ImmutableArray.Create<Vector3>(new Vector3(0.95f, y, -0.6f), new Vector3(0.95f, y, 0.6f), new Vector3(-0.175f, y, -0.6f), new Vector3(-0.175f, y, 0.6f), new Vector3(-1.3f, y, -0.6f), new Vector3(-1.3f, y, 0.6f), new Vector3(-2.425f, y, -0.6f), new Vector3(-2.425f, y, 0.6f));
        }
        else
          this.maxProductRenderCapacity = 0;
        base.Initialize(proto);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FlatBedAttachmentProto.Gfx.Empty = new FlatBedAttachmentProto.Gfx(0, "EMPTY", "EMPTY");
      }
    }
  }
}
