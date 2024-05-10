// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.IdsUnity
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public static class IdsUnity
  {
    public static class TerrainFeatureTemplates
    {
      public static readonly Proto.ID Hidden;
      public static readonly Proto.ID TerrainFeatures;
      public static readonly Proto.ID TerrainSculpting;
      public static readonly Proto.ID MineableResources;
      public static readonly Proto.ID VirtualResources;
      public static readonly Proto.ID Plants;
      public static readonly Proto.ID PlaceProps;
      public static readonly Proto.ID RemoveProps;
      public static readonly Proto.ID Special;
      public static readonly Proto.ID CheckPlacement;

      public static Proto.ID CreateID(string id) => new Proto.ID("TerEdCat_" + id);

      static TerrainFeatureTemplates()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        IdsUnity.TerrainFeatureTemplates.Hidden = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (Hidden));
        IdsUnity.TerrainFeatureTemplates.TerrainFeatures = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (TerrainFeatures));
        IdsUnity.TerrainFeatureTemplates.TerrainSculpting = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (TerrainSculpting));
        IdsUnity.TerrainFeatureTemplates.MineableResources = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (MineableResources));
        IdsUnity.TerrainFeatureTemplates.VirtualResources = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (VirtualResources));
        IdsUnity.TerrainFeatureTemplates.Plants = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (Plants));
        IdsUnity.TerrainFeatureTemplates.PlaceProps = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (PlaceProps));
        IdsUnity.TerrainFeatureTemplates.RemoveProps = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (RemoveProps));
        IdsUnity.TerrainFeatureTemplates.Special = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (Special));
        IdsUnity.TerrainFeatureTemplates.CheckPlacement = IdsUnity.TerrainFeatureTemplates.CreateID(nameof (CheckPlacement));
      }
    }
  }
}
