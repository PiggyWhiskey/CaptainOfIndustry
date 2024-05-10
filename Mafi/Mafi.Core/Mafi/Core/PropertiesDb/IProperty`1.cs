// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.IProperty`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  /// <summary>
  /// Reference to a property in <see cref="T:Mafi.Core.PropertiesDb.IPropertiesDb" />. Allows for quick read or write of the property's value.
  /// </summary>
  public interface IProperty<T>
  {
    string Id { get; }

    IEvent<T> OnChange { get; }

    T Value { get; }

    void OverrideValue(T value);

    bool TryGetModifier(string owner, out PropertyModifier<T> modifier);

    void AddModifier(PropertyModifier<T> modifier);

    void AddOrSetModifier(string owner, T value, Option<string> group);

    void RemoveModifier(PropertyModifier<T> modifier);

    /// <summary>
    /// Will register all entities of type TEntity for this property updates.
    /// </summary>
    void RegisterEntityTypeForUpdate(Type type);
  }
}
