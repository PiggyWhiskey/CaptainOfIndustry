// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.IGameScene
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Represents the main scene which can be main game or main menu.
  /// </summary>
  public interface IGameScene
  {
    IEnumerator<string> Initialize();

    void Update(Fix32 deltaMs);

    void Terminate();

    void OnProjectChanged();

    event Action<DependencyResolver> ResolverCreated;
  }
}
