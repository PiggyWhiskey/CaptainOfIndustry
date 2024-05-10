// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileMaterialLayers
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Utils;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <remarks>
  /// Layers that are not valid MUST be set to default value. This is to avoid checking count when accessing them.
  /// Overflow index value can be arbitrary value when count is not greater than 4 (preferably keep the index value
  /// valid, e.g. don't set it to -1).
  /// </remarks>
  [ExpectedStructSize(24)]
  [StructLayout(LayoutKind.Explicit)]
  public struct TileMaterialLayers
  {
    [FieldOffset(0)]
    public int Count;
    [FieldOffset(4)]
    public TerrainMaterialThicknessSlim First;
    [FieldOffset(8)]
    public TerrainMaterialThicknessSlim Second;
    [FieldOffset(12)]
    public TerrainMaterialThicknessSlim Third;
    [FieldOffset(16)]
    public TerrainMaterialThicknessSlim Fourth;
    [FieldOffset(20)]
    public int OverflowIndex;

    public override readonly string ToString() => string.Format("Layers: {0}", (object) this.Count);
  }
}
