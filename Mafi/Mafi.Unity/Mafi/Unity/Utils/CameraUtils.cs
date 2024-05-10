// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.CameraUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity.Utils
{
  public static class CameraUtils
  {
    public static Matrix4x4 GetWorldToImageTransformMatrix(
      int imageWidth,
      int imageHeight,
      Camera camera)
    {
      Matrix4x4 identity = Matrix4x4.identity;
      identity[0, 0] = (float) imageWidth * 0.5f;
      identity[1, 1] = (float) -imageHeight * 0.5f;
      identity[0, 3] = (float) imageWidth * 0.5f;
      identity[1, 3] = (float) imageHeight * 0.5f;
      return identity * camera.projectionMatrix * camera.worldToCameraMatrix;
    }
  }
}
