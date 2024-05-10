// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TileValidityInstanceData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Utils;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// Per-instance data that is passed to GPU. Layout of this struct must match the `InstanceData` struct
  /// in the shader.
  /// </summary>
  [ExpectedStructSize(12)]
  public readonly struct TileValidityInstanceData
  {
    public const int SIZE_BYTES = 12;
    public readonly int TileX;
    public readonly int TileY;
    public readonly uint ColorAndValidity;

    public TileValidityInstanceData(int tileX, int tileY, ColorRgba color, byte validity)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.TileX = tileX;
      this.TileY = tileY;
      this.ColorAndValidity = color.Rgba & 4294967040U | (uint) validity;
    }
  }
}
