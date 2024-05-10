// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.PathFindingEntityMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Unity.Audio;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  internal class PathFindingEntityMb : DynamicGroundEntityMb
  {
    private PathFindingEntity m_pathFindingEntity;

    internal void Initialize(
      PathFindingEntity pathFindingEntity,
      AssetsDb assetsDb,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      this.m_pathFindingEntity = pathFindingEntity;
      this.Initialize((DrivingEntity) pathFindingEntity, assetsDb, audioManager, dependencies);
    }

    public PathFindingEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
