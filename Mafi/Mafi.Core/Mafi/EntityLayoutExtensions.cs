// Decompiled with JetBrains decompiler
// Type: Mafi.EntityLayoutExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;

#nullable disable
namespace Mafi
{
  public static class EntityLayoutExtensions
  {
    public static DebugGameMapDrawing DrawLayout(
      this DebugGameMapDrawing drawing,
      EntityLayout layout,
      TileTransform transform)
    {
      foreach (OccupiedTileRelative occupiedTileRelative in layout.GetOccupiedTilesRelative(transform))
        drawing.FillTile(transform.Position.Xy + occupiedTileRelative.RelCoord, occupiedTileRelative.Constraint.ToColor().SetA((byte) 92));
      return drawing;
    }
  }
}
