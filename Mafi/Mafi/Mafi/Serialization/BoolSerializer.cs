// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.BoolSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class BoolSerializer : IGenericSerializer<bool>
  {
    private static BoolSerializer s_instance;

    public static BoolSerializer Instance
    {
      get => BoolSerializer.s_instance ?? (BoolSerializer.s_instance = new BoolSerializer());
    }

    public Action<bool, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, bool> DeserializeFunction { get; }

    private BoolSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<bool, BlobWriter>) ((value, writer) => writer.WriteBool(value));
      this.DeserializeFunction = (Func<BlobReader, bool>) (reader => reader.ReadBool());
    }
  }
}
