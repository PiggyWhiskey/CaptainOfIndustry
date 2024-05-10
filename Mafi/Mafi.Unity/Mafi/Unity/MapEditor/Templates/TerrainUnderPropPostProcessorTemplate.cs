// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.TerrainUnderPropPostProcessorTemplate
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
  public class TerrainUnderPropPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<TerrainUnderPropsPostProcessor>
  {
    private readonly ProtosDb m_protosDb;

    public override string Name => "Terrain under props";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public TerrainUnderPropPostProcessorTemplate(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public override TerrainUnderPropsPostProcessor CreateNewFeature(IRandom rng)
    {
      return new TerrainUnderPropsPostProcessor(new TerrainUnderPropsPostProcessor.Configuration());
    }
  }
}
