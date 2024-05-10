// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ResearchLabInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Research;
using Mafi.Unity.InputControl.Research;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ResearchLabInspector : EntityInspector<Mafi.Core.Buildings.ResearchLab.ResearchLab, ResearchLabWindowView>
  {
    private readonly ResearchLabWindowView m_windowView;

    public ResearchLabInspector(
      InspectorContext inspectorContext,
      ResearchManager researchManager,
      ResearchController resController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new ResearchLabWindowView(this, researchManager, (Action) (() => inspectorContext.InputMgr.ActivateNewController((IUnityInputController) resController)));
    }

    protected override ResearchLabWindowView GetView() => this.m_windowView;
  }
}
