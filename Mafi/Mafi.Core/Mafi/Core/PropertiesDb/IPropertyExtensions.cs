// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.IPropertyExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  public static class IPropertyExtensions
  {
    public static bool TryGetValueAs<T>(
      this IProperty property,
      PropertyId<T> propertyId,
      out T value)
    {
      if (property.Id == propertyId.Value && property is IProperty<T> property1)
      {
        value = property1.Value;
        return true;
      }
      value = default (T);
      return false;
    }
  }
}
