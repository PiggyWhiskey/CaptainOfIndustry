// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.UniversalProductsSinkView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class UniversalProductsSinkView : StaticEntityInspectorBase<UniversalProductsSink>
  {
    private readonly UniversalProductsSinkInspector m_controller;

    protected override UniversalProductsSink Entity => this.m_controller.SelectedEntity;

    public UniversalProductsSinkView(UniversalProductsSinkInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
    }
  }
}
