// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.SByteSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class SByteSerializer : IGenericSerializer<sbyte>
  {
    private static SByteSerializer s_instance;

    public static SByteSerializer Instance
    {
      get => SByteSerializer.s_instance ?? (SByteSerializer.s_instance = new SByteSerializer());
    }

    public Action<sbyte, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, sbyte> DeserializeFunction { get; }

    private SByteSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<sbyte, BlobWriter>) ((value, writer) => writer.WriteSByte(value));
      this.DeserializeFunction = (Func<BlobReader, sbyte>) (reader => reader.ReadSByte());
    }
  }
}
