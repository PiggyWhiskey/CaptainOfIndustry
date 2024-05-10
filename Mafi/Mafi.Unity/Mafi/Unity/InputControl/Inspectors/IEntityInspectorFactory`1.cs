// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.IEntityInspectorFactory`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  /// <summary>
  /// Inspector factory interface that forces every inspector implement method that initializes them with entity. We
  /// use factory interface to allow hierarchical resolving of inspectors.
  /// </summary>
  /// <typeparam name="T">Type of inspected entity</typeparam>
  public interface IEntityInspectorFactory<T> : IFactory<T, IEntityInspector> where T : IEntity
  {
  }
}
