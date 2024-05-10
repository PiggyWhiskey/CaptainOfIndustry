// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntitiesAnimationsUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class EntitiesAnimationsUpdater : IEntityMbUpdater
  {
    private readonly Dict<IEntity, EntitiesAnimationsUpdater.EntityAnimationsData> m_animationsData;
    private readonly Set<EntitiesAnimationsUpdater.EntityAnimationsData> m_animationsDataToSyncUpdate;
    private readonly Lyst<EntitiesAnimationsUpdater.AnimationData> m_dataTemp;
    private readonly Lyst<Animator> m_animatorsTemp;
    private static readonly int SPEED_MULT_HASH;

    public IReadOnlyDictionary<IEntity, EntitiesAnimationsUpdater.EntityAnimationsData> AnimationsData
    {
      get
      {
        return (IReadOnlyDictionary<IEntity, EntitiesAnimationsUpdater.EntityAnimationsData>) this.m_animationsData;
      }
    }

    public void AddOnSyncIfNeeded(EntityMb entityMb)
    {
      IEntity entity = entityMb.Entity;
      if (!(entity is IAnimatedEntity animatedEntity) || animatedEntity.AnimationStatesProvider.AnimationStates.IsEmpty)
        return;
      bool flag1 = entity.Prototype is ILayoutEntityProto prototype && prototype.Graphics.UseSemiInstancedRendering;
      bool flag2 = !flag1;
      this.m_animatorsTemp.Clear();
      entityMb.gameObject.GetAllComponentsInHierarchy<Animator>(this.m_animatorsTemp);
      if (this.m_animatorsTemp.IsEmpty<Animator>())
      {
        Log.Error(string.Format("Animated entity {0} MB does not have an animator.", (object) entity));
      }
      else
      {
        foreach (IAnimationState animationState in animatedEntity.AnimationStatesProvider.AnimationStates)
        {
          IAnimationState animatedState = animationState;
          int stateHash = Animator.StringToHash(animatedState.AnimationStateName);
          Animator[] array = this.m_animatorsTemp.Where<Animator>((Func<Animator, bool>) (x => x.HasState(0, stateHash))).ToArray<Animator>();
          if (array.Length != 1)
          {
            Log.Error(string.Format("Animated entity {0} MB does not have exactly 1 animator for state {1}, found {2} of them.", (object) entity, (object) animatedState.AnimationStateName, (object) array.Length));
          }
          else
          {
            Animator animator = array.First<Animator>();
            if ((UnityEngine.Object) animator.runtimeAnimatorController == (UnityEngine.Object) null)
              Log.Error(string.Format("Animated entity {0} MB does not have an animation controller.", (object) entity));
            else if (animator.runtimeAnimatorController.animationClips.Length == 0)
            {
              Log.Error(string.Format("Animated entity {0} MB does not have an animation clips.", (object) entity));
            }
            else
            {
              animator.Play(animatedState.AnimationStateName, 0, 0.0f);
              AnimationClip animationClip;
              if (animatedState.AnimationStateName == "Main")
              {
                if (animator.runtimeAnimatorController.animationClips.Length != 1)
                  Log.Warning(string.Format("Animator of entity '{0}' has more than one ", (object) entity) + "clip matching for its 'Main' state: " + ((IEnumerable<string>) animator.runtimeAnimatorController.animationClips.MapArray<AnimationClip, string>((Func<AnimationClip, string>) (x => x.name))).JoinStrings(", "));
                animationClip = animator.runtimeAnimatorController.animationClips[0];
              }
              else
              {
                animationClip = ((IEnumerable<AnimationClip>) animator.runtimeAnimatorController.animationClips).FirstOrDefault<AnimationClip>((Func<AnimationClip, bool>) (x => x.name == animatedState.AnimationStateName));
                if ((UnityEngine.Object) animationClip == (UnityEngine.Object) null)
                {
                  Log.Error("Failed to find clip " + animatedState.AnimationStateName);
                  continue;
                }
              }
              Mafi.Core.Entities.AnimationState state = animatedState.GetState();
              EntitiesAnimationsUpdater.AnimationData animData = new EntitiesAnimationsUpdater.AnimationData()
              {
                AnimationLengthMs = animationClip.length * 1000f,
                AnimationLoops = animationClip.isLooping,
                AnimationState = animatedState,
                AnimationStateHash = stateHash,
                Animator = animator,
                Speed = state.Speed.ToFloat()
              };
              this.m_dataTemp.Add(animData);
              if (!flag1)
              {
                if (state.UseSpeed)
                {
                  EntitiesAnimationsUpdater.setSpeedBackwardsAware(state.Speed.ToFloat(), animData, animator);
                  if (!animData.AnimationLoops)
                    Log.Error(string.Format("Animated entity {0} ({1}) MB has uses speed control ", (object) entity, (object) entity.Prototype) + "but looping is not enabled.");
                }
                else
                  animator.Play(animatedState.AnimationStateName, 0, state.TimeMs.ToFloat() / animData.AnimationLengthMs);
              }
              else if (state.UseSpeed)
              {
                flag2 = true;
                EntitiesAnimationsUpdater.setSpeedBackwardsAware(state.Speed.ToFloat(), animData);
                if (!animData.AnimationLoops)
                  Log.Error(string.Format("Animated entity {0} ({1}) MB has uses speed control ", (object) entity, (object) entity.Prototype) + "but looping is not enabled.");
              }
              animator.speed = 0.0f;
            }
          }
        }
        EntitiesAnimationsUpdater.EntityAnimationsData entityAnimationsData = new EntitiesAnimationsUpdater.EntityAnimationsData(animatedEntity, this.m_dataTemp.ToImmutableArrayAndClear());
        this.m_animationsData.AddAndAssertNew((IEntity) animatedEntity, entityAnimationsData);
        if (flag2)
          this.m_animationsDataToSyncUpdate.Add(entityAnimationsData);
        if (!flag1)
          return;
        foreach (UnityEngine.Object @object in this.m_animatorsTemp)
          UnityEngine.Object.Destroy(@object);
      }
    }

    private void syncUpdateEntity(
      GameTime time,
      EntitiesAnimationsUpdater.EntityAnimationsData entityData)
    {
      foreach (EntitiesAnimationsUpdater.AnimationData animation in entityData.Animations)
      {
        Mafi.Core.Entities.AnimationState state = animation.AnimationState.GetState();
        if (entityData.Entity.Prototype is ILayoutEntityProto prototype && prototype.Graphics.UseSemiInstancedRendering)
        {
          if (!state.UseSpeed)
            break;
          EntitiesAnimationsUpdater.setSpeedBackwardsAware(state.Speed.ToFloat() * (float) time.GameSpeedMult, animation);
          break;
        }
        Animator animator1 = animation.Animator;
        if (!entityData.Entity.IsEnabled || time.IsGamePaused)
        {
          animator1.speed = 0.0f;
        }
        else
        {
          if (state.UseSpeed)
          {
            EntitiesAnimationsUpdater.setSpeedBackwardsAware(state.Speed.ToFloat() * (float) time.GameSpeedMult, animation, animator1);
            break;
          }
          float num1 = animator1.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
          float num2 = (state.TimeMs.ToFloat() / animation.AnimationLengthMs).Min(1f);
          float self = num2 - num1;
          if (animation.AnimationLoops)
          {
            if ((double) self > 0.5)
              self = (float) -(1.0 - (double) self);
            else if ((double) self < -0.5)
              self = 1f + self;
          }
          Fix32 updateDurationMs;
          if ((double) self.Abs() > 0.5)
          {
            double num3 = (double) num2;
            updateDurationMs = time.CurrSimUpdateDurationMs;
            double num4 = (double) updateDurationMs.ToFloat() / (double) animation.AnimationLengthMs;
            float normalizedTime = ((float) (num3 - num4)).Max(0.0f);
            float num5 = num2 - normalizedTime;
            Assert.That<float>(num5).IsNotNegative();
            animator1.Play(animation.AnimationStateHash, 0, normalizedTime);
            Animator animator2 = animator1;
            double num6 = (double) num5 * (double) animation.AnimationLengthMs;
            updateDurationMs = time.CurrSimUpdateDurationMs;
            double num7 = (double) updateDurationMs.ToFloat();
            double num8 = num6 / num7;
            animator2.speed = (float) num8;
          }
          else if ((double) self > 0.0)
          {
            Animator animator3 = animator1;
            double num9 = (double) self * (double) animation.AnimationLengthMs;
            updateDurationMs = time.CurrSimUpdateDurationMs;
            double num10 = (double) updateDurationMs.ToFloat();
            double num11 = num9 / num10;
            animator3.speed = (float) num11;
          }
          else
            animator1.speed = 0.0f;
        }
      }
    }

    private static void setSpeedBackwardsAware(
      float speed,
      EntitiesAnimationsUpdater.AnimationData animData,
      Animator animator)
    {
      if ((double) speed == 0.0)
        animator.speed = 0.0f;
      else if ((double) speed > 0.0)
      {
        if (animData.IsPlayingBackwards)
        {
          animData.IsPlayingBackwards = false;
          animator.SetFloat(EntitiesAnimationsUpdater.SPEED_MULT_HASH, 1f);
        }
        animator.speed = speed;
      }
      else
      {
        if (!animData.IsPlayingBackwards)
        {
          animData.IsPlayingBackwards = true;
          animator.SetFloat(EntitiesAnimationsUpdater.SPEED_MULT_HASH, -1f);
        }
        animator.speed = -speed;
      }
    }

    private static void setSpeedBackwardsAware(
      float speed,
      EntitiesAnimationsUpdater.AnimationData animData)
    {
      if ((double) speed == 0.0)
        animData.Speed = 0.0f;
      else if ((double) speed > 0.0)
      {
        if (animData.IsPlayingBackwards)
          animData.IsPlayingBackwards = false;
        animData.Speed = speed;
      }
      else
      {
        if (!animData.IsPlayingBackwards)
          animData.IsPlayingBackwards = true;
        animData.Speed = -speed;
      }
    }

    public void RemoveOnSyncIfNeeded(EntityMb entityMb)
    {
      EntitiesAnimationsUpdater.EntityAnimationsData entityAnimationsData;
      if (!this.m_animationsData.TryRemove(entityMb.Entity, out entityAnimationsData))
        return;
      this.m_animationsDataToSyncUpdate.Remove(entityAnimationsData);
    }

    public void RenderUpdate(GameTime time)
    {
    }

    public void SyncUpdate(GameTime time)
    {
      foreach (EntitiesAnimationsUpdater.EntityAnimationsData entityData in this.m_animationsDataToSyncUpdate)
        this.syncUpdateEntity(time, entityData);
    }

    public EntitiesAnimationsUpdater()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_animationsData = new Dict<IEntity, EntitiesAnimationsUpdater.EntityAnimationsData>();
      this.m_animationsDataToSyncUpdate = new Set<EntitiesAnimationsUpdater.EntityAnimationsData>();
      this.m_dataTemp = new Lyst<EntitiesAnimationsUpdater.AnimationData>();
      this.m_animatorsTemp = new Lyst<Animator>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static EntitiesAnimationsUpdater()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      EntitiesAnimationsUpdater.SPEED_MULT_HASH = Animator.StringToHash("SpeedMult");
    }

    public class EntityAnimationsData
    {
      public readonly IAnimatedEntity Entity;
      public readonly ImmutableArray<EntitiesAnimationsUpdater.AnimationData> Animations;

      public EntityAnimationsData(
        IAnimatedEntity entity,
        ImmutableArray<EntitiesAnimationsUpdater.AnimationData> animations)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Entity = entity;
        this.Animations = animations;
      }
    }

    public class AnimationData
    {
      public int AnimationStateHash;
      public IAnimationState AnimationState;
      public float AnimationLengthMs;
      public bool AnimationLoops;
      public Animator Animator;
      public bool IsPlayingBackwards;
      public float Speed;

      public AnimationData()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
