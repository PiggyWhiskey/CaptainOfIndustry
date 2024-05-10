// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.RocketLaunchPadMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Input;
using Mafi.Core.SpaceProgram;
using Mafi.Unity.Audio;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class RocketLaunchPadMbFactory : 
    IEntityMbFactory<RocketLaunchPad>,
    IFactory<RocketLaunchPad, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;
    private readonly EntityAudioManager m_audioManager;
    private readonly IRocketLaunchManager m_rocketLaunchManager;
    private readonly IInputScheduler m_inputScheduler;

    public RocketLaunchPadMbFactory(
      ProtoModelFactory modelFactory,
      EntityAudioManager audioManager,
      IRocketLaunchManager rocketLaunchManager,
      IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_audioManager = audioManager;
      this.m_rocketLaunchManager = rocketLaunchManager;
      this.m_inputScheduler = inputScheduler;
    }

    public EntityMb Create(RocketLaunchPad pad)
    {
      RocketLaunchPadMb rocketLaunchPadMb = this.m_modelFactory.CreateModelFor<RocketLaunchPadProto>(pad.Prototype).AddComponent<RocketLaunchPadMb>();
      rocketLaunchPadMb.Initialize(pad, this.m_audioManager, this.m_rocketLaunchManager, this.m_inputScheduler);
      return (EntityMb) rocketLaunchPadMb;
    }
  }
}
