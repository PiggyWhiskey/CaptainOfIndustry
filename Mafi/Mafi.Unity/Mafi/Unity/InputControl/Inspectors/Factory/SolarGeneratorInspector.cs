// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.SolarGeneratorInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Machines;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class SolarGeneratorInspector : 
    EntityInspector<SolarElectricityGenerator, SolarGeneratorWindowView>
  {
    private readonly IResolver m_resolver;

    public SolarGeneratorInspector(InspectorContext inspectorContext, IResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_resolver = resolver;
    }

    protected override SolarGeneratorWindowView GetView()
    {
      return this.m_resolver.Instantiate<SolarGeneratorWindowView>((object) this);
    }
  }
}
