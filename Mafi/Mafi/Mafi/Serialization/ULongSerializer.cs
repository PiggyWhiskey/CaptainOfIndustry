// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ULongSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class ULongSerializer : IGenericSerializer<ulong>
  {
    private static ULongSerializer s_instance;

    public static ULongSerializer Instance
    {
      get => ULongSerializer.s_instance ?? (ULongSerializer.s_instance = new ULongSerializer());
    }

    public Action<ulong, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, ulong> DeserializeFunction { get; }

    private ULongSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<ulong, BlobWriter>) ((value, writer) => writer.WriteULong(value));
      this.DeserializeFunction = (Func<BlobReader, ulong>) (reader => reader.ReadULong());
    }
  }
}
