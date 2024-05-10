// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.OreSortingPlantInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.OreSorting;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class OreSortingPlantInspector : 
    EntityInspector<OreSortingPlant, OreSortingPlantWindowView>
  {
    private readonly OreSortingPlantWindowView m_windowView;

    public OreSortingPlantInspector(
      InspectorContext inspectorContext,
      BuildingsAssigner buildingsAssigner)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new OreSortingPlantWindowView(this);
      this.SetBuildingsAssigner(buildingsAssigner);
    }

    protected override OreSortingPlantWindowView GetView() => this.m_windowView;
  }
}
