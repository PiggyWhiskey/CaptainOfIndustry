// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToRefuelShip
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToRefuelShip : Goal
  {
    private readonly GoalToRefuelShip.Proto m_goalProto;
    private readonly TravelingFleetManager m_fleetManager;
    private Quantity m_currentFuelQuantity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToRefuelShip(GoalToRefuelShip.Proto goalProto, TravelingFleetManager fleetManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_fleetManager = fleetManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      Quantity quantity = this.m_fleetManager.HasFleet ? this.m_fleetManager.TravelingFleet.FuelBuffer.Quantity : Quantity.Zero;
      if (this.m_currentFuelQuantity != quantity)
      {
        this.m_currentFuelQuantity = quantity;
        this.updateTitle();
      }
      return this.m_fleetManager.HasFleet && this.m_currentFuelQuantity >= this.m_fleetManager.TravelingFleet.FuelBuffer.Capacity;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.Value;
      if (!this.m_fleetManager.HasFleet)
        return;
      this.Title += string.Format(" ({0} / {1})", (object) this.m_currentFuelQuantity, (object) this.m_fleetManager.TravelingFleet.FuelBuffer.Capacity);
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToRefuelShip value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToRefuelShip>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToRefuelShip.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Quantity.Serialize(this.m_currentFuelQuantity, writer);
      TravelingFleetManager.Serialize(this.m_fleetManager, writer);
      writer.WriteGeneric<GoalToRefuelShip.Proto>(this.m_goalProto);
    }

    public static GoalToRefuelShip Deserialize(BlobReader reader)
    {
      GoalToRefuelShip goalToRefuelShip;
      if (reader.TryStartClassDeserialization<GoalToRefuelShip>(out goalToRefuelShip))
        reader.EnqueueDataDeserialization((object) goalToRefuelShip, GoalToRefuelShip.s_deserializeDataDelayedAction);
      return goalToRefuelShip;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentFuelQuantity = Quantity.Deserialize(reader);
      reader.SetField<GoalToRefuelShip>(this, "m_fleetManager", (object) TravelingFleetManager.Deserialize(reader));
      reader.SetField<GoalToRefuelShip>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToRefuelShip.Proto>());
    }

    static GoalToRefuelShip()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToRefuelShip.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToRefuelShip.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToRefuelShip);

      public Proto(string id, LocStrFormatted title, int lockedByIndex = -1, LocStrFormatted? tip = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex, tip);
        this.Title = title;
      }
    }
  }
}
