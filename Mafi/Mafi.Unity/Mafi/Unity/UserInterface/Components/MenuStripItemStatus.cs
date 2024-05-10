// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.MenuStripItemStatus
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

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
  internal class MenuStripItemStatus : IUiElement
  {
    private readonly Option<Action> m_onClick;
    private Btn m_button;
    private Txt m_currentStatusText;
    private Txt m_lastChangeText;
    private UiStyle m_style;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    public MenuStripItemStatus(UiBuilder builder, string iconAssetPath, Action onClick = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onClick = (Option<Action>) onClick;
      this.build(builder, iconAssetPath);
    }

    private void build(UiBuilder builder, string iconAssetPath)
    {
      this.m_style = builder.Style;
      this.m_button = builder.NewBtn("Container component");
      if (this.m_onClick.HasValue)
        this.m_button.OnClick(this.m_onClick.Value, muted: true);
      builder.NewIconContainer("Icon").SetIcon(iconAssetPath, this.m_style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) this.m_button, 18f);
      Panel parent = builder.NewPanel("Text Container").SetBackground(builder.AssetsDb.GetSharedSprite(this.m_style.StatusBar.QuantityStateBg), new ColorRgba?(this.m_style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) this.m_button, new Offset(0.0f, 1f, 22f, 1f));
      this.m_currentStatusText = builder.NewTxt("Current state").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(builder.Style.StatusBar.QuantityStateText).PutTo<Txt>((IUiElement) parent, Offset.Left(5f));
      this.m_lastChangeText = builder.NewTxt("Last diff").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(builder.Style.StatusBar.QuantityChangeText).PutTo<Txt>((IUiElement) parent, Offset.Right(5f));
    }

    public void SetValues(
      string currentState,
      string lastChange,
      bool isStatePositive,
      bool isLastChangePositive)
    {
      this.m_currentStatusText.SetText(currentState.ToUpper(LocalizationManager.CurrentCultureInfo));
      this.m_currentStatusText.SetColor(isStatePositive ? this.m_style.StatusBar.QuantityStatePositiveColor : this.m_style.StatusBar.QuantityNegativeColor);
      this.m_lastChangeText.SetText(lastChange.ToUpper(LocalizationManager.CurrentCultureInfo));
      this.m_lastChangeText.SetColor(isLastChangePositive ? this.m_style.StatusBar.QuantityChangePositiveColor : this.m_style.StatusBar.QuantityNegativeColor);
    }
  }
}
