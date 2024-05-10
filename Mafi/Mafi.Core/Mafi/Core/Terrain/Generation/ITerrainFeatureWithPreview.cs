// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainFeatureWithPreview
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainFeatureWithPreview : IEditableTerrainFeature, ITerrainFeatureBase
  {
    /// <summary>
    /// Called once when the feature is selected. This can be used to populate config with actions that will
    /// show as buttons in the UI.
    /// </summary>
    void InitializePreview(IResolver resolver);

    bool TryGetPreviews(
      IResolver resolver,
      int timeBudgetMs,
      out IEnumerable<ITerrainFeaturePreview> previews);
  }
}
