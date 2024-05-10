// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.NoProtoAllowedSerializerFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Prototypes
{
  /// <summary>
  /// Factory that throws when proto is attempted to be serialized/deserialized.
  /// </summary>
  /// <remarks>This is for example used during config serialization in game saves.</remarks>
  public sealed class NoProtoAllowedSerializerFactory : 
    ISpecialSerializerFactoryCustom,
    ISpecialSerializerFactory
  {
    private readonly string m_serializeExceptionMessage;
    private readonly string m_deserializeExceptionMessage;

    public NoProtoAllowedSerializerFactory(
      string serializeExceptionMessage,
      string deserializeExceptionMessage)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_serializeExceptionMessage = serializeExceptionMessage;
      this.m_deserializeExceptionMessage = deserializeExceptionMessage;
    }

    public bool CanSerialize<T>() => typeof (T).IsAssignableTo<Proto>();

    public Action<T, BlobWriter> GetSerializeAction<T>()
    {
      throw new InvalidCastException(this.m_serializeExceptionMessage);
    }

    public Func<BlobReader, T> GetDeserializeFunction<T>()
    {
      throw new InvalidCastException(this.m_deserializeExceptionMessage);
    }
  }
}
