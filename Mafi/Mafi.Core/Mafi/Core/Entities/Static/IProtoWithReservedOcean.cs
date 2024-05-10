// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IProtoWithReservedOcean
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IProtoWithReservedOcean : 
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto
  {
    /// <summary>
    /// Specifies one or more reserved areas sets. The <see cref="T:Mafi.Core.Entities.Static.IStaticEntityWithReservedOcean" /> will maintain
    /// the first valid set.
    /// </summary>
    ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> ReservedOceanAreasSets { get; }

    HeightTilesI MinGroundHeight { get; }

    HeightTilesI MaxGroundHeight { get; }
  }
}
