// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.ExpBendNoise2D
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
  /// <summary>
  /// "Bends" values so that they go exponentially up in positive coordinates and slow down logarithmically for negative.
  /// This uses <c>-ln(-x * A + 1) / A</c> function for x &gt;= 0 and <c>(e^(x * A) - 1) / A</c> for x less than 0.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class ExpBendNoise2D : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly double MAX_VALUE;
    public readonly INoise2D BaseNoise;
    public readonly Fix64 Amount;
    public readonly Fix64 Bias;

    public static void Serialize(ExpBendNoise2D value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ExpBendNoise2D>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ExpBendNoise2D.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix64.Serialize(this.Amount, writer);
      writer.WriteGeneric<INoise2D>(this.BaseNoise);
      Fix64.Serialize(this.Bias, writer);
    }

    public static ExpBendNoise2D Deserialize(BlobReader reader)
    {
      ExpBendNoise2D expBendNoise2D;
      if (reader.TryStartClassDeserialization<ExpBendNoise2D>(out expBendNoise2D))
        reader.EnqueueDataDeserialization((object) expBendNoise2D, ExpBendNoise2D.s_deserializeDataDelayedAction);
      return expBendNoise2D;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ExpBendNoise2D>(this, "Amount", (object) Fix64.Deserialize(reader));
      reader.SetField<ExpBendNoise2D>(this, "BaseNoise", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<ExpBendNoise2D>(this, "Bias", (object) Fix64.Deserialize(reader));
    }

    public Fix32 MeanValue => this.BaseNoise.MeanValue;

    public Fix32 Amplitude => this.BaseNoise.Amplitude;

    public Fix32 Period => this.BaseNoise.Period;

    public ExpBendNoise2D(INoise2D noise, Fix64 amount, Fix64 bias)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Bias = bias;
      this.BaseNoise = noise.CheckNotNull<INoise2D>();
      this.Amount = amount.Clamp(Fix64.EpsilonNear, 0.125.ToFix64());
    }

    public Fix64 GetValue(Vector2f point)
    {
      Fix64 fix64 = (this.BaseNoise.GetValue(point) - this.Bias) * this.Amount;
      return (fix64 >= this.Bias ? fix64.ExpClamped(ExpBendNoise2D.MAX_VALUE) - Fix64.One : -(Fix64.One - fix64).LogNatural()).DivByPositiveUncheckedUnrounded(this.Amount) + this.Bias;
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new ExpBendNoise2D(this.BaseNoise.ReseedClone(random), this.Amount, this.Bias);
    }

    static ExpBendNoise2D()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ExpBendNoise2D.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ExpBendNoise2D) obj).SerializeData(writer));
      ExpBendNoise2D.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ExpBendNoise2D) obj).DeserializeData(reader));
      ExpBendNoise2D.MAX_VALUE = 4096.0;
    }
  }
}
