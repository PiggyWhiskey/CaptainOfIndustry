// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Priorities.GlobalPrioritiesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Priorities
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class GlobalPrioritiesManager : 
    ICommandProcessor<SetGlobalPriorityCmd>,
    IAction<SetGlobalPriorityCmd>
  {
    public const string CONSTRUCTION_PRIORITY_ID = "ConstructionPriority";
    public const string DECONSTRUCTION_PRIORITY_ID = "DeconstructionPriority";
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int ConstructionPriority { get; private set; }

    public int DeconstructionPriority { get; private set; }

    public GlobalPrioritiesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConstructionPriority = 7;
      this.DeconstructionPriority = 7;
    }

    public void Invoke(SetGlobalPriorityCmd cmd)
    {
      if (!GeneralPriorities.AssertAssignableRange(cmd.Priority))
        cmd.SetResultError("Priority " + cmd.PriorityId + " is out of range");
      else if (cmd.PriorityId == "ConstructionPriority")
      {
        this.ConstructionPriority = cmd.Priority;
        cmd.SetResultSuccess();
      }
      else if (cmd.PriorityId == "DeconstructionPriority")
      {
        this.DeconstructionPriority = cmd.Priority;
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError("Priority category " + cmd.PriorityId + " not found");
    }

    public static void Serialize(GlobalPrioritiesManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GlobalPrioritiesManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GlobalPrioritiesManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.ConstructionPriority);
      writer.WriteInt(this.DeconstructionPriority);
    }

    public static GlobalPrioritiesManager Deserialize(BlobReader reader)
    {
      GlobalPrioritiesManager prioritiesManager;
      if (reader.TryStartClassDeserialization<GlobalPrioritiesManager>(out prioritiesManager))
        reader.EnqueueDataDeserialization((object) prioritiesManager, GlobalPrioritiesManager.s_deserializeDataDelayedAction);
      return prioritiesManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ConstructionPriority = reader.ReadInt();
      this.DeconstructionPriority = reader.ReadInt();
    }

    static GlobalPrioritiesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GlobalPrioritiesManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GlobalPrioritiesManager) obj).SerializeData(writer));
      GlobalPrioritiesManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GlobalPrioritiesManager) obj).DeserializeData(reader));
    }
  }
}
