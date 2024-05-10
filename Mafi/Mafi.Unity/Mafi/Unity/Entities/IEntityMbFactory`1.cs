// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.IEntityMbFactory`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>Common interface for all factories of entities.</summary>
  /// <typeparam name="T">Type of entity handled by this factory.</typeparam>
  public interface IEntityMbFactory<T> : IFactory<T, EntityMb> where T : IEntity
  {
  }
}
