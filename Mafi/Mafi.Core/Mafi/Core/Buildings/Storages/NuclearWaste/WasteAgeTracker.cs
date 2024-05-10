// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.NuclearWaste.WasteAgeTracker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Storages.NuclearWaste
{
  /// <summary>
  /// Tracks how old product is after being inserted.
  /// Helps to deprecate radioactive waste.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class WasteAgeTracker
  {
    /// <summary>
    /// Each index represents 1 year of age. First item is the youngest (0 age).
    /// </summary>
    private readonly Quantity[] m_ageQueue;
    private int m_simStepToNextRotation;
    public Quantity WasteToRetire;
    private readonly ISimLoopEvents m_simLoopEvents;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Quantity QuantityTotal => this.QuantityInQueue + this.WasteToRetire;

    [DoNotSave(0, null)]
    public Quantity QuantityInQueue { get; private set; }

    public WasteAgeTracker(ISimLoopEvents simLoopEvents, int maxAgeInYears)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_simLoopEvents = simLoopEvents;
      if (maxAgeInYears > 1000)
      {
        Log.Error(string.Format("Lifespan of a product seems to be a bit too big to track ({0} years), clamping to 1k", (object) maxAgeInYears));
        maxAgeInYears = 1000;
      }
      this.m_ageQueue = new Quantity[maxAgeInYears];
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      foreach (Quantity age in this.m_ageQueue)
        this.QuantityInQueue += age;
    }

    /// <summary>
    /// Returns -1 if there is no fuel to retire or retiring is not possible.
    /// </summary>
    public int YearsUntilFirstWasteGetsRetired()
    {
      for (int index = this.m_ageQueue.Length - 1; index >= 0; --index)
      {
        if (this.m_ageQueue[index].IsPositive)
          return this.m_ageQueue.Length - index;
      }
      return -1;
    }

    public void SimStepAndRotateFuel()
    {
      if (this.QuantityInQueue == Quantity.Zero || this.m_simLoopEvents.CurrentStep.Value < this.m_simStepToNextRotation)
        return;
      Quantity[] ageQueue = this.m_ageQueue;
      this.WasteToRetire += ageQueue[ageQueue.Length - 1];
      this.QuantityInQueue -= ageQueue[ageQueue.Length - 1];
      ageQueue[ageQueue.Length - 1] = Quantity.Zero;
      for (int index = ageQueue.Length - 1; index > 0; --index)
        ageQueue[index] = ageQueue[index - 1];
      ageQueue[0] = Quantity.Zero;
      this.m_simStepToNextRotation = this.m_simLoopEvents.CurrentStep.Value + 1.Years().Ticks;
    }

    public void ReportQuantityChanged(Quantity diff)
    {
      if (diff.IsPositive)
      {
        this.m_ageQueue[0] += diff;
        this.QuantityInQueue += diff;
        if (!(this.QuantityInQueue == diff))
          return;
        this.m_simStepToNextRotation = this.m_simLoopEvents.CurrentStep.Value + 1.Years().Ticks;
      }
      else
      {
        if (!diff.IsNegative)
          return;
        Quantity abs = diff.Abs;
        for (int index = 0; index < this.m_ageQueue.Length; ++index)
        {
          Quantity quantity = this.m_ageQueue[index].Min(abs);
          this.m_ageQueue[index] -= quantity;
          this.QuantityInQueue -= quantity;
          abs -= quantity;
          if (abs.IsNotPositive)
            break;
        }
      }
    }

    public static void Serialize(WasteAgeTracker value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WasteAgeTracker>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WasteAgeTracker.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteArray<Quantity>(this.m_ageQueue);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteInt(this.m_simStepToNextRotation);
      Quantity.Serialize(this.WasteToRetire, writer);
    }

    public static WasteAgeTracker Deserialize(BlobReader reader)
    {
      WasteAgeTracker wasteAgeTracker;
      if (reader.TryStartClassDeserialization<WasteAgeTracker>(out wasteAgeTracker))
        reader.EnqueueDataDeserialization((object) wasteAgeTracker, WasteAgeTracker.s_deserializeDataDelayedAction);
      return wasteAgeTracker;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WasteAgeTracker>(this, "m_ageQueue", (object) reader.ReadArray<Quantity>());
      reader.SetField<WasteAgeTracker>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_simStepToNextRotation = reader.ReadInt();
      this.WasteToRetire = Quantity.Deserialize(reader);
      reader.RegisterInitAfterLoad<WasteAgeTracker>(this, "initSelf", InitPriority.Normal);
    }

    static WasteAgeTracker()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WasteAgeTracker.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WasteAgeTracker) obj).SerializeData(writer));
      WasteAgeTracker.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WasteAgeTracker) obj).DeserializeData(reader));
    }
  }
}
