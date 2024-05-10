// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ShipsAnimationData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class ShipsAnimationData
  {
    public readonly string AnimatorPrefabPath;
    public readonly ImmutableArray<RectangleTerrainArea2iRelative> ReservedOceanAreas;

    public ShipsAnimationData(
      string animatorPrefabPath,
      ImmutableArray<RectangleTerrainArea2iRelative> reservedOceanAreas)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AnimatorPrefabPath = animatorPrefabPath;
      this.ReservedOceanAreas = reservedOceanAreas;
    }
  }
}
