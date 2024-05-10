// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.TerrainFeatureTemplateFactoryBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain.Generation;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  public abstract class TerrainFeatureTemplateFactoryBase<T> : 
    TerrainFeatureTemplateBase<T>,
    ITerrainFeatureTemplateFactory
    where T : ITerrainFeatureBase
  {
    IEnumerable<ITerrainFeatureTemplate> ITerrainFeatureTemplateFactory.GetTemplates()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<ITerrainFeatureTemplate>) new TerrainFeatureTemplateFactoryBase<T>.\u003CMafi\u002DUnity\u002DMapEditor\u002DTemplates\u002DITerrainFeatureTemplateFactory\u002DGetTemplates\u003Ed__0(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    protected TerrainFeatureTemplateFactoryBase()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
