// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.TerrainPropsData
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
using Mafi.Core.Terrain.Props;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class TerrainPropsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      LooseProductProto orThrow = prototypesDb.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Rock);
      prototypesDb.Add<TerrainPropProto>(new TerrainPropProto(Ids.TerrainProps.Stone01, Proto.CreateStr(Ids.TerrainProps.Stone01, "Stone 1", translationComment: "HIDE"), TerrainPropBoundingShape.Circle, new RelTile2f(1.25.Tiles(), 1.25.Tiles()), Percent.Hundred, ThicknessTilesF.Zero, ThicknessTilesF.Half, orThrow.WithQuantity(10), true, ImmutableArray.Create<TerrainPropData.PropVariant>(new TerrainPropData.PropVariant(new Vector2i(0, 0), 1)), new TerrainPropProto.PropGfx("Assets/Base/Terrain/Props/Stone01.prefab", new Aabb(new Vector3f(-1.5.ToFix32(), -1.5.ToFix32(), -1.5.ToFix32()), new Vector3f(1.5.ToFix32(), 1.5.ToFix32(), 1.5.ToFix32())), "Assets/Base/Icons/Rock.svg", "Assets/Base/Terrain/Props/TerrainPropPreview.mat", true), true, true));
      prototypesDb.Add<TerrainPropProto>(new TerrainPropProto(Ids.TerrainProps.Stone02, Proto.CreateStr(Ids.TerrainProps.Stone02, "Stone 2", translationComment: "HIDE"), TerrainPropBoundingShape.Circle, new RelTile2f(1.25.Tiles(), 1.25.Tiles()), Percent.Hundred, ThicknessTilesF.Zero, ThicknessTilesF.Half, orThrow.WithQuantity(10), true, ImmutableArray.Create<TerrainPropData.PropVariant>(new TerrainPropData.PropVariant(new Vector2i(0, 0), 1)), new TerrainPropProto.PropGfx("Assets/Base/Terrain/Props/Stone02.prefab", new Aabb(new Vector3f(-1.5.ToFix32(), -1.5.ToFix32(), -1.5.ToFix32()), new Vector3f(1.5.ToFix32(), 1.5.ToFix32(), 1.5.ToFix32())), "Assets/Base/Icons/Rock.svg", "Assets/Base/Terrain/Props/TerrainPropPreview.mat", true), true, true));
      prototypesDb.Add<TerrainPropProto>(new TerrainPropProto(Ids.TerrainProps.Stone03, Proto.CreateStr(Ids.TerrainProps.Stone03, "Stone 3", translationComment: "HIDE"), TerrainPropBoundingShape.Circle, new RelTile2f(1.25.Tiles(), 1.25.Tiles()), Percent.Hundred, ThicknessTilesF.Zero, ThicknessTilesF.Half, orThrow.WithQuantity(10), true, ImmutableArray.Create<TerrainPropData.PropVariant>(new TerrainPropData.PropVariant(new Vector2i(0, 0), 1)), new TerrainPropProto.PropGfx("Assets/Base/Terrain/Props/Stone08.prefab", new Aabb(new Vector3f(-1.5.ToFix32(), -1.5.ToFix32(), -1.5.ToFix32()), new Vector3f(1.5.ToFix32(), 1.5.ToFix32(), 1.5.ToFix32())), "Assets/Base/Icons/Rock.svg", "Assets/Base/Terrain/Props/TerrainPropPreview.mat", true), true, true));
      prototypesDb.Add<TerrainPropProto>(new TerrainPropProto(Ids.TerrainProps.StoneSharp01, Proto.CreateStr(Ids.TerrainProps.StoneSharp01, "Stone 4", translationComment: "HIDE"), TerrainPropBoundingShape.Circle, new RelTile2f(1.25.Tiles(), 1.25.Tiles()), Percent.Hundred, ThicknessTilesF.Zero, ThicknessTilesF.Half, orThrow.WithQuantity(10), true, ImmutableArray.Create<TerrainPropData.PropVariant>(new TerrainPropData.PropVariant(new Vector2i(0, 0), 1)), new TerrainPropProto.PropGfx("Assets/Base/Terrain/Props/Stone09.prefab", new Aabb(new Vector3f(-1.5.ToFix32(), -1.5.ToFix32(), -1.5.ToFix32()), new Vector3f(1.5.ToFix32(), 1.5.ToFix32(), 1.5.ToFix32())), "Assets/Base/Icons/Rock.svg", "Assets/Base/Terrain/Props/TerrainPropPreview.mat", true), true, true));
      prototypesDb.Add<TerrainPropProto>(new TerrainPropProto(Ids.TerrainProps.BushSmall, Proto.CreateStr(Ids.TerrainProps.BushSmall, "Bush small", translationComment: "HIDE"), TerrainPropBoundingShape.Circle, new RelTile2f(0.4.Tiles(), 0.4.Tiles()), Percent.Hundred, ThicknessTilesF.Zero, ThicknessTilesF.Half, ProductQuantity.None, true, Enumerable.Range(0, 16).Select<int, TerrainPropData.PropVariant>((Func<int, TerrainPropData.PropVariant>) (i => new TerrainPropData.PropVariant(new Vector2i(i % 4, i / 4), 4))).ToImmutableArray<TerrainPropData.PropVariant>(), new TerrainPropProto.PropGfx("Assets/Base/Terrain/Props/Bush02.prefab", new Aabb(new Vector3f(-0.5.ToFix32(), -0.5.ToFix32(), -0.0.ToFix32()), new Vector3f(0.5.ToFix32(), 0.5.ToFix32(), 1.0.ToFix32())), "EMPTY", "Assets/Base/Terrain/Props/TerrainPropPreviewCutOut.mat"), doesNotBlocksVehicles: true));
      prototypesDb.Add<TerrainPropProto>(new TerrainPropProto(Ids.TerrainProps.BushMedium, Proto.CreateStr(Ids.TerrainProps.BushMedium, "Bush medium", translationComment: "HIDE"), TerrainPropBoundingShape.Circle, new RelTile2f(1.0.Tiles(), 1.0.Tiles()), Percent.Hundred, ThicknessTilesF.Zero, ThicknessTilesF.Half, ProductQuantity.None, true, Enumerable.Range(0, 16).Select<int, TerrainPropData.PropVariant>((Func<int, TerrainPropData.PropVariant>) (i => new TerrainPropData.PropVariant(new Vector2i(i % 4, i / 4), 4))).ToImmutableArray<TerrainPropData.PropVariant>(), new TerrainPropProto.PropGfx("Assets/Base/Terrain/Props/Bush-4x4.prefab", new Aabb(new Vector3f(-1.5.ToFix32(), -1.5.ToFix32(), -0.0.ToFix32()), new Vector3f(1.5.ToFix32(), 1.5.ToFix32(), 4.0.ToFix32())), "EMPTY", "Assets/Base/Terrain/Props/TerrainPropPreviewCutOut.mat")));
    }

    public TerrainPropsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
