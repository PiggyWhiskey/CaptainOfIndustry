// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.Settlements.SettlementFoodModuleInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Settlements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings.Settlements
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class SettlementFoodModuleInspector : 
    EntityInspector<SettlementFoodModule, SettlementFoodModuleWindowView>
  {
    private readonly SettlementFoodModuleWindowView m_windowView;

    public SettlementFoodModuleInspector(
      InspectorContext inspectorContext,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new SettlementFoodModuleWindowView(this, resolver);
    }

    protected override SettlementFoodModuleWindowView GetView() => this.m_windowView;
  }
}
