// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.LoopAnimationHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Handles simple looped animations.
  /// 
  /// Also makes sure that game speed is properly handled.
  /// </summary>
  public class LoopAnimationHandler
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private Option<Animator> m_animator;
    private int m_syncSpeedMult;
    private int m_currentSpeedMult;
    private float m_speed;
    private bool m_enabled;

    public void LoadAnimationFor(GameObject go)
    {
      Animator component;
      if (go.TryGetComponent<Animator>(out component))
      {
        Assert.That<Animator>(component).IsValidUnityObject<Animator>();
        this.m_animator = (Option<Animator>) component;
        this.m_animator.Value.Play(LoopAnimationHandler.ANIM_MAIN_STATE_ID);
        this.m_animator.Value.speed = (float) this.m_currentSpeedMult * this.m_speed;
        this.m_animator.Value.enabled = this.m_enabled;
      }
      else
        this.m_animator = Option<Animator>.None;
    }

    public void SetSpeed(float speed)
    {
      this.m_currentSpeedMult = 0;
      this.m_speed = speed;
    }

    public void SetEnabled(bool isEnabled)
    {
      this.m_enabled = isEnabled;
      if (!this.m_animator.HasValue)
        return;
      this.m_animator.Value.enabled = this.m_enabled;
    }

    public void SyncUpdate(GameTime time) => this.m_syncSpeedMult = time.GameSpeedMult;

    public void RenderUpdate(GameTime time)
    {
      if (!this.m_animator.HasValue || this.m_currentSpeedMult == this.m_syncSpeedMult)
        return;
      if (!(bool) (Object) this.m_animator.Value)
      {
        Log.Warning("Looping animation is invalid, removing.");
        this.m_animator = Option<Animator>.None;
      }
      else
      {
        this.m_currentSpeedMult = this.m_syncSpeedMult;
        this.m_animator.Value.speed = (float) this.m_currentSpeedMult * this.m_speed;
      }
    }

    public LoopAnimationHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_speed = 1f;
      this.m_enabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static LoopAnimationHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LoopAnimationHandler.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
