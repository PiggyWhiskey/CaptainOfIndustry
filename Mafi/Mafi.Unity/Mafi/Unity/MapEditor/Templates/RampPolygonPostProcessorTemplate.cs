// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.RampPolygonPostProcessorTemplate
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
  public class RampPolygonPostProcessorTemplate : 
    TerrainFeatureTemplateFactoryBase<PolygonRampPostProcessor>
  {
    public override string Name => "Ramp";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.TerrainSculpting;

    public override Option<string> IconAssetPath
    {
      get => (Option<string>) "Assets/Unity/UserInterface/Toolbar/Ramp.svg";
    }

    public override float Order => 1f;

    public override PolygonRampPostProcessor CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      PolygonRampPostProcessor.Configuration initialConfig = new PolygonRampPostProcessor.Configuration()
      {
        Polygon = new Polygon3fMutable(clampZMinMax: 2048),
        TransitionDistance = 10.Tiles(),
        SortingPriorityAdjustment = 100
      };
      Vector3f vector3f = position.Vector3f;
      initialConfig.Polygon.Initialize((IEnumerable<Vector3f>) new \u003C\u003Ez__ReadOnlyArray<Vector3f>(new Vector3f[4]
      {
        vector3f + new Vector2f((Fix32) -15, (Fix32) -15),
        vector3f + new Vector2f((Fix32) 15, (Fix32) -15),
        vector3f + new Vector2f((Fix32) 15, (Fix32) 15),
        vector3f + new Vector2f((Fix32) -15, (Fix32) 15)
      }));
      return new PolygonRampPostProcessor(initialConfig);
    }

    public RampPolygonPostProcessorTemplate()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
