// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPillarProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public class TransportPillarProto : StaticEntityProto
  {
    public static readonly ThicknessTilesI MAX_PILLAR_HEIGHT;
    public readonly TransportPillarProto.Gfx Graphics;

    public override Type EntityType => typeof (TransportPillar);

    public TransportPillarProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      TransportPillarProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, EntityCosts.None, 1.Ticks(), new ThicknessIRange?(), (StaticEntityProto.Gfx) graphics, true);
      this.Graphics = graphics;
    }

    static TransportPillarProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportPillarProto.MAX_PILLAR_HEIGHT = 4.TilesThick();
    }

    public new class Gfx : StaticEntityProto.Gfx
    {
      public readonly string CornerBeamsPrefabPath;
      public readonly string CornerBasePrefabPath;
      public readonly string SideFillPlusXPrefabPath;
      public readonly string BaseWithSideFillsPrefabPath;

      public static TransportPillarProto.Gfx Empty
      {
        get => new TransportPillarProto.Gfx("EMPTY", "EMPTY", "EMPTY", "EMPTY");
      }

      public Gfx(
        string cornerBeamsPrefabPath,
        string cornerBasePrefabPath,
        string sideFillPlusXPrefabPath,
        string baseWithSideFillsPrefabPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty, false);
        this.CornerBeamsPrefabPath = cornerBeamsPrefabPath;
        this.CornerBasePrefabPath = cornerBasePrefabPath;
        this.SideFillPlusXPrefabPath = sideFillPlusXPrefabPath;
        this.BaseWithSideFillsPrefabPath = baseWithSideFillsPrefabPath;
      }
    }
  }
}
