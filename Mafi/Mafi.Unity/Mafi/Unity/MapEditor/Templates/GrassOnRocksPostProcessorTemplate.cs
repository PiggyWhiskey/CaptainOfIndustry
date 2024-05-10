// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.GrassOnRocksPostProcessorTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class GrassOnRocksPostProcessorTemplate : 
    GlobalTerrainFeatureTemplateFactoryBase<GrassOnRocksPostProcessor>
  {
    private readonly ProtosDb m_protosDb;

    public override string Name => "Grass on rock";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Hidden;

    public GrassOnRocksPostProcessorTemplate(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public override GrassOnRocksPostProcessor CreateNewFeature(IRandom rng)
    {
      return new GrassOnRocksPostProcessor(new GrassOnRocksPostProcessor.Configuration()
      {
        GrassMaterialProto = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass)
      });
    }
  }
}
