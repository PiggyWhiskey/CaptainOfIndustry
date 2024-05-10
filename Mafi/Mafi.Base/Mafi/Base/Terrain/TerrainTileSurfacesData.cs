// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.TerrainTileSurfacesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Base.Terrain
{
  public class TerrainTileSurfacesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      Percent maintenanceScale = Percent.FromFloat(0.8f);
      string str = "Surface that can be placed on the ground outside.";
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.ConcreteSlab);
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Bricks);
      ProductProto orThrow3 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Iron);
      ProductProto orThrow4 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Steel);
      ProductProto orThrow5 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Gold);
      TileSurfacesEdgesSpec surfacesEdgesSpec1 = new TileSurfacesEdgesSpec("Assets/Base/Terrain/Surfaces/ConcreteEdges/A-albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/A-normals.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/A-smoothmetal.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/B-albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/B-normals.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/B-smoothmetal.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/C-albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/C-normals.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/C-smoothmetal.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/D-albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/D-normals.png", "Assets/Base/Terrain/Surfaces/ConcreteEdges/D-smoothmetal.png");
      TileSurfacesEdgesSpec edgesSpec1 = new TileSurfacesEdgesSpec("Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/A_albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/A_normals.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/A_smoothmetal.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/B_albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/B_normals.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/B_smoothmetal.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/C_albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/C_normals.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/C_smoothmetal.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/D_albedo.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/D_normals.png", "Assets/Base/Terrain/Surfaces/ConcreteReinforcedEdges/D_smoothmetal.png");
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.DefaultConcrete, Proto.CreateStr(Ids.TerrainTileSurfaces.DefaultConcrete, "Concrete surface", translationComment: str), maintenanceScale, orThrow1.WithQuantity(1), true, ImmutableArray.Create<Proto.ID>(Ids.TerrainTileSurfaces.Sand1, Ids.TerrainTileSurfaces.Sand2), new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Concrete))), surfacesEdgesSpec1, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/ConcreteIcon.png")));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.ConcreteReinforced, Proto.CreateStr(Ids.TerrainTileSurfaces.ConcreteReinforced, "Reinforced surface", translationComment: str), maintenanceScale, orThrow1.WithQuantity(1), true, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.ConcreteReinforced))), edgesSpec1, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/ReinforcedConcreteIcon.png")));
      TileSurfacesEdgesSpec edgesSpec2 = new TileSurfacesEdgesSpec("Assets/Base/Terrain/Surfaces/BricksEdges/A_albedo.png", "Assets/Base/Terrain/Surfaces/BricksEdges/A_normals.png", "Assets/Base/Terrain/Surfaces/BricksEdges/A_smoothmetal.png", "Assets/Base/Terrain/Surfaces/BricksEdges/B_albedo.png", "Assets/Base/Terrain/Surfaces/BricksEdges/B_normals.png", "Assets/Base/Terrain/Surfaces/BricksEdges/B_smoothmetal.png", "Assets/Base/Terrain/Surfaces/BricksEdges/C_albedo.png", "Assets/Base/Terrain/Surfaces/BricksEdges/C_normals.png", "Assets/Base/Terrain/Surfaces/BricksEdges/C_smoothmetal.png", "Assets/Base/Terrain/Surfaces/BricksEdges/D_albedo.png", "Assets/Base/Terrain/Surfaces/BricksEdges/D_normals.png", "Assets/Base/Terrain/Surfaces/BricksEdges/D_smoothmetal.png");
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Bricks, Proto.CreateStr(Ids.TerrainTileSurfaces.Bricks, "Brick surface", translationComment: str), maintenanceScale, orThrow2.WithQuantity(1), true, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Bricks))), edgesSpec2, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/BricksIcon.png")));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Cobblestone, Proto.CreateStr(Ids.TerrainTileSurfaces.Cobblestone, "Cobblestone", translationComment: str), maintenanceScale, orThrow1.WithQuantity(1), true, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Cobblestone))), surfacesEdgesSpec1, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/CobblestoneIcon.png")));
      Proto.Str strings1 = new Proto.Str(Loc.Str("Stone_TerrainSurface", "Stone surface", str));
      TileSurfacesEdgesSpec from1 = createFrom(surfacesEdgesSpec1, "Assets/Base/Terrain/Surfaces/Sand1Edges/A-albedo.png", "Assets/Base/Terrain/Surfaces/Sand1Edges/B-albedo.png", "Assets/Base/Terrain/Surfaces/Sand1Edges/C-albedo.png", "Assets/Base/Terrain/Surfaces/Sand1Edges/D-albedo.png");
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Sand1, strings1, maintenanceScale, orThrow1.WithQuantity(1), true, ImmutableArray.Create<Proto.ID>(IdsCore.TerrainTileSurfaces.DefaultConcrete, Ids.TerrainTileSurfaces.Sand2), new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Sand1))), from1, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/Sand1Icon.png")));
      TileSurfacesEdgesSpec from2 = createFrom(surfacesEdgesSpec1, "Assets/Base/Terrain/Surfaces/Sand2Edges/A-albedo.png", "Assets/Base/Terrain/Surfaces/Sand2Edges/B-albedo.png", "Assets/Base/Terrain/Surfaces/Sand2Edges/C-albedo.png", "Assets/Base/Terrain/Surfaces/Sand2Edges/D-albedo.png");
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Sand2, strings1, maintenanceScale, orThrow1.WithQuantity(1), true, ImmutableArray.Create<Proto.ID>(IdsCore.TerrainTileSurfaces.DefaultConcrete, Ids.TerrainTileSurfaces.Sand1), new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Sand2))), from2, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/Sand2Icon.png")));
      Proto.Str strings2 = new Proto.Str(Loc.Str("Metal_TerrainSurface", "Metal surface", str));
      TileSurfacesEdgesSpec surfacesEdgesSpec2 = new TileSurfacesEdgesSpec("Assets/Base/Terrain/Surfaces/MetalEdges/A-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdges/A-normals.png", "Assets/Base/Terrain/Surfaces/MetalEdges/A-smoothmetal.png", "Assets/Base/Terrain/Surfaces/MetalEdges/B-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdges/B-normals.png", "Assets/Base/Terrain/Surfaces/MetalEdges/B-smoothmetal.png", "Assets/Base/Terrain/Surfaces/MetalEdges/C-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdges/C-normals.png", "Assets/Base/Terrain/Surfaces/MetalEdges/C-smoothmetal.png", "Assets/Base/Terrain/Surfaces/MetalEdges/D-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdges/D-normals.png", "Assets/Base/Terrain/Surfaces/MetalEdges/D-smoothmetal.png");
      TileSurfacesEdgesSpec from3 = createFrom(surfacesEdgesSpec2, "Assets/Base/Terrain/Surfaces/MetalEdgesRust/A-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdgesRust/B-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdgesRust/C-albedo.png", "Assets/Base/Terrain/Surfaces/MetalEdgesRust/D-albedo.png");
      TileSurfaceTextureSpec textureSpec = new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Metal2)));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Metal2, strings2, maintenanceScale, orThrow3.WithQuantity(1), true, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(textureSpec, from3, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/Metal2Icon.png")));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Metal3, strings2, maintenanceScale, orThrow4.WithQuantity(1), true, ImmutableArray.Create<Proto.ID>(Ids.TerrainTileSurfaces.Metal4), new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Metal3))), surfacesEdgesSpec2, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/Metal3Icon.png")));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Metal4, strings2, maintenanceScale, orThrow4.WithQuantity(1), true, ImmutableArray.Create<Proto.ID>(Ids.TerrainTileSurfaces.Metal3), new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Metal4))), surfacesEdgesSpec2, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/Metal4Icon.png")));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Metal1, strings2, maintenanceScale, orThrow4.WithQuantity(1), true, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(getFields(typeof (Assets.Base.Terrain.Surfaces.Metal1))), surfacesEdgesSpec2, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/Metal1Icon.png")));
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.Gold, Proto.CreateStr(Ids.TerrainTileSurfaces.Gold, "Golden surface", translationComment: str), maintenanceScale, orThrow5.WithQuantity(8), true, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(textureSpec, from3, customIconPath: "Assets/Base/Terrain/Surfaces/Icons/GoldIcon.png"))).SetAvailability(false);
      prototypesDb.Add<TerrainTileSurfaceProto>(new TerrainTileSurfaceProto(Ids.TerrainTileSurfaces.SettlementPaths, Proto.Str.Empty, Percent.FromFloat(0.9f), ProductQuantity.None, false, ImmutableArray<Proto.ID>.Empty, new TerrainTileSurfaceProto.Gfx(new TileSurfaceTextureSpec(Enumerable.Repeat<string>("Assets/Base/Terrain/Surfaces/SettlementPaths/SettlementPath-256-albedo.png", 8).ToImmutableArray<string>(), Enumerable.Repeat<string>("Assets/Base/Terrain/Surfaces/SettlementPaths/SettlementPath-256-normals.png", 8).ToImmutableArray<string>(), Enumerable.Repeat<string>("Assets/Base/Terrain/Surfaces/SettlementPaths/SettlementPath-256-smoothmetal.png", 8).ToImmutableArray<string>()), surfacesEdgesSpec1)));

      static FieldInfo[] getFields(Type type)
      {
        return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
      }

      static TileSurfacesEdgesSpec createFrom(
        TileSurfacesEdgesSpec source,
        string albedoTextureEdgeFullPath,
        string albedoTextureEdgeHorizontalPath,
        string albedoTextureEdgeVerticalPath,
        string albedoTextureEdgeCornersPath)
      {
        return new TileSurfacesEdgesSpec(albedoTextureEdgeFullPath, source.NormalsTextureEdgeFullPath, source.SmoothMetalTextureEdgeFullPath, albedoTextureEdgeHorizontalPath, source.NormalsTextureEdgeHorizontalPath, source.SmoothMetalTextureEdgeHorizontalPath, albedoTextureEdgeVerticalPath, source.NormalsTextureEdgeVerticalPath, source.SmoothMetalTextureEdgeVerticalPath, albedoTextureEdgeCornersPath, source.NormalsTextureEdgeCornersPath, source.SmoothMetalTextureEdgeCornersPath);
      }
    }

    public TerrainTileSurfacesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
