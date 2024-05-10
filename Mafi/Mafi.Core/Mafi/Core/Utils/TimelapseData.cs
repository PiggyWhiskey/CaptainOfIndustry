// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.TimelapseData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  [GenerateSerializer(false, null, 0)]
  public class TimelapseData
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly string Name;
    public readonly Duration CaptureInterval;
    public readonly Vector2i Size;
    public readonly Vector3f CameraPosition;
    public readonly UnitQuaternion4f CameraRotation;
    public readonly Vector2f CameraNearFarPlanes;
    public readonly bool SuperSize;
    public readonly bool SaveAsJpg;

    public static void Serialize(TimelapseData value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TimelapseData>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TimelapseData.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Vector2f.Serialize(this.CameraNearFarPlanes, writer);
      Vector3f.Serialize(this.CameraPosition, writer);
      UnitQuaternion4f.Serialize(this.CameraRotation, writer);
      writer.WriteInt(this.CapturedCount);
      Duration.Serialize(this.CaptureInterval, writer);
      writer.WriteString(this.Name);
      Option<string>.Serialize(this.PreviousCaptureError, writer);
      SimStep.Serialize(this.PreviousInvokeStep, writer);
      writer.WriteBool(this.SaveAsJpg);
      Vector2i.Serialize(this.Size, writer);
      writer.WriteBool(this.SuperSize);
    }

    public static TimelapseData Deserialize(BlobReader reader)
    {
      TimelapseData timelapseData;
      if (reader.TryStartClassDeserialization<TimelapseData>(out timelapseData))
        reader.EnqueueDataDeserialization((object) timelapseData, TimelapseData.s_deserializeDataDelayedAction);
      return timelapseData;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TimelapseData>(this, "CameraNearFarPlanes", (object) Vector2f.Deserialize(reader));
      reader.SetField<TimelapseData>(this, "CameraPosition", (object) Vector3f.Deserialize(reader));
      reader.SetField<TimelapseData>(this, "CameraRotation", (object) UnitQuaternion4f.Deserialize(reader));
      this.CapturedCount = reader.ReadInt();
      reader.SetField<TimelapseData>(this, "CaptureInterval", (object) Duration.Deserialize(reader));
      reader.SetField<TimelapseData>(this, "Name", (object) reader.ReadString());
      this.PreviousCaptureError = Option<string>.Deserialize(reader);
      this.PreviousInvokeStep = SimStep.Deserialize(reader);
      reader.SetField<TimelapseData>(this, "SaveAsJpg", (object) reader.ReadBool());
      reader.SetField<TimelapseData>(this, "Size", (object) Vector2i.Deserialize(reader));
      reader.SetField<TimelapseData>(this, "SuperSize", (object) reader.ReadBool());
    }

    public SimStep PreviousInvokeStep { get; private set; }

    public Option<string> PreviousCaptureError { get; private set; }

    public SimStep NextCaptureStep => this.PreviousInvokeStep + this.CaptureInterval;

    public int CapturedCount { get; private set; }

    public TimelapseData(
      string name,
      Duration captureInterval,
      Vector2i size,
      Vector3f cameraPosition,
      UnitQuaternion4f cameraRotation,
      Vector2f cameraNearFarPlanes,
      bool superSize,
      bool saveAsJpg)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.CaptureInterval = captureInterval;
      this.Size = size;
      this.CameraPosition = cameraPosition;
      this.CameraRotation = cameraRotation;
      this.CameraNearFarPlanes = cameraNearFarPlanes;
      this.SuperSize = superSize;
      this.SaveAsJpg = saveAsJpg;
    }

    internal void NotifyInvoked(SimStep step) => this.PreviousInvokeStep = step;

    internal void NotifyCaptured(Option<string> error)
    {
      this.PreviousCaptureError = error;
      if (!error.IsNone)
        return;
      ++this.CapturedCount;
    }

    static TimelapseData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TimelapseData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TimelapseData) obj).SerializeData(writer));
      TimelapseData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TimelapseData) obj).DeserializeData(reader));
    }
  }
}
