// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutTileConstraint
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  /// <summary>
  /// Constraint for a single tile of an entity layout. For instance if a tile of the entity is allowed to be only
  /// above the ocean or flat ground.
  /// </summary>
  [Flags]
  public enum LayoutTileConstraint
  {
    None = 0,
    Ground = 1,
    Ocean = 2,
    UsingPillar = 4,
    DisableTerrainPhysics = 16, // 0x00000010
    NoRubbleAfterCollapse = 64, // 0x00000040
  }
}
