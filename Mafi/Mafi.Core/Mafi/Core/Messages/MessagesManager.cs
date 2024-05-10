// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessagesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Console;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class MessagesManager : 
    ICommandProcessor<MarkMessageAsReadCmd>,
    IAction<MarkMessageAsReadCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly ITutorialProgressTracker m_progressTracker;
    private readonly DependencyResolver m_resolver;
    private readonly IMessageNotificationsManager m_messageNotificationsManager;
    private readonly Event<Message> m_onNewMessage;
    private readonly Lyst<Message> m_allMessages;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<KeyValuePair<Proto.ID, bool>> m_messagesToAddFromRender;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IEvent<Message> OnNewMessage => (IEvent<Message>) this.m_onNewMessage;

    /// <summary>
    /// Raised when message notification should be added to the UI.
    /// Bool if it should also open the message right away.
    /// </summary>
    [DoNotSave(0, null)]
    public event Action<Message, bool> OnNewMessageForUi;

    public IIndexable<Message> AllMessages => (IIndexable<Message>) this.m_allMessages;

    public MessagesManager(
      ProtosDb protosDb,
      ITutorialProgressTracker progressTracker,
      DependencyResolver resolver,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      IMessageNotificationsManager messageNotificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onNewMessage = new Event<Message>();
      this.m_allMessages = new Lyst<Message>();
      this.m_messagesToAddFromRender = new Lyst<KeyValuePair<Proto.ID, bool>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_progressTracker = progressTracker;
      this.m_resolver = resolver;
      this.m_messageNotificationsManager = messageNotificationsManager;
      gameLoopEvents.RegisterNewGameCreated((object) this, new Action(this.onNewGameCreated));
      simLoopEvents.Sync.Add<MessagesManager>(this, new Action(this.syncUpdate));
    }

    private void syncUpdate()
    {
      if (this.m_messagesToAddFromRender.IsEmpty)
        return;
      foreach (KeyValuePair<Proto.ID, bool> keyValuePair in this.m_messagesToAddFromRender)
      {
        MessageProto proto;
        if (this.m_protosDb.TryGetProto<MessageProto>(keyValuePair.Key, out proto))
        {
          if (keyValuePair.Value)
            this.AddMessage(proto, true, true);
          else
            this.AddMessage(proto);
        }
      }
      this.m_messagesToAddFromRender.Clear();
    }

    private void onNewGameCreated()
    {
      foreach (MessageProto messageProto in this.m_protosDb.All<MessageProto>())
      {
        if (!messageProto.IsObsolete && !this.m_progressTracker.IsTutorialNew(messageProto))
          this.AddMessage(messageProto, true);
      }
      foreach (MessageTriggerProto messageTriggerProto in this.m_protosDb.All<MessageTriggerProto>())
      {
        if (!messageTriggerProto.IsObsolete && !this.hasMessage(messageTriggerProto.MessageProto))
          this.m_resolver.Instantiate(messageTriggerProto.Implementation, (object) messageTriggerProto);
      }
      foreach (MessageProto messageProto in this.m_protosDb.Filter<MessageProto>((Func<MessageProto, bool>) (x => x.UnlockSilentlyFromStart)))
        this.AddMessage(messageProto, true);
    }

    [ConsoleCommand(false, false, null, null)]
    private void showAllMessages()
    {
      foreach (MessageProto messageProto in this.m_protosDb.All<MessageProto>())
      {
        if (!messageProto.IsObsolete)
          this.AddMessage(messageProto);
      }
    }

    public void AddMessageFromRenderThread(Proto.ID messageId, bool openMessage)
    {
      this.m_messagesToAddFromRender.Add(Make.Kvp<Proto.ID, bool>(messageId, openMessage));
    }

    public void AddMessage(MessageProto messageProto, bool doNotNotify = false, bool openInUi = false)
    {
      if (this.hasMessage(messageProto))
        return;
      Message message = new Message(messageProto);
      bool flag1 = this.m_progressTracker.IsTutorialNew(messageProto);
      if (!doNotNotify & flag1)
        this.m_messageNotificationsManager.AddMessage((IMessageNotification) new NewMessageNotification(message));
      if (!flag1)
        message.MarkAsRead();
      this.m_allMessages.Add(message);
      this.m_onNewMessage?.Invoke(message);
      bool flag2 = openInUi || !message.IsRead && message.Proto.ForceOpen;
      Action<Message, bool> onNewMessageForUi = this.OnNewMessageForUi;
      if (onNewMessageForUi == null)
        return;
      onNewMessageForUi(message, flag2);
    }

    private bool hasMessage(MessageProto messageProto)
    {
      return this.m_allMessages.Any<Message>((Predicate<Message>) (x => (Proto) x.Proto == (Proto) messageProto));
    }

    public void Invoke(MarkMessageAsReadCmd cmd)
    {
      Message message = this.m_allMessages.FirstOrDefault<Message>((Predicate<Message>) (x => x.Proto.Id == cmd.ProtoId));
      if (message == null)
      {
        cmd.SetResultError(string.Format("Message with id {0} could not be found", (object) cmd.ProtoId));
      }
      else
      {
        this.m_messageNotificationsManager.DismissNotificationForMessageIfExists(message.Proto);
        message.MarkAsRead();
        this.m_progressTracker.MarkTutorialAsReadFromSim(message.Proto);
        cmd.SetResultSuccess();
      }
    }

    public static void Serialize(MessagesManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessagesManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessagesManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<Message>.Serialize(this.m_allMessages, writer);
      writer.WriteGeneric<IMessageNotificationsManager>(this.m_messageNotificationsManager);
      Event<Message>.Serialize(this.m_onNewMessage, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
    }

    public static MessagesManager Deserialize(BlobReader reader)
    {
      MessagesManager messagesManager;
      if (reader.TryStartClassDeserialization<MessagesManager>(out messagesManager))
        reader.EnqueueDataDeserialization((object) messagesManager, MessagesManager.s_deserializeDataDelayedAction);
      return messagesManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MessagesManager>(this, "m_allMessages", (object) Lyst<Message>.Deserialize(reader));
      reader.SetField<MessagesManager>(this, "m_messageNotificationsManager", (object) reader.ReadGenericAs<IMessageNotificationsManager>());
      reader.SetField<MessagesManager>(this, "m_messagesToAddFromRender", (object) new Lyst<KeyValuePair<Proto.ID, bool>>());
      reader.SetField<MessagesManager>(this, "m_onNewMessage", (object) Event<Message>.Deserialize(reader));
      reader.RegisterResolvedMember<MessagesManager>(this, "m_progressTracker", typeof (ITutorialProgressTracker), true);
      reader.RegisterResolvedMember<MessagesManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<MessagesManager>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
    }

    static MessagesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessagesManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessagesManager) obj).SerializeData(writer));
      MessagesManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessagesManager) obj).DeserializeData(reader));
    }
  }
}
