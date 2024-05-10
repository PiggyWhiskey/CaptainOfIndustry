// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTowerProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  [DebuggerDisplay("MineTowerProto: {Id}")]
  public class MineTowerProto : LayoutEntityProto
  {
    public readonly MineTowerProto.MineArea Area;

    public override Type EntityType => typeof (MineTower);

    /// <summary>
    /// This product determines the default cargo type of assigned trucks and thus type of their default attachment.
    /// </summary>
    public Option<LooseProductProto> DefaultProductOfAssignedTrucks { get; private set; }

    public MineTowerProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      MineTowerProto.MineArea area,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.Area = area;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      this.DefaultProductOfAssignedTrucks = protosDb.First<LooseProductProto>();
    }

    /// <summary>
    /// Holds the configuration of the initial mining area of a <see cref="T:Mafi.Core.Buildings.Mine.MineTowerProto" />.
    /// </summary>
    public struct MineArea
    {
      /// <summary>
      /// Denotes the point (relative to the entity layout) where the area starts. Please see the example.
      /// </summary>
      /// <example>Below is an example of such area where size is (4,6). X denotes the origin point.</example>
      /// <code>
      /// halfWidth (2)
      /// ▲
      /// ******
      /// *    *
      /// TT   X    * ► length (6)
      /// *    *
      /// ******
      /// ▼
      /// halfWidth (2)
      /// </code>
      public readonly RelTile2i Origin;
      /// <summary>
      /// Initial size of the area before player makes any changes.
      /// </summary>
      public readonly RelTile2i InitialSize;
      /// <summary>
      /// Maximal allowed area size (for both the width and length).
      /// </summary>
      public readonly RelTile1i MaxAreaEdgeSize;

      public MineArea(RelTile2i origin, RelTile2i initialSize, RelTile1i maxAreaEdgeSize)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Origin = origin;
        this.InitialSize = initialSize;
        this.MaxAreaEdgeSize = maxAreaEdgeSize;
      }
    }
  }
}
