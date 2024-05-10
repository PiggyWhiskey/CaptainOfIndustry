// Decompiled with JetBrains decompiler
// Type: RTG.SpriteEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class SpriteEx
  {
    public static List<Vector3> GetWorldVerts(this Sprite sprite, Transform spriteTransform)
    {
      List<Vector3> points = new List<Vector3>((IEnumerable<Vector3>) sprite.GetModelVerts());
      spriteTransform.TransformPoints(points);
      return points;
    }

    public static List<Vector3> GetModelVerts(this Sprite sprite)
    {
      List<Vector3> modelVerts = new List<Vector3>(7);
      foreach (Vector2 vertex in sprite.vertices)
        modelVerts.Add((Vector3) vertex);
      return modelVerts;
    }
  }
}
