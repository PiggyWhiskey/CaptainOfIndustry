﻿// Decompiled with JetBrains decompiler
// Type: RTG.IXZGrid
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IXZGrid
  {
    Plane WorldPlane { get; }

    Matrix4x4 WorldMatrix { get; }

    XZGridCell CellFromWorldPoint(Vector3 worldPoint);

    bool Raycast(Ray ray, out float t);
  }
}
