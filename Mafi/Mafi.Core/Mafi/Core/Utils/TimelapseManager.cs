// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.TimelapseManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Mafi.Core.Utils
{
  /// <summary>
  /// Manages timelapses. This class is in core since it has savable state.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TimelapseManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly Dict<string, TimelapseData> m_data;
    [DoNotSave(0, null)]
    private bool m_isSubscribedToSync;

    public static void Serialize(TimelapseManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TimelapseManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TimelapseManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<string, TimelapseData>.Serialize(this.m_data, writer);
      writer.WriteGeneric<IGameLoopEvents>(this.m_gameLoopEvents);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
    }

    public static TimelapseManager Deserialize(BlobReader reader)
    {
      TimelapseManager timelapseManager;
      if (reader.TryStartClassDeserialization<TimelapseManager>(out timelapseManager))
        reader.EnqueueDataDeserialization((object) timelapseManager, TimelapseManager.s_deserializeDataDelayedAction);
      return timelapseManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TimelapseManager>(this, "m_data", (object) Dict<string, TimelapseData>.Deserialize(reader));
      reader.SetField<TimelapseManager>(this, "m_gameLoopEvents", (object) reader.ReadGenericAs<IGameLoopEvents>());
      reader.SetField<TimelapseManager>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.RegisterInitAfterLoad<TimelapseManager>(this, "initAfterLoad", InitPriority.Normal);
    }

    public event Action<TimelapseData> OnTimelapseCapture;

    public IReadOnlyDictionary<string, TimelapseData> Data
    {
      get => (IReadOnlyDictionary<string, TimelapseData>) this.m_data;
    }

    public TimelapseManager(IGameLoopEvents gameLoopEvents, ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_data = new Dict<string, TimelapseData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_simLoopEvents = simLoopEvents;
    }

    [InitAfterLoad(InitPriority.Normal)]
    public void initAfterLoad() => this.updateEventsRegistration();

    private void updateEventsRegistration()
    {
      if (this.m_data.IsNotEmpty)
      {
        if (this.m_isSubscribedToSync)
          return;
        this.m_isSubscribedToSync = true;
        this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<TimelapseManager>(this, new Action<GameTime>(this.syncUpdate));
      }
      else
      {
        if (!this.m_isSubscribedToSync)
          return;
        this.m_isSubscribedToSync = false;
        this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<TimelapseManager>(this, new Action<GameTime>(this.syncUpdate));
      }
    }

    private void syncUpdate(GameTime time)
    {
      Assert.That<bool>(this.m_isSubscribedToSync).IsTrue();
      foreach (TimelapseData timelapseData in this.m_data.Values)
      {
        SimStep currentStep = this.m_simLoopEvents.CurrentStep;
        if (currentStep >= timelapseData.NextCaptureStep)
        {
          timelapseData.NotifyInvoked(currentStep);
          Action<TimelapseData> timelapseCapture = this.OnTimelapseCapture;
          if (timelapseCapture != null)
            timelapseCapture(timelapseData);
        }
      }
    }

    public bool TryAddTimelapse(TimelapseData data, out string error)
    {
      Assert.That<int>(data.CapturedCount).IsEqualTo(0);
      if (data.Name.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
      {
        error = "Timelapse name '" + data.Name + "' contains illegal characters. The name needs to be valid directory name.";
        return false;
      }
      if (this.m_data.ContainsKey(data.Name))
      {
        error = "Timelapse with the same name already exists.";
        return false;
      }
      data.NotifyInvoked(this.m_simLoopEvents.CurrentStep);
      Action<TimelapseData> timelapseCapture = this.OnTimelapseCapture;
      if (timelapseCapture != null)
        timelapseCapture(data);
      if (data.PreviousCaptureError.HasValue)
      {
        error = data.PreviousCaptureError.Value;
        return false;
      }
      this.m_data.Add(data.Name, data);
      this.updateEventsRegistration();
      error = (string) null;
      return true;
    }

    public bool TryRemoveTimelapse(string name)
    {
      if (!this.m_data.Remove(name))
        return false;
      Assert.That<bool>(this.m_isSubscribedToSync).IsTrue();
      this.updateEventsRegistration();
      return true;
    }

    static TimelapseManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TimelapseManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TimelapseManager) obj).SerializeData(writer));
      TimelapseManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TimelapseManager) obj).DeserializeData(reader));
    }
  }
}
