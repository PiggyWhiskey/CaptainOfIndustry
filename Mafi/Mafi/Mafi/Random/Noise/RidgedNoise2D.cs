// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.RidgedNoise2D
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
  public class RidgedNoise2D : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_noise;

    public static void Serialize(RidgedNoise2D value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RidgedNoise2D>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RidgedNoise2D.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_noise);
    }

    public static RidgedNoise2D Deserialize(BlobReader reader)
    {
      RidgedNoise2D ridgedNoise2D;
      if (reader.TryStartClassDeserialization<RidgedNoise2D>(out ridgedNoise2D))
        reader.EnqueueDataDeserialization((object) ridgedNoise2D, RidgedNoise2D.s_deserializeDataDelayedAction);
      return ridgedNoise2D;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<RidgedNoise2D>(this, "m_noise", (object) reader.ReadGenericAs<INoise2D>());
    }

    public Fix32 MeanValue => this.m_noise.MeanValue;

    public Fix32 Amplitude => this.m_noise.Amplitude;

    public Fix32 Period => this.m_noise.Period / 2;

    public RidgedNoise2D(INoise2D noise)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_noise = noise.CheckNotNull<INoise2D>();
    }

    public Fix64 GetValue(Vector2f point)
    {
      return this.m_noise.Amplitude - 2 * (this.m_noise.GetValue(point) - this.m_noise.MeanValue).Abs() + this.m_noise.MeanValue;
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new RidgedNoise2D(this.m_noise.ReseedClone(random));
    }

    static RidgedNoise2D()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      RidgedNoise2D.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RidgedNoise2D) obj).SerializeData(writer));
      RidgedNoise2D.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RidgedNoise2D) obj).DeserializeData(reader));
    }
  }
}
