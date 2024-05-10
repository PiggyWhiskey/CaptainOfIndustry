// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Rockets.RocketEntityMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.SpaceProgram;
using Mafi.Unity.Audio;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Rockets
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RocketEntityMbFactory : 
    IEntityMbFactory<RocketEntity>,
    IFactory<RocketEntity, EntityMb>,
    IProtoModelFactory<RocketProto>,
    IFactory<RocketProto, GameObject>
  {
    private readonly AssetsDb m_assetsDb;
    private readonly MbBasedEntitiesRenderer m_entitiesRenderer;
    private readonly EntityAudioManager m_audioManager;

    public RocketEntityMbFactory(
      AssetsDb assetsDb,
      MbBasedEntitiesRenderer entitiesRenderer,
      EntityAudioManager audioManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesRenderer = entitiesRenderer;
      this.m_audioManager = audioManager;
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
    }

    public GameObject Create(RocketProto proto)
    {
      GameObject result;
      if (!this.m_assetsDb.TryGetSharedAsset<GameObject>(proto.Graphics.PrefabPath, out result))
        return this.GenerateMesh(proto);
      GameObject gameObject = Object.Instantiate<GameObject>(result);
      gameObject.SetActive(true);
      return gameObject;
    }

    public GameObject GenerateMesh(RocketProto proto)
    {
      GameObject mesh = new GameObject(proto.Id.Value);
      BuildableMb buildableMb = mesh.AddComponent<BuildableMb>();
      MeshBuilder instance = MeshBuilder.Instance;
      instance.AddBox(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 20f, 0.0f), 2f, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0));
      instance.UpdateMbAndClear((IBuildable) buildableMb);
      buildableMb.GetComponent<MeshRenderer>().material = this.m_assetsDb.DefaultMaterial;
      mesh.AddComponent<MeshCollider>();
      return mesh;
    }

    public EntityMb Create(RocketEntity entity)
    {
      RocketEntityMb rocketEntityMb = this.Create(entity.Prototype).AddComponent<RocketEntityMb>();
      rocketEntityMb.Initialize(entity, this.m_entitiesRenderer);
      return (EntityMb) rocketEntityMb;
    }
  }
}
