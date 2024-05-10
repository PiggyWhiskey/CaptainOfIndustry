// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.DoubleSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class DoubleSerializer : IGenericSerializer<double>
  {
    private static DoubleSerializer s_instance;

    public static DoubleSerializer Instance
    {
      get => DoubleSerializer.s_instance ?? (DoubleSerializer.s_instance = new DoubleSerializer());
    }

    public Action<double, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, double> DeserializeFunction { get; }

    private DoubleSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<double, BlobWriter>) ((value, writer) => writer.WriteDouble(value));
      this.DeserializeFunction = (Func<BlobReader, double>) (reader => reader.ReadDouble());
    }
  }
}
