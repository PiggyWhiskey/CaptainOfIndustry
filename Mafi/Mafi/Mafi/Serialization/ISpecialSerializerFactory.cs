// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ISpecialSerializerFactory
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Serialization
{
  [NotGlobalDependency]
  public interface ISpecialSerializerFactory
  {
    /// <summary>
    /// Whether this serializer can handle serialization of the given type.
    /// </summary>
    bool CanSerialize<T>();

    Action<T, BlobWriter> GetSerializeAction<T>();

    Func<BlobReader, T> GetDeserializeFunction<T>();
  }
}
