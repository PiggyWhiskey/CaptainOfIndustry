// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ShortSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class ShortSerializer : IGenericSerializer<short>
  {
    private static ShortSerializer s_instance;

    public static ShortSerializer Instance
    {
      get => ShortSerializer.s_instance ?? (ShortSerializer.s_instance = new ShortSerializer());
    }

    public Action<short, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, short> DeserializeFunction { get; }

    private ShortSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<short, BlobWriter>) ((value, writer) => writer.WriteShort(value));
      this.DeserializeFunction = (Func<BlobReader, short>) (reader => reader.ReadShort());
    }
  }
}
