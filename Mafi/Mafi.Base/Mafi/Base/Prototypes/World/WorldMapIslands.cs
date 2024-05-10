// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.World.WorldMapIslands
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Entities;

#nullable disable
namespace Mafi.Base.Prototypes.World
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class WorldMapIslands : IModData
  {
    private ProtosDb m_db;

    public void RegisterData(ProtoRegistrator registrator)
    {
      this.m_db = registrator.PrototypesDb;
      this.createFromFileName(Ids.Islands.HomeIsland, "IslandHome", new Vector2i(227, 427));
    }

    private WorldMapLocationGfxProto createFromFileName(
      Proto.ID protoId,
      string fileName,
      Vector2i size,
      float scale = 0.4f)
    {
      return this.m_db.Add<WorldMapLocationGfxProto>(new WorldMapLocationGfxProto(protoId, "Assets/Unity/UserInterface/WorldMap/Islands/" + fileName + ".png", new Vector2i(((float) size.X * scale).RoundToInt(), ((float) size.Y * scale).RoundToInt())));
    }

    public WorldMapIslands()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
