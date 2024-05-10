// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.RainwaterHarvesterInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.RainwaterHarvesters;
using Mafi.Core.Environment;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RainwaterHarvesterInspector : 
    EntityInspector<RainwaterHarvester, RainwaterHarvesterWindowView>
  {
    private readonly RainwaterHarvesterWindowView m_windowView;

    public RainwaterHarvesterInspector(
      InspectorContext inspectorContext,
      WeatherManager weatherManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new RainwaterHarvesterWindowView(this, weatherManager);
    }

    protected override RainwaterHarvesterWindowView GetView() => this.m_windowView;
  }
}
