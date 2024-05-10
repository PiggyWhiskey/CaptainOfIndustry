// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.CustomPropsPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Collections;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class CustomPropsPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<CustomPropsPostProcessor>
  {
    public override string Name => "Manually placed props";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public override CustomPropsPostProcessor CreateNewFeature(IRandom rng)
    {
      return new CustomPropsPostProcessor(new CustomPropsPostProcessor.Configuration()
      {
        TerrainInfo = new Dict<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>()
        {
          {
            Ids.TerrainMaterials.Grass,
            new CustomPropsPostProcessor.TerrainInfoForMaterial()
            {
              BelowPropMaterial = new Proto.ID?(Ids.TerrainMaterials.DirtBare),
              PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Rock)
            }
          },
          {
            Ids.TerrainMaterials.GrassLush,
            new CustomPropsPostProcessor.TerrainInfoForMaterial()
            {
              BelowPropMaterial = new Proto.ID?(Ids.TerrainMaterials.DirtBare),
              PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Rock)
            }
          },
          {
            Ids.TerrainMaterials.ForestFloor,
            new CustomPropsPostProcessor.TerrainInfoForMaterial()
            {
              BelowPropMaterial = new Proto.ID?(Ids.TerrainMaterials.DirtBare),
              PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Rock)
            }
          },
          {
            Ids.TerrainMaterials.DirtBare,
            new CustomPropsPostProcessor.TerrainInfoForMaterial()
            {
              BelowPropMaterial = new Proto.ID?(Ids.TerrainMaterials.DirtBare),
              PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Rock)
            }
          },
          {
            Ids.TerrainMaterials.Sand,
            new CustomPropsPostProcessor.TerrainInfoForMaterial()
            {
              PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Limestone)
            }
          }
        }
      });
    }

    public CustomPropsPostProcessorTemplate()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
