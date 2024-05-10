// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportedProductModelFactory
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
  /// Convenience class for transported products model retrieval. This class does not perform any pooling and always
  /// calls the factory. Each product may have multiple meshes returned randomly.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportedProductModelFactory
  {
    private readonly DependencyResolver m_resolver;

    public TransportedProductModelFactory(DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
    }

    /// <summary>Returns model of transported product for given proto.</summary>
    public GameObject CreateModelFor(ProductProto proto)
    {
      return this.m_resolver.InvokeFactoryHierarchy<GameObject>((object) proto);
    }
  }
}
