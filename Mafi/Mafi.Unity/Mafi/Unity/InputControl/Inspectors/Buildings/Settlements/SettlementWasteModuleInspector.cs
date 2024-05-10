// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.Settlements.SettlementWasteModuleInspector
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
  internal class SettlementWasteModuleInspector : 
    EntityInspector<SettlementWasteModule, SettlementWasteModuleWindowView>
  {
    private readonly SettlementWasteModuleWindowView m_windowView;

    public SettlementWasteModuleInspector(InspectorContext inspectorContext)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new SettlementWasteModuleWindowView(this);
    }

    protected override SettlementWasteModuleWindowView GetView() => this.m_windowView;
  }
}
