// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.RocketTransporterMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities.Dynamic;
using Mafi.Unity.Entities.Rockets;
using Mafi.Unity.Utils;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class RocketTransporterMb : PathFindingEntityMb, IRocketHoldingEntityMb
  {
    public static readonly int HOLDER_ANIMATION_STATE_ID;
    private RocketTransporter m_transporter;
    private TracksAnimator m_tracksAnimator;
    private ReversibleAnimationSyncer m_boomAnimationSyncer;

    public Transform RocketParent { get; private set; }

    public Quaternion? LocalRotation => new Quaternion?(Quaternion.identity);

    internal void Initialize(
      RocketTransporter transporter,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      this.m_transporter = transporter;
      Transform resultTransform;
      if (this.transform.TryFindChild("RocketHolder", out resultTransform))
      {
        this.RocketParent = resultTransform;
      }
      else
      {
        Log.Warning(string.Format("Rocket holder on '{0}' was not found.", (object) transporter.Prototype.Id));
        this.RocketParent = this.transform;
      }
      RocketTransporterProto.Gfx graphics = transporter.Prototype.Graphics;
      this.m_tracksAnimator = new TracksAnimator(this.transform, graphics.LeftTrackModelName, graphics.RightTrackModelName, graphics.SpacingBetweenTracks.ToUnityUnits(), graphics.TrackTextureLength.ToUnityUnits());
      Animator component;
      if (this.gameObject.TryGetComponent<Animator>(out component))
      {
        this.m_boomAnimationSyncer = new ReversibleAnimationSyncer(component);
        this.m_boomAnimationSyncer.PlayNew(RocketTransporterMb.HOLDER_ANIMATION_STATE_ID, Animator.StringToHash("SpeedMult"), transporter.RocketHolderExtensionPerc.ToFloat());
      }
      else
        Log.Warning(string.Format("The '{0}' GO has no animator", (object) transporter));
      this.Initialize((PathFindingEntity) transporter, assetsDb, audioManager, dependencies);
    }

    public override void SyncUpdate(GameTime time)
    {
      this.m_tracksAnimator.Sync(time, this.m_transporter.Speed, -this.m_transporter.SteeringAngle);
      this.m_boomAnimationSyncer.Sync(this.m_transporter.RocketHolderExtensionPerc.ToFloat(), (float) time.GameSpeedMult);
      base.SyncUpdate(time);
    }

    public override void RenderUpdate(GameTime time)
    {
      this.m_tracksAnimator.RenderUpdate(time);
      base.RenderUpdate(time);
    }

    public RocketTransporterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static RocketTransporterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      RocketTransporterMb.HOLDER_ANIMATION_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
