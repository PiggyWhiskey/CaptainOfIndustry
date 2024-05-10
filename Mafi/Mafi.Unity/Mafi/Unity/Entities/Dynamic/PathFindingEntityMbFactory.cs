// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.PathFindingEntityMbFactory
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
  /// <summary>
  /// Default MB factory for all entities of base type <see cref="T:Mafi.Core.Entities.Dynamic.PathFindingEntity" />. This serves as a fall-back to
  /// all entities that do not have their own custom factories.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class PathFindingEntityMbFactory : 
    IEntityMbFactory<PathFindingEntity>,
    IFactory<PathFindingEntity, EntityMb>
  {
    private readonly DynamicGroundEntityModelFactory m_modelFactory;
    private readonly AssetsDb m_assetsDb;
    private readonly EntityAudioManager m_audioManager;
    private readonly DynamicGroundEntityDeps m_dependencies;

    public PathFindingEntityMbFactory(
      DynamicGroundEntityModelFactory modelFactory,
      AssetsDb assetsDb,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_assetsDb = assetsDb;
      this.m_audioManager = audioManager;
      this.m_dependencies = dependencies;
    }

    public EntityMb Create(PathFindingEntity entity)
    {
      PathFindingEntityMb pathFindingEntityMb = this.m_modelFactory.Create((DynamicGroundEntityProto) entity.Prototype).AddComponent<PathFindingEntityMb>();
      pathFindingEntityMb.Initialize(entity, this.m_assetsDb, this.m_audioManager, this.m_dependencies);
      return (EntityMb) pathFindingEntityMb;
    }
  }
}
