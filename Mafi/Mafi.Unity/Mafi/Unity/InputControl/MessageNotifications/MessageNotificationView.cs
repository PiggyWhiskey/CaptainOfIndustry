// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.MessageNotificationView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.MessageNotifications;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications
{
  public class MessageNotificationView : IUiElement
  {
    private IMessageNotification m_messageNotification;
    private Option<Action> m_onClick;
    private readonly Btn m_container;
    private readonly Txt m_text;
    private readonly IconContainer m_icon;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public MessageNotificationView(UiBuilder builder, IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      MessageNotificationView notificationView = this;
      this.m_container = builder.NewBtnPrimary("Notification").OnClick((Action) (() =>
      {
        Action valueOrNull = notificationView.m_onClick.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull();
        inputScheduler.ScheduleInputCmd<MessageNotificationDismissCmd>(new MessageNotificationDismissCmd(notificationView.m_messageNotification.NotificationId));
      })).OnRightClick((Action) (() => inputScheduler.ScheduleInputCmd<MessageNotificationDismissCmd>(new MessageNotificationDismissCmd(notificationView.m_messageNotification.NotificationId))));
      this.m_icon = builder.NewIconContainer("Icon").PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_container, 22.Vector2(), Offset.Left(10f) + Offset.Right(10f));
      Txt txt = builder.NewTxt("Text");
      TextStyle text = builder.Style.Global.Text;
      ref TextStyle local = ref text;
      FontStyle? nullable1 = new FontStyle?(FontStyle.Bold);
      int? nullable2 = new int?(12);
      bool? nullable3 = new bool?(true);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = nullable1;
      int? fontSize = nullable2;
      bool? isCapitalized = nullable3;
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_text = txt.SetTextStyle(textStyle).IncreaseFontForSymbols(2).SetAlignment(TextAnchor.MiddleLeft).AllowVerticalOverflow().PutTo<Txt>((IUiElement) this.m_container, Offset.Left(this.m_icon.GetWidth() + 20f));
    }

    public float GetWidthRequired()
    {
      return (float) ((double) this.m_text.GetPreferedWidth() + 46.0 + 40.0);
    }

    public void SetData(
      IMessageNotification notification,
      LocStrFormatted text,
      string iconPath,
      Action onClick)
    {
      this.m_messageNotification = notification;
      this.m_text.SetText(text);
      this.m_icon.SetIcon(iconPath);
      this.m_onClick = (Option<Action>) onClick;
    }

    public void SetBgColor(ColorRgba color) => this.m_container.SetBackgroundColor(color);

    public void SetDefaultBgColor() => this.m_container.SetBackgroundColor((ColorRgba) 2697513);
  }
}
