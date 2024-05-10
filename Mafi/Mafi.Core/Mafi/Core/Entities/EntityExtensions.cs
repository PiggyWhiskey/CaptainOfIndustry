// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities
{
  public static class EntityExtensions
  {
    public static bool IsNotPaused(this IEntity entity) => !entity.IsPaused;

    public static bool IsNotEnabled(this IEntity entity) => !entity.IsEnabled;
  }
}
