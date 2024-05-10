// Decompiled with JetBrains decompiler
// Type: RTG.SpriteRendererEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class SpriteRendererEx
  {
    /// <summary>
    /// Returns the sprite's world center point. This function assumes that the
    /// sprite renderer has a valid sprite associated with it.
    /// </summary>
    public static Vector3 GetWorldCenterPoint(this SpriteRenderer spriteRenderer)
    {
      return spriteRenderer.transform.TransformPoint(spriteRenderer.GetModelSpaceAABB().Center);
    }

    /// <summary>
    /// Returns the sprite's model space size. This function assumes that the
    /// sprite renderer has a valid sprite associated with it.
    /// </summary>
    public static Vector3 GetModelSpaceSize(this SpriteRenderer spriteRenderer)
    {
      return spriteRenderer.GetModelSpaceAABB().Size;
    }

    /// <summary>
    /// Returns the sprite's model space AABB. The function will return an invalid
    /// AABB if the sprite renderer doesn't have a sprite associated with it.
    /// </summary>
    public static AABB GetModelSpaceAABB(this SpriteRenderer spriteRenderer)
    {
      Sprite sprite = spriteRenderer.sprite;
      return (Object) sprite == (Object) null ? AABB.GetInvalid() : new AABB((IEnumerable<Vector2>) new List<Vector2>((IEnumerable<Vector2>) sprite.vertices));
    }

    /// <summary>
    /// Checks if the sprite pixel which resides at 'worldPos' is fully transparent.
    /// </summary>
    /// <remarks>
    /// Works only when the Read/Write enabled flag is set inside the sprite texture
    /// properties. Otherwise, it will always return false.
    /// </remarks>
    public static bool IsPixelFullyTransparent(this SpriteRenderer spriteRenderer, Vector3 worldPos)
    {
      Sprite sprite = spriteRenderer.sprite;
      if ((Object) sprite == (Object) null)
        return true;
      Texture2D texture = sprite.texture;
      if ((Object) texture == (Object) null)
        return true;
      Vector3 pt = spriteRenderer.transform.InverseTransformPoint(worldPos);
      Plane plane = new Plane(Vector3.forward, 0.0f);
      Vector3 point = plane.ProjectPoint(pt);
      AABB modelSpaceAabb = spriteRenderer.GetModelSpaceAABB();
      modelSpaceAabb.Size = new Vector3(modelSpaceAabb.Size.x, modelSpaceAabb.Size.y, 1f);
      if (!modelSpaceAabb.ContainsPoint(point))
        return true;
      Vector3 vector3_1 = plane.ProjectPoint(modelSpaceAabb.Min);
      Vector3 vector3_2 = point - vector3_1;
      Vector2 vector2 = new Vector2(vector3_2.x * sprite.pixelsPerUnit, vector3_2.y * sprite.pixelsPerUnit) + sprite.textureRectOffset;
      try
      {
        return (double) texture.GetPixel((int) ((double) vector2.x + 0.5), (int) ((double) vector2.y + 0.5)).a <= 1.0 / 1000.0;
      }
      catch (UnityException ex)
      {
        return ex != null && false;
      }
    }
  }
}
