// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.DataCenterProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable enable
namespace Mafi.Core.Factory.Datacenters
{
  public sealed class DataCenterProtoBuilder : IProtoBuilder
  {
    public 
    #nullable disable
    ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public DataCenterProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public DataCenterProtoBuilder.State Start(string name, DataCenterProto.ID dataCenterId)
    {
      return new DataCenterProtoBuilder.State(this, dataCenterId, name);
    }

    public class State : LayoutEntityBuilderState<DataCenterProtoBuilder.State>
    {
      private readonly DataCenterProto.ID m_protoId;
      private int? m_rackCapacity;
      private Option<Func<int, DataCenterProto.RackPosition>> m_rackPositionGenerator;
      private 
      #nullable enable
      ProductProto? m_coolantIn;
      private ProductProto? m_coolantOut;
      private Quantity? m_coolantBufferCapacity;
      private ImmutableArray<char>? m_coolantInPorts;
      private ImmutableArray<char>? m_coolantOutPorts;

      public State(
      #nullable disable
      DataCenterProtoBuilder builder, DataCenterProto.ID protoId, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (StaticEntityProto.ID) protoId, name);
        this.m_protoId = protoId;
      }

      /// <summary>Sets internal buffers size multiplier. Default is 1.</summary>
      [MustUseReturnValue]
      public DataCenterProtoBuilder.State SetRacksCapacity(int racksCapacity)
      {
        this.m_rackCapacity = new int?(racksCapacity.CheckPositive());
        return this;
      }

      /// <summary>
      /// Sets a function used to generate position of racks relative to the datacenter. The function has to return
      /// positions for rack indices in range [0, RacksCapacity).
      /// </summary>
      [MustUseReturnValue]
      public DataCenterProtoBuilder.State SetRackPositionsGenerator(
        Func<int, DataCenterProto.RackPosition> rackPositionGenerator)
      {
        this.m_rackPositionGenerator = (Option<Func<int, DataCenterProto.RackPosition>>) rackPositionGenerator.CheckNotNull<Func<int, DataCenterProto.RackPosition>>();
        return this;
      }

      [MustUseReturnValue]
      public DataCenterProtoBuilder.State SetCoolants(
        ProductProto.ID coolantInId,
        ProductProto.ID coolantOutId,
        Quantity coolantBufferCapacity)
      {
        this.m_coolantIn = this.Builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) coolantInId);
        this.m_coolantOut = this.Builder.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) coolantOutId);
        this.m_coolantBufferCapacity = new Quantity?(coolantBufferCapacity);
        return this;
      }

      [MustUseReturnValue]
      public DataCenterProtoBuilder.State SetPortsSpec(
        ImmutableArray<char> coolantInPorts,
        ImmutableArray<char> coolantOutPorts)
      {
        this.m_coolantInPorts = new ImmutableArray<char>?(coolantInPorts);
        this.m_coolantOutPorts = new ImmutableArray<char>?(coolantOutPorts);
        return this;
      }

      private DataCenterProto.Gfx createGraphics()
      {
        if (string.IsNullOrEmpty(this.PrefabPath))
          return DataCenterProto.Gfx.Empty;
        int valueOrDefault = this.m_rackCapacity.GetValueOrDefault();
        ImmutableArrayBuilder<DataCenterProto.RackPosition> immutableArrayBuilder = new ImmutableArrayBuilder<DataCenterProto.RackPosition>(valueOrDefault);
        if (this.m_rackPositionGenerator.HasValue)
        {
          for (int i = 0; i < valueOrDefault; ++i)
            immutableArrayBuilder[i] = this.m_rackPositionGenerator.Value(i);
        }
        return new DataCenterProto.Gfx(this.PrefabPath, this.PrefabOrigin, this.CustomIconPath, immutableArrayBuilder.GetImmutableArrayAndClear(), this.GetCategoriesOrThrow());
      }

      public DataCenterProto BuildAndAdd()
      {
        return this.AddToDb<DataCenterProto>(new DataCenterProto(this.m_protoId, this.Strings, this.LayoutOrThrow, this.Costs, this.ValueOrThrow<int>(this.m_rackCapacity, "Racks capacity must be set!"), this.ValueOrThrow<ProductProto>((Option<ProductProto>) this.m_coolantIn, "Coolant in must be set!"), this.ValueOrThrow<ProductProto>((Option<ProductProto>) this.m_coolantOut, "Coolant out must be set!"), this.ValueOrThrow<Quantity>(this.m_coolantBufferCapacity, "Coolant cap must be set!"), this.ValueOrThrow<ImmutableArray<char>>(this.m_coolantInPorts, "CoolantInPorts must be set!"), this.ValueOrThrow<ImmutableArray<char>>(this.m_coolantOutPorts, "CoolantOutPorts must be set!"), this.createGraphics()));
      }
    }
  }
}
