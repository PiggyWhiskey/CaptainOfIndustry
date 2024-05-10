// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.CharSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class CharSerializer : IGenericSerializer<char>
  {
    private static CharSerializer s_instance;

    public static CharSerializer Instance
    {
      get => CharSerializer.s_instance ?? (CharSerializer.s_instance = new CharSerializer());
    }

    public Action<char, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, char> DeserializeFunction { get; }

    private CharSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<char, BlobWriter>) ((value, writer) => writer.WriteChar(value));
      this.DeserializeFunction = (Func<BlobReader, char>) (reader => reader.ReadChar());
    }
  }
}
