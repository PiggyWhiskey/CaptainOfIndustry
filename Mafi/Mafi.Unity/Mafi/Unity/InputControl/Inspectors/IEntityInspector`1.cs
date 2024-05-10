// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.IEntityInspector`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  /// <summary>
  /// Generic entity inspector. This interface should be implemented by all inspectors.
  /// </summary>
  /// <typeparam name="T">Type of inspected entity</typeparam>
  public interface IEntityInspector<T> : 
    IEntityInspector,
    IEntityInspectorFactory<T>,
    IFactory<T, IEntityInspector>
    where T : IEntity
  {
    /// <summary>
    /// Entity is null when this inspector is deactivated and not null when activated. We do not use option here
    /// because any time this class is used it has to be activated first.
    /// </summary>
    T SelectedEntity { get; }
  }
}
