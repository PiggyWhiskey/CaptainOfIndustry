// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Notifications.NotificationsViewController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.TopStatusBar;
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
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class NotificationsViewController : IUnityUi
  {
    private readonly INotificationsManager m_notificationsManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly CameraController m_cameraController;
    private readonly StatusBar m_statusBar;
    private ViewsCacheHomogeneous<NotificationView> m_notificationViews;
    private readonly Dict<LocStrFormatted, NotificationView> m_notificationsByMessage;
    private readonly Lyst<INotification> m_toCreate;
    private readonly Lyst<INotification> m_onSuppressChange;
    private readonly Lyst<INotification> m_toDestroy;
    private UiBuilder m_builder;
    private Btn m_buttonNoWarnings;
    private Btn m_buttonWarnings;
    private Btn m_buttonWarningsCollapsed;
    private Btn m_buttonMuted;
    private Btn m_buttonUnmuted;
    private StackContainer m_itemsContainer;
    private Txt m_itemsCountText;
    private AudioSource m_criticalNotifSound;
    private long m_lastErrorSoundPlayTime;
    private bool m_notificationsSuppressed;

    public NotificationsViewController(
      INotificationsManager notificationsManager,
      IGameLoopEvents gameLoop,
      IInputScheduler inputScheduler,
      CameraController cameraController,
      StatusBar statusBar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_notificationsByMessage = new Dict<LocStrFormatted, NotificationView>();
      this.m_toCreate = new Lyst<INotification>();
      this.m_onSuppressChange = new Lyst<INotification>();
      this.m_toDestroy = new Lyst<INotification>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_notificationsManager = notificationsManager;
      this.m_inputScheduler = inputScheduler;
      this.m_cameraController = cameraController;
      this.m_statusBar = statusBar;
      gameLoop.SyncUpdate.AddNonSaveable<NotificationsViewController>(this, new Action<GameTime>(this.syncUpdate));
      gameLoop.RenderUpdate.AddNonSaveable<NotificationsViewController>(this, new Action<GameTime>(this.renderUpdate));
      notificationsManager.NotificationAdded += new Action<INotification>(this.m_toCreate.Add);
      notificationsManager.NotificationSuppressChanged += new Action<INotification>(this.m_onSuppressChange.Add);
      notificationsManager.NotificationRemoved += new Action<INotification>(this.m_toDestroy.Add);
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_criticalNotifSound = builder.AudioDb.GetClonedAudio("Assets/Unity/UserInterface/Audio/InvalidOp.prefab", AudioChannel.UserInterface);
      this.m_notificationViews = new ViewsCacheHomogeneous<NotificationView>((Func<NotificationView>) (() => new NotificationView(builder, this.m_cameraController, new Action<NotificationView>(this.onItemDismiss))));
      float size1 = 20f;
      float size2 = 16f;
      Panel panel = builder.NewPanel("NotificationsButton").SetHeight<Panel>(20f).SetWidth<Panel>((float) ((double) size1 + (double) size2 + 2.0));
      this.m_statusBar.AddElementToRight((IUiElement) panel, 300f, false);
      this.m_buttonNoWarnings = builder.NewBtn("Main button").SetButtonStyle(style.Notifications.MainButtonNoNotifications).SetIcon(style.Icons.Alert).AddToolTip(Tr.Notifications__NoNew).PutToLeftOf<Btn>((IUiElement) panel, size1);
      this.m_buttonWarningsCollapsed = builder.NewBtn("Main button active").SetButtonStyle(style.Notifications.MainButtonActive).SetIcon(style.Icons.Alert).OnClick(new Action(this.mainButtonClicked)).PutToLeftOf<Btn>((IUiElement) panel, size1);
      this.m_buttonWarnings = builder.NewBtn("Main button").SetButtonStyle(style.Notifications.MainButtonCollapsed).SetIcon(style.Icons.Alert).OnClick(new Action(this.mainButtonClicked)).PutToLeftOf<Btn>((IUiElement) panel, size1);
      this.m_buttonMuted = builder.NewBtn("Muted").SetButtonStyle(style.Notifications.MutedButton).SetIcon("Assets/Unity/UserInterface/General/Muted.svg").OnClick(new Action(onMuteToggle)).AddToolTip(Tr.Notifications__Unmute).PutToRightOf<Btn>((IUiElement) panel, size2);
      this.m_buttonUnmuted = builder.NewBtn("Unmuted").SetButtonStyle(style.Notifications.UnmutedButton).SetIcon("Assets/Unity/UserInterface/General/Unmuted.svg").OnClick(new Action(onMuteToggle)).AddToolTip(Tr.Notifications__Mute).PutToRightOf<Btn>((IUiElement) panel, size2);
      updateMuteButtons();
      this.m_itemsCountText = builder.NewTxt("Items count").SetTextStyle(style.Notifications.NotificationsCountTextStyle).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_buttonWarnings);
      this.m_itemsContainer = builder.NewStackContainer("NotificationsContainer").SetStackingDirection(StackContainer.Direction.BottomToTop).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f);
      foreach (INotification fetchAllNotification in this.m_notificationsManager.FetchAllNotifications())
        this.addNotification(fetchAllNotification, false, true);
      this.updatePanel();
      this.m_statusBar.OnHeightChanged += new Action<float>(positionSelf);

      void updateMuteButtons()
      {
        this.m_buttonMuted.SetVisibility<Btn>(builder.UiPreferences.AreNotificationsMuted);
        this.m_buttonUnmuted.SetVisibility<Btn>(!builder.UiPreferences.AreNotificationsMuted);
      }

      void onMuteToggle()
      {
        builder.UiPreferences.SetNotificationsMuted(!builder.UiPreferences.AreNotificationsMuted);
        updateMuteButtons();
      }

      void positionSelf(float barHeight)
      {
        int num = 5;
        this.m_itemsContainer.PutToRightTopOf<StackContainer>((IUiElement) builder.MainCanvas, new Vector2(0.0f, this.m_itemsContainer.GetDynamicHeight()), Offset.TopRight(barHeight + (float) num, style.Notifications.RightOffset));
      }
    }

    private void onItemDismiss(NotificationView notificationView)
    {
      if (notificationView.Notifications.Count <= 0)
        return;
      LocStrFormatted message = notificationView.Notifications.First.Message;
      this.m_inputScheduler.ScheduleInputCmd<NotificationDismissCmd>(new NotificationDismissCmd(notificationView.Notifications.ToImmutableArray<NotificationId>((Func<INotification, NotificationId>) (x => x.NotificationId))));
      this.m_notificationsByMessage.Remove(message);
      notificationView.Notifications.Clear();
      this.returnView(notificationView);
    }

    private void addNotification(INotification notification)
    {
      if (this.m_toDestroy.Contains(notification))
        return;
      this.addNotification(notification, true, false);
    }

    private void addNotification(INotification notification, bool playAudio, bool doNotUpdatePanel)
    {
      if (notification.Proto.HideInNotificationPanel || notification.IsSuppressed)
        return;
      NotificationView view;
      if (!this.m_notificationsByMessage.TryGetValue(notification.Message, out view))
      {
        view = this.m_notificationViews.GetView();
        view.AddNotification(notification);
        view.AppendTo<NotificationView>(this.m_itemsContainer, new Vector2?(new Vector2(view.GetWidth(), this.m_builder.Style.Notifications.ItemHeight)), ContainerPosition.RightOrBottom);
        this.m_notificationsByMessage.Add(notification.Message, view);
      }
      else
      {
        view.AddNotification(notification);
        this.m_itemsContainer.UpdateItemWidth((IUiElement) view, view.GetWidth());
      }
      if (!this.m_builder.UiPreferences.AreNotificationsMuted & playAudio && notification.Proto.Style == NotificationStyle.Critical && ((long) Environment.TickCount - this.m_lastErrorSoundPlayTime).Abs() > 10000L)
      {
        this.m_criticalNotifSound.Play();
        this.m_lastErrorSoundPlayTime = (long) Environment.TickCount;
      }
      if (doNotUpdatePanel)
        return;
      this.updatePanel();
    }

    private void suppressChange(INotification notification)
    {
      if (notification.IsSuppressed)
        this.removeNotification(notification);
      else
        this.addNotification(notification);
    }

    private void removeNotification(INotification notification)
    {
      NotificationView notificationView;
      if (notification.Proto.HideInNotificationPanel || !this.m_notificationsByMessage.TryGetValue(notification.Message, out notificationView))
        return;
      notificationView.RemoveNotification(notification);
      if (notificationView.NotificationsCount == 0)
      {
        this.m_notificationsByMessage.Remove(notification.Message);
        this.returnView(notificationView);
      }
      else
        this.m_itemsContainer.UpdateItemWidth((IUiElement) notificationView, notificationView.GetWidth());
    }

    private void returnView(NotificationView view)
    {
      Assert.That<int>(view.NotificationsCount).IsZero();
      this.m_notificationViews.Return(view);
      this.m_itemsContainer.Remove((IUiElement) view);
      this.updatePanel();
    }

    private void updatePanel()
    {
      int count = this.m_notificationsByMessage.Count;
      this.m_itemsCountText.SetText(count.ToString());
      if (count > 0)
      {
        this.m_buttonNoWarnings.Hide<Btn>();
        this.m_itemsContainer.SetVisibility<StackContainer>(!this.m_notificationsSuppressed);
        this.m_buttonWarnings.SetVisibility<Btn>(this.m_notificationsSuppressed);
        this.m_buttonWarningsCollapsed.SetVisibility<Btn>(!this.m_notificationsSuppressed);
      }
      else
      {
        this.m_buttonNoWarnings.Show<Btn>();
        this.m_buttonWarnings.Hide<Btn>();
        this.m_buttonWarningsCollapsed.Hide<Btn>();
        this.m_itemsContainer.SetVisibility<StackContainer>(false);
      }
    }

    private void mainButtonClicked()
    {
      if (this.m_notificationsByMessage.Count <= 0)
        return;
      this.m_notificationsSuppressed = this.m_itemsContainer.IsVisible();
      this.updatePanel();
    }

    private void syncUpdate(GameTime gameTime)
    {
      this.m_notificationViews.Updater.SyncUpdate();
      this.m_toCreate.ForEachAndClear(new Action<INotification>(this.addNotification));
      this.m_onSuppressChange.ForEachAndClear(new Action<INotification>(this.suppressChange));
      this.m_toDestroy.ForEachAndClear(new Action<INotification>(this.removeNotification));
    }

    private void renderUpdate(GameTime gameTime) => this.m_notificationViews.Updater.RenderUpdate();
  }
}
