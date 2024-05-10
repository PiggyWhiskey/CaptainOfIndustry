// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ReversibleAnimationSyncer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Similar to <see cref="T:Mafi.Unity.Utils.AnimationSyncer" /> for for reversible animations.
  /// 
  /// Note that speed mult parameter is used to reverse animations. It must be defined as a parameter on the AC
  /// and be set as speed multiplier on the animation clip. Since parameters are global for the entire AC, you will
  /// need to define more parameters if there are more reversible clips in the animator.
  /// </summary>
  /// <remarks>
  /// For no good reason, Unity does not support setting <see cref="P:UnityEngine.Animator.speed" /> negative, but allows
  /// setting a custom animator parameter that is speed multiplier to negative number. This is the only way to
  /// achieve backwards playback.
  /// </remarks>
  public struct ReversibleAnimationSyncer
  {
    private readonly Animator m_animator;
    private float m_startNormTime;
    private int m_currentStateHash;
    private int m_speedMultHash;
    private bool m_isPlayingBackwards;

    public ReversibleAnimationSyncer(Animator animator)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_animator = animator;
      this.m_animator.speed = 0.0f;
      this.m_animator.enabled = false;
      this.m_startNormTime = 0.0f;
      this.m_currentStateHash = 0;
      this.m_speedMultHash = 0;
      this.m_isPlayingBackwards = false;
    }

    /// <summary>Call every sync.</summary>
    public void Sync(float targetNormTime, float simSpeedMult)
    {
      if ((Object) this.m_animator == (Object) null)
        return;
      if (this.m_animator.enabled)
      {
        this.m_animator.StartPlayback();
        float normalizedTime = this.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float num1 = targetNormTime - this.m_startNormTime;
        if (num1.IsNearZero())
          this.m_animator.speed = 0.0f;
        else if ((double) num1 > 0.0)
        {
          float num2 = (targetNormTime - normalizedTime) / num1;
          if (this.m_isPlayingBackwards || (double) num2 < 0.5 || (double) num2 > 2.0)
          {
            this.m_isPlayingBackwards = false;
            this.m_animator.Play(this.m_currentStateHash, 0, this.m_startNormTime);
            this.m_animator.speed = simSpeedMult;
            this.m_animator.SetFloat(this.m_speedMultHash, 1f);
          }
          else
            this.m_animator.speed = num2 * simSpeedMult;
        }
        else
        {
          float num3 = (targetNormTime - normalizedTime) / num1;
          if (!this.m_isPlayingBackwards || (double) num3 < 0.5 || (double) num3 > 2.0)
          {
            this.m_isPlayingBackwards = true;
            this.m_animator.Play(this.m_currentStateHash, 0, this.m_startNormTime);
            this.m_animator.speed = simSpeedMult;
            this.m_animator.SetFloat(this.m_speedMultHash, -1f);
          }
          else
            this.m_animator.speed = num3 * simSpeedMult;
        }
      }
      this.m_startNormTime = targetNormTime;
    }

    /// <summary>
    /// Starts new animation. Must be called BEFORE <see cref="M:Mafi.Unity.Utils.ReversibleAnimationSyncer.Sync(System.Single,System.Single)" />. This assumes that the animation was
    /// started at the beginning of the last sim update is already playing for one tick at this point.
    /// </summary>
    public void PlayNew(int stateHash, int speedMultHash, float startNormTime = 0.0f, bool playBackwards = false)
    {
      if ((Object) this.m_animator == (Object) null)
        return;
      this.m_animator.enabled = true;
      this.m_animator.Play(stateHash);
      this.m_startNormTime = startNormTime;
      this.m_currentStateHash = stateHash;
      this.m_speedMultHash = speedMultHash;
      this.m_animator.SetFloat(this.m_speedMultHash, playBackwards ? -1f : 1f);
      this.m_isPlayingBackwards = playBackwards;
    }

    /// <summary>
    /// Stops the animation. Similarly as <see cref="M:Mafi.Unity.Utils.ReversibleAnimationSyncer.PlayNew(System.Int32,System.Int32,System.Single,System.Boolean)" /> this assumes that the animation was stopped at the
    /// start of last sim update (last sync was updated to a value near 1.0).
    /// </summary>
    public void Stop()
    {
      if ((Object) this.m_animator == (Object) null)
        return;
      this.m_animator.enabled = false;
    }
  }
}
