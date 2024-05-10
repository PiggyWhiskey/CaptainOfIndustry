// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.IntSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class IntSerializer : IGenericSerializer<int>
  {
    private static IntSerializer s_instance;

    public static IntSerializer Instance
    {
      get => IntSerializer.s_instance ?? (IntSerializer.s_instance = new IntSerializer());
    }

    public Action<int, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, int> DeserializeFunction { get; }

    private IntSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<int, BlobWriter>) ((value, writer) => writer.WriteInt(value));
      this.DeserializeFunction = (Func<BlobReader, int>) (reader => reader.ReadInt());
    }
  }
}
