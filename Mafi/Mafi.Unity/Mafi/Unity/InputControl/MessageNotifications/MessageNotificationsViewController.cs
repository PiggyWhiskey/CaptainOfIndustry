// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.MessageNotificationsViewController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl.MessageNotifications.Handlers;
using Mafi.Unity.InputControl.Messages.Goals;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class MessageNotificationsViewController : IUnityUi
  {
    public const int MAX_HEIGHT = 195;
    private readonly IMessageNotificationsManager m_notificationsManager;
    private readonly GoalsController m_goalsController;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IInputScheduler m_inputScheduler;
    private readonly LazyResolve<ResearchNotificationHandler> m_researchNotifsHandler;
    private readonly LazyResolve<NewRefugeesNotificationHandler> m_newRefugeesNotifsHandler;
    private readonly LazyResolve<LocationExploredNotificationHandler> m_locationExploredNotifsHandler;
    private readonly LazyResolve<NewMessageNotificationHandler> m_newMessageNotifHandler;
    private readonly LazyResolve<GameOverNotificationHandler> m_gameOverNotifsHandler;
    private readonly LazyResolve<ShipInBattleNotificationHandler> m_shipInBattleNotifsHandler;
    private readonly Dict<IMessageNotification, MessageNotificationView> m_notifications;
    private ViewsCacheHomogeneous<MessageNotificationView> m_notificationViews;
    private readonly Lyst<IMessageNotification> m_toCreate;
    private readonly Lyst<IMessageNotification> m_toDestroy;
    private StackContainer m_itemsContainer;
    private AudioSource m_newMessageAudio;
    private AudioSource m_newBattleAudio;
    private IconContainer m_arrowIcon;
    private Txt m_extraCountTxt;

    public MessageNotificationsViewController(
      IMessageNotificationsManager notificationsManager,
      GoalsController goalsController,
      IGameLoopEvents gameLoop,
      IInputScheduler inputScheduler,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_notifications = new Dict<IMessageNotification, MessageNotificationView>();
      this.m_toCreate = new Lyst<IMessageNotification>();
      this.m_toDestroy = new Lyst<IMessageNotification>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_notificationsManager = notificationsManager;
      this.m_goalsController = goalsController;
      this.m_gameLoop = gameLoop;
      this.m_inputScheduler = inputScheduler;
      this.m_researchNotifsHandler = new LazyResolve<ResearchNotificationHandler>((IResolver) resolver);
      this.m_newRefugeesNotifsHandler = new LazyResolve<NewRefugeesNotificationHandler>((IResolver) resolver);
      this.m_locationExploredNotifsHandler = new LazyResolve<LocationExploredNotificationHandler>((IResolver) resolver);
      this.m_newMessageNotifHandler = new LazyResolve<NewMessageNotificationHandler>((IResolver) resolver);
      this.m_gameOverNotifsHandler = new LazyResolve<GameOverNotificationHandler>((IResolver) resolver);
      this.m_shipInBattleNotifsHandler = new LazyResolve<ShipInBattleNotificationHandler>((IResolver) resolver);
    }

    public void RegisterUi(UiBuilder builder)
    {
      UiStyle style = builder.Style;
      this.m_notificationViews = new ViewsCacheHomogeneous<MessageNotificationView>((Func<MessageNotificationView>) (() => new MessageNotificationView(builder, this.m_inputScheduler)));
      this.m_itemsContainer = builder.NewStackContainer("MessageNotificationsContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f);
      positionContainer();
      this.m_arrowIcon = builder.NewIconContainer("Arrow", (IUiElement) this.m_itemsContainer).SetIcon("Assets/Unity/UserInterface/General/ArrowDown.svg", builder.Style.Global.TitleBig.Color).AddOutline().Hide<IconContainer>();
      this.m_extraCountTxt = builder.NewTxt("ExtraText", (IUiElement) this.m_itemsContainer).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleCenter).Hide<Txt>();
      foreach (IMessageNotification allNotification in (IEnumerable<IMessageNotification>) this.m_notificationsManager.AllNotifications)
        this.addNotification(allNotification, false);
      this.m_notificationsManager.OnNotificationAdded += new Action<IMessageNotification>(this.m_toCreate.Add);
      this.m_notificationsManager.OnNotificationRemoved += new Action<IMessageNotification>(this.m_toDestroy.Add);
      this.m_newMessageAudio = builder.AudioDb.GetClonedAudio("Assets/Unity/UserInterface/Audio/NewMessage.prefab", AudioChannel.UserInterface);
      this.m_newBattleAudio = builder.AudioDb.GetClonedAudio("Assets/Unity/UserInterface/Audio/TurretShot.prefab", AudioChannel.UserInterface);
      this.updatePanel();
      this.m_goalsController.OnHeightChanged += new Action(positionContainer);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<MessageNotificationsViewController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<MessageNotificationsViewController>(this, new Action<GameTime>(this.renderUpdate));

      void positionContainer()
      {
        this.m_itemsContainer.PutToLeftTopOf<StackContainer>((IUiElement) builder.MainCanvas, new Vector2(340f, this.m_itemsContainer.GetHeight()), Offset.TopLeft(100f + this.m_goalsController.GetOccupiedHeight(), 16f));
        this.m_itemsContainer.SendToBack<StackContainer>();
      }
    }

    private void addNotification(IMessageNotification notification)
    {
      this.addNotification(notification, true);
    }

    private void addNotification(IMessageNotification notification, bool playAudio)
    {
      Assert.That<Dict<IMessageNotification, MessageNotificationView>>(this.m_notifications).NotContainsKey<IMessageNotification, MessageNotificationView>(notification);
      MessageNotificationView view = this.m_notificationViews.GetView();
      switch (notification)
      {
        case ResearchFinishedMessage notification1:
          this.m_researchNotifsHandler.Value.PopulateViewFor(notification1, view);
          break;
        case NewRefugeesMessage notification2:
          this.m_newRefugeesNotifsHandler.Value.PopulateViewFor(notification2, view);
          break;
        case LocationExploredMessage notification3:
          this.m_locationExploredNotifsHandler.Value.PopulateViewFor(notification3, view);
          break;
        case NewMessageNotification notification4:
          this.m_newMessageNotifHandler.Value.PopulateViewFor(notification4, view);
          break;
        case GameOverNotification notification5:
          this.m_gameOverNotifsHandler.Value.PopulateViewFor(notification5, view);
          break;
        case ShipInBattleNotification notification6:
          this.m_shipInBattleNotifsHandler.Value.PopulateViewFor(notification6, view);
          break;
      }
      this.m_notifications[notification] = view;
      view.AppendTo<MessageNotificationView>(this.m_itemsContainer, new Vector2?(new Vector2(view.GetWidthRequired(), 28f)), ContainerPosition.LeftOrTop);
      if (playAudio)
      {
        if (notification is ShipInBattleNotification)
          this.m_newBattleAudio.Play();
        else
          this.m_newMessageAudio.Play();
      }
      this.updatePanel();
    }

    private void removeNotification(IMessageNotification notification)
    {
      MessageNotificationView element;
      if (!this.m_notifications.TryGetValue(notification, out element))
        return;
      this.m_notificationViews.Return(element);
      this.m_itemsContainer.Remove((IUiElement) element);
      this.m_notifications.Remove(notification);
      this.updatePanel();
    }

    private void updatePanel()
    {
      this.m_itemsContainer.SetVisibility<StackContainer>(this.m_notifications.Count > 0);
      this.m_itemsContainer.StartBatchOperation();
      for (int index = 0; index < this.m_notifications.Count; ++index)
        this.m_itemsContainer.SetItemVisibility(index, index < 5);
      this.m_itemsContainer.FinishBatchOperation();
      int coord = 16;
      int num = this.m_notifications.Count - 5;
      this.m_arrowIcon.SetVisibility<IconContainer>(num > 0);
      this.m_extraCountTxt.SetVisibility<Txt>(num > 0);
      if (num <= 0)
        return;
      this.m_extraCountTxt.SetText(string.Format("(+{0})", (object) num));
      float preferedWidth = this.m_extraCountTxt.GetPreferedWidth();
      float width = this.m_itemsContainer.GetItemAt(4).GetWidth();
      this.m_arrowIcon.PutToLeftBottomOf<IconContainer>((IUiElement) this.m_itemsContainer, coord.Vector2(), Offset.Left((float) (((double) width - (double) coord - 5.0 - (double) preferedWidth) / 2.0)) + Offset.Bottom((float) (-coord - 6)));
      this.m_extraCountTxt.PutToRightOf<Txt>((IUiElement) this.m_arrowIcon, preferedWidth, Offset.Right((float) (-(double) preferedWidth - 5.0)));
    }

    private void syncUpdate(GameTime gameTime)
    {
      this.m_notificationViews.Updater.SyncUpdate();
      this.m_toCreate.ForEachAndClear(new Action<IMessageNotification>(this.addNotification));
      this.m_toDestroy.ForEachAndClear(new Action<IMessageNotification>(this.removeNotification));
    }

    private void renderUpdate(GameTime gameTime) => this.m_notificationViews.Updater.RenderUpdate();
  }
}
