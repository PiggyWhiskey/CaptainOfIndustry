// Decompiled with JetBrains decompiler
// Type: RTG.RTSystemValues
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class RTSystemValues
  {
    public static Color XAxisColor
    {
      get => ColorEx.FromByteValues((byte) 219, (byte) 62, (byte) 29, byte.MaxValue);
    }

    public static Color YAxisColor
    {
      get => ColorEx.FromByteValues((byte) 154, (byte) 243, (byte) 72, byte.MaxValue);
    }

    public static Color ZAxisColor
    {
      get => ColorEx.FromByteValues((byte) 58, (byte) 122, (byte) 248, byte.MaxValue);
    }

    public static Color GridLineColor
    {
      get => ColorEx.FromByteValues((byte) 128, (byte) 128, (byte) 128, (byte) 102);
    }

    public static Color HoveredAxisColor
    {
      get => ColorEx.FromByteValues((byte) 246, (byte) 242, (byte) 50, byte.MaxValue);
    }

    public static Color CenterAxisColor
    {
      get => ColorEx.FromByteValues((byte) 204, (byte) 204, (byte) 204, byte.MaxValue);
    }

    public static float AxisAlpha => 0.3f;

    public static Color CameraBkGradientFirstColor
    {
      get => ColorEx.FromByteValues((byte) 71, (byte) 71, (byte) 71, byte.MaxValue);
    }

    public static Color CameraBkGradientSecondColor => Color.black;

    public static Color GuideFillColor => new Color(0.5f, 0.5f, 0.5f, 0.1f);

    public static Color GuideBorderColor => new Color(0.8f, 0.8f, 0.8f, 0.8f);
  }
}
