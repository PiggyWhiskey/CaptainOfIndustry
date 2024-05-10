// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.TypeSerializer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class TypeSerializer : IGenericSerializer<Type>
  {
    private static TypeSerializer s_instance;

    public static TypeSerializer Instance
    {
      get => TypeSerializer.s_instance ?? (TypeSerializer.s_instance = new TypeSerializer());
    }

    public Action<Type, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, Type> DeserializeFunction { get; }

    private TypeSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<Type, BlobWriter>) ((value, writer) => writer.WriteType(value));
      this.DeserializeFunction = (Func<BlobReader, Type>) (reader => reader.ReadType());
    }
  }
}
