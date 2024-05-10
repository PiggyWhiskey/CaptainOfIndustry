// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ByteSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class ByteSerializer : IGenericSerializer<byte>
  {
    private static ByteSerializer s_instance;

    public static ByteSerializer Instance
    {
      get => ByteSerializer.s_instance ?? (ByteSerializer.s_instance = new ByteSerializer());
    }

    public Action<byte, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, byte> DeserializeFunction { get; }

    private ByteSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<byte, BlobWriter>) ((value, writer) => writer.WriteByte(value));
      this.DeserializeFunction = (Func<BlobReader, byte>) (reader => reader.ReadByte());
    }
  }
}
