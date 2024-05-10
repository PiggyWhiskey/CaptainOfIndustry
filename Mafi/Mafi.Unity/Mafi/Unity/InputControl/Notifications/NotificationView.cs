// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Notifications.NotificationView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Notifications;
using Mafi.Unity.Camera;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Notifications
{
  internal class NotificationView : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly CameraController m_cameraController;
    private readonly Panel m_container;
    private readonly Txt m_text;
    private IconContainer m_icon;
    public Lyst<INotification> Notifications;
    private int m_lastClickIndex;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public NotificationView(
      UiBuilder builder,
      CameraController cameraController,
      Action<NotificationView> onDismissAction)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Notifications = new Lyst<INotification>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      NotificationView notificationView = this;
      this.m_builder = builder;
      this.m_cameraController = cameraController;
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("Notification").OnClick(new Action(this.onClick)).OnRightClick((Action) (() => onDismissAction(notificationView))).SetBackground(style.Notifications.ItemBackground);
      this.m_icon = builder.NewIconContainer("Icon").PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_container, 16.Vector2(), Offset.Left(10f));
      this.m_text = builder.NewTxt("Text").SetAlignment(TextAnchor.MiddleLeft).AllowVerticalOverflow().PutTo<Txt>((IUiElement) this.m_container, Offset.Left(this.m_icon.GetWidth() + 20f) + Offset.Right(22f));
      builder.NewBtn("Close button").SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Close.svg").OnClick((Action) (() => onDismissAction(notificationView))).PutToRightOf<Btn>((IUiElement) this.m_container, 12f, Offset.Right(5f));
    }

    private void onClick()
    {
      if (this.Notifications.IsEmpty)
        return;
      this.m_lastClickIndex %= this.Notifications.Count;
      for (int index = 0; index < this.Notifications.Count; ++index)
      {
        INotification notification = this.Notifications[this.m_lastClickIndex];
        if (notification.Entity.HasValue && notification.Entity.Value is IEntityWithPosition entityWithPosition)
        {
          this.m_cameraController.PanTo(entityWithPosition.Position2f);
          ++this.m_lastClickIndex;
          break;
        }
        this.m_lastClickIndex = (this.m_lastClickIndex + 1) % this.Notifications.Count;
      }
    }

    public void AddNotification(INotification notification)
    {
      this.Notifications.Add(notification);
      if (this.Notifications.Count == 1)
      {
        this.m_text.SetTextStyle(this.resolveTextStyle(notification));
        this.m_icon.SetIcon(this.resolveIcon(notification));
      }
      this.updateText(notification);
    }

    public void RemoveNotification(INotification notification)
    {
      this.Notifications.Remove(notification);
      if (this.Notifications.Count <= 0)
        return;
      this.updateText(notification);
    }

    private void updateText(INotification notification)
    {
      if (this.Notifications.Count <= 1)
        this.m_text.SetText(notification.Message);
      else
        this.m_text.SetText(notification.Message.ToString() + string.Format(" ({0}x)", (object) this.Notifications.Count));
      this.SetWidth<NotificationView>(this.getWidth());
    }

    public int NotificationsCount => this.Notifications.Count;

    private float getWidth()
    {
      return (float) (36.0 + (double) this.m_text.GetPreferedWidth() + 12.0 + 10.0 + 5.0);
    }

    private TextStyle resolveTextStyle(INotification notification)
    {
      UiStyle style = this.m_builder.Style;
      switch (notification.Proto.Style)
      {
        case NotificationStyle.Success:
          return style.Notifications.SuccessTextStyle;
        case NotificationStyle.Warning:
          return style.Notifications.WarningTextStyle;
        default:
          return style.Notifications.CriticalTextStyle;
      }
    }

    private IconStyle resolveIcon(INotification notification)
    {
      UiStyle style = this.m_builder.Style;
      IconStyle iconStyle;
      switch (notification.Proto.Style)
      {
        case NotificationStyle.Success:
          iconStyle = style.Notifications.SuccessIcon;
          break;
        case NotificationStyle.Warning:
          iconStyle = style.Notifications.WarningIcon;
          break;
        default:
          iconStyle = style.Notifications.CriticalIcon;
          break;
      }
      if (notification.Proto.IconAssetPath.HasValue)
        iconStyle = iconStyle.Extend(notification.Proto.IconAssetPath.Value);
      return iconStyle;
    }
  }
}
