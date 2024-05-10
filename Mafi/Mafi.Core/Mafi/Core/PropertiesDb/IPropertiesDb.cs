// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.IPropertiesDb
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  /// <summary>
  /// Interface of a store of game properties that are (or are expected to be) read from/written to/listened to change
  /// from multiple different places. The store interface allows to get/set/register listener to change of a set of
  /// properties.
  /// </summary>
  public interface IPropertiesDb
  {
    IProperty<T> GetProperty<T>(PropertyId<T> id) where T : IEquatable<T>;
  }
}
