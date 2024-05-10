// Decompiled with JetBrains decompiler
// Type: RTG.ObjectSnap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class ObjectSnap
  {
    public static void Snap(GameObject root, Vector3 pivot, Vector3 dest)
    {
      Transform transform = root.transform;
      Vector3 vector3 = transform.position - pivot;
      transform.position = dest + vector3;
    }

    public static void Snap(List<GameObject> roots, Vector3 pivot, Vector3 dest)
    {
      foreach (GameObject root in roots)
        ObjectSnap.Snap(root, pivot, dest);
    }
  }
}
