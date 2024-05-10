// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.NoMaintenanceProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GenerateSerializer(false, null, 0)]
  public class NoMaintenanceProvider : IEntityMaintenanceProvidersFactory, IEntityMaintenanceProvider
  {
    public static readonly NoMaintenanceProvider Instance;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MaintenanceCosts Costs { get; private set; }

    public MaintenanceStatus Status { get; private set; }

    public Upoints RepairCost => Upoints.Zero;

    public bool CanWork() => true;

    public bool ShouldSlowDown() => false;

    private NoMaintenanceProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCosts\u003Ek__BackingField = new MaintenanceCosts(VirtualProductProto.Phantom, PartialQuantity.Zero);
      // ISSUE: reference to a compiler-generated field
      this.\u003CStatus\u003Ek__BackingField = new MaintenanceStatus(false, PartialQuantity.Zero, PartialQuantity.Zero, Percent.Hundred, (Fix32) 0);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public IEntityMaintenanceProvider CreateFor(IMaintainedEntity entity)
    {
      return (IEntityMaintenanceProvider) NoMaintenanceProvider.Instance;
    }

    public void SetCurrentMaintenanceTo(Percent percent)
    {
    }

    public void SetExtraMultiplierProperty(IProperty<Percent> property)
    {
    }

    public void SetDynamicExtraMultiplier(Percent percent)
    {
    }

    public void DecreaseBy(Percent percent)
    {
    }

    public void RefreshMaintenanceCost()
    {
    }

    public static void Serialize(NoMaintenanceProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NoMaintenanceProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NoMaintenanceProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      MaintenanceCosts.Serialize(this.Costs, writer);
      MaintenanceStatus.Serialize(this.Status, writer);
    }

    public static NoMaintenanceProvider Deserialize(BlobReader reader)
    {
      NoMaintenanceProvider maintenanceProvider;
      if (reader.TryStartClassDeserialization<NoMaintenanceProvider>(out maintenanceProvider))
        reader.EnqueueDataDeserialization((object) maintenanceProvider, NoMaintenanceProvider.s_deserializeDataDelayedAction);
      return maintenanceProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Costs = MaintenanceCosts.Deserialize(reader);
      this.Status = MaintenanceStatus.Deserialize(reader);
    }

    static NoMaintenanceProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NoMaintenanceProvider.Instance = new NoMaintenanceProvider();
      NoMaintenanceProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NoMaintenanceProvider) obj).SerializeData(writer));
      NoMaintenanceProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NoMaintenanceProvider) obj).DeserializeData(reader));
    }
  }
}
