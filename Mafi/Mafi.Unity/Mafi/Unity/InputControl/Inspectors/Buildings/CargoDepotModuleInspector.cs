// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CargoDepotModuleInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Economy;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Unity.InputControl.World;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CargoDepotModuleInspector : 
    EntityInspector<CargoDepotModule, CargoDepotModuleWindowView>
  {
    private readonly CargoDepotModuleWindowView m_windowView;

    public CargoDepotModuleInspector(
      InspectorContext inspectorContext,
      ContractsManager contractsManager,
      SourceProductsAnalyzer sourceProductsAnalyzer,
      TradeWindowController tradeWindowController,
      WorldMapManager worldMapManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new CargoDepotModuleWindowView(this, contractsManager, sourceProductsAnalyzer, tradeWindowController, worldMapManager);
    }

    protected override CargoDepotModuleWindowView GetView() => this.m_windowView;
  }
}
