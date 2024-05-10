// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.MineableResourcesFeaturesTemplates
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  internal class MineableResourcesFeaturesTemplates : ITerrainFeatureTemplateFactory
  {
    private readonly ProtosDb m_protosDb;

    public MineableResourcesFeaturesTemplates(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    private KeyValuePair<Proto.ID, ITerrainFeatureTemplate> createHillFeature(
      Proto.ID materialId,
      HillFeatureTemplate.HillFeatureType type,
      bool isNotResource = false)
    {
      LooseProductProto minedProduct = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(materialId).MinedProduct;
      return Make.Kvp<Proto.ID, ITerrainFeatureTemplate>(materialId, (ITerrainFeatureTemplate) new HillFeatureTemplate(minedProduct.Strings.Name.TranslatedString, type, this.m_protosDb, (Option<string>) minedProduct.IconPath, new Proto.ID?(materialId), new Proto.ID?(IdsUnity.TerrainFeatureTemplates.MineableResources), isNotResource: isNotResource));
    }

    public IEnumerable<ITerrainFeatureTemplate> GetTemplates()
    {
      return this.GetTemplatesForResources().Select<KeyValuePair<Proto.ID, ITerrainFeatureTemplate>, ITerrainFeatureTemplate>((Func<KeyValuePair<Proto.ID, ITerrainFeatureTemplate>, ITerrainFeatureTemplate>) (x => x.Value));
    }

    public IEnumerable<KeyValuePair<Proto.ID, ITerrainFeatureTemplate>> GetTemplatesForResources()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<KeyValuePair<Proto.ID, ITerrainFeatureTemplate>>) new MineableResourcesFeaturesTemplates.\u003CGetTemplatesForResources\u003Ed__4(-2)
      {
        \u003C\u003E4__this = this
      };
    }
  }
}
