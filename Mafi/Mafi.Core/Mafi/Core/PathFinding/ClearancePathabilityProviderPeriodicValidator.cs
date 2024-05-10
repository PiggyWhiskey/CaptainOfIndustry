// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.ClearancePathabilityProviderPeriodicValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Console;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.PathFinding
{
  [GlobalDependency(RegistrationMode.AsSelf, true, false)]
  [GenerateSerializer(false, null, 0)]
  internal class ClearancePathabilityProviderPeriodicValidator
  {
    private static readonly Duration CHECK_FREQUENCY;
    private readonly ClearancePathabilityProvider m_clearancePathabilityProvider;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly TickTimer m_timer;
    private bool m_isEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ClearancePathabilityProviderPeriodicValidator(
      ClearancePathabilityProvider clearancePathabilityProvider,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_clearancePathabilityProvider = clearancePathabilityProvider;
      this.m_simLoopEvents = simLoopEvents;
    }

    public void SetCppValidationEnabled(bool isEnabled)
    {
      if (isEnabled == this.m_isEnabled)
        return;
      this.m_isEnabled = isEnabled;
      if (isEnabled)
      {
        this.m_simLoopEvents.UpdateEnd.AddNonSaveable<ClearancePathabilityProviderPeriodicValidator>(this, new Action(this.validateData));
        this.m_timer.Start(ClearancePathabilityProviderPeriodicValidator.CHECK_FREQUENCY);
      }
      else
        this.m_simLoopEvents.UpdateEnd.RemoveNonSaveable<ClearancePathabilityProviderPeriodicValidator>(this, new Action(this.validateData));
    }

    [ConsoleCommand(false, false, null, null)]
    private void enablePathFindingValidation(bool isEnabled = true)
    {
      this.SetCppValidationEnabled(isEnabled);
    }

    private void validateData()
    {
      Assert.That<bool>(this.m_isEnabled).IsTrue();
      if (this.m_timer.Decrement())
        return;
      this.m_timer.Start(ClearancePathabilityProviderPeriodicValidator.CHECK_FREQUENCY);
      Lyst<KeyValuePair<Tile2iSlim, string>> lyst = this.m_clearancePathabilityProvider.ValidateData();
      if (!lyst.IsNotEmpty)
        return;
      Log.Error("Clearance pathability provider data validation failed:\n" + ((IEnumerable<string>) lyst.Select<string>((Func<KeyValuePair<Tile2iSlim, string>, string>) (kvp => kvp.Value))).JoinStrings("\n"));
    }

    public static void Serialize(
      ClearancePathabilityProviderPeriodicValidator value,
      BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ClearancePathabilityProviderPeriodicValidator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ClearancePathabilityProviderPeriodicValidator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ClearancePathabilityProvider.Serialize(this.m_clearancePathabilityProvider, writer);
      writer.WriteBool(this.m_isEnabled);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      TickTimer.Serialize(this.m_timer, writer);
    }

    public static ClearancePathabilityProviderPeriodicValidator Deserialize(BlobReader reader)
    {
      ClearancePathabilityProviderPeriodicValidator periodicValidator;
      if (reader.TryStartClassDeserialization<ClearancePathabilityProviderPeriodicValidator>(out periodicValidator))
        reader.EnqueueDataDeserialization((object) periodicValidator, ClearancePathabilityProviderPeriodicValidator.s_deserializeDataDelayedAction);
      return periodicValidator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ClearancePathabilityProviderPeriodicValidator>(this, "m_clearancePathabilityProvider", (object) ClearancePathabilityProvider.Deserialize(reader));
      this.m_isEnabled = reader.ReadBool();
      reader.SetField<ClearancePathabilityProviderPeriodicValidator>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<ClearancePathabilityProviderPeriodicValidator>(this, "m_timer", (object) TickTimer.Deserialize(reader));
    }

    static ClearancePathabilityProviderPeriodicValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ClearancePathabilityProviderPeriodicValidator.CHECK_FREQUENCY = 30.Seconds();
      ClearancePathabilityProviderPeriodicValidator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ClearancePathabilityProviderPeriodicValidator) obj).SerializeData(writer));
      ClearancePathabilityProviderPeriodicValidator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ClearancePathabilityProviderPeriodicValidator) obj).DeserializeData(reader));
    }
  }
}
