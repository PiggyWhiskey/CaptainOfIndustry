// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellInjectionPumpProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  public sealed class WellInjectionPumpProtoBuilder : MachineProtoBuilder
  {
    public WellInjectionPumpProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(registrator);
    }

    public WellInjectionPumpProtoBuilder.State Start(string name, MachineProto.ID machineId)
    {
      return new WellInjectionPumpProtoBuilder.State(this, machineId, name);
    }

    public class State : MachineProtoBuilder.StateBase<WellInjectionPumpProtoBuilder.State>
    {
      private Option<LooseProductProto> m_terrainProductRequired;

      public State(WellInjectionPumpProtoBuilder builder, MachineProto.ID protoId, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((MachineProtoBuilder) builder, protoId, name, "pump name");
      }

      [MustUseReturnValue]
      public WellInjectionPumpProtoBuilder.State SetRequiredTerrainProduct(ProductProto.ID productId)
      {
        this.m_terrainProductRequired = (Option<LooseProductProto>) this.Builder.ProtosDb.GetOrThrow<LooseProductProto>((Proto.ID) productId);
        TerrainMaterialProto terrainMaterialProto = this.Builder.ProtosDb.All<TerrainMaterialProto>().FirstOrDefault<TerrainMaterialProto>((Func<TerrainMaterialProto, bool>) (x => x.MinedProduct == this.m_terrainProductRequired));
        if (!((Proto) terrainMaterialProto != (Proto) null))
          return this;
        return this.ShowTerrainMaterialsOnCreation(terrainMaterialProto);
      }

      public WellInjectionPumpProto BuildAndAdd()
      {
        return this.AddToDb<WellInjectionPumpProto>(new WellInjectionPumpProto(this.ProtoId, this.Strings, this.LayoutOrThrow, this.Costs, this.Electricity, this.ValueOrThrow<LooseProductProto>(this.m_terrainProductRequired, "Terrain product has to be set."), this.BuffersMultiplier, this.AnimationParams.ToImmutableArray(), this.CreateMachineGfx()));
      }
    }
  }
}
