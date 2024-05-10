// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.LineDistanceNoise
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random.Noise
{
  /// <summary>Returns distance to a line segment as value.</summary>
  [GenerateSerializer(false, null, 0)]
  public class LineDistanceNoise : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Line2f m_line;
    private readonly Fix32 m_insideRadius;
    private readonly Fix32 m_transitionDistance;
    private readonly Fix64 m_insideValue;
    private readonly bool m_clampInsideValue;
    private readonly Fix64 m_outsideValue;
    private readonly bool m_clampOutsideValue;
    private readonly LineTransitionFn m_transitionFn;

    public static void Serialize(LineDistanceNoise value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LineDistanceNoise>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LineDistanceNoise.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.m_clampInsideValue);
      writer.WriteBool(this.m_clampOutsideValue);
      Fix32.Serialize(this.m_insideRadius, writer);
      Fix64.Serialize(this.m_insideValue, writer);
      Line2f.Serialize(this.m_line, writer);
      Fix64.Serialize(this.m_outsideValue, writer);
      Fix32.Serialize(this.m_transitionDistance, writer);
      writer.WriteInt((int) this.m_transitionFn);
      Fix32.Serialize(this.MeanValue, writer);
      Fix32.Serialize(this.Period, writer);
    }

    public static LineDistanceNoise Deserialize(BlobReader reader)
    {
      LineDistanceNoise lineDistanceNoise;
      if (reader.TryStartClassDeserialization<LineDistanceNoise>(out lineDistanceNoise))
        reader.EnqueueDataDeserialization((object) lineDistanceNoise, LineDistanceNoise.s_deserializeDataDelayedAction);
      return lineDistanceNoise;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<LineDistanceNoise>(this, "m_clampInsideValue", (object) reader.ReadBool());
      reader.SetField<LineDistanceNoise>(this, "m_clampOutsideValue", (object) reader.ReadBool());
      reader.SetField<LineDistanceNoise>(this, "m_insideRadius", (object) Fix32.Deserialize(reader));
      reader.SetField<LineDistanceNoise>(this, "m_insideValue", (object) Fix64.Deserialize(reader));
      reader.SetField<LineDistanceNoise>(this, "m_line", (object) Line2f.Deserialize(reader));
      reader.SetField<LineDistanceNoise>(this, "m_outsideValue", (object) Fix64.Deserialize(reader));
      reader.SetField<LineDistanceNoise>(this, "m_transitionDistance", (object) Fix32.Deserialize(reader));
      reader.SetField<LineDistanceNoise>(this, "m_transitionFn", (object) (LineTransitionFn) reader.ReadInt());
      this.MeanValue = Fix32.Deserialize(reader);
      this.Period = Fix32.Deserialize(reader);
    }

    public Fix32 MeanValue { get; private set; }

    public Fix32 Amplitude => this.MeanValue;

    public Fix32 Period { get; private set; }

    public LineDistanceNoise(
      Line2f line,
      Fix32 insideRadius,
      Fix32 transitionDistance,
      Fix64 insideValue,
      bool clampInsideValue,
      Fix64 outsideValue,
      bool clampOutsideValue,
      LineTransitionFn transitionFn)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector2f>(line.Direction).IsNotZero("Degenerate line.");
      this.m_line = line;
      this.m_insideRadius = insideRadius.CheckNotNegative();
      this.m_transitionDistance = transitionDistance.CheckPositive();
      this.m_insideValue = insideValue;
      this.m_clampInsideValue = clampInsideValue;
      this.m_outsideValue = outsideValue;
      this.m_clampOutsideValue = clampOutsideValue;
      this.m_transitionFn = transitionFn;
      this.MeanValue = (this.m_insideValue + this.m_outsideValue).DivToFix32(2);
      this.Period = line.P0.DistanceTo(line.P1);
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 lineSegment = this.m_line.DistanceSqrToLineSegment(point);
      if (this.m_clampInsideValue && lineSegment <= this.m_insideRadius.Squared())
        return this.m_insideValue;
      if (this.m_clampOutsideValue && lineSegment >= (this.m_insideRadius + this.m_transitionDistance).Squared())
        return this.m_outsideValue;
      Percent t = Percent.FromRatio(this.m_insideRadius - lineSegment.SqrtToFix32(), this.m_transitionDistance) + Percent.Hundred;
      switch (this.m_transitionFn)
      {
        case LineTransitionFn.Sine:
          if (t > Percent.Hundred)
          {
            t = Percent.Hundred;
            break;
          }
          if (t.IsPositive)
          {
            t = (t * Percent.Tau / 4).Sin();
            break;
          }
          break;
      }
      return this.m_outsideValue.Lerp(this.m_insideValue, t);
    }

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static LineDistanceNoise()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LineDistanceNoise.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LineDistanceNoise) obj).SerializeData(writer));
      LineDistanceNoise.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LineDistanceNoise) obj).DeserializeData(reader));
    }
  }
}
