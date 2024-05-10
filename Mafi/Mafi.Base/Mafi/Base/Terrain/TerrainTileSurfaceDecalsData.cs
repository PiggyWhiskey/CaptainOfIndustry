// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.TerrainTileSurfaceDecalsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Surfaces;
using System.IO;

#nullable disable
namespace Mafi.Base.Terrain
{
  public class TerrainTileSurfaceDecalsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb db = registrator.PrototypesDb;
      Proto.ID id1 = new Proto.ID("SurfaceDecalsCat_Lines");
      Proto.ID id2 = new Proto.ID("SurfaceDecalsCat_Hazard");
      Proto.ID id3 = new Proto.ID("SurfaceDecalsCat_LinesDouble");
      Proto.ID id4 = new Proto.ID("SurfaceDecalsCat_Areas");
      Proto.ID id5 = new Proto.ID("SurfaceDecalsCat_Symbols");
      Proto.ID id6 = new Proto.ID("SurfaceDecalsCat_Characters");
      string translationComment = "Title for category of decals - lines & symbols pained on surfaces";
      SurfaceDecalCategoryProto linesCat = db.Add<SurfaceDecalCategoryProto>(new SurfaceDecalCategoryProto(id1, Proto.CreateStr(id1, "Lines & arrows", translationComment: translationComment), 0.0f));
      SurfaceDecalCategoryProto linesDoubleCat = db.Add<SurfaceDecalCategoryProto>(new SurfaceDecalCategoryProto(id3, Proto.CreateStr(id3, "Double lines", translationComment: translationComment), 10f));
      SurfaceDecalCategoryProto hazardCat = db.Add<SurfaceDecalCategoryProto>(new SurfaceDecalCategoryProto(id2, Proto.CreateStr(id2, "Hazard", translationComment: translationComment), 20f));
      SurfaceDecalCategoryProto areasCat = db.Add<SurfaceDecalCategoryProto>(new SurfaceDecalCategoryProto(id4, Proto.CreateStr(id4, "Areas & borders", translationComment: translationComment), 30f));
      SurfaceDecalCategoryProto symbolsCat = db.Add<SurfaceDecalCategoryProto>(new SurfaceDecalCategoryProto(id5, Proto.CreateStr(id5, "Symbols", translationComment: translationComment), 40f));
      SurfaceDecalCategoryProto charsCat = db.Add<SurfaceDecalCategoryProto>(new SurfaceDecalCategoryProto(id6, Proto.CreateStr(id6, "Characters", translationComment: translationComment + " Contains letters and numbers."), 50f));
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Line1.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Line2.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Line3.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Line4.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Line5.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Line6.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/RoundLine2.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/RoundLine3.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/RoundLine4.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/RoundLine5.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Dashed1.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Dashed2.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Dashed3.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Dashed4.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Dashed5.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Arrow1.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Arrow2.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Arrow3.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Symbol1.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Symbol2.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Symbol3.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Symbol4.png");
      createLine("Assets/Base/Terrain/Surfaces/Decals/Lines/Symbol5.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double1.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double2.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double3.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double4.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double5.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double6.png");
      createDouble("Assets/Base/Terrain/Surfaces/Decals/Double/Double7.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill1.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill2.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill3.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill4.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill5.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill6.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Fill7.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Border1.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Border2.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Border3.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Border4.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Border5.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Corner1.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Corner2.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Corner3.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/Corner4.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundBorder3.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundBorder4.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundBorder5.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundCorner1.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundCorner2.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundCorner3.png");
      createArea("Assets/Base/Terrain/Surfaces/Decals/Areas/RoundCorner4.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard1.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard2.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard2Alt.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard3.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard4.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard5.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard6.png");
      createHazard("Assets/Base/Terrain/Surfaces/Decals/Hazard/Hazard7.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol01.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol02.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol03.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol04.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol05.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol06.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol07.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol08.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol09.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol10.png");
      createSymbol("Assets/Base/Terrain/Surfaces/Decals/Symbols/Symbol11.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num0.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num1.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num2.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num3.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num4.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num5.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num6.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num7.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num8.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Numbers/Num9.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/A.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/B.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/C.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/D.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/E.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/F.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/G.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/H.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/I.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/J.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/K.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/L.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/M.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/N.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/O.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/P.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/Q.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/R.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/S.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/T.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/U.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/V.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/W.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/X.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/Y.png");
      createCharacter("Assets/Base/Terrain/Surfaces/Decals/Alphabet/Z.png");

      void createArea(string pathToDecal) => createDecal(pathToDecal, "Area", areasCat);

      void createDouble(string pathToDecal) => createDecal(pathToDecal, "Double", linesDoubleCat);

      void createHazard(string pathToDecal) => createDecal(pathToDecal, "Hazard", hazardCat);

      void createSymbol(string pathToDecal) => createDecal(pathToDecal, "Symbol", symbolsCat);

      void createCharacter(string pathToDecal) => createDecal(pathToDecal, "Symbol", charsCat);

      void createLine(string pathToDecal) => createDecal(pathToDecal, "Lines", linesCat);

      void createDecal(string pathToDecal, string idPrefix, SurfaceDecalCategoryProto category = null)
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(pathToDecal);
        db.Add<TerrainTileSurfaceDecalProto>(new TerrainTileSurfaceDecalProto(new Proto.ID("Decal" + idPrefix + "_" + withoutExtension), Proto.Str.Empty, new TerrainTileSurfaceDecalProto.Gfx(pathToDecal, (Option<SurfaceDecalCategoryProto>) (category ?? linesCat))));
      }
    }

    public TerrainTileSurfaceDecalsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
