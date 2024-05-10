// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.ElectricityGeneratorFromMechPower
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class ElectricityGeneratorFromMechPower : 
    LayoutEntity,
    IElectricityGeneratingEntity,
    IElectricityGenerator,
    IEntity,
    IIsSafeAsHashKey,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IAnimatedEntity,
    IEntityWithSound,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ElectricityGeneratorFromMechPowerProto Prototype;
    private readonly IShaftBuffer m_shaftBuffer;

    public static void Serialize(ElectricityGeneratorFromMechPower value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ElectricityGeneratorFromMechPower>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ElectricityGeneratorFromMechPower.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteGeneric<IElectricityGeneratorRegistrator>(this.ElectricityGenerator);
      Electricity.Serialize(this.GeneratedElectricityThisTick, writer);
      writer.WriteGeneric<IShaftBuffer>(this.m_shaftBuffer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<ElectricityGeneratorFromMechPowerProto>(this.Prototype);
      MechPower.Serialize(this.UsedMechPowerThisTick, writer);
    }

    public static ElectricityGeneratorFromMechPower Deserialize(BlobReader reader)
    {
      ElectricityGeneratorFromMechPower generatorFromMechPower;
      if (reader.TryStartClassDeserialization<ElectricityGeneratorFromMechPower>(out generatorFromMechPower))
        reader.EnqueueDataDeserialization((object) generatorFromMechPower, ElectricityGeneratorFromMechPower.s_deserializeDataDelayedAction);
      return generatorFromMechPower;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentState = (MechPowerGeneratorFromProduct.State) reader.ReadInt();
      this.ElectricityGenerator = reader.ReadGenericAs<IElectricityGeneratorRegistrator>();
      this.GeneratedElectricityThisTick = Electricity.Deserialize(reader);
      reader.SetField<ElectricityGeneratorFromMechPower>(this, "m_shaftBuffer", (object) reader.ReadGenericAs<IShaftBuffer>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      reader.SetField<ElectricityGeneratorFromMechPower>(this, "Prototype", (object) reader.ReadGenericAs<ElectricityGeneratorFromMechPowerProto>());
      this.UsedMechPowerThisTick = MechPower.Deserialize(reader);
    }

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.Prototype.Graphics.SoundPrefabPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.SoundPrefabPath.Value, SoundSignificance.Normal));
      }
    }

    public bool IsSoundOn
    {
      get => this.IsEnabled && this.CurrentState == MechPowerGeneratorFromProduct.State.Working;
    }

    public override bool CanBePaused => true;

    public IElectricityGeneratorRegistrator ElectricityGenerator { get; private set; }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance
    {
      get => this.CurrentState != MechPowerGeneratorFromProduct.State.Working;
    }

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public Electricity MaxGenerationCapacity
    {
      get => !this.IsEnabled ? Electricity.Zero : this.Prototype.OutputElectricity;
    }

    public MechPower UsedMechPowerThisTick { get; private set; }

    public Electricity GeneratedElectricityThisTick { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public MechPowerGeneratorFromProduct.State CurrentState { get; private set; }

    public ElectricityGeneratorFromMechPower(
      EntityId id,
      ElectricityGeneratorFromMechPowerProto proto,
      TileTransform transform,
      EntityContext context,
      IElectricityGeneratorRegistratorFactory generatorRegistratorFactory,
      IShaftManager shaftManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.ElectricityGenerator = generatorRegistratorFactory.CreateAndRegisterFor((IElectricityGeneratingEntity) this, this.Prototype.GenerationPriority);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_shaftBuffer = shaftManager.GetOrCreateShaftBufferFor((IEntityWithPorts) this, proto.InputMechPower);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
    }

    public Electricity GetCurrentMaxGeneration(out bool canGenerate)
    {
      canGenerate = this.IsEnabled;
      if (!canGenerate)
      {
        this.UsedMechPowerThisTick = MechPower.Zero;
        this.CurrentState = this.Maintenance.Status.IsBroken ? MechPowerGeneratorFromProduct.State.Broken : MechPowerGeneratorFromProduct.State.Paused;
        this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
        return Electricity.Zero;
      }
      if (!this.m_shaftBuffer.Shaft.IsDefaultNoCapacityShaft)
        return this.Prototype.OutputElectricity;
      canGenerate = false;
      this.UsedMechPowerThisTick = MechPower.Zero;
      this.CurrentState = MechPowerGeneratorFromProduct.State.NoShaft;
      this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
      return Electricity.Zero;
    }

    public Electricity GenerateAsMuchAs(Electricity freeCapacity, Electricity currentMaxGeneration)
    {
      if (freeCapacity.IsNotPositive)
      {
        this.UsedMechPowerThisTick = MechPower.Zero;
        this.CurrentState = MechPowerGeneratorFromProduct.State.None;
        this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
        return Electricity.Zero;
      }
      this.UsedMechPowerThisTick = this.m_shaftBuffer.RemoveAsMuchAs(this.getRequiredMechPowerForElectricity(freeCapacity));
      if (this.UsedMechPowerThisTick.IsNotPositive)
      {
        this.CurrentState = MechPowerGeneratorFromProduct.State.NotEnoughInput;
        this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
        return Electricity.Zero;
      }
      Percent utilization;
      this.GeneratedElectricityThisTick = this.convertMechPowerToElectricity(this.UsedMechPowerThisTick, out utilization);
      this.CurrentState = MechPowerGeneratorFromProduct.State.Working;
      this.AnimationStatesProvider.Step(utilization, Percent.Zero);
      return this.GeneratedElectricityThisTick;
    }

    private Electricity convertMechPowerToElectricity(MechPower mechPower, out Percent utilization)
    {
      utilization = Percent.FromRatio(mechPower.Value, this.Prototype.InputMechPower.Value);
      if (utilization < this.Prototype.MinUtilization)
        return Electricity.Zero;
      utilization = (utilization - this.Prototype.MinUtilization) / (Percent.Hundred - this.Prototype.MinUtilization);
      return this.Prototype.OutputElectricity.ScaledBy(utilization);
    }

    private MechPower getRequiredMechPowerForElectricity(Electricity electricity)
    {
      return new MechPower(this.Prototype.MinUtilization.Lerp(Percent.Hundred, Percent.FromRatio(electricity.Value, this.Prototype.OutputElectricity.Value)).ApplyCeiled(this.Prototype.InputMechPower.Value));
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      Assert.Fail("Invalid operation.");
      return pq.Quantity;
    }

    static ElectricityGeneratorFromMechPower()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ElectricityGeneratorFromMechPower.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      ElectricityGeneratorFromMechPower.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
