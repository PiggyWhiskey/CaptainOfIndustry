// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertiesDbExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using System;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  public static class PropertiesDbExtensions
  {
    /// <summary>
    /// Will return value of the given property and will register all entities
    /// of type TEntity for this property updates.
    /// </summary>
    public static TProperty GetValueAndRegisterForUpdates<TProperty>(
      this IPropertiesDb db,
      IEntity entity,
      PropertyId<TProperty> id)
      where TProperty : IEquatable<TProperty>
    {
      IProperty<TProperty> property = db.GetProperty<TProperty>(id);
      property.RegisterEntityTypeForUpdate(entity.GetType());
      return property.Value;
    }
  }
}
