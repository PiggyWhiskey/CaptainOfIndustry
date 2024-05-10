// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityNameExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities
{
  public static class EntityNameExtensions
  {
    public static string GetTitle(this IEntity entity)
    {
      return entity is IEntityWithCustomTitle entityWithCustomTitle ? entityWithCustomTitle.CustomTitle.ValueOrNull ?? entity.DefaultTitle.Value : entity.DefaultTitle.Value;
    }

    public static void SetCustomTitle(this IEntityWithCustomTitle entity, string title)
    {
      entity.CustomTitle = title.IsNullOrEmpty() ? Option<string>.None : (Option<string>) title;
    }

    public static bool HasCustomTitle(this IEntityWithCustomTitle entity)
    {
      return entity.CustomTitle.HasValue && entity.CustomTitle.Value.IsNotEmpty();
    }
  }
}
