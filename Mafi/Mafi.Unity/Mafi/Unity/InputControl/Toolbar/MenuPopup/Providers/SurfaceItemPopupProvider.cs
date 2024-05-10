// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.SurfaceItemPopupProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.EntitiesMenu;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class SurfaceItemPopupProvider : IPopupProvider
  {
    public SurfaceItemPopupProvider()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    /// <summary>Type of item supported by this provider.</summary>
    public Type SupportedType => typeof (TerrainTileSurfaceMenuItem);

    /// <summary>
    /// Populates the given popup view with the given item. The item's type must be assignable to <see cref="P:Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.SurfaceItemPopupProvider.SupportedType" />.
    /// </summary>
    public void PopulateView(MenuPopupView view, object item, bool isForResearch)
    {
      if (!(item is TerrainTileSurfaceMenuItem tileSurfaceMenuItem) || tileSurfaceMenuItem.Proto.IsNone)
      {
        view.SetTitle((LocStrFormatted) Tr.ClearSurface__Title);
        view.SetDescription((LocStrFormatted) Tr.ClearSurface__Tooltip);
      }
      else
      {
        view.SetTitle((LocStrFormatted) tileSurfaceMenuItem.Proto.Value.Strings.Name);
        view.SetDescription((LocStrFormatted) Tr.PlaceSurface__Tooltip);
        view.SetVehiclesMaintenanceMult(tileSurfaceMenuItem.Proto.Value.MaintenanceScale);
        view.SetPricePerTile(new AssetValue(tileSurfaceMenuItem.Proto.Value.CostPerTile));
      }
    }
  }
}
