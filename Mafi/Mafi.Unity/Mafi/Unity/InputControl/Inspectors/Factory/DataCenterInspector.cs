// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.DataCenterInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class DataCenterInspector : EntityInspector<DataCenter, DataCenterWindowView>
  {
    private readonly DataCenterWindowView m_windowView;

    public DataCenterInspector(
      InspectorContext inspectorContext,
      IAssetTransactionManager assetTransactions,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      ImmutableArray<ServerRackProto> immutableArray = protosDb.All<ServerRackProto>().ToImmutableArray<ServerRackProto>();
      this.m_windowView = new DataCenterWindowView(this, assetTransactions, immutableArray);
    }

    protected override DataCenterWindowView GetView() => this.m_windowView;

    public void AddRack(ServerRackProto proto)
    {
      bool flag = NotMappedShortcuts.IsBuildMultiple();
      this.InputScheduler.ScheduleInputCmd<DataCenterToggleRackCmd>(new DataCenterToggleRackCmd(this.SelectedEntity, proto, flag ? 8 : 1));
    }

    public void SellRack(ServerRackProto proto)
    {
      bool flag = NotMappedShortcuts.IsBuildMultiple();
      this.InputScheduler.ScheduleInputCmd<DataCenterToggleRackCmd>(new DataCenterToggleRackCmd(this.SelectedEntity, proto, flag ? -8 : -1));
    }
  }
}
