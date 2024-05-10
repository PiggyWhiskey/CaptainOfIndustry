// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.VehicleDepotMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Static;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.Utils;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  internal class VehicleDepotMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb
  {
    private static readonly int DOORS_ANIM_HASH;
    private static readonly int DOORS_SPEED_MULT_HASH;
    private VehicleDepot m_depot;
    private ReversibleAnimationSyncer m_doorAnimationSyncer;

    public void Initialize(VehicleDepot depot)
    {
      this.Initialize((ILayoutEntity) depot);
      this.m_depot = depot.CheckNotNull<VehicleDepot>();
      Animator componentInChildren = this.gameObject.GetComponentInChildren<Animator>();
      if ((Object) componentInChildren != (Object) null)
      {
        this.m_doorAnimationSyncer = new ReversibleAnimationSyncer(componentInChildren);
        this.m_doorAnimationSyncer.PlayNew(VehicleDepotMb.DOORS_ANIM_HASH, VehicleDepotMb.DOORS_SPEED_MULT_HASH, depot.DoorOpenPerc.ToFloat());
      }
      else
        Log.Warning(string.Format("Vehicle depot {0} has no door animator", (object) depot));
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      this.m_doorAnimationSyncer.Sync(this.m_depot.DoorOpenPerc.ToFloat(), (float) time.GameSpeedMult);
    }

    public VehicleDepotMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleDepotMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      VehicleDepotMb.DOORS_ANIM_HASH = Animator.StringToHash("VehicleDepot_DoorUp");
      VehicleDepotMb.DOORS_SPEED_MULT_HASH = Animator.StringToHash("SpeedMult");
    }
  }
}
