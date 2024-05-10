// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.LooseProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  public class LooseProductProto : 
    ProductProto,
    IProtoWithSlimID<LooseProductSlimId>,
    IComparable<LooseProductProto>
  {
    public static readonly ProductType ProductType;
    /// <summary>
    /// Phantom prototype does not represent any valid prototype and serves as convenient placeholder to avoid
    /// redundant null checks or unnecessary usage of Option{T}. This is also useful for unit tests. Phantom
    /// prototype should be NEVER returned through public interface of the class that uses it.
    /// </summary>
    public static readonly LooseProductProto Phantom;
    /// <summary>
    /// Whether this loose product is dumped on terrain by default. Otherwise, player has to mark the product for
    /// dumping explicitly.
    /// </summary>
    public readonly bool IsDumpedOnTerrainByDefault;
    public readonly LooseProductProto.Gfx Graphics;
    private LooseProductSlimId m_slimId;

    /// <summary>
    /// If set, this product can be dumped on the terrain and will transform to this material.
    /// </summary>
    public Option<TerrainMaterialProto> TerrainMaterial { get; private set; }

    /// <summary>Whether this product can be dumped on the terrain.</summary>
    public bool CanBeOnTerrain => this.TerrainMaterial.HasValue;

    /// <summary>Slim ID of this material.</summary>
    public LooseProductSlimId LooseSlimId => this.m_slimId;

    LooseProductSlimId IProtoWithSlimID<LooseProductSlimId>.SlimId => this.LooseSlimId;

    public LooseProductProto(
      ProductProto.ID id,
      Proto.Str strings,
      bool isDumpedOnTerrainByDefault,
      bool isStorable,
      LooseProductProto.Gfx graphics,
      bool isWaste = false,
      bool pinToHomeScreenByDefault = false,
      bool isRecyclable = false,
      bool doNotTrackSourceProducts = false,
      ProductProto.ID? sourceProduct = null,
      PartialQuantity? sourceProductQuantity = null,
      Quantity? maxQuantityPerTransportedProduct = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductProto.ID id1 = id;
      Proto.Str strings1 = strings;
      Quantity maxQuantityPerTransportedProduct1 = maxQuantityPerTransportedProduct ?? 5.Quantity();
      int num1 = isStorable ? 1 : 0;
      int num2 = isWaste ? 1 : 0;
      bool flag1 = pinToHomeScreenByDefault;
      ProductProto.ID? nullable1 = sourceProduct;
      PartialQuantity? nullable2 = sourceProductQuantity;
      bool flag2 = isRecyclable;
      bool flag3 = doNotTrackSourceProducts;
      LooseProductProto.Gfx graphics1 = graphics;
      int num3 = flag1 ? 1 : 0;
      int num4 = flag2 ? 1 : 0;
      int num5 = flag3 ? 1 : 0;
      ProductProto.ID? sourceProduct1 = nullable1;
      PartialQuantity? sourceProductQuantity1 = nullable2;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, maxQuantityPerTransportedProduct1, num1 != 0, false, num2 != 0, (ProductProto.Gfx) graphics1, pinToHomeScreenByDefault: num3 != 0, isRecyclable: num4 != 0, doNotTrackSourceProducts: num5 != 0, sourceProduct: sourceProduct1, sourceProductQuantity: sourceProductQuantity1);
      this.IsDumpedOnTerrainByDefault = isDumpedOnTerrainByDefault;
      this.Graphics = graphics.CheckNotNull<LooseProductProto.Gfx>();
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      LooseProductParam paramValue;
      if (!this.TryGetParam<LooseProductParam>(out paramValue))
        return;
      TerrainMaterialProto proto;
      if (protosDb.TryGetProto<TerrainMaterialProto>(paramValue.DumpAs, out proto))
      {
        this.TerrainMaterial = (Option<TerrainMaterialProto>) proto;
        this.DumpableProduct = (Option<LooseProductProto>) this;
      }
      else
        Log.Error(string.Format("Failed to find dumped material '{0}' for product '{1}'.", (object) paramValue.DumpAs, (object) this.Id));
    }

    public int CompareTo(LooseProductProto other) => this.CompareTo((Proto) other);

    public override string ToString() => string.Format("{0} (loose)", (object) this.Id);

    void IProtoWithSlimID<LooseProductSlimId>.SetSlimId(LooseProductSlimId id)
    {
      if (this.m_slimId.Value != (byte) 0 && this.m_slimId != id)
        throw new InvalidOperationException(string.Format("Slim ID of '{0}' was already set to '{1}'.", (object) this, (object) this.m_slimId));
      this.m_slimId = id;
    }

    static LooseProductProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LooseProductProto.ProductType = new ProductType(typeof (LooseProductProto));
      LooseProductProto.Phantom = Proto.RegisterPhantom<LooseProductProto>(new LooseProductProto(new ProductProto.ID(ProductProto.PHANTOM_PRODUCT_ID.Value + "LOOSE__"), Proto.Str.Empty, false, false, LooseProductProto.Gfx.Empty));
    }

    public new class Gfx : ProductProto.Gfx
    {
      public static readonly LooseProductProto.Gfx Empty;
      /// <summary>Material for the pile prefabs.</summary>
      public readonly string PileMaterialAssetPath;
      /// <summary>
      /// Whether to use rough pile meshes (like rock or ore), otherwise, smoother meshes are used (like sand).
      /// </summary>
      public readonly bool UseRoughPileMeshes;
      /// <summary>
      /// Color that should be used for the material when visualizing it in the resources visualization.
      /// </summary>
      public ColorRgba ResourcesVizColor;

      /// <summary>
      /// Whether this material should be displayed as a resource in the resources visualization.
      /// </summary>
      public bool DisplayInResources => this.ResourcesVizColor.IsNotEmpty;

      public Gfx(
        string prefabPath,
        string pileMaterialAssetPath,
        bool useRoughPileMeshes,
        ColorRgba resourcesVizColor,
        Option<string> customIconPath = default (Option<string>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((Option<string>) prefabPath, customIconPath);
        this.PileMaterialAssetPath = pileMaterialAssetPath;
        this.UseRoughPileMeshes = useRoughPileMeshes;
        this.ResourcesVizColor = resourcesVizColor;
      }

      private Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PileMaterialAssetPath = "";
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        LooseProductProto.Gfx.Empty = new LooseProductProto.Gfx();
      }
    }
  }
}
