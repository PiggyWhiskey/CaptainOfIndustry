// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.SorterInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Factory.Sorters;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class SorterInspector : EntityInspector<Sorter, SorterWindowView>
  {
    private readonly SorterWindowView m_windowView;

    public SorterInspector(InspectorContext inspectorContext, UnlockedProtosDb unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new SorterWindowView(this, unlockedProtosDb);
    }

    protected override SorterWindowView GetView() => this.m_windowView;

    public void AddFilteredProduct(ProductProto product)
    {
      if (this.SelectedEntity.FilteredProducts.Contains(product))
        return;
      this.toggleProduct(product);
    }

    public void RemoveFilteredProduct(ProductProto product) => this.toggleProduct(product);

    private void toggleProduct(ProductProto product)
    {
      this.InputScheduler.ScheduleInputCmd<SorterToggleProductCmd>(new SorterToggleProductCmd(this.SelectedEntity, product));
    }
  }
}
