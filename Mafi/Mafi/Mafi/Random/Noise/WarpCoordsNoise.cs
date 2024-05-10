// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.WarpCoordsNoise
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
  public class WarpCoordsNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_noise;
    private readonly INoise2D m_warpNoise;

    public static void Serialize(WarpCoordsNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WarpCoordsNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WarpCoordsNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_noise);
      writer.WriteGeneric<INoise2D>(this.m_warpNoise);
    }

    public static WarpCoordsNoise Deserialize(BlobReader reader)
    {
      WarpCoordsNoise warpCoordsNoise;
      if (reader.TryStartClassDeserialization<WarpCoordsNoise>(out warpCoordsNoise))
        reader.EnqueueDataDeserialization((object) warpCoordsNoise, WarpCoordsNoise.s_deserializeDataDelayedAction);
      return warpCoordsNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WarpCoordsNoise>(this, "m_noise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<WarpCoordsNoise>(this, "m_warpNoise", (object) reader.ReadGenericAs<INoise2D>());
    }

    public Fix32 MeanValue => this.m_noise.MeanValue;

    public Fix32 Amplitude => this.m_noise.Amplitude;

    public Fix32 Period => this.m_noise.Period;

    public WarpCoordsNoise(INoise2D noise, INoise2D warpNoise)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_noise = noise;
      this.m_warpNoise = warpNoise;
    }

    public Fix64 GetValue(Vector2f point)
    {
      INoise2D noise = this.m_noise;
      Fix64 fix64 = this.m_warpNoise.GetValue(point);
      Fix32 x = fix64.ToFix32() + point.X;
      fix64 = this.m_warpNoise.GetValue(new Vector2f(point.Y, -point.X));
      Fix32 y = fix64.ToFix32() + point.Y;
      Vector2f point1 = new Vector2f(x, y);
      return noise.GetValue(point1);
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new WarpCoordsNoise(this.m_noise.ReseedClone(random), this.m_warpNoise.ReseedClone(random));
    }

    static WarpCoordsNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      WarpCoordsNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WarpCoordsNoise) obj).SerializeData(writer));
      WarpCoordsNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WarpCoordsNoise) obj).DeserializeData(reader));
    }
  }
}
