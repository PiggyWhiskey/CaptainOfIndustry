// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UnityExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Numerics;
using System;
using System.IO;
using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  public static class UnityExtensions
  {
    public const float REFERENCE_FPS = 60f;
    public const int TILE_TO_METERS = 2;
    public const float METERS_TO_TILE = 0.5f;

    public static int TilesToUnityUnits(this int tiles) => tiles * 2;

    public static float ToUnityUnits(this ThicknessTilesI discrete) => (float) (discrete.Value * 2);

    public static float ToUnityUnits(this ThicknessTilesF discrete)
    {
      return (discrete.Value * 2).ToFloat();
    }

    public static float ToUnityUnits(this RelTile1f tile) => tile.Value.ToFloat() * 2f;

    public static bool IsFinite(this Vector3 v)
    {
      return v.x.IsFinite() && v.y.IsFinite() && v.z.IsFinite();
    }

    public static UnityEngine.Vector2 ToVector2(this Vector2i v)
    {
      return new UnityEngine.Vector2((float) v.X, (float) v.Y);
    }

    public static UnityEngine.Vector2 ToVector2(this Vector2f v)
    {
      return new UnityEngine.Vector2(v.X.ToFloat(), v.Y.ToFloat());
    }

    public static UnityEngine.Vector2 ToVector2(this Tile2i tileCoord)
    {
      return new UnityEngine.Vector2((float) (tileCoord.X * 2), (float) (tileCoord.Y * 2));
    }

    public static UnityEngine.Vector2 ToVector2(this RelTile2f tileCoord)
    {
      return new UnityEngine.Vector2(tileCoord.X.ToFloat() * 2f, tileCoord.Y.ToFloat() * 2f);
    }

    /// <summary>Converts this Mafi vector to Unity vector.</summary>
    public static Vector3 ToVector3(this Vector3f v)
    {
      return new Vector3(v.X.ToFloat(), v.Z.ToFloat(), v.Y.ToFloat());
    }

    /// <summary>Converts this Mafi vector to Unity vector.</summary>
    public static Vector3 ToVector3(this Vector3i v)
    {
      return new Vector3((float) v.X, (float) v.Z, (float) v.Y);
    }

    public static Vector3 ToVector3(this Chunk2i chunkCoord)
    {
      return new Vector3((float) (chunkCoord.X * 128), 0.0f, (float) (chunkCoord.Y * 128));
    }

    public static Vector3 ToCornerVector3(this Tile3i tile)
    {
      return new Vector3((float) tile.X, (float) tile.Z, (float) tile.Y) * 2f;
    }

    public static Vector3 ToGroundCenterVector3(this Tile3i tile)
    {
      return new Vector3((float) tile.X + 0.5f, (float) tile.Z, (float) tile.Y + 0.5f) * 2f;
    }

    public static Vector3 ToCenterVector3(this Tile3i tile)
    {
      return new Vector3((float) tile.X + 0.5f, (float) tile.Z + 0.5f, (float) tile.Y + 0.5f) * 2f;
    }

    public static Vector3 ToVector3(this RelTile3i tile)
    {
      return new Vector3((float) tile.X, (float) tile.Z, (float) tile.Y) * 2f;
    }

    public static UnityEngine.Vector2 ToVector2(this Tile2f tile)
    {
      return new UnityEngine.Vector2(tile.X.ToFloat(), tile.Y.ToFloat()) * 2f;
    }

    public static Tile2f ToTile2f(this UnityEngine.Vector2 v)
    {
      return new Tile2f((v.x * 0.5f).ToFix32(), (v.y * 0.5f).ToFix32());
    }

    public static RelTile2f ToRelTile2f(this UnityEngine.Vector2 v)
    {
      return new RelTile2f((v.x * 0.5f).ToFix32(), (v.y * 0.5f).ToFix32());
    }

    public static Vector3 ToVector3(this Tile3f tile)
    {
      return new Vector3(tile.X.ToFloat(), tile.Z.ToFloat(), tile.Y.ToFloat()) * 2f;
    }

    public static Vector3 ToVector3(this RelTile3f tile)
    {
      return new Vector3(tile.X.ToFloat(), tile.Z.ToFloat(), tile.Y.ToFloat()) * 2f;
    }

    public static UnityEngine.Vector2 Vector2(this int coord)
    {
      return new UnityEngine.Vector2((float) coord, (float) coord);
    }

    public static UnityEngine.Vector2 Min(this UnityEngine.Vector2 first, UnityEngine.Vector2 second)
    {
      return UnityEngine.Vector2.Min(first, second);
    }

    /// <summary>
    /// Returns an (arbitrary) orthogonal vector. Returned vector is not normalized.
    /// </summary>
    /// <remarks>
    /// Returned vector is a cross product with an axis corresponding to the smallest component of the vector.
    /// </remarks>
    public static Vector3 Orthogonal(this Vector3 v)
    {
      float num1 = Mathf.Abs(v.x);
      float num2 = Mathf.Abs(v.y);
      float num3 = Mathf.Abs(v.z);
      Vector3 rhs = (double) num1 < (double) num2 ? ((double) num1 < (double) num3 ? new Vector3(1f, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 1f)) : ((double) num2 < (double) num3 ? new Vector3(0.0f, 1f, 0.0f) : new Vector3(0.0f, 0.0f, 1f));
      return Vector3.Cross(v, rhs);
    }

    public static UnityEngine.Vector2 LeftOrthogonalVector(this UnityEngine.Vector2 v)
    {
      return new UnityEngine.Vector2(-v.y, v.x);
    }

    /// <summary>Converts this Unity vector to Mafi vector.</summary>
    public static Vector3f ToVector3f(this Vector3 v)
    {
      return new Vector3f(v.x.ToFix32(), v.z.ToFix32(), v.y.ToFix32());
    }

    /// <summary>Converts this Unity vector to Mafi vector.</summary>
    public static Vector4f ToVector4f(this Vector4 v)
    {
      return new Vector4f(v.x.ToFix32(), v.y.ToFix32(), v.z.ToFix32(), v.w.ToFix32());
    }

    /// <summary>Converts this Mafi vector to Unity vector.</summary>
    public static Vector4 ToVector4(this Vector4f v)
    {
      return new Vector4(v.X.ToFloat(), v.Y.ToFloat(), v.Z.ToFloat(), v.W.ToFloat());
    }

    /// <summary>
    /// Converts this Unity vector to terrain-tile floating points vector.
    /// </summary>
    public static Tile3f ToTile3f(this Vector3 v)
    {
      return new Tile3f((v.x * 0.5f).ToFix32(), (v.z * 0.5f).ToFix32(), (v.y * 0.5f).ToFix32());
    }

    public static RelTile3f ToRelTile3f(this Vector3 v)
    {
      return new RelTile3f((v.x * 0.5f).ToFix32(), (v.z * 0.5f).ToFix32(), (v.y * 0.5f).ToFix32());
    }

    /// <summary>Discards height.</summary>
    public static Tile2f ToTile2f(this Vector3 v)
    {
      return new Tile2f((v.x * 0.5f).ToFix32(), (v.z * 0.5f).ToFix32());
    }

    /// <summary>Converts this Unity quaternion to Mafi quaternion.</summary>
    public static UnitQuaternion4f ToQuaternion4f(this Quaternion q)
    {
      return new UnitQuaternion4f(-q.x.ToFix32(), -q.z.ToFix32(), -q.y.ToFix32(), q.w.ToFix32());
    }

    /// <summary>
    /// Converts this Mafi color to Unity color. PERF: This is fairly expensive operation.
    /// </summary>
    public static Color ToColor(this ColorRgba c)
    {
      return new Color((float) c.R * 0.003921569f, (float) c.G * 0.003921569f, (float) c.B * 0.003921569f, (float) c.A * 0.003921569f);
    }

    /// <summary>
    /// Converts this Mafi color to Unity color. PERF: This is cheap.
    /// </summary>
    public static Color AsColor(this ColorUniversal c) => new Color(c.R, c.G, c.B, c.A);

    /// <summary>Converts this Mafi color to Unity color 32.</summary>
    public static Color32 ToColor32(this ColorRgba c) => new Color32(c.R, c.G, c.B, c.A);

    /// <summary>
    /// Converts this Unity ray to Mafi ray and swaps Y and Z in the internal vectors.
    /// </summary>
    public static Ray3f ToRay3f(this Ray ray)
    {
      return new Ray3f(ray.origin.ToVector3f(), ray.direction.ToVector3f());
    }

    /// <summary>
    /// Converts this Mafi ray to Unity ray and swaps Y and Z in the internal vectors.
    /// </summary>
    public static Ray ToRay(this Ray3f ray)
    {
      return new Ray(ray.Origin.ToVector3(), ray.Direction.ToVector3());
    }

    /// <summary>Converts tiles height to unity units of height.</summary>
    public static int ToUnityUnits(this HeightTilesI height) => height.Value * 2;

    /// <summary>Converts tiles height to unity units of height.</summary>
    public static float ToUnityUnits(this HeightTilesF height) => height.Value.ToFloat() * 2f;

    /// <summary>Converts tiles to unity units.</summary>
    public static float ToUnityUnits(this RelTile1i tiles) => (float) (tiles.Value * 2);

    public static UnityEngine.Vector2 ToUnityUnits(this RelTile2i tiles)
    {
      return new UnityEngine.Vector2((float) (tiles.X * 2), (float) (tiles.Y * 2));
    }

    /// <summary>Converts angle to Unity angle.</summary>
    /// <remarks>
    /// Since our world uses right-handed system and Unity has left-handed system we have to inverse the sign of the
    /// angle.
    /// </remarks>
    public static float ToUnityAngleDegrees(this AngleDegrees1f value) => -value.Degrees.ToFloat();

    public static float ToUnityAngleDegrees(this AngleSlim value)
    {
      return (float) (360.0 * -((double) value.Raw / 65536.0));
    }

    /// <summary>Converts quaternion to Unity representation.</summary>
    /// <remarks>
    /// WARNING: This conversion is total guess by me. It seems to work though. My intuition is that we have to swap
    /// Y and Z and also conjugate the quaternion (similarly as we had to negate 2D angles) due to RHS =} LHS
    /// conversion.
    /// </remarks>
    public static Quaternion ToUnityQuaternion(this UnitQuaternion4f value)
    {
      Vector4 vector4;
      ref Vector4 local = ref vector4;
      Fix32 fix32 = value.X;
      double x = -(double) fix32.ToFloat();
      fix32 = value.Z;
      double y = -(double) fix32.ToFloat();
      fix32 = value.Y;
      double z = -(double) fix32.ToFloat();
      double w = (double) value.W.ToFloat();
      local = new Vector4((float) x, (float) y, (float) z, (float) w);
      vector4.Normalize();
      return new Quaternion(vector4.x, vector4.y, vector4.z, vector4.w);
    }

    [Pure]
    public static Color Lerp(this Color from, Color to, float t)
    {
      return new Color(from.r.Lerp(to.r, t), from.g.Lerp(to.g, t), from.b.Lerp(to.b, t), from.a.Lerp(to.a, t));
    }

    [Pure]
    public static Color LerpNoAlpha(this Color from, Color to, float t)
    {
      return new Color(from.r.Lerp(to.r, t), from.g.Lerp(to.g, t), from.b.Lerp(to.b, t));
    }

    public static void SaveAsPng(this RenderTexture rt, string pngOutPath)
    {
      Assert.That<RenderTexture>(rt).IsValidUnityObject<RenderTexture>("Saving invalid texture.");
      RenderTexture active = RenderTexture.active;
      Texture2D tex = new Texture2D(rt.width, rt.height);
      RenderTexture.active = rt;
      tex.ReadPixels(new Rect(0.0f, 0.0f, (float) rt.width, (float) rt.height), 0, 0);
      tex.Apply();
      File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
      RenderTexture.active = active;
    }

    [Pure]
    public static bool IntersectsRay(this BoundingSphere sphere, Ray ray)
    {
      float num1 = Vector3.Dot(ray.direction, ray.direction);
      float num2 = Vector3.Dot(ray.direction, 2f * (ray.origin - sphere.position));
      float num3 = (float) ((double) Vector3.Dot(sphere.position, sphere.position) + (double) Vector3.Dot(ray.origin, ray.origin) - 2.0 * (double) Vector3.Dot(ray.origin, sphere.position) - (double) sphere.radius * (double) sphere.radius);
      return (double) num2 * (double) num2 + -4.0 * (double) num1 * (double) num3 > 0.0;
    }

    [Pure]
    public static UnityEngine.Vector2 RotateRad(this UnityEngine.Vector2 v, float angleRad)
    {
      float num1 = Mathf.Cos(angleRad);
      float num2 = Mathf.Sin(angleRad);
      return new UnityEngine.Vector2((float) ((double) v.x * (double) num1 - (double) v.y * (double) num2), (float) ((double) v.x * (double) num2 + (double) v.y * (double) num1));
    }

    [Pure]
    public static UnityEngine.Vector2 RotateDeg(this UnityEngine.Vector2 v, float angleDeg)
    {
      return v.RotateRad((float) Math.PI / 180f * angleDeg);
    }

    [Pure]
    public static UnityEngine.Vector2 CubicInterpolate(this UnityEngine.Vector2[] data, int baseI, float t)
    {
      UnityEngine.Vector2 vector2_1 = data[baseI - 1];
      UnityEngine.Vector2 vector2_2 = data[baseI];
      UnityEngine.Vector2 vector2_3 = data[baseI + 1];
      UnityEngine.Vector2 vector2_4 = data[baseI + 2];
      return new UnityEngine.Vector2(MafiMath.CubicInterpolate(vector2_1.x, vector2_2.x, vector2_3.x, vector2_4.x, t), MafiMath.CubicInterpolate(vector2_1.y, vector2_2.y, vector2_3.y, vector2_4.y, t));
    }

    [Pure]
    public static UnityEngine.Vector2 BicubicInterpolate(
      this UnityEngine.Vector2[] data,
      int stride,
      int baseI,
      float tx,
      float ty)
    {
      UnityEngine.Vector2 vector2_1 = data.CubicInterpolate(baseI - stride, tx);
      UnityEngine.Vector2 vector2_2 = data.CubicInterpolate(baseI, tx);
      UnityEngine.Vector2 vector2_3 = data.CubicInterpolate(baseI + stride, tx);
      UnityEngine.Vector2 vector2_4 = data.CubicInterpolate(baseI + 2 * stride, tx);
      return new UnityEngine.Vector2(MafiMath.CubicInterpolate(vector2_1.x, vector2_2.x, vector2_3.x, vector2_4.x, ty), MafiMath.CubicInterpolate(vector2_1.y, vector2_2.y, vector2_3.y, vector2_4.y, ty));
    }

    /// <summary>Frame-rate-independent exponential damping.</summary>
    public static float SmoothDampTo(this float value, float target, float sharpness)
    {
      float t = 1f - Mathf.Pow(1f - sharpness, Time.deltaTime * 60f);
      return Mathf.Lerp(value, target, t);
    }

    /// <summary>
    /// Frame-rate-independent exponential damping with unscaled delta-time.
    /// </summary>
    public static float SmoothDampToUnscaledTime(this float value, float target, float sharpness)
    {
      float t = 1f - Mathf.Pow(1f - sharpness, Time.unscaledDeltaTime * 60f);
      return Mathf.Lerp(value, target, t);
    }

    /// <summary>
    /// Frame-rate-independent exponential damping with unscaled delta-time.
    /// </summary>
    public static UnityEngine.Vector2 SmoothDampToUnscaledTime(
      this UnityEngine.Vector2 value,
      UnityEngine.Vector2 target,
      float sharpness)
    {
      float t = 1f - Mathf.Pow(1f - sharpness, Time.unscaledDeltaTime * 60f);
      return new UnityEngine.Vector2(Mathf.Lerp(value.x, target.x, t), Mathf.Lerp(value.y, target.y, t));
    }
  }
}
