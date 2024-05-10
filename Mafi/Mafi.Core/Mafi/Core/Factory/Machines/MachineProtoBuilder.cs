// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.MachineProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  public class MachineProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public MachineProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public MachineProtoBuilder.MachineProtoBuilderStateBase Start(
      string name,
      MachineProto.ID machineId,
      string translationComment = "name of a machine")
    {
      return new MachineProtoBuilder.MachineProtoBuilderStateBase(this, machineId, name, translationComment);
    }

    public class StateBase<T> : LayoutEntityBuilderState<T> where T : MachineProtoBuilder.StateBase<T>
    {
      protected readonly MachineProto.ID ProtoId;
      protected int? BuffersMultiplier;
      protected Option<MachineProto> NextTier;
      protected bool UseAllRecipesAtStart;
      protected Lyst<AnimationParams> AnimationParams;
      protected readonly Lyst<ParticlesParams> ParticlesParams;
      protected readonly Lyst<EmissionParams> EmissionParams;
      protected Option<string> MachineSoundPrefabPath;
      protected ColorRgba Color;
      protected string InstancedRenderingAnimationProtoSwap;
      protected Dict<string, string> instancedRenderingAnimationMaterialSwap;
      protected bool HasSign;
      protected int? EmissionWhenWorking;
      protected bool m_isWasteDisposal;
      protected bool m_disableLogisticsByDefault;
      protected Computing Computing;
      protected Upoints? BoostCost;

      public StateBase(
        MachineProtoBuilder builder,
        MachineProto.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.AnimationParams = new Lyst<AnimationParams>();
        this.ParticlesParams = new Lyst<ParticlesParams>();
        this.EmissionParams = new Lyst<EmissionParams>();
        this.MachineSoundPrefabPath = (Option<string>) Option.None;
        this.Color = ColorRgba.Empty;
        this.instancedRenderingAnimationMaterialSwap = new Dict<string, string>();
        this.Computing = Computing.Zero;
        this.BoostCost = new Upoints?(MachineProto.BOOST_COST);
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (StaticEntityProto.ID) protoId, name, translationComment);
        this.ProtoId = protoId;
      }

      [MustUseReturnValue]
      public override T Description(string description, string explanation = "short description of a machine")
      {
        return base.Description(description, explanation);
      }

      /// <summary>
      /// Sets internal buffers size multiplier. Default is 1 for transport and automatic for logistics.
      /// 
      /// Note: It should not be needed to change this unless you have some super rare products to transport.
      /// </summary>
      [MustUseReturnValue]
      public T SetBuffersMultiplier(int buffersMultiplier)
      {
        this.BuffersMultiplier = new int?(buffersMultiplier.CheckPositive());
        return (T) this;
      }

      /// <summary>Sets electricity consumption. Default is 0.</summary>
      [MustUseReturnValue]
      public T SetElectricityConsumption(Electricity electricity)
      {
        this.Electricity = electricity;
        return (T) this;
      }

      /// <summary>Sets computing consumption. Default is 0.</summary>
      [MustUseReturnValue]
      public T SetComputingConsumption(Computing computing)
      {
        this.Computing = computing;
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetNextTier(MachineProto.ID nextTierId)
      {
        this.NextTier = (Option<MachineProto>) this.Builder.ProtosDb.GetOrThrow<MachineProto>((Proto.ID) nextTierId);
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetAsWasteDisposal()
      {
        this.m_isWasteDisposal = true;
        return (T) this;
      }

      [MustUseReturnValue]
      public T DisableLogisticsByDefault()
      {
        this.m_disableLogisticsByDefault = true;
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetNextTier(MachineProto nextTier)
      {
        this.NextTier = (Option<MachineProto>) nextTier.CheckNotNull<MachineProto>();
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetAnimationParams(AnimationParams animParams)
      {
        this.AnimationParams.Add(animParams);
        return (T) this;
      }

      [MustUseReturnValue]
      public T AddParticleParams(ParticlesParams particlesParams)
      {
        this.ParticlesParams.Add(particlesParams);
        return (T) this;
      }

      [MustUseReturnValue]
      public T AddEmissionParams(EmissionParams emissionParams)
      {
        this.EmissionParams.Add(emissionParams);
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetMachineSound(string machineSoundPrefabPath)
      {
        this.MachineSoundPrefabPath = (Option<string>) machineSoundPrefabPath;
        return (T) this;
      }

      [MustUseReturnValue]
      public T UseAllRecipesAtStartOrAfterUnlock()
      {
        this.UseAllRecipesAtStart = true;
        return (T) this;
      }

      [MustUseReturnValue]
      public new T SetMaterialColor(ColorRgba color)
      {
        this.Color = color;
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetEmissionWhenWorking(int emissionValue)
      {
        this.EmissionWhenWorking = new int?(emissionValue);
        return (T) this;
      }

      [MustUseReturnValue]
      public T DisableBoost()
      {
        this.BoostCost = new Upoints?();
        return (T) this;
      }

      [MustUseReturnValue]
      public T SetProtoToCopyAnimationsFrom(MachineProto.ID protoId)
      {
        this.InstancedRenderingAnimationProtoSwap = protoId.ToString();
        return (T) this;
      }

      [MustUseReturnValue]
      public T AddMaterialSwapForAnimationsLoad(string currentMaterial, string newMaterial)
      {
        this.instancedRenderingAnimationMaterialSwap.AddAndAssertNew(currentMaterial, newMaterial);
        return (T) this;
      }

      [MustUseReturnValue]
      public T AddSign()
      {
        this.HasSign = true;
        return (T) this;
      }

      protected MachineProto.Gfx CreateMachineGfx()
      {
        string prefabPath = string.IsNullOrEmpty(this.PrefabPath) ? "EMPTY" : this.PrefabPath;
        RelTile3f prefabOrigin = this.PrefabOrigin;
        Option<string> customIconPath1 = this.CustomIconPath;
        ImmutableArray<ParticlesParams> immutableArray1 = this.ParticlesParams.ToImmutableArray();
        ImmutableArray<EmissionParams> immutableArray2 = this.EmissionParams.ToImmutableArray();
        Option<string> machineSoundPrefabPath1 = this.MachineSoundPrefabPath;
        ColorRgba color1 = this.Color;
        LayoutEntityProto.VisualizedLayers? nullable = new LayoutEntityProto.VisualizedLayers?(this.VisualizedResourcesList);
        bool renderingEnabled1 = this.InstancedRenderingEnabled;
        bool renderingEnabled2 = this.SemiInstancedRenderingEnabled;
        ImmutableArray<string> renderingExcludedObjects = this.InstancedRenderingExcludedObjects;
        string animationProtoSwap = this.InstancedRenderingAnimationProtoSwap;
        IReadOnlyDictionary<string, string> animationMaterialSwap = (IReadOnlyDictionary<string, string>) this.instancedRenderingAnimationMaterialSwap;
        bool hasSign = this.HasSign;
        ImmutableArray<ToolbarCategoryProto> categoriesOrThrow = this.GetCategoriesOrThrow();
        RelTile3f prefabOffset = prefabOrigin;
        Option<string> customIconPath2 = customIconPath1;
        ImmutableArray<ParticlesParams> particlesParams = immutableArray1;
        ImmutableArray<EmissionParams> emissionsParams = immutableArray2;
        Option<string> machineSoundPrefabPath2 = machineSoundPrefabPath1;
        int num1 = renderingEnabled1 ? 1 : 0;
        int num2 = renderingEnabled2 ? 1 : 0;
        ImmutableArray<string> instancedRenderingExcludedObjects = renderingExcludedObjects;
        string instancedRenderingAnimationProtoSwap = animationProtoSwap;
        int num3 = hasSign ? 1 : 0;
        IReadOnlyDictionary<string, string> instancedRenderingAnimationMaterialSwap = animationMaterialSwap;
        ColorRgba color2 = color1;
        LayoutEntityProto.VisualizedLayers? visualizedLayers = nullable;
        return new MachineProto.Gfx(prefabPath, categoriesOrThrow, prefabOffset, customIconPath2, particlesParams, emissionsParams, machineSoundPrefabPath2, num1 != 0, num2 != 0, instancedRenderingExcludedObjects, instancedRenderingAnimationProtoSwap, num3 != 0, instancedRenderingAnimationMaterialSwap, color2, visualizedLayers);
      }
    }

    public class MachineProtoBuilderStateBase : 
      MachineProtoBuilder.StateBase<MachineProtoBuilder.MachineProtoBuilderStateBase>
    {
      public MachineProtoBuilderStateBase(
        MachineProtoBuilder builder,
        MachineProto.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, protoId, name, translationComment);
      }

      public MachineProto BuildAndAdd()
      {
        return this.AddToDb<MachineProto>(new MachineProto(this.ProtoId, this.Strings, this.LayoutOrThrow, this.Costs, this.Electricity, this.Computing, this.BuffersMultiplier, this.NextTier, this.UseAllRecipesAtStart, this.AnimationParams.ToImmutableArray(), this.CreateMachineGfx(), this.EmissionWhenWorking, this.m_isWasteDisposal, this.m_disableLogisticsByDefault, this.BoostCost));
      }
    }
  }
}
