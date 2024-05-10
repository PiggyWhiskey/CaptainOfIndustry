// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.UShortSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class UShortSerializer : IGenericSerializer<ushort>
  {
    private static UShortSerializer s_instance;

    public static UShortSerializer Instance
    {
      get => UShortSerializer.s_instance ?? (UShortSerializer.s_instance = new UShortSerializer());
    }

    public Action<ushort, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, ushort> DeserializeFunction { get; }

    private UShortSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<ushort, BlobWriter>) ((value, writer) => writer.WriteUShort(value));
      this.DeserializeFunction = (Func<BlobReader, ushort>) (reader => reader.ReadUShort());
    }
  }
}
