// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.TerrainTileSurfaceMenuItem
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Terrain;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  public class TerrainTileSurfaceMenuItem : EntitiesMenuItem
  {
    public Option<TerrainTileSurfaceProto> Proto { get; }

    public TerrainTileSurfaceMenuItem(
      Option<TerrainTileSurfaceProto> proto,
      Option<Mafi.Core.Prototypes.Proto> extraLockingProto,
      LocStrFormatted name,
      string iconPath,
      ImmutableArray<ToolbarCategoryProto> categories)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(proto.As<Mafi.Core.Prototypes.Proto>(), extraLockingProto, name, iconPath, categories);
      this.Proto = proto;
    }
  }
}
