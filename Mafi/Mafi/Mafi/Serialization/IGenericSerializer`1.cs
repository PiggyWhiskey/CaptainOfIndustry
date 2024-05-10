// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.IGenericSerializer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Serialization
{
  public interface IGenericSerializer<T>
  {
    Action<T, BlobWriter> SerializeAction { get; }

    Func<BlobReader, T> DeserializeFunction { get; }
  }
}
