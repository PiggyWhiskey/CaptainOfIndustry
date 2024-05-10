// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.SumOp
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
  public class SumOp : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly INoise2D m_lhs;
    private readonly INoise2D m_rhs;

    public static void Serialize(SumOp value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SumOp>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SumOp.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<INoise2D>(this.m_lhs);
      writer.WriteGeneric<INoise2D>(this.m_rhs);
    }

    public static SumOp Deserialize(BlobReader reader)
    {
      SumOp sumOp;
      if (reader.TryStartClassDeserialization<SumOp>(out sumOp))
        reader.EnqueueDataDeserialization((object) sumOp, SumOp.s_deserializeDataDelayedAction);
      return sumOp;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SumOp>(this, "m_lhs", (object) reader.ReadGenericAs<INoise2D>());
      reader.SetField<SumOp>(this, "m_rhs", (object) reader.ReadGenericAs<INoise2D>());
    }

    public SumOp(INoise2D lhs, INoise2D rhs)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_lhs = lhs.CheckNotNull<INoise2D>();
      this.m_rhs = rhs.CheckNotNull<INoise2D>();
    }

    public Fix32 MeanValue => (this.m_lhs.MeanValue + this.m_rhs.MeanValue) / 2;

    public Fix32 Amplitude => this.m_lhs.Amplitude + this.m_rhs.Amplitude;

    public Fix32 Period => this.m_lhs.Period.Min(this.m_rhs.Period);

    public Fix64 GetValue(Vector2f point)
    {
      return this.m_lhs.GetValue(point) + this.m_rhs.GetValue(point);
    }

    public INoise2D ReseedClone(IRandom random)
    {
      return (INoise2D) new SumOp(this.m_lhs.ReseedClone(random), this.m_rhs.ReseedClone(random));
    }

    static SumOp()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SumOp.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SumOp) obj).SerializeData(writer));
      SumOp.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SumOp) obj).DeserializeData(reader));
    }
  }
}
