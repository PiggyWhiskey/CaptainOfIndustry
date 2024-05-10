// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ActivatorState
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Encapsulates an activation state - active/inactive, that can be changed by multiple different sources.
  /// This class counts number of activations to allow overlapping calls.
  /// The activation itself is done through <see cref="T:Mafi.Unity.Utils.ActivatorState.Activator" /> created using <see cref="M:Mafi.Unity.Utils.ActivatorState.CreateActivator" />.
  /// </summary>
  public class ActivatorState
  {
    private readonly Action m_onActivate;
    private readonly Action m_onDeactivate;
    private int m_activeCount;

    public bool IsActive => this.m_activeCount > 0;

    public ActivatorState(Action onActivate, Action onDeactivate)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onActivate = onActivate.CheckNotNull<Action>();
      this.m_onDeactivate = onDeactivate.CheckNotNull<Action>();
    }

    public IActivator CreateActivator() => (IActivator) new ActivatorState.Activator(this);

    private void increaseActiveCounter()
    {
      ++this.m_activeCount;
      if (this.m_activeCount != 1)
        return;
      this.m_onActivate();
    }

    private void decreaseActiveCounter()
    {
      --this.m_activeCount;
      Assert.That<int>(this.m_activeCount).IsNotNegative("Activate counter went negative.");
      if (this.m_activeCount != 0)
        return;
      this.m_onDeactivate();
    }

    public void ActivateDirect() => this.increaseActiveCounter();

    public void DeactivateDirect() => this.decreaseActiveCounter();

    public void Destroy()
    {
      if (this.m_activeCount <= 0)
        return;
      this.m_onDeactivate();
      this.m_activeCount = 0;
    }

    private class Activator : IActivator
    {
      private readonly ActivatorState m_activatorState;

      public bool IsActive { get; private set; }

      public Activator(ActivatorState activatorState)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_activatorState = activatorState;
      }

      public void Activate()
      {
        Assert.That<bool>(this.IsActive).IsFalse();
        this.ActivateIfNotActive();
      }

      public void ActivateIfNotActive()
      {
        if (this.IsActive)
          return;
        this.IsActive = true;
        this.m_activatorState.increaseActiveCounter();
      }

      public void Deactivate()
      {
        Assert.That<bool>(this.IsActive).IsTrue();
        this.DeactivateIfActive();
      }

      public void DeactivateIfActive()
      {
        if (!this.IsActive)
          return;
        this.IsActive = false;
        this.m_activatorState.decreaseActiveCounter();
      }

      public void SetActive(bool active)
      {
        if (active == this.IsActive)
          return;
        this.IsActive = active;
        if (active)
          this.m_activatorState.increaseActiveCounter();
        else
          this.m_activatorState.decreaseActiveCounter();
      }
    }
  }
}
