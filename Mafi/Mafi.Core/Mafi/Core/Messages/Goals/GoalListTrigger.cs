// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalListTrigger
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  /// <summary>
  /// Triggers list of goals based on some specific condition.
  /// Once a list of goals is triggered it will be shown to the player.
  /// </summary>
  public abstract class GoalListTrigger
  {
    protected readonly GoalsManager GoalsManager;
    private readonly GoalsList m_goalListToActivate;

    public int Version { get; private set; }

    protected GoalListTrigger(GoalsList goalListToActivate, GoalsManager goalsManager, int version)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_goalListToActivate = goalListToActivate;
      this.GoalsManager = goalsManager;
      this.Version = version;
    }

    protected void ActivateGoal()
    {
      this.OnDestroy();
      this.GoalsManager.ActivateGoal(this.m_goalListToActivate);
    }

    protected abstract void OnDestroy();

    public void Destroy() => this.OnDestroy();

    protected virtual void SerializeData(BlobWriter writer)
    {
      GoalsManager.Serialize(this.GoalsManager, writer);
      GoalsList.Serialize(this.m_goalListToActivate, writer);
      writer.WriteInt(this.Version);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GoalListTrigger>(this, "GoalsManager", (object) GoalsManager.Deserialize(reader));
      reader.SetField<GoalListTrigger>(this, "m_goalListToActivate", (object) GoalsList.Deserialize(reader));
      this.Version = reader.ReadInt();
    }
  }
}
