// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TileTerrainData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public struct TileTerrainData
  {
    public HeightTilesF SurfaceHeight;
    /// <summary>
    /// Products under the surface from bottom to top. This is a struct, use ref when needed!
    /// </summary>
    public LystStruct<TerrainMaterialThicknessSlim> Products;
    public TreeData TreeData;
    public TerrainPropData TerrainPropData;

    public static void SerializeToCache(TileTerrainData data, BlobWriter writer)
    {
      HeightTilesF.Serialize(data.SurfaceHeight, writer);
      LystStruct<TerrainMaterialThicknessSlim>.Serialize(data.Products, writer);
      bool isValid1 = data.TreeData.IsValid;
      writer.WriteBool(isValid1);
      if (isValid1)
        TreeData.Serialize(data.TreeData, writer);
      bool isValid2 = data.TerrainPropData.IsValid;
      writer.WriteBool(isValid2);
      if (!isValid2)
        return;
      TerrainPropData.Serialize(data.TerrainPropData, writer);
    }

    public static TileTerrainData DeserializeFromCache(BlobReader reader)
    {
      TileTerrainData tileTerrainData = new TileTerrainData();
      tileTerrainData.SurfaceHeight = HeightTilesF.Deserialize(reader);
      tileTerrainData.Products = LystStruct<TerrainMaterialThicknessSlim>.Deserialize(reader);
      if (reader.ReadBool())
        tileTerrainData.TreeData = TreeData.Deserialize(reader);
      if (reader.ReadBool())
        tileTerrainData.TerrainPropData = TerrainPropData.Deserialize(reader);
      return tileTerrainData;
    }
  }
}
