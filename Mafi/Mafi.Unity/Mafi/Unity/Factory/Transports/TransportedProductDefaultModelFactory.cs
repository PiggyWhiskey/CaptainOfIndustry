// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportedProductDefaultModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  /// <summary>
  /// Default transported product factory that serves as a fall-back when no specialized factories are defines.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TransportedProductDefaultModelFactory : 
    ITransportedProductModelFactory<ProductProto>,
    IFactory<ProductProto, GameObject>
  {
    private readonly AssetsDb m_assetsDb;

    public TransportedProductDefaultModelFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
    }

    public GameObject Create(ProductProto proto)
    {
      Log.Warning(string.Format("No model factory for transported product '{0}', returning fall-back cube.", (object) proto.Id));
      return this.generateMesh(proto);
    }

    private GameObject generateMesh(ProductProto proto)
    {
      GameObject mesh = new GameObject(proto.Id.Value);
      BuildableMb buildableMb = mesh.AddComponent<BuildableMb>();
      MeshBuilder instance = MeshBuilder.Instance;
      Vector3 extents = new Vector3(0.25f, 0.25f, 0.25f);
      instance.AddAaBox(new Vector3(0.0f, extents.y, 0.0f), extents, new Color32((byte) 192, (byte) 192, (byte) 192, byte.MaxValue));
      instance.UpdateMbAndClear((IBuildable) buildableMb);
      buildableMb.GetComponent<MeshRenderer>().material = this.m_assetsDb.DefaultMaterial;
      return mesh;
    }
  }
}
