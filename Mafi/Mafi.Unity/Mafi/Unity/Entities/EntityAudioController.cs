// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntityAudioController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Unity.Audio;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  internal class EntityAudioController : IAudioController
  {
    private readonly IEntityWithSound m_entity;
    private readonly Option<EntitySoundMb> m_entitySound;
    private bool m_machineIsRunning;
    private bool m_prevPaused;

    public EntityAudioController(
      IEntityWithSound entity,
      EntityMb entityMb,
      EntityAudioManager audioManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity;
      if (!entity.SoundParams.HasValue)
        return;
      this.m_entitySound = (Option<EntitySoundMb>) audioManager.CreateSound(entityMb, entity.SoundParams.Value).Value;
    }

    public void UpdateSimState() => this.m_machineIsRunning = this.m_entity.IsSoundOn;

    public void RenderUpdate(GameTime gameTime)
    {
      this.m_entitySound.ValueOrNull?.RenderUpdate(gameTime, this.m_machineIsRunning);
    }
  }
}
