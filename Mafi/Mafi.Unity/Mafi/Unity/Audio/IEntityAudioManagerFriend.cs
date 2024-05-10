// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.IEntityAudioManagerFriend
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.Audio
{
  internal interface IEntityAudioManagerFriend
  {
    void EntitySoundStarted(IEntitySoundFriend sound);

    void EntitySoundStopped(IEntitySoundFriend sound);

    void EntitySoundDestroyed(IEntitySoundFriend sound);
  }
}
