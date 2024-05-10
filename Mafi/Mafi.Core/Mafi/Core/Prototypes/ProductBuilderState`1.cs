// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.ProductBuilderState`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public abstract class ProductBuilderState<TState> : ProtoBuilderState<TState> where TState : ProtoBuilderState<TState>
  {
    private readonly bool m_colorIsMandatory;

    protected ProductBuilderState(
      IProtoBuilder builder,
      ProductProto.ID id,
      string name,
      string translationComment,
      bool colorIsMandatory = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CIsStorable\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, (Proto.ID) id, name, translationComment);
      this.m_colorIsMandatory = colorIsMandatory;
    }

    protected string PrefabPath { get; private set; }

    protected bool IsStorable { get; private set; }

    protected bool CanBeDiscarded { get; private set; }

    protected bool IsWaste { get; private set; }

    protected bool PinToHomeScreenByDefault { get; private set; }

    protected bool IsRecyclable { get; private set; }

    protected ColorRgba? Color { get; private set; }

    protected ColorRgba? TransportColor { get; private set; }

    protected ColorRgba? TransportAccentColor { get; private set; }

    protected int Radioactivity { get; private set; }

    protected ProductProto.ID? SourceProduct { get; private set; }

    protected Quantity? SourceProductQuantity { get; private set; }

    protected Option<string> CustomIconPath { get; private set; }

    protected ProductProto.Gfx Graphics
    {
      get
      {
        ColorRgba? nullable;
        if (this.PrefabPath == null && this.CustomIconPath == (string) null)
        {
          nullable = this.Color;
          if (!nullable.HasValue)
            return ProductProto.Gfx.Empty;
        }
        Option<string> prefabPath = (Option<string>) this.PrefabPath;
        Option<string> customIconPath = this.CustomIconPath;
        ColorRgba color;
        if (!this.m_colorIsMandatory)
        {
          nullable = this.Color;
          color = nullable.GetValueOrDefault();
        }
        else
          color = this.ValueOrThrow<ColorRgba>(this.Color, "Color");
        nullable = this.TransportColor;
        ColorRgba valueOrDefault1 = nullable.GetValueOrDefault();
        nullable = this.TransportAccentColor;
        ColorRgba valueOrDefault2 = nullable.GetValueOrDefault();
        return new ProductProto.Gfx(prefabPath, customIconPath, color, valueOrDefault1, valueOrDefault2);
      }
    }

    [MustUseReturnValue]
    public TState SetPrefabPath(string prefabPath)
    {
      this.PrefabPath = prefabPath;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCustomIconPath(string customIconPath)
    {
      this.CustomIconPath = (Option<string>) customIconPath.CheckNotNullOrEmpty();
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState CannotBeStored()
    {
      this.IsStorable = false;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetIsStorable(bool isStorable)
    {
      this.IsStorable = isStorable;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetRadioactivity(int radioactivity)
    {
      this.Radioactivity = radioactivity;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetSourceProduct(ProductProto.ID productId, int quantity)
    {
      this.SourceProduct = new ProductProto.ID?(productId);
      this.SourceProductQuantity = new Quantity?(quantity.Quantity());
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetIsWaste(bool isWaste)
    {
      this.IsWaste = isWaste;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCanBeDiscarded(bool canBeDiscarded)
    {
      this.CanBeDiscarded = canBeDiscarded;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetColor(ColorRgba color)
    {
      this.Color = new ColorRgba?(color);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetTransportColor(ColorRgba color)
    {
      this.TransportColor = new ColorRgba?(color);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetTransportAccentColor(ColorRgba color)
    {
      this.TransportAccentColor = new ColorRgba?(color);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState PinToHomeScreen()
    {
      this.PinToHomeScreenByDefault = true;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetIsRecyclable()
    {
      this.IsRecyclable = true;
      return (this as TState).CheckNotNull<TState>();
    }
  }
}
