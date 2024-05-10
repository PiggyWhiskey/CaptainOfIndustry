// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.EnumSerializer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class EnumSerializer<T> : IGenericSerializer<T>
  {
    private static EnumSerializer<T> s_instance;

    public static EnumSerializer<T> Instance
    {
      get
      {
        return EnumSerializer<T>.s_instance ?? (EnumSerializer<T>.s_instance = new EnumSerializer<T>());
      }
    }

    public Action<T, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, T> DeserializeFunction { get; }

    private EnumSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (!typeof (T).IsEnum)
        throw new Exception("Failed to create 'EnumSerializer',type '" + typeof (T).Name + "' is not an enum.");
      this.SerializeAction = (Action<T, BlobWriter>) ((value, writer) => writer.WriteInt(CastTo<int>.From<T>(value)));
      this.DeserializeFunction = (Func<BlobReader, T>) (reader => CastTo<T>.From<int>(reader.ReadInt()));
    }
  }
}
