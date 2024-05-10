// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.ReplaceMaterialFeatureTemplateFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  public class ReplaceMaterialFeatureTemplateFactory : ITerrainFeatureTemplateFactory
  {
    private readonly ProtosDb m_protosDb;

    public ReplaceMaterialFeatureTemplateFactory(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public IEnumerable<ITerrainFeatureTemplate> GetTemplates()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<ITerrainFeatureTemplate>) new ReplaceMaterialFeatureTemplateFactory.\u003CGetTemplates\u003Ed__2(-2)
      {
        \u003C\u003E4__this = this
      };
    }
  }
}
