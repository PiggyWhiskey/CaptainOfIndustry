// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.ProductCell
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  public class ProductCell : IUiElement
  {
    private readonly Panel m_panel;
    private readonly IconContainer m_icon;
    private readonly Txt m_productNameLabel;
    private readonly UiBuilder m_builder;

    public GameObject GameObject => this.m_panel.GameObject;

    public RectTransform RectTransform => this.m_panel.RectTransform;

    public Option<ProductProto> Product { get; private set; }

    public ProductCell(UiBuilder builder, string name, int iconSize, int iconTextSpace)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_panel = builder.NewPanel(name);
      this.m_icon = builder.NewIconContainer("ProductIcon").PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_panel, new Vector2((float) iconSize, (float) iconSize));
      this.m_productNameLabel = builder.NewTxt("ProductName").PutTo<Txt>((IUiElement) this.m_panel, Offset.Left((float) (iconSize + iconTextSpace)));
    }

    public ProductCell SetTextStyle(TextStyle textStyle)
    {
      this.m_productNameLabel.SetTextStyle(textStyle);
      return this;
    }

    public ProductCell SetTextAlignment(TextAnchor anchor)
    {
      this.m_productNameLabel.SetAlignment(anchor);
      return this;
    }

    public ProductCell SetProduct(Option<ProductProto> product)
    {
      this.Product = product;
      if (product.IsNone)
      {
        this.m_productNameLabel.SetText("");
        this.m_icon.SetIcon(this.m_builder.Style.Icons.Empty, this.m_builder.Style.Global.Text.Color);
        return this;
      }
      this.m_productNameLabel.SetText((LocStrFormatted) product.Value.Strings.Name);
      this.m_icon.SetIcon(product.Value.Graphics.IconPath);
      return this;
    }
  }
}
