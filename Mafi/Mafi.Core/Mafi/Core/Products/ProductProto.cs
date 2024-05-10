// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Products
{
  public class ProductProto : 
    Proto,
    IProtoWithIconAndName,
    IProtoWithIcon,
    IProto,
    IProtoWithSlimID<ProductSlimId>,
    IEquatable<ProductProto>
  {
    protected static readonly ProductProto.ID PHANTOM_PRODUCT_ID;
    /// <summary>
    /// Phantom prototype does not represent any valid prototype and serves as convenient placeholder to avoid
    /// redundant null checks or unnecessary usage of Option{T}. This is also useful for unit tests. Phantom
    /// prototype should be NEVER returned through public interface of the class that uses it.
    /// </summary>
    public static readonly ProductProto Phantom;
    /// <summary>Maximum stacking quantity per transported product.</summary>
    public readonly Mafi.Quantity MaxQuantityPerTransportedProduct;
    public readonly bool IsStorable;
    public readonly int Radioactivity;
    public readonly bool PinToHomeScreenByDefault;
    public readonly bool IsExcludedFromStats;
    /// <summary>
    /// Whether this product can be freely thrown away when truck can's store it anywhere.
    /// All non-storable products can be also discarded.
    /// </summary>
    public readonly bool CanBeDiscarded;
    /// <summary>
    /// Whether this product can be destroyed by player when explicitly deleting it from
    /// the shipyard.
    /// </summary>
    public readonly bool CanBeDestroyedInShipyard;
    /// <summary>
    /// Waste should not be insta-discarded when clearing transports for instance.
    /// </summary>
    public readonly bool IsWaste;
    /// <summary>
    /// If this product can be part of Recyclables mix and thus also extracted from it by a sorting facility.
    /// </summary>
    public readonly bool IsRecyclable;
    private ProductSlimId m_slimId;
    private readonly ProductProto.ID? m_sourceProduct;
    private readonly PartialQuantity m_recycledIntoQuantity;

    /// <summary>
    /// Allows to formats quantity of the current product with proper units so it can displayed to the player.
    /// </summary>
    public QuantityFormatter QuantityFormatter { get; }

    public ProductProto.Gfx Graphics { get; }

    public string IconPath => this.Graphics.IconPath;

    public ProductType Type { get; }

    public bool CanBeLoadedOnTruck => this.IsStorable;

    public virtual bool TrackSourceProducts { get; }

    /// <summary>
    /// Whether this product can be mixed with other products on trucks.
    /// </summary>
    public bool IsMixable
    {
      get => this is LooseProductProto looseProductProto && looseProductProto.CanBeOnTerrain;
    }

    /// <summary>Slim ID of this material.</summary>
    public ProductSlimId SlimId => this.m_slimId;

    /// <summary>
    /// Used for products like power, or computing where normalization makes no sense.
    /// </summary>
    public bool DoNotNormalize { get; private set; }

    /// <summary>
    /// Has value if this is a loose dumpable product. Used for fast checking to reduce casting.
    /// </summary>
    public Option<LooseProductProto> DumpableProduct { get; protected set; }

    /// <summary>
    /// Product that should be tracked as a source product.
    /// E.g. metal scrap for iron or bioWaste for food.
    /// </summary>
    public PartialProductQuantity SourceProduct { get; private set; }

    public ProductProto(
      ProductProto.ID id,
      Proto.Str strings,
      Mafi.Quantity maxQuantityPerTransportedProduct,
      bool isStorable,
      bool canBeDiscarded,
      bool isWaste,
      ProductProto.Gfx graphics,
      bool doNotNormalize = false,
      bool isExcludedFromStats = false,
      int radioactivity = 0,
      bool pinToHomeScreenByDefault = false,
      bool isRecyclable = false,
      bool doNotTrackSourceProducts = false,
      ProductProto.ID? sourceProduct = null,
      PartialQuantity? sourceProductQuantity = null,
      QuantityFormatter quantityFormatter = null,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings, tags);
      if (maxQuantityPerTransportedProduct.IsNotPositive)
        throw new InvalidProtoException(string.Format("Max quantity per transported product of '{0}' is not positive: {1}", (object) id, (object) maxQuantityPerTransportedProduct));
      if (isExcludedFromStats & pinToHomeScreenByDefault)
        throw new ProtoBuilderException("Product excluded from stats cannot be displayed on StatusBar.");
      if (!isStorable & isWaste)
      {
        Mafi.Log.Error(string.Format("You can't have un-storable waste {0}!", (object) id));
        isStorable = true;
      }
      this.MaxQuantityPerTransportedProduct = maxQuantityPerTransportedProduct;
      this.IsStorable = isStorable;
      this.CanBeDiscarded = canBeDiscarded || !isStorable;
      this.IsWaste = isWaste;
      this.QuantityFormatter = quantityFormatter ?? (QuantityFormatter) ProductCountQuantityFormatter.Instance;
      this.DoNotNormalize = doNotNormalize;
      this.PinToHomeScreenByDefault = pinToHomeScreenByDefault;
      this.IsRecyclable = isRecyclable;
      this.TrackSourceProducts = !doNotTrackSourceProducts;
      this.Radioactivity = radioactivity;
      this.IsExcludedFromStats = isExcludedFromStats;
      this.m_sourceProduct = sourceProduct;
      this.m_recycledIntoQuantity = sourceProductQuantity ?? 1.Quantity().AsPartial;
      this.Graphics = graphics.CheckNotNull<ProductProto.Gfx>();
      this.Type = new ProductType(this.GetType());
      this.Graphics.Initialize(this);
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      if (this.m_sourceProduct.HasValue)
        this.SourceProduct = new PartialProductQuantity(protosDb.GetOrThrow<ProductProto>((Proto.ID) this.m_sourceProduct.Value), this.m_recycledIntoQuantity);
      else
        this.SourceProduct = PartialProductQuantity.None;
    }

    void IProtoWithSlimID<ProductSlimId>.SetSlimId(ProductSlimId id)
    {
      if (this.m_slimId.Value != (byte) 0 && this.m_slimId != id)
        throw new InvalidOperationException(string.Format("Slim ID of '{0}' was already set to '{1}'.", (object) this, (object) this.m_slimId));
      this.m_slimId = id;
    }

    public bool Equals(ProductProto other) => this.CompareTo((Proto) other) == 0;

    public override string ToString() => this.Id.Value + " (" + this.GetType().Name + ")";

    public ProductProto.ID Id => new ProductProto.ID(base.Id.Value);

    static ProductProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductProto.PHANTOM_PRODUCT_ID = new ProductProto.ID("__PHANTOM__PRODUCT__");
      ProductProto.Phantom = Proto.RegisterPhantom<ProductProto>(new ProductProto(ProductProto.PHANTOM_PRODUCT_ID, Proto.Str.Empty, Mafi.Quantity.One, false, false, false, ProductProto.Gfx.Empty));
    }

    [ManuallyWrittenSerialization]
    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    public new readonly struct ID : IEquatable<ProductProto.ID>, IComparable<ProductProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      [Pure]
      public AssetValue ToAssetValue(int quantity, ProtosDb db)
      {
        return new AssetValue(new ProductQuantity(db.GetOrThrow<ProductProto>((Proto.ID) this), new Mafi.Quantity(quantity)));
      }

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(ProductProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, ProductProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(ProductProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, ProductProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(ProductProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(ProductProto.ID lhs, ProductProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(ProductProto.ID lhs, ProductProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is ProductProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(ProductProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(ProductProto.ID other) => string.CompareOrdinal(this.Value, other.Value);

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(ProductProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static ProductProto.ID Deserialize(BlobReader reader)
      {
        return new ProductProto.ID(reader.ReadString());
      }
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly ProductProto.Gfx Empty;
      /// <summary>Color that is associated with this product.</summary>
      public readonly ColorRgba Color;
      public readonly ColorRgba TransportColor;
      public readonly ColorRgba TransportAccentColor;

      public Option<string> PrefabsPath { get; }

      /// <summary>Icon asset path to be used in UI.</summary>
      /// <remarks>This path is valid only after <see cref="M:Mafi.Core.Products.ProductProto.Gfx.Initialize(Mafi.Core.Products.ProductProto)" /> was called.</remarks>
      internal string IconPath { get; private set; }

      /// <summary>
      /// Whether custom icon path was set. Otherwise, icon path is automatically generated.
      /// </summary>
      public bool IconIsCustom { get; }

      public Gfx(
        Option<string> prefabPath,
        Option<string> customIconPath,
        ColorRgba color = default (ColorRgba),
        ColorRgba transportColor = default (ColorRgba),
        ColorRgba transportAccentColor = default (ColorRgba))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        if (prefabPath.IsNone && customIconPath.IsNone)
          throw new ProtoBuilderException("Product GFX must have either prefab of custom icon set.");
        this.PrefabsPath = prefabPath;
        this.IconPath = customIconPath.ValueOrNull;
        this.IconIsCustom = customIconPath.HasValue;
        this.Color = color.IsEmpty ? ColorRgba.Magenta : color;
        this.TransportColor = transportColor.IsEmpty ? ColorRgba.Magenta : transportColor;
        this.TransportAccentColor = transportAccentColor.IsEmpty ? ColorRgba.Magenta : transportAccentColor;
      }

      protected Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconPath = "TODO: Phantom icon.";
        this.IconIsCustom = true;
      }

      public void Initialize(ProductProto proto)
      {
        Mafi.Assert.That<ProductProto.Gfx>(proto.Graphics).IsEqualTo<ProductProto.Gfx>(this);
        if (this == ProductProto.Gfx.Empty || !string.IsNullOrEmpty(this.IconPath))
          return;
        this.IconPath = string.Format("{0}/Product/{1}.png", (object) "Assets/Unity/Generated/Icons", (object) proto.Id);
      }

      public virtual ImmutableArray<RelTile3f> GetStackingOffsets(
        float sizeX,
        float sizeY,
        float sizeZ)
      {
        RelTile1f relTile1f = 0.5.Meters();
        ThicknessTilesF zero = ThicknessTilesF.Zero;
        return ImmutableArray.Create<RelTile3f>(new RelTile3f(relTile1f, relTile1f, zero), new RelTile3f(relTile1f, relTile1f, zero), new RelTile3f(relTile1f, relTile1f, zero), new RelTile3f(relTile1f, relTile1f, zero), new RelTile3f(relTile1f, relTile1f, zero));
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ProductProto.Gfx.Empty = new ProductProto.Gfx();
      }
    }
  }
}
