// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Prototypes.TerrainEditorMenuCategoriesData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Prototypes
{
  internal class TerrainEditorMenuCategoriesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.Hidden, Option<string>.None, "Hidden", 0.0f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.TerrainFeatures, (Option<string>) "Assets/Unity/MapEditor/Icons/Mountain.svg", "Terrain features", 10f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.TerrainSculpting, (Option<string>) "Assets/Unity/UserInterface/Toolbar/SculptTerrain.svg", "Terrain sculpting", 20f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.MineableResources, (Option<string>) "Assets/Unity/UserInterface/Toolbar/Mine.svg", "Mineable resources", 30f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.VirtualResources, (Option<string>) "Assets/Unity/UserInterface/Toolbar/Liquid.svg", "Liquid resources", 40f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.Plants, (Option<string>) "Assets/Unity/MapEditor/Icons/Forest.svg", "Plant & surface zones", 50f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.PlaceProps, (Option<string>) "Assets/Unity/UserInterface/Toolbar/PlaceDetail.svg", "Individual trees & rocks", 60f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.RemoveProps, (Option<string>) "Assets/Unity/UserInterface/Toolbar/PlaceDetail-Remove.svg", "Remove trees & rocks", 61f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.Special, (Option<string>) "Assets/Unity/UserInterface/Toolbar/MapPin.svg", "Special", 70f));
      prototypesDb.Add<TerrainEditorMenuCategoryProto>(new TerrainEditorMenuCategoryProto(IdsUnity.TerrainFeatureTemplates.CheckPlacement, (Option<string>) "Assets/Unity/UserInterface/Toolbar/CheckPlacement.svg", "Test entity placement", 80f));
    }

    public TerrainEditorMenuCategoriesData()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
