// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.IProtoModelFactory`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Common interface for all factories of models (represented as <see cref="T:UnityEngine.GameObject" />) that use just proto for
  /// creation of the model. Those are many static entities or ports. But transports are not the case.
  /// </summary>
  public interface IProtoModelFactory<TProto> : IFactory<TProto, GameObject> where TProto : IProto
  {
  }
}
