﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.Noise2dTransformAdd
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
  public class Noise2dTransformAdd : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_baseNoise;
    public readonly Fix64 Addend;

    public static void Serialize(Noise2dTransformAdd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Noise2dTransformAdd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Noise2dTransformAdd.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix64.Serialize(this.Addend, writer);
      writer.WriteGeneric<INoise2D>(this.m_baseNoise);
    }

    public static Noise2dTransformAdd Deserialize(BlobReader reader)
    {
      Noise2dTransformAdd noise2dTransformAdd;
      if (reader.TryStartClassDeserialization<Noise2dTransformAdd>(out noise2dTransformAdd))
        reader.EnqueueDataDeserialization((object) noise2dTransformAdd, Noise2dTransformAdd.s_deserializeDataDelayedAction);
      return noise2dTransformAdd;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<Noise2dTransformAdd>(this, "Addend", (object) Fix64.Deserialize(reader));
      reader.SetField<Noise2dTransformAdd>(this, "m_baseNoise", (object) reader.ReadGenericAs<INoise2D>());
    }

    public Noise2dTransformAdd(INoise2D baseNoise, Fix64 addend)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Addend = addend;
      this.m_baseNoise = baseNoise.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => (this.m_baseNoise.MeanValue + this.Addend).ToFix32();

    public Fix32 Amplitude => this.m_baseNoise.Amplitude;

    public Fix32 Period => this.m_baseNoise.Period;

    public Fix64 GetValue(Vector2f point) => this.Addend + this.m_baseNoise.GetValue(point);

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static Noise2dTransformAdd()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Noise2dTransformAdd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Noise2dTransformAdd) obj).SerializeData(writer));
      Noise2dTransformAdd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Noise2dTransformAdd) obj).DeserializeData(reader));
    }
  }
}
