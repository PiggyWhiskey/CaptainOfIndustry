// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CaptainOfficeInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Offices;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Population.Edicts;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CaptainOfficeInspector : EntityInspector<CaptainOffice, CaptainOfficeWindowView>
  {
    private readonly CaptainOfficeWindowView m_windowView;

    public CaptainOfficeInspector(
      InspectorContext inspectorContext,
      EdictsManager edictsManager,
      IInputScheduler inputScheduler,
      UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new CaptainOfficeWindowView(this, edictsManager, inputScheduler, unlockedProtosDb);
      inspectorContext.InputMgr.RegisterGlobalShortcut((Func<ShortcutsManager, KeyBindings>) (x => x.ToggleCaptainsOffice), (Action) (() =>
      {
        if (inspectorContext.MainController.ActiveInspector.HasValue && inspectorContext.MainController.ActiveInspector.Value is CaptainOfficeInspector)
        {
          inspectorContext.InputMgr.DeactivateController((IUnityInputController) inspectorContext.MainController);
          Assert.That<Option<IEntityInspector>>(inspectorContext.MainController.ActiveInspector).IsNone<IEntityInspector>();
        }
        else
        {
          CaptainOffice entity = inspectorContext.EntitiesManager.GetAllEntitiesOfType<CaptainOffice>().FirstOrDefault<CaptainOffice>();
          if (entity == null)
            return;
          inspectorContext.MainController.TryActivateFor(inspectorContext.InputMgr, (IRenderedEntity) entity);
          inspectorContext.InputMgr.ActivateNewController((IUnityInputController) inspectorContext.MainController);
        }
      }));
    }

    protected override CaptainOfficeWindowView GetView() => this.m_windowView;
  }
}
