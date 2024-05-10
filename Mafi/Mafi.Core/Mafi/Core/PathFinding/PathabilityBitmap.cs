// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.PathabilityBitmap
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System.Text;

#nullable disable
namespace Mafi.Core.PathFinding
{
  public readonly struct PathabilityBitmap
  {
    public readonly ulong Bitmap;

    public PathabilityBitmap(ulong bitmap)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Bitmap = bitmap;
    }

    private static ulong getBitMask(int x, int y) => 1UL << (y << 3) + x;

    public bool IsPathableAt(int x, int y)
    {
      return (this.Bitmap & PathabilityBitmap.getBitMask(x, y)) > 0UL;
    }

    public PathabilityBitmap SetPathableAt(int x, int y)
    {
      return new PathabilityBitmap(this.Bitmap | PathabilityBitmap.getBitMask(x, y));
    }

    public PathabilityBitmap SetNotPathableAt(int x, int y)
    {
      return new PathabilityBitmap(this.Bitmap & ~PathabilityBitmap.getBitMask(x, y));
    }

    public string ToDebugString()
    {
      StringBuilder stringBuilder = new StringBuilder(72);
      for (int y = 7; y >= 0; --y)
      {
        for (int x = 0; x < 8; ++x)
          stringBuilder.Append(this.IsPathableAt(x, y) ? ' ' : '#');
        stringBuilder.Append('\n');
      }
      return stringBuilder.ToString();
    }
  }
}
