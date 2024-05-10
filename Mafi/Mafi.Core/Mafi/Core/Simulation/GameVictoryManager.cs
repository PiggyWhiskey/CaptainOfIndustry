// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.GameVictoryManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.SpaceProgram;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class GameVictoryManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Event m_onGameVictory;

    public static void Serialize(GameVictoryManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameVictoryManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameVictoryManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsGameVictorious);
      Event.Serialize(this.m_onGameVictory, writer);
    }

    public static GameVictoryManager Deserialize(BlobReader reader)
    {
      GameVictoryManager gameVictoryManager;
      if (reader.TryStartClassDeserialization<GameVictoryManager>(out gameVictoryManager))
        reader.EnqueueDataDeserialization((object) gameVictoryManager, GameVictoryManager.s_deserializeDataDelayedAction);
      return gameVictoryManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsGameVictorious = reader.ReadBool();
      reader.SetField<GameVictoryManager>(this, "m_onGameVictory", (object) Event.Deserialize(reader));
    }

    public bool IsGameVictorious { get; private set; }

    public IEvent OnGameVictory => (IEvent) this.m_onGameVictory;

    public GameVictoryManager(IRocketLaunchManager rocketLaunchManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onGameVictory = new Event();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      rocketLaunchManager.RocketLaunched.Add<GameVictoryManager>(this, new Action<RocketEntity>(this.rocketLaunched));
    }

    private void rocketLaunched(RocketEntity rocket)
    {
      if (this.IsGameVictorious)
        return;
      this.IsGameVictorious = true;
      this.onGameVictory();
    }

    private void onGameVictory() => this.m_onGameVictory.Invoke();

    static GameVictoryManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameVictoryManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameVictoryManager) obj).SerializeData(writer));
      GameVictoryManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameVictoryManager) obj).DeserializeData(reader));
    }
  }
}
