// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ProtoWithIcon`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class ProtoWithIcon<T> : IUiElement where T : class, IProtoWithIcon
  {
    private Option<T> m_proto;
    private ColorRgba m_hoveredTextClr;
    private Option<IconContainer> m_iconOnHover;
    private string m_titleForSearchLower;
    private readonly UiBuilder m_builder;
    private readonly Txt m_protoName;
    private readonly Btn m_button;
    private readonly IconContainer m_icon;
    private Option<IconContainer> m_secondaryIcon;
    private readonly Option<Action<T>> m_clickAction;
    private readonly Offset m_iconOffset;
    private Option<TitleTooltip> m_nameTooltip;
    private Option<Tooltip> m_tooltip;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    public ProtoWithIcon(IUiElement parent, UiBuilder builder, Action<T> clickAction = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_hoveredTextClr = (ColorRgba) 0;
      this.m_titleForSearchLower = "";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_clickAction = (Option<Action<T>>) clickAction;
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_button = builder.NewBtn(nameof (ProtoWithIcon<T>), parent).OnClick(new Action(this.onClick));
      this.m_protoName = builder.NewTxt("Proto name", (IUiElement) this.m_button).AllowVerticalOverflow().SetAlignment(TextAnchor.UpperCenter).SetTextStyle(style.Panel.Text).PutToBottomOf<Txt>((IUiElement) this.m_button, style.ProductWithIcon.QuantityLineHeight);
      this.m_iconOffset = Offset.Bottom(style.ProductWithIcon.QuantityLineHeight) + Offset.All(2f);
      this.m_icon = this.m_builder.NewIconContainer("Icon", (IUiElement) this.m_button).PutTo<IconContainer>((IUiElement) this.m_button, this.m_iconOffset);
      this.setNone();
    }

    public void ShowTitleOnHoverOnly(int? maxWidthOverflow = null)
    {
      if (this.m_nameTooltip.IsNone)
      {
        this.m_nameTooltip = (Option<TitleTooltip>) new TitleTooltip(this.m_builder);
        if (maxWidthOverflow.HasValue)
          this.m_nameTooltip.Value.SetMaxWidthOverflow(maxWidthOverflow.Value);
      }
      this.m_protoName.Hide<Txt>();
      this.m_icon.PutTo<IconContainer>((IUiElement) this.m_button, Offset.All(2f));
    }

    public void SetTooltip(LocStrFormatted tooltip)
    {
      if (this.m_tooltip.IsNone)
        this.m_tooltip = (Option<Tooltip>) this.m_button.AddToolTipAndReturn();
      this.m_tooltip.Value.SetText(tooltip);
    }

    public void ShowHoveredProtoName()
    {
      this.m_nameTooltip.ValueOrNull?.SetText(new LocStrFormatted(this.m_protoName.Text));
      this.m_nameTooltip.ValueOrNull?.Show((IUiElement) this.m_button);
    }

    public void HideHoveredProductName() => this.m_nameTooltip.ValueOrNull?.Hide();

    public void SetOnRightClick(Action onRightClick) => this.m_button.OnRightClick(onRightClick);

    public ProtoWithIcon<T> SetProto(Option<T> proto)
    {
      if ((object) this.m_proto.ValueOrNull == (object) proto.ValueOrNull)
        return this;
      this.m_proto = proto;
      IconContainer valueOrNull = this.m_secondaryIcon.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.Hide<IconContainer>();
      if (proto.IsNone)
      {
        this.setNone();
        return this;
      }
      this.m_titleForSearchLower = proto.Value.Strings.Name.TranslatedString.ToLower(LocalizationManager.CurrentCultureInfo);
      this.m_protoName.SetText((LocStrFormatted) proto.Value.Strings.Name);
      this.m_icon.SetIcon(proto.Value.IconPath);
      if (proto.Value is DrivingEntityProto drivingEntityProto && drivingEntityProto.FuelTankProto.HasValue)
        this.getOrCreateSecondaryIcon().SetIcon(drivingEntityProto.FuelTankProto.Value.Product.IconPath).Show<IconContainer>();
      return this;
    }

    private IconContainer getOrCreateSecondaryIcon()
    {
      if (this.m_secondaryIcon.IsNone)
        this.m_secondaryIcon = (Option<IconContainer>) this.m_builder.NewIconContainer("CornerIcon", (IUiElement) this.m_button).PutToLeftBottomOf<IconContainer>((IUiElement) this.m_icon, 20.Vector2());
      return this.m_secondaryIcon.Value;
    }

    public void SetCustomIcon(string icon) => this.m_icon.SetIcon(icon);

    public void SetCustomIconText(string text) => this.m_protoName.SetText(text);

    public void AppendQuantityAfterName(Quantity quantity)
    {
      if (!(this.m_proto.Value is IProtoWithIconAndName proto))
        return;
      this.m_protoName.SetText(string.Format("{0} ({1})", (object) this.m_proto.Value.Strings.Name, (object) proto.QuantityFormatter.FormatNumberAndUnitOnly(proto, (QuantityLarge) quantity)));
    }

    public void ReplaceNameWithQuantity(Quantity quantity)
    {
      if (!(this.m_proto.Value is IProtoWithIconAndName proto))
        return;
      this.m_protoName.SetText(string.Format("({0})", (object) proto.QuantityFormatter.FormatNumberAndUnitOnly(proto, (QuantityLarge) quantity)));
    }

    public ProtoWithIcon<T> EnablePositiveHoverEffect()
    {
      this.m_hoveredTextClr = this.m_builder.Style.EntitiesMenu.ItemTitleHoveredClr;
      this.m_button.SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave));
      return this;
    }

    public ProtoWithIcon<T> EnableRemoveHoverEffect()
    {
      this.m_hoveredTextClr = this.m_builder.Style.Global.DangerClr;
      this.m_button.SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave));
      if (this.m_iconOnHover.IsNone)
        this.m_iconOnHover = (Option<IconContainer>) this.m_builder.NewIconContainer("OnHoverIcon", (IUiElement) this.m_icon).SetIcon("Assets/Unity/UserInterface/General/Trash128.png", this.m_builder.Style.Global.DangerClr).PutToRightTopOf<IconContainer>((IUiElement) this.m_icon, 18.Vector2(), Offset.TopRight(2f, 2f)).Hide<IconContainer>();
      return this;
    }

    public bool Matches(string[] query)
    {
      foreach (string str in query)
      {
        if (!this.m_titleForSearchLower.Contains(str))
          return false;
      }
      return true;
    }

    private void onMouseEnter()
    {
      this.m_protoName.SetColor(this.m_hoveredTextClr);
      this.m_icon.PutTo<IconContainer>((IUiElement) this.m_button, this.m_iconOffset - Offset.All(2f));
      IconContainer valueOrNull = this.m_iconOnHover.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Show<IconContainer>();
    }

    private void onMouseLeave()
    {
      this.m_protoName.SetColor(this.m_builder.Style.EntitiesMenu.ItemTitleStyle.Color);
      this.m_icon.PutTo<IconContainer>((IUiElement) this.m_button, this.m_iconOffset);
      IconContainer valueOrNull = this.m_iconOnHover.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Hide<IconContainer>();
    }

    private void onClick()
    {
      if (!this.m_proto.HasValue || !this.m_clickAction.HasValue)
        return;
      this.onMouseLeave();
      this.m_clickAction.Value(this.m_proto.Value);
    }

    private void setNone()
    {
      this.m_protoName.SetText((LocStrFormatted) Tr.Empty);
      this.m_icon.SetIcon(this.m_builder.Style.Icons.Empty, this.m_builder.Style.Global.Text.Color);
    }

    public class Cache : ViewsCache<T, ProtoWithIcon<T>>
    {
      private readonly IUiElement m_parent;
      private readonly Action<T> m_onClickAction;
      private readonly bool m_enablePositiveHoverEffect;

      public Cache(
        IUiElement parent,
        UiBuilder builder,
        Action<T> onClickAction = null,
        bool enablePositiveHoverEffect = false)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.m_parent = parent;
        this.m_onClickAction = onClickAction;
        this.m_enablePositiveHoverEffect = enablePositiveHoverEffect;
      }

      protected override ProtoWithIcon<T> CreateView(UiBuilder builder, T data)
      {
        ProtoWithIcon<T> view = new ProtoWithIcon<T>(this.m_parent, builder, this.m_onClickAction);
        view.SetProto(Option<T>.Create(data));
        if (this.m_enablePositiveHoverEffect)
          view.EnablePositiveHoverEffect();
        return view;
      }
    }
  }
}
