// Decompiled with JetBrains decompiler
// Type: Mafi.TileInChunk256
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi
{
  [ExpectedStructSize(2)]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct TileInChunk256
  {
    [FieldOffset(0)]
    public readonly byte X;
    [FieldOffset(1)]
    public readonly byte Y;
    [FieldOffset(0)]
    public readonly ushort Index;

    public TileInChunk256(ushort index)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.X = (byte) 0;
      this.Y = (byte) 0;
      this.Index = index;
    }

    public TileInChunk256(byte x, byte y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Index = (ushort) 0;
      this.X = x;
      this.Y = y;
    }

    public RelTile2i AsTileOffset => new RelTile2i((int) this.X, (int) this.Y);

    public static TileInChunk256 FromTile(Tile2i tile)
    {
      return new TileInChunk256((byte) tile.X, (byte) tile.Y);
    }
  }
}
