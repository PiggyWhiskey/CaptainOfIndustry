// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.RocketAssemblyBuildingMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities.Static;
using Mafi.Unity.Entities.Rockets;
using Mafi.Unity.Utils;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class RocketAssemblyBuildingMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IRocketHoldingEntityMb
  {
    private static readonly int SPEED_MULT_HASH;
    private static readonly int OUTSIDE_ANIM_HASH;
    private static readonly int INSIDE_ANIM_HASH;
    private static readonly int DOORS_ANIM_HASH;
    private static readonly int VENTS_ANIM_HASH;
    private RocketAssemblyBuilding m_building;
    private GameObject m_insideGo;
    private ReversibleAnimationSyncer m_roofAnimationSyncer;
    private ReversibleAnimationSyncer m_liftAnimationSyncer;
    private ReversibleAnimationSyncer m_doorAnimationSyncer;
    private Animator m_ventsAnimator;

    public Transform RocketParent { get; private set; }

    public Quaternion? LocalRotation => new Quaternion?(Quaternion.identity);

    public void Initialize(RocketAssemblyBuilding building)
    {
      this.Initialize((ILayoutEntity) building);
      this.m_building = building;
      Transform resultTransform;
      if (this.transform.TryFindChild(building.Prototype.RocketHolderObjectPath, out resultTransform))
      {
        this.RocketParent = resultTransform;
      }
      else
      {
        Log.Warning(string.Format("Rocket holder on '{0}' was not found.", (object) building.Prototype.Id));
        this.RocketParent = this.transform;
      }
      GameObject resultGo1;
      Animator component1;
      if (this.gameObject.TryFindChild("Outside", out resultGo1) && resultGo1.TryGetComponent<Animator>(out component1))
      {
        this.m_roofAnimationSyncer = new ReversibleAnimationSyncer(component1);
        this.m_roofAnimationSyncer.PlayNew(RocketAssemblyBuildingMb.OUTSIDE_ANIM_HASH, RocketAssemblyBuildingMb.SPEED_MULT_HASH, building.RoofOpenPerc.ToFloat());
      }
      else
        Log.Warning("The 'Outside' object with animator was not found " + string.Format("on rocket assembly building '{0}'.", (object) building));
      Animator component2;
      if (this.gameObject.TryFindChild("Inside", out this.m_insideGo) && this.m_insideGo.TryGetComponent<Animator>(out component2))
      {
        this.m_liftAnimationSyncer = new ReversibleAnimationSyncer(component2);
        this.m_liftAnimationSyncer.PlayNew(RocketAssemblyBuildingMb.INSIDE_ANIM_HASH, RocketAssemblyBuildingMb.SPEED_MULT_HASH, building.RocketRaisePerc.ToFloat());
      }
      else
        Log.Warning("The 'Inside' object with animator was not found " + string.Format("on rocket assembly building '{0}'.", (object) building));
      GameObject resultGo2;
      Animator component3;
      if (this.gameObject.TryFindChild("Doors", out resultGo2) && resultGo2.TryGetComponent<Animator>(out component3))
      {
        this.m_doorAnimationSyncer = new ReversibleAnimationSyncer(component3);
        this.m_doorAnimationSyncer.PlayNew(RocketAssemblyBuildingMb.DOORS_ANIM_HASH, RocketAssemblyBuildingMb.SPEED_MULT_HASH, building.DoorOpenPerc.ToFloat());
      }
      else
        Log.Warning("The 'Doors' object with animator was not found " + string.Format("on rocket assembly building '{0}'.", (object) building));
      GameObject resultGo3;
      if (this.gameObject.TryFindChild("Vents", out resultGo3) && resultGo3.TryGetComponent<Animator>(out this.m_ventsAnimator))
        this.m_ventsAnimator.Play(RocketAssemblyBuildingMb.VENTS_ANIM_HASH);
      else
        Log.Warning("The 'Vents' object with animator was not found " + string.Format("on rocket assembly building '{0}'.", (object) building));
    }

    public void SyncUpdate(GameTime time)
    {
      float gameSpeedMult = (float) time.GameSpeedMult;
      this.m_roofAnimationSyncer.Sync(this.m_building.RoofOpenPerc.ToFloat(), gameSpeedMult);
      this.m_liftAnimationSyncer.Sync(this.m_building.RocketRaisePerc.ToFloat(), gameSpeedMult);
      this.m_doorAnimationSyncer.Sync(this.m_building.DoorOpenPerc.ToFloat(), gameSpeedMult);
      if (!((Object) this.m_ventsAnimator != (Object) null))
        return;
      this.m_ventsAnimator.speed = gameSpeedMult;
    }

    public RocketAssemblyBuildingMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static RocketAssemblyBuildingMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      RocketAssemblyBuildingMb.SPEED_MULT_HASH = Animator.StringToHash("SpeedMult");
      RocketAssemblyBuildingMb.OUTSIDE_ANIM_HASH = Animator.StringToHash("RocketAssemblyRoof");
      RocketAssemblyBuildingMb.INSIDE_ANIM_HASH = Animator.StringToHash("RocketAssembly_RocketRaise");
      RocketAssemblyBuildingMb.DOORS_ANIM_HASH = Animator.StringToHash("RocketAssemblyDoors");
      RocketAssemblyBuildingMb.VENTS_ANIM_HASH = Animator.StringToHash("RocketAssemblyVents");
    }
  }
}
