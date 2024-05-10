// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SoftCapNoise2D
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random.Noise
{
  [GenerateSerializer(false, null, 0)]
  public class SoftCapNoise2D : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly INoise2D Noise;
    public readonly SoftCapNoiseParams Parameters;
    private readonly Fix64 m_capStart;
    private readonly Fix64 m_capEnd;
    private readonly Fix64 m_transitionSize;

    public static void Serialize(SoftCapNoise2D value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SoftCapNoise2D>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SoftCapNoise2D.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix64.Serialize(this.m_capEnd, writer);
      Fix64.Serialize(this.m_capStart, writer);
      Fix64.Serialize(this.m_transitionSize, writer);
      writer.WriteGeneric<INoise2D>(this.Noise);
      SoftCapNoiseParams.Serialize(this.Parameters, writer);
    }

    public static SoftCapNoise2D Deserialize(BlobReader reader)
    {
      SoftCapNoise2D softCapNoise2D;
      if (reader.TryStartClassDeserialization<SoftCapNoise2D>(out softCapNoise2D))
        reader.EnqueueDataDeserialization((object) softCapNoise2D, SoftCapNoise2D.s_deserializeDataDelayedAction);
      return softCapNoise2D;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SoftCapNoise2D>(this, "m_capEnd", (object) Fix64.Deserialize(reader));
      reader.SetField<SoftCapNoise2D>(this, "m_capStart", (object) Fix64.Deserialize(reader));
      reader.SetField<SoftCapNoise2D>(this, "m_transitionSize", (object) Fix64.Deserialize(reader));
      reader.SetField<SoftCapNoise2D>(this, "Noise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<SoftCapNoise2D>(this, "Parameters", (object) SoftCapNoiseParams.Deserialize(reader));
    }

    public Fix32 MeanValue => this.Noise.MeanValue;

    public Fix32 Amplitude => this.Noise.Amplitude;

    public Fix32 Period => this.Noise.Period;

    public SoftCapNoise2D(INoise2D noise, SoftCapNoiseParams parameters)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Noise = noise;
      this.Parameters = parameters;
      this.m_capStart = parameters.CapStart.ToFix64();
      this.m_capEnd = parameters.CapEnd.ToFix64();
      this.m_transitionSize = this.m_capEnd - this.m_capStart;
      if (!this.m_transitionSize.IsNearZero())
        return;
      Log.Warning("SoftCapNoise2D with too close start and end");
      this.m_transitionSize = 0.01.ToFix64();
      this.m_capEnd = this.m_capStart + this.m_transitionSize;
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 fix64_1 = this.Noise.GetValue(point);
      Fix64 fix64_2 = (fix64_1 - this.m_capStart) / this.m_transitionSize;
      if (fix64_2.IsNotPositive)
        return fix64_1;
      if (fix64_2 >= Fix64.TauOver4)
        return this.m_capEnd;
      fix64_2 = fix64_2.Sin();
      return this.m_capStart + fix64_2 * this.m_transitionSize;
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new SoftCapNoise2D(this.Noise.ReseedClone(random), this.Parameters);
    }

    static SoftCapNoise2D()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SoftCapNoise2D.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SoftCapNoise2D) obj).SerializeData(writer));
      SoftCapNoise2D.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SoftCapNoise2D) obj).DeserializeData(reader));
    }
  }
}
