// Decompiled with JetBrains decompiler
// Type: RTG.ColorEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class ColorEx
  {
    public static Color FromByteValues(byte r, byte g, byte b, byte a)
    {
      float num = 0.003921569f;
      return new Color((float) r * num, (float) g * num, (float) b * num, (float) a * num);
    }

    public static Color[] GetFilledColorArray(int arrayLength, Color fillValue)
    {
      Color[] filledColorArray = new Color[arrayLength];
      for (int index = 0; index < arrayLength; ++index)
        filledColorArray[index] = fillValue;
      return filledColorArray;
    }

    public static Color KeepAllButAlpha(this Color color, float newAlpha)
    {
      return new Color(color.r, color.g, color.b, newAlpha);
    }
  }
}
