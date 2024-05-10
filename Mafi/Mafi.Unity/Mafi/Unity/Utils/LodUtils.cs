// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.LodUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  public static class LodUtils
  {
    public const float TERRAIN_PIXELS_PER_METER = 32f;
    public const float LOD_0_PIXELS_PER_METER = 32f;
    public const float LOD_1_PIXELS_PER_METER = 16f;
    public const float LOD_2_PIXELS_PER_METER = 8f;
    public const float LOD_3_PIXELS_PER_METER = 4f;
    public const float LOD_4_PIXELS_PER_METER = 2f;
    public const float LOD_5_PIXELS_PER_METER = 1f;
    public const float LOD_6_PIXELS_PER_METER = 0.5f;
    public const int MAX_LOD = 6;
    public const int LODS_COUNT = 7;
    private static readonly float[] PX_PER_METER_LODS;

    public static Mesh[] SameMeshForAllLods(Mesh mesh)
    {
      Mesh[] meshArray = new Mesh[7];
      for (int index = 0; index < meshArray.Length; ++index)
        meshArray[index] = mesh;
      return meshArray;
    }

    public static float GetPixelsPerMeterForLodLevel(int lodLevel)
    {
      if (lodLevel <= 0)
        return 32f;
      return lodLevel >= LodUtils.PX_PER_METER_LODS.Length ? LodUtils.PX_PER_METER_LODS.Last<float>() : LodUtils.PX_PER_METER_LODS[lodLevel];
    }

    public static float GetPixelsPerMeterForLodLevelSmooth(float lodLevel)
    {
      if ((double) lodLevel <= 0.0)
        return 32f;
      return (double) lodLevel >= (double) LodUtils.PX_PER_METER_LODS.Length ? LodUtils.PX_PER_METER_LODS.Last<float>() : (float) (32.0 / ((double) lodLevel + 1.0));
    }

    public static float GetCameraDistanceForPpm(float pixelsPerMeter, Camera camera)
    {
      return MafiMath.SizeAlongFovToDistance((float) camera.pixelHeight / pixelsPerMeter, camera.fieldOfView);
    }

    public static float GetCameraDistanceForLodLevel(int lodLevel, Camera camera)
    {
      return LodUtils.GetCameraDistanceForPpm(LodUtils.GetPixelsPerMeterForLodLevel(lodLevel), camera);
    }

    public static float GetCameraDistanceForLodLevelSmooth(float lodLevel, Camera camera)
    {
      return LodUtils.GetCameraDistanceForPpm(LodUtils.GetPixelsPerMeterForLodLevelSmooth(lodLevel), camera);
    }

    public static int GetLodLevelForCameraDistance(float distance, Camera camera)
    {
      return LodUtils.GetLodLevelFromPxPerMeter(LodUtils.GetPxPerMeterForCameraDistance(distance, camera));
    }

    public static float GetPxPerMeterForCameraDistance(float distance, Camera camera)
    {
      float sizeAlongFov = MafiMath.DistanceToSizeAlongFov(distance, camera.fieldOfView);
      return (float) camera.pixelHeight / sizeAlongFov;
    }

    public static int GetLodLevelFromPxPerMeter(float pxPerMeter)
    {
      for (int index = 1; index < LodUtils.PX_PER_METER_LODS.Length; ++index)
      {
        if ((double) pxPerMeter >= (double) LodUtils.PX_PER_METER_LODS[index])
          return index - 1;
      }
      return 6;
    }

    public static float GetLodLevelForCameraDistanceSmooth(float distance, Camera camera)
    {
      float sizeAlongFov = MafiMath.DistanceToSizeAlongFov(distance, camera.fieldOfView);
      float num = (float) camera.pixelHeight / sizeAlongFov;
      if ((double) num <= 32.0)
        return 0.0f;
      return (double) num >= 0.5 ? 6f : (float) (32.0 / (double) num - 1.0);
    }

    static LodUtils()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LodUtils.PX_PER_METER_LODS = new float[7]
      {
        32f,
        16f,
        8f,
        4f,
        2f,
        1f,
        0.5f
      };
    }
  }
}
