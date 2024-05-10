// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile3iExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class RelTile3iExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static RelTile3i NextRelTile3i(this IRandom random, int minValueIncl, int maxValueExcl)
    {
      return new RelTile3i(random.NextInt(minValueIncl, maxValueExcl), random.NextInt(minValueIncl, maxValueExcl), random.NextInt(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static RelTile3i NextRelTile3i(
      this IRandom random,
      RelTile3i minValueIncl,
      RelTile3i maxValueExcl)
    {
      return new RelTile3i(random.NextInt(minValueIncl.X, maxValueExcl.X), random.NextInt(minValueIncl.Y, maxValueExcl.Y), random.NextInt(minValueIncl.Z, maxValueExcl.Z));
    }

    public static RelTile3i ToTileDirection(this Direction903d direction)
    {
      return new RelTile3i(direction.DirectionVector);
    }
  }
}
