// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.StackerInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class StackerInspector : EntityInspector<Stacker, StackerWindowView>
  {
    private readonly StackerWindowView m_view;

    public StackerInspector(
      InspectorContext inspectorContext,
      LinesFactory linesFactory,
      MbBasedEntitiesRenderer entitiesRenderer,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      ImmutableArray<ProductProto> immutableArray = protosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.CanBeOnTerrain)).Cast<ProductProto>().ToImmutableArray<ProductProto>();
      this.m_view = new StackerWindowView(this, linesFactory, entitiesRenderer, immutableArray);
    }

    protected override StackerWindowView GetView() => this.m_view;

    protected override void OnDeactivated()
    {
      this.m_view.OnDeactivated();
      base.OnDeactivated();
    }
  }
}
