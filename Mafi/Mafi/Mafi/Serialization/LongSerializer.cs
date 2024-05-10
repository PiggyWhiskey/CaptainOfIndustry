﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.LongSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class LongSerializer : IGenericSerializer<long>
  {
    private static LongSerializer s_instance;

    public static LongSerializer Instance
    {
      get => LongSerializer.s_instance ?? (LongSerializer.s_instance = new LongSerializer());
    }

    public Action<long, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, long> DeserializeFunction { get; }

    private LongSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<long, BlobWriter>) ((value, writer) => writer.WriteLong(value));
      this.DeserializeFunction = (Func<BlobReader, long>) (reader => reader.ReadLong());
    }
  }
}
