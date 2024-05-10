// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.IActivator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// General interface for an activator of UI components (mainly for visualizations or UI tools). There may be
  /// several classes that want to activate/deactivate (show/hide) certain UI components, each of them gets
  /// one IActivator and activates the component through the activator.
  /// The activator allows for greater flexibility and error checks than simple Activate/Deactivate methods.
  /// </summary>
  public interface IActivator
  {
    bool IsActive { get; }

    void Activate();

    void ActivateIfNotActive();

    /// <summary>
    /// Deactivates the target component, logs an error, if the target component was not active.
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Same as <see cref="M:Mafi.Unity.Utils.IActivator.Deactivate" />, but does not log errors.
    /// </summary>
    void DeactivateIfActive();

    void SetActive(bool active);
  }
}
