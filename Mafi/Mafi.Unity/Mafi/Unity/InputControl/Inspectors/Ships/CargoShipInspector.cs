// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Ships.CargoShipInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.World;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Ships
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CargoShipInspector : EntityInspector<CargoShip, CargoShipView>
  {
    private readonly WorldMapCargoManager m_cargoManager;

    public CargoShipInspector(InspectorContext inspectorContext, WorldMapCargoManager cargoManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_cargoManager = cargoManager;
    }

    protected override CargoShipView GetView() => new CargoShipView(this, this.m_cargoManager);

    public override void RenderUpdate(GameTime gameTime)
    {
      if (this.SelectedEntity.IsDestroyed)
        this.InputMgr.DeactivateController((IUnityInputController) this.Controller);
      else
        base.RenderUpdate(gameTime);
    }
  }
}
