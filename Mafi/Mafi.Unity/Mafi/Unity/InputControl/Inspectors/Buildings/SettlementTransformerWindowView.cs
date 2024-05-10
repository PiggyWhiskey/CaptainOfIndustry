// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.SettlementTransformerWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Settlements;
using Mafi.Unity.InputControl.Population;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class SettlementTransformerWindowView : StaticEntityInspectorBase<SettlementTransformer>
  {
    private readonly DependencyResolver m_resolver;
    private readonly SettlementTransformerInspector m_controller;

    protected override SettlementTransformer Entity => this.m_controller.SelectedEntity;

    public SettlementTransformerWindowView(
      SettlementTransformerInspector controller,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_resolver = resolver;
      this.m_controller = controller.CheckNotNull<SettlementTransformerInspector>();
      this.SetWindowOffsetGroup(ItemDetailWindowView.WindowOffsetGroup.LargeScreen);
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      SettlementWindow element = this.m_resolver.Instantiate<SettlementWindow>();
      element.SetSettlementProvider((Func<Settlement>) (() => this.Entity.Settlement));
      element.BuildUi(this.Builder, (IUiElement) this);
      this.AttachSidePanel((IWindow) element);
      element.Show();
    }
  }
}
