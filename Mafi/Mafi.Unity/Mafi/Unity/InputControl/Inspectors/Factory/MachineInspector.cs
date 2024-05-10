﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.MachineInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Factory.Machines;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class MachineInspector : EntityInspector<Machine, MachineWindowView>
  {
    private readonly IResolver m_resolver;

    public MachineInspector(InspectorContext inspectorContext, IResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_resolver = resolver;
    }

    protected override MachineWindowView GetView()
    {
      return this.m_resolver.Instantiate<MachineWindowView>((object) this);
    }
  }
}
