// Decompiled with JetBrains decompiler
// Type: RTG.LayerEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class LayerEx
  {
    public static int GetMinLayer() => 0;

    public static int GetMaxLayer() => 31;

    public static bool IsLayerBitSet(int layerBits, int layerNumber)
    {
      return (layerBits & 1 << layerNumber) != 0;
    }

    public static int SetLayerBit(int layerBits, int layerNumber) => layerBits | 1 << layerNumber;

    public static int ClearLayerBit(int layerBits, int layerNumber)
    {
      return layerBits & ~(1 << layerNumber);
    }

    public static bool IsLayerValid(int layerNumber)
    {
      return layerNumber >= LayerEx.GetMinLayer() && layerNumber <= LayerEx.GetMaxLayer();
    }

    public static List<string> GetAllLayerNames()
    {
      List<string> allLayerNames = new List<string>();
      for (int layer = 0; layer <= 31; ++layer)
      {
        string name = LayerMask.LayerToName(layer);
        if (!string.IsNullOrEmpty(name))
          allLayerNames.Add(name);
      }
      return allLayerNames;
    }
  }
}
