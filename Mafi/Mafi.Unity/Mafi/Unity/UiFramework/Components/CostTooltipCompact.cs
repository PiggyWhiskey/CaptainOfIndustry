// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.CostTooltipCompact
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class CostTooltipCompact : TooltipBase
  {
    private readonly PricePanelCompact m_price;
    private IUiElement m_parent;
    private Option<Txt> m_title;

    public CostTooltipCompact(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      this.m_price = new PricePanelCompact(builder);
      this.m_price.PutToLeftOf<PricePanelCompact>((IUiElement) this.Container, 0.0f, Offset.All(10f));
    }

    public CostTooltipCompact SetCost(IIndexable<ProductQuantity> products)
    {
      this.m_price.SetPrice(products);
      return this;
    }

    public void SetText(LocStrFormatted title, bool isTitle)
    {
      if (this.m_title.IsNone)
      {
        this.m_title = (Option<Txt>) this.Builder.NewTxt("Title").SetTextStyle(isTitle ? this.Builder.Style.Global.Title : this.Builder.Style.Global.TextInc).SetText(title).EnableRichText().PutToTopOf<Txt>((IUiElement) this.Container, 20f, Offset.All(10f));
        this.m_price.PutToLeftBottomOf<PricePanelCompact>((IUiElement) this.Container, new Vector2(0.0f, 0.0f), Offset.All(10f));
      }
      else
        this.m_title.Value.SetText(title);
    }

    private void OnParentMouseEnter()
    {
      if (this.m_price.IsEmpty)
      {
        Txt valueOrNull = this.m_title.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.Text.IsEmpty() ? 1 : 0) : 1) != 0)
          return;
      }
      Vector2 vector2 = Vector2.zero;
      if (this.m_title.HasValue)
      {
        Vector2 preferredSize = this.m_title.Value.GetPreferredSize(300f, float.MaxValue);
        vector2 = new Vector2(preferredSize.x + 20f, preferredSize.y + 20f);
      }
      float height = this.m_price.IsEmpty ? 0.0f : this.m_price.PreferredHeight + 20f;
      float dynamicWidth = this.m_price.GetDynamicWidth();
      this.m_price.SetHeight<PricePanelCompact>(height);
      this.PositionSelf(this.m_parent, dynamicWidth.Max(vector2.x), height + vector2.y);
    }

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(((TooltipBase) this).onParentMouseLeave));
    }
  }
}
