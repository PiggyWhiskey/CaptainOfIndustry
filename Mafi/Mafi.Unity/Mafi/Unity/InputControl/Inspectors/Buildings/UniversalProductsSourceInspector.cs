// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.UniversalProductsSourceInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class UniversalProductsSourceInspector : 
    EntityInspector<UniversalProductsSource, UniversalProductsSourceView>
  {
    private readonly UniversalProductsSourceView m_windowView;

    public UniversalProductsSourceInspector(InspectorContext inspectorContext, ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new UniversalProductsSourceView(this, protosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.Type != VirtualProductProto.ProductType)).OrderBy<ProductProto, string>((Func<ProductProto, string>) (x => x.Strings.Name.TranslatedString)).ToImmutableArray<ProductProto>());
    }

    protected override UniversalProductsSourceView GetView() => this.m_windowView;
  }
}
