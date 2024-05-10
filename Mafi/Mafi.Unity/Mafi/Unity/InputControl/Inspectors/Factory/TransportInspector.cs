// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.TransportInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Products;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TransportInspector : EntityInspector<Mafi.Core.Factory.Transports.Transport, TransportWindowView>
  {
    private readonly IAssetTransactionManager m_assetsManager;
    private readonly ProductsSlimIdManager m_productsSlimIdManager;

    public TransportInspector(
      InspectorContext inspectorContext,
      IAssetTransactionManager assetsManager,
      ProductsSlimIdManager productsSlimIdManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_assetsManager = assetsManager;
      this.m_productsSlimIdManager = productsSlimIdManager;
    }

    protected override TransportWindowView GetView()
    {
      return new TransportWindowView(this, this.m_assetsManager, this.m_productsSlimIdManager);
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.Context.ShortcutsManager.IsDown(this.Context.ShortcutsManager.Flip))
        return base.InputUpdate(inputScheduler);
      this.InputScheduler.ScheduleInputCmd<ReverseTransportCmd>(new ReverseTransportCmd(this.SelectedEntity.Id));
      return true;
    }
  }
}
