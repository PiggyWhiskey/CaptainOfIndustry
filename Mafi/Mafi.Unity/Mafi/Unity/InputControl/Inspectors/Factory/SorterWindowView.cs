// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.SorterWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Factory.Sorters;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class SorterWindowView : StaticEntityInspectorBase<Sorter>
  {
    private readonly SorterInspector m_controller;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private ProtosFilterEditor<ProductProto> m_filterView;

    protected override Sorter Entity => this.m_controller.SelectedEntity;

    public SorterWindowView(SorterInspector controller, UnlockedProtosDb unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<SorterInspector>();
      this.m_unlockedProtosDb = unlockedProtosDb.CheckNotNull<UnlockedProtosDb>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddClearButton(new Action(((EntityInspector<Sorter, SorterWindowView>) this.m_controller).Clear));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ProductsToFilter);
      this.m_filterView = new ProtosFilterEditor<ProductProto>(this.Builder, (IWindowWithInnerWindowsSupport) this, this.ItemsContainer, new Action<ProductProto>(this.m_controller.RemoveFilteredProduct), new Action<ProductProto>(this.m_controller.AddFilteredProduct), (Func<IEnumerable<ProductProto>>) (() => this.m_unlockedProtosDb.FilterUnlocked<ProductProto>((IEnumerable<ProductProto>) this.m_controller.SelectedEntity.Prototype.AssignableProducts)), (Func<IEnumerable<ProductProto>>) (() => (IEnumerable<ProductProto>) this.m_controller.SelectedEntity.FilteredProducts));
      this.m_filterView.SetTextToShowWhenEmpty(string.Format("({0})", (object) Tr.ProductsToFilter__None));
      this.SetWidth(this.m_filterView.GetRequiredWidth());
      updaterBuilder.Observe<ProductProto>((Func<IReadOnlyCollection<ProductProto>>) (() => (IReadOnlyCollection<ProductProto>) this.Entity.FilteredProducts), (ICollectionComparator<ProductProto, IReadOnlyCollection<ProductProto>>) CompareByCount<ProductProto>.Instance).Do(new Action<Lyst<ProductProto>>(this.m_filterView.UpdateFilteredProtos));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
