// Decompiled with JetBrains decompiler
// Type: RTG.GameObjectType
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace RTG
{
  [Flags]
  public enum GameObjectType
  {
    Mesh = 1,
    Terrain = 2,
    Sprite = 4,
    Camera = 8,
    Light = 16, // 0x00000010
    ParticleSystem = 32, // 0x00000020
    /// <summary>
    /// Identifies an empty object. An empty object is an object which doesn't
    /// belong to any of the types above.
    /// </summary>
    Empty = 64, // 0x00000040
  }
}
