// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.AnimationSyncer
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
  /// Synchronizes animation with sim state. This uses speed-based synchronization that smoothly changes animation
  /// speed to match the sim state instead of jumping the animation every sync that introduces jitter.  Another
  /// benefit is that calling <see cref="M:UnityEngine.Animator.Play(System.Int32)" /> every sync causes performance overhead.
  /// </summary>
  /// <remarks>
  /// When using this syncer make sure that the first <see cref="M:Mafi.Unity.Utils.AnimationSyncer.Sync(System.Single,System.Single)" /> after <see cref="M:Mafi.Unity.Utils.AnimationSyncer.PlayNew(System.Int32,System.Single)" /> already has
  /// non-zero <c>targetNormTime</c>.
  /// 
  /// If sim state counts <c>animValue</c> from <c>0</c> to <c>n-1</c> (animation of length <c>animLength = n</c>),
  /// the correct formula for <c>targetNormTime</c> passed to <see cref="M:Mafi.Unity.Utils.AnimationSyncer.Sync(System.Single,System.Single)" /> is
  /// <c>targetNormTime = (float)(animValue + 1) / targetNormTime</c>.
  /// 
  /// This syncer still assumes that the animation speed in Unity is the same as speed in game sim.
  /// If the speed is too different, this class reverts to jumping to avoid crazy fast or crazy slow animations
  /// in case of "jumps" in animations state.
  /// </remarks>
  public struct AnimationSyncer
  {
    private readonly Animator m_animator;
    private float m_startNormTime;
    private int m_currentStateHash;
    private float m_prevSpeed;

    public AnimationSyncer(Animator animator)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_animator = animator;
      this.m_startNormTime = 0.0f;
      this.m_currentStateHash = 0;
      this.m_prevSpeed = 0.0f;
      this.m_animator.speed = 0.0f;
      this.m_animator.enabled = false;
    }

    /// <summary>Call every sync.</summary>
    public void Sync(float targetNormTime, float simSpeedMult)
    {
      if ((Object) this.m_animator == (Object) null)
        return;
      if (this.m_animator.enabled)
      {
        float normalizedTime = this.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float num1 = targetNormTime - this.m_startNormTime;
        if (num1.IsNearZero())
        {
          this.m_animator.speed = 0.0f;
          this.m_prevSpeed = 1f;
        }
        else
        {
          Assert.That<float>(num1).IsPositive("Animation cannot go backwards, use ReversibleAnimationSyncer.");
          float num2 = (float) (((double) ((targetNormTime - normalizedTime) / num1) + (double) this.m_prevSpeed) / 2.0);
          if ((double) num2 < 0.20000000298023224 || (double) num2 > 4.0)
          {
            this.m_animator.Play(this.m_currentStateHash, 0, this.m_startNormTime);
            this.m_animator.speed = simSpeedMult;
            this.m_prevSpeed = 1f;
          }
          else
          {
            this.m_animator.speed = num2 * simSpeedMult;
            this.m_prevSpeed = num2;
          }
        }
      }
      this.m_startNormTime = targetNormTime;
    }

    /// <summary>
    /// Starts new animation. Must be called BEFORE <see cref="M:Mafi.Unity.Utils.AnimationSyncer.Sync(System.Single,System.Single)" />. This assumes that the animation was
    /// started at the beginning of the last sim update is already playing for one tick at this point.
    /// </summary>
    public void PlayNew(int stateHash, float startNormTime = 0.0f)
    {
      if ((Object) this.m_animator == (Object) null)
        return;
      this.m_animator.enabled = true;
      this.m_animator.Play(stateHash);
      this.m_startNormTime = startNormTime;
      this.m_currentStateHash = stateHash;
    }

    /// <summary>
    /// Stops the animation. Similarly as <see cref="M:Mafi.Unity.Utils.AnimationSyncer.PlayNew(System.Int32,System.Single)" /> this assumes that the animation was stopped at the
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
