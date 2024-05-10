// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public abstract class WorldMapEntityProto : EntityProto, IProtoWithIcon, IProto
  {
    public readonly WorldMapEntityProto.Gfx Graphics;

    public string IconPath => this.Graphics.IconPath;

    public WorldMapEntityProto(
      EntityProto.ID id,
      Proto.Str strings,
      EntityCosts costs,
      WorldMapEntityProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, costs, (EntityProto.Gfx) graphics, tags);
      this.Graphics = graphics;
    }

    public new class Gfx : EntityProto.Gfx
    {
      public static readonly WorldMapEntityProto.Gfx Empty;

      /// <summary>Icon asset path to be used in UI.</summary>
      public string IconPath { get; private set; }

      /// <summary>
      /// Icon used in world map to denote a location with this entity.
      /// </summary>
      public string WorldMapIconPath { get; private set; }

      public Gfx(string iconPath, string worldMapIconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty);
        this.IconPath = iconPath;
        this.WorldMapIconPath = worldMapIconPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WorldMapEntityProto.Gfx.Empty = new WorldMapEntityProto.Gfx("EMPTY", "EMPTY");
      }
    }
  }
}
