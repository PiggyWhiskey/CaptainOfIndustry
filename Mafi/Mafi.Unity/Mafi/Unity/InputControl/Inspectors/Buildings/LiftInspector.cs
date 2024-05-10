// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.LiftInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Lifts;
using Mafi.Core.Input;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class LiftInspector : EntityInspector<Lift, LiftWindowView>
  {
    public LiftInspector(InspectorContext inspectorContext)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
    }

    protected override LiftWindowView GetView() => new LiftWindowView(this);

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.Context.ShortcutsManager.IsDown(this.Context.ShortcutsManager.Flip))
        return base.InputUpdate(inputScheduler);
      this.InputScheduler.ScheduleInputCmd<ReverseLiftCmd>(new ReverseLiftCmd(this.SelectedEntity.Id));
      return true;
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.Context.Highlighter.RemoveHighlight((IRenderedEntity) this.SelectedEntity);
      this.Context.Highlighter.Highlight((IRenderedEntity) this.SelectedEntity, ColorRgba.Yellow);
    }
  }
}
