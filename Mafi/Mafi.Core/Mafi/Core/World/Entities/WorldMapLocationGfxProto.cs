// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapLocationGfxProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public class WorldMapLocationGfxProto : Proto
  {
    public readonly string IconPath;
    public readonly Vector2i Size;

    public static WorldMapLocationGfxProto CreateFromFileName(
      ProtosDb protosDb,
      string fileName,
      Vector2i size,
      float scale = 0.4f)
    {
      Proto.ID id = new Proto.ID("WorldGfx" + fileName);
      return protosDb.Add<WorldMapLocationGfxProto>(new WorldMapLocationGfxProto(id, "Assets/Unity/UserInterface/WorldMap/Islands/" + fileName + ".png", new Vector2i(((float) size.X * scale).RoundToInt(), ((float) size.Y * scale).RoundToInt())));
    }

    public WorldMapLocationGfxProto(Proto.ID id, string iconPath, Vector2i size)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.IconPath = iconPath;
      this.Size = size;
    }
  }
}
