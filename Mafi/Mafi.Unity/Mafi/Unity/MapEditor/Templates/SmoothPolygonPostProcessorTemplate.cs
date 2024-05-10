// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.SmoothPolygonPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.PostProcessors;
using Mafi.Core.Prototypes;
using Mafi.Numerics;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class SmoothPolygonPostProcessorTemplate : 
    TerrainFeatureTemplateFactoryBase<PolygonSmoothPostProcessor>
  {
    public override string Name => "Smooth region";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.TerrainSculpting;

    public override Option<string> IconAssetPath
    {
      get => (Option<string>) "Assets/Unity/UserInterface/Toolbar/Smooth.svg";
    }

    public override float Order => 2f;

    public override PolygonSmoothPostProcessor CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      PolygonSmoothPostProcessor.Configuration initialConfig = new PolygonSmoothPostProcessor.Configuration()
      {
        Polygon = new Polygon2fMutable(),
        TransitionDistance = 10.Tiles(),
        SortingPriorityAdjustment = 100
      };
      Vector2f vector2f = position.Xy.Vector2f;
      initialConfig.Polygon.Initialize((IEnumerable<Vector2f>) new \u003C\u003Ez__ReadOnlyArray<Vector2f>(new Vector2f[4]
      {
        vector2f + new Vector2f((Fix32) -15, (Fix32) -15),
        vector2f + new Vector2f((Fix32) 15, (Fix32) -15),
        vector2f + new Vector2f((Fix32) 15, (Fix32) 15),
        vector2f + new Vector2f((Fix32) -15, (Fix32) 15)
      }));
      return new PolygonSmoothPostProcessor(initialConfig);
    }

    public SmoothPolygonPostProcessorTemplate()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
