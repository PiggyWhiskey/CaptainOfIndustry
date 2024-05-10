// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.SurfaceDecalPopupProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class SurfaceDecalPopupProvider : PopupProviderBase<TerrainTileSurfaceDecalProto>
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly Lyst<IRecipeForUi> m_unlockedRecipes;
    private readonly Lyst<ProductProto> m_unlockedProducts;

    public SurfaceDecalPopupProvider(UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_unlockedRecipes = new Lyst<IRecipeForUi>();
      this.m_unlockedProducts = new Lyst<ProductProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
    }

    protected override void PopulateView(
      MenuPopupView view,
      TerrainTileSurfaceDecalProto proto,
      bool isForResearch)
    {
      view.SetTitle((LocStrFormatted) proto.Strings.Name);
      view.SetDescription("Paint decals on existing surfaces such as concrete.");
    }
  }
}
