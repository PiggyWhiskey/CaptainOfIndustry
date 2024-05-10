// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.CustomTreesPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.PostProcessors;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class CustomTreesPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<CustomTreesPostProcessor>
  {
    public override string Name => "Manually placed trees";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public override CustomTreesPostProcessor CreateNewFeature(IRandom rng)
    {
      return new CustomTreesPostProcessor(new CustomTreesPostProcessor.Configuration());
    }

    public CustomTreesPostProcessorTemplate()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
