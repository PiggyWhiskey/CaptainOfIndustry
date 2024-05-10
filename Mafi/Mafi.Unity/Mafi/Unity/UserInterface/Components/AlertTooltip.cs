// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.AlertTooltip
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Notifications;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class AlertTooltip : TooltipBase
  {
    private IUiElement m_parent;
    private readonly IconContainer m_notificationIcon;
    private readonly Txt m_notificationText;
    private readonly Panel m_container;
    private readonly Panel m_learnMoreContainer;

    public AlertTooltip(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, nameof (AlertTooltip));
      this.m_container = builder.NewPanel("Container").PutTo<Panel>((IUiElement) this.Container, Offset.All(10f));
      this.m_notificationIcon = this.Builder.NewIconContainer("Icon", (IUiElement) this.m_container).SetColor(builder.Style.Global.OrangeText).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_container, 24.Vector2());
      this.m_notificationText = this.Builder.NewTxt("NotifText", (IUiElement) this.m_container).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(builder.Style.Global.TextControls.Extend(new ColorRgba?(builder.Style.Global.OrangeText))).PutTo<Txt>((IUiElement) this.m_container, Offset.Left(34f));
      this.m_learnMoreContainer = this.Builder.NewPanel("LearnMore").SetBackground(ColorRgba.Black).PutToTopOf<Panel>((IUiElement) this.Container, 28f, Offset.Top(-28f)).Hide<Panel>();
      Txt txt = this.Builder.NewTxt("NotifText", (IUiElement) this.m_container).EnableRichText().SetTextStyle(builder.Style.Global.Text).SetText(Tr.ClickToLearnMore.Format("<b>alt + click</b>")).SetAlignment(TextAnchor.MiddleRight).PutTo<Txt>((IUiElement) this.m_learnMoreContainer, Offset.Right(10f));
      this.Builder.NewIconContainer("TutorialIcon", (IUiElement) this.m_container).SetIcon("Assets/Unity/UserInterface/Toolbar/Tutorials.svg").SetColor(builder.Style.Global.Text.Color).PutToRightOf<IconContainer>((IUiElement) this.m_learnMoreContainer, 18f, Offset.Right(15f + txt.GetPreferedWidth()));
      this.Container.SetBackground((ColorRgba) 2500134);
    }

    public void SetNotification(INotification notif, bool hasTutorial)
    {
      if (notif.Proto is EntityNotificationProto proto)
      {
        if (proto.ExtraMessageForInspector.IsNotEmpty)
          this.m_notificationText.SetText(notif.Message.ToString() + " " + proto.ExtraMessageForInspector.Value);
        else
          this.m_notificationText.SetText(notif.Message);
      }
      else
        this.m_notificationText.SetText(notif.Message);
      if (notif.Proto.IconAssetPath.HasValue)
      {
        this.m_notificationIcon.SetIcon(notif.Proto.IconAssetPath.Value);
        this.m_notificationIcon.Show<IconContainer>();
      }
      else
        this.m_notificationIcon.Hide<IconContainer>();
      this.m_learnMoreContainer.SetVisibility<Panel>(hasTutorial);
    }

    public void SetMessage(string message)
    {
      this.m_notificationIcon.Hide<IconContainer>();
      this.m_notificationText.SetText(message);
    }

    public void HideTooltipIcon()
    {
      this.m_notificationIcon.Hide<IconContainer>();
      this.m_notificationText.PutTo<Txt>((IUiElement) this.m_container, Offset.Left(10f));
    }

    private void OnParentMouseEnter()
    {
      float width = 380f;
      float num = this.m_notificationText.GetPreferedHeight(width).Max(30f);
      this.PositionSelf(this.m_parent, width + 20f, num + 20f);
    }

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(((TooltipBase) this).onParentMouseLeave));
    }
  }
}
