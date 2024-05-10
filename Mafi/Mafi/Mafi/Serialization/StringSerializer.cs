// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.StringSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class StringSerializer : IGenericSerializer<string>
  {
    private static StringSerializer s_instance;

    public static StringSerializer Instance
    {
      get => StringSerializer.s_instance ?? (StringSerializer.s_instance = new StringSerializer());
    }

    public Action<string, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, string> DeserializeFunction { get; }

    private StringSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<string, BlobWriter>) ((value, writer) => writer.WriteString(value));
      this.DeserializeFunction = (Func<BlobReader, string>) (reader => reader.ReadString());
    }
  }
}
