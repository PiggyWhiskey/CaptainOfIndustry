// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.DynamicGroundEntityModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class DynamicGroundEntityModelFactory : IFactory<DynamicGroundEntityProto, GameObject>
  {
    private readonly AssetsDb m_assetsDb;

    public DynamicGroundEntityModelFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
    }

    public GameObject Create(DynamicGroundEntityProto proto)
    {
      GameObject result;
      if (!this.m_assetsDb.TryGetSharedAsset<GameObject>(proto.Graphics.PrefabPath, out result))
        return this.GenerateMesh(proto);
      GameObject gameObject = Object.Instantiate<GameObject>(result);
      gameObject.SetActive(true);
      return gameObject;
    }

    public GameObject GenerateMesh(DynamicGroundEntityProto proto)
    {
      GameObject mesh = new GameObject(proto.Id.Value);
      BuildableMb buildableMb = mesh.AddComponent<BuildableMb>();
      MeshBuilder instance = MeshBuilder.Instance;
      this.AppendMesh(instance, proto);
      instance.UpdateMbAndClear((IBuildable) buildableMb);
      buildableMb.GetComponent<MeshRenderer>().material = this.m_assetsDb.DefaultMaterial;
      mesh.AddComponent<MeshCollider>();
      return mesh;
    }

    public void AppendMesh(MeshBuilder builder, DynamicGroundEntityProto proto)
    {
      RelTile3f entitySize = proto.EntitySize;
      builder.AddAaBox(new Tile3f(RelTile1f.Zero, RelTile1f.Zero, entitySize.HeightTiles1f / 2).ToVector3(), (entitySize / (Fix32) 2).ToVector3(), new Color32((byte) 192, (byte) 192, (byte) 192, byte.MaxValue));
    }
  }
}
