// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.PlanningModeManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class PlanningModeManager : 
    ICommandProcessor<SetPlanningModeEnabledCmd>,
    IAction<SetPlanningModeEnabledCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsPlanningModeEnabled { get; private set; }

    public void Invoke(SetPlanningModeEnabledCmd cmd)
    {
      this.IsPlanningModeEnabled = cmd.IsEnabled;
      cmd.SetResultSuccess();
    }

    public static void Serialize(PlanningModeManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PlanningModeManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PlanningModeManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsPlanningModeEnabled);
    }

    public static PlanningModeManager Deserialize(BlobReader reader)
    {
      PlanningModeManager planningModeManager;
      if (reader.TryStartClassDeserialization<PlanningModeManager>(out planningModeManager))
        reader.EnqueueDataDeserialization((object) planningModeManager, PlanningModeManager.s_deserializeDataDelayedAction);
      return planningModeManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsPlanningModeEnabled = reader.ReadBool();
    }

    public PlanningModeManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static PlanningModeManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PlanningModeManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PlanningModeManager) obj).SerializeData(writer));
      PlanningModeManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PlanningModeManager) obj).DeserializeData(reader));
    }
  }
}
