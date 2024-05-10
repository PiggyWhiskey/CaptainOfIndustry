// Decompiled with JetBrains decompiler
// Type: Mafi.CollectionAccessType
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Provides a value for the <see cref="T:Mafi.CollectionAccessAttribute" /> to define
  /// how the collection method invocation affects the contents of the collection.
  /// </summary>
  [Flags]
  public enum CollectionAccessType
  {
    /// <summary>Method does not use or modify content of the collection.</summary>
    None = 0,
    /// <summary>Method only reads content of the collection but does not modify it.</summary>
    Read = 1,
    /// <summary>Method can change content of the collection but does not add new elements.</summary>
    ModifyExistingContent = 2,
    /// <summary>Method can add new elements to the collection.</summary>
    UpdatedContent = 6,
  }
}
