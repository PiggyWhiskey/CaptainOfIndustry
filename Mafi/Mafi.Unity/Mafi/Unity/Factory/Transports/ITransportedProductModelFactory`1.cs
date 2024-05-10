// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.ITransportedProductModelFactory`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  /// <summary>
  /// Common interface for all transported product factories. Given a product prototype returns a game object with
  /// attached 3D model.
  /// </summary>
  /// <typeparam name="T">Type of product prototype handled by the factory.</typeparam>
  public interface ITransportedProductModelFactory<T> : IFactory<T, GameObject> where T : ProductProto
  {
  }
}
