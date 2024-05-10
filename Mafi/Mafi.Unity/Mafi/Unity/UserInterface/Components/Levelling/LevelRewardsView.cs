// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.Levelling.LevelRewardsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.Levelling
{
  /// <summary>
  /// A single row showing rewards a player gets for attaining a level.
  /// </summary>
  public class LevelRewardsView : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly LevelRewardsView.Style m_style;
    private readonly Panel m_container;
    private Option<Txt> m_label;
    private readonly Lyst<IconTextPair> m_productLabels;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    private UiStyle UiStyle => this.m_builder.Style;

    public LevelRewardsView(UiBuilder builder, LevelRewardsView.Style style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_productLabels = new Lyst<IconTextPair>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_style = style;
      this.m_container = builder.NewPanel(nameof (LevelRewardsView)).SetHeight<Panel>(style.ItemStyle.IconSize);
    }

    public LevelRewardsView SetReward(AssetValue reward)
    {
      float spaceBetweeinItems = this.m_style.SpaceBetweeinItems;
      float leftOffset = this.m_label.HasValue ? this.m_label.Value.GetWidth() + spaceBetweeinItems : 0.0f;
      for (int index = 0; index < reward.Products.Length; ++index)
      {
        IconTextPair element;
        if (index < this.m_productLabels.Count)
        {
          element = this.m_productLabels[index];
          element.Show<IconTextPair>();
        }
        else
        {
          element = new IconTextPair(this.m_builder, this.m_style.ItemStyle);
          this.m_productLabels.Add(element);
        }
        ProductQuantity product = reward.Products[index];
        element.SetIcon(product.Product.Graphics.IconPath).SetText(product.FormatNumberAndUnitOnly().Value).PutToLeftMiddleOf<IconTextPair>((IUiElement) this.m_container, element.GetSize(), Offset.Left(leftOffset));
        float width = element.GetWidth();
        leftOffset += width + spaceBetweeinItems;
      }
      for (int length = reward.Products.Length; length < this.m_productLabels.Count; ++length)
        this.m_productLabels[length].Hide<IconTextPair>();
      this.m_container.SetWidth<Panel>((leftOffset - spaceBetweeinItems).Max(0.0f));
      this.m_container.SetVisibility<Panel>(reward.IsNotEmpty);
      return this;
    }

    public LevelRewardsView SetLabel(string text) => this.SetLabel(text, this.UiStyle.Global.Text);

    /// <summary>
    /// After setting label, set reward too, setting reward will correct positions of reward labels and icons.
    /// </summary>
    public LevelRewardsView SetLabel(string text, TextStyle textStyle)
    {
      if (this.m_label.IsNone)
        this.m_label = (Option<Txt>) this.m_builder.NewTxt("Label").AllowHorizontalOverflow();
      Txt txt = this.m_label.Value;
      txt.SetTextStyle(textStyle).SetText(text).SetPreferredSize().PutToLeftMiddleOf<Txt>((IUiElement) this.m_container, txt.GetPreferedSize());
      return this;
    }

    public class Style
    {
      public readonly float SpaceBetweeinItems;
      public readonly IconTextPair.Style ItemStyle;

      public Style(float spaceBetweenItems, IconTextPair.Style itemStyle)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.SpaceBetweeinItems = spaceBetweenItems;
        this.ItemStyle = itemStyle;
      }
    }
  }
}
