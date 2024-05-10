// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile2fExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class RelTile2fExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static RelTile2f NextRelTile2f(
      this IRandom random,
      Fix32 minValueIncl,
      Fix32 maxValueExcl)
    {
      return new RelTile2f(random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static RelTile2f NextRelTile2f(
      this IRandom random,
      RelTile2f minValueIncl,
      RelTile2f maxValueExcl)
    {
      return new RelTile2f(random.NextFix32(minValueIncl.X, maxValueExcl.X), random.NextFix32(minValueIncl.Y, maxValueExcl.Y));
    }

    public static RelTile2f NextRelTile2fBetween01(this IRandom random)
    {
      return new RelTile2f(random.NextFix32Between01Fast(), random.NextFix32Between01Fast());
    }

    /// <summary>
    /// Very fast way of generating random relative Fix32 coordinate in a chunk.
    /// </summary>
    public static RelTile2f NextTileInChunk2f(this IRandom random)
    {
      ulong num = random.NextUlong();
      return new RelTile2f(Fix32.FromRaw((int) num & (int) ushort.MaxValue), Fix32.FromRaw((int) (num >> 32) & (int) ushort.MaxValue));
    }

    public static Tile2f NextTileInChunk2f(this IRandom random, Chunk2i chunk)
    {
      RelTile2f relTile2f = random.NextTileInChunk2f();
      return chunk.Tile2i.CornerTile2f + relTile2f;
    }
  }
}
