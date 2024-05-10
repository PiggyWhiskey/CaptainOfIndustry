// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.QuickTradesTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.World;
using Mafi.Core.World.QuickTrade;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class QuickTradesTab : Tab
  {
    private readonly WorldMapManager m_worldMapManager;
    private readonly InspectorContext m_context;

    internal QuickTradesTab(WorldMapManager worldMapManager, InspectorContext context)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("QuickTrades");
      this.m_worldMapManager = worldMapManager;
      this.m_context = context;
    }

    protected override void BuildUi()
    {
      QuickTradeView objectToPlace = new QuickTradeView((IUiElement) this, this.Builder, this.m_context.InputScheduler, (IAvailableProductsProvider) new ProductsAvailableInStorage(this.m_context.AssetsManager), (float) this.AvailableWidth, (Func<IIndexable<QuickTradeProvider>>) (() => this.m_worldMapManager.AllQuickTrades));
      this.AddUpdater(objectToPlace.Updater);
      objectToPlace.PutToTopOf<QuickTradeView>((IUiElement) this, 0.0f);
      objectToPlace.SizeChanged += (Action<IUiElement>) (x => this.SetHeight<QuickTradesTab>(x.GetHeight()));
      this.SetWidth<QuickTradesTab>((float) this.AvailableWidth);
    }
  }
}
