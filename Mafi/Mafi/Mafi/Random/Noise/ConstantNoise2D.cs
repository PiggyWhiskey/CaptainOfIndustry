// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.ConstantNoise2D
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
  public class ConstantNoise2D : INoise2D
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(ConstantNoise2D value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ConstantNoise2D>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ConstantNoise2D.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.MeanValue, writer);
    }

    public static ConstantNoise2D Deserialize(BlobReader reader)
    {
      ConstantNoise2D constantNoise2D;
      if (reader.TryStartClassDeserialization<ConstantNoise2D>(out constantNoise2D))
        reader.EnqueueDataDeserialization((object) constantNoise2D, ConstantNoise2D.s_deserializeDataDelayedAction);
      return constantNoise2D;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.MeanValue = Fix32.Deserialize(reader);
    }

    public Fix32 MeanValue { get; private set; }

    public Fix32 Amplitude => Fix32.Zero;

    public Fix32 Period => Fix32.One;

    public ConstantNoise2D(Fix32 meanValue)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MeanValue = meanValue;
    }

    public Fix64 GetValue(Vector2f point) => this.MeanValue.ToFix64();

    public INoise2D ReseedClone(IRandom random) => (INoise2D) this;

    static ConstantNoise2D()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      ConstantNoise2D.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ConstantNoise2D) obj).SerializeData(writer));
      ConstantNoise2D.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ConstantNoise2D) obj).DeserializeData(reader));
    }
  }
}
