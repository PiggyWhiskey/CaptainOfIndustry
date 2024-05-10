// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.CombinedActivator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Combines multiple activators into one which affects all at once.
  /// </summary>
  public class CombinedActivator : IActivator
  {
    private readonly IActivator m_first;
    private readonly IActivator m_second;

    public bool IsActive => this.m_first.IsActive && this.m_second.IsActive;

    private CombinedActivator(IActivator first, IActivator second)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_first = first.CheckNotNull<IActivator>();
      this.m_second = second.CheckNotNull<IActivator>();
    }

    public void Activate()
    {
      this.m_first.Activate();
      this.m_second.Activate();
    }

    public void ActivateIfNotActive()
    {
      this.m_first.ActivateIfNotActive();
      this.m_second.ActivateIfNotActive();
    }

    public void Deactivate()
    {
      this.m_first.Deactivate();
      this.m_second.Deactivate();
    }

    public void DeactivateIfActive()
    {
      this.m_first.DeactivateIfActive();
      this.m_second.DeactivateIfActive();
    }

    public void SetActive(bool active)
    {
      this.m_first.SetActive(active);
      this.m_second.SetActive(active);
    }

    public static IActivator Combine(IActivator first, IActivator second)
    {
      return (IActivator) new CombinedActivator(first, second);
    }
  }
}
