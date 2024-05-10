// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.FloatSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class FloatSerializer : IGenericSerializer<float>
  {
    private static FloatSerializer s_instance;

    public static FloatSerializer Instance
    {
      get => FloatSerializer.s_instance ?? (FloatSerializer.s_instance = new FloatSerializer());
    }

    public Action<float, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, float> DeserializeFunction { get; }

    private FloatSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<float, BlobWriter>) ((value, writer) => writer.WriteFloat(value));
      this.DeserializeFunction = (Func<BlobReader, float>) (reader => reader.ReadFloat());
    }
  }
}
