// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.GameOverManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class GameOverManager : IGameOverManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Event m_onGameOver;
    private readonly SettlementsManager m_settlementsManager;
    private readonly IMessageNotificationsManager m_messageNotificationsManager;

    public static void Serialize(GameOverManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameOverManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameOverManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsGameOver);
      writer.WriteGeneric<IMessageNotificationsManager>(this.m_messageNotificationsManager);
      Event.Serialize(this.m_onGameOver, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
    }

    public static GameOverManager Deserialize(BlobReader reader)
    {
      GameOverManager gameOverManager;
      if (reader.TryStartClassDeserialization<GameOverManager>(out gameOverManager))
        reader.EnqueueDataDeserialization((object) gameOverManager, GameOverManager.s_deserializeDataDelayedAction);
      return gameOverManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsGameOver = reader.ReadBool();
      reader.SetField<GameOverManager>(this, "m_messageNotificationsManager", (object) reader.ReadGenericAs<IMessageNotificationsManager>());
      reader.SetField<GameOverManager>(this, "m_onGameOver", (object) Event.Deserialize(reader));
      reader.SetField<GameOverManager>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
    }

    public bool IsGameOver { get; private set; }

    public IEvent OnGameOver => (IEvent) this.m_onGameOver;

    public GameOverManager(
      SettlementsManager settlementsManager,
      IMessageNotificationsManager messageNotificationsManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onGameOver = new Event();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settlementsManager = settlementsManager;
      this.m_messageNotificationsManager = messageNotificationsManager;
      calendar.NewDay.Add<GameOverManager>(this, new Action(this.newDay));
    }

    private void newDay()
    {
      int totalPopulation = this.m_settlementsManager.GetTotalPopulation();
      if (totalPopulation >= 10 && !this.IsGameOver)
        return;
      if (totalPopulation > 0)
        this.m_settlementsManager.RemovePopsAsMuchAs(totalPopulation);
      if (this.IsGameOver)
        return;
      Log.Info("Game over");
      this.performGameOver(new GameOverNotification((LocStrFormatted) TrCore.GameOver__Title, (LocStrFormatted) TrCore.GameOver__Message, true));
    }

    public void ShowCustomGameOver(GameOverNotification notification)
    {
      Log.Info("Game over (custom)");
      this.performGameOver(notification);
    }

    private void performGameOver(GameOverNotification notification)
    {
      this.IsGameOver = true;
      this.m_messageNotificationsManager.AddMessage((IMessageNotification) notification);
      this.m_onGameOver.Invoke();
    }

    static GameOverManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameOverManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameOverManager) obj).SerializeData(writer));
      GameOverManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameOverManager) obj).DeserializeData(reader));
    }
  }
}
