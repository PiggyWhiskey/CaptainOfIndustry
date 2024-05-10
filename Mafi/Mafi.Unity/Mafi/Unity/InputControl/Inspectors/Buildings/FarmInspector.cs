﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.FarmInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Farms;
using Mafi.Core.PropertiesDb;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class FarmInspector : EntityInspector<Farm, FarmWindowView>
  {
    private readonly FarmWindowView m_windowView;

    public FarmInspector(InspectorContext inspectorContext, IPropertiesDb propsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new FarmWindowView(this, propsDb);
    }

    protected override FarmWindowView GetView() => this.m_windowView;
  }
}
