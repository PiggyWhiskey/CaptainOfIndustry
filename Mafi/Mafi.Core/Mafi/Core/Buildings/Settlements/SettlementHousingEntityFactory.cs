// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementHousingEntityFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class SettlementHousingEntityFactory : 
    IFactory<SettlementHousingModuleProto, TileTransform, StaticEntity>
  {
    private readonly DefaultStaticEntityFactory m_defaultFactory;
    private readonly IRandom m_random;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SettlementHousingEntityFactory(
      DefaultStaticEntityFactory defaultFactory,
      RandomProvider randomProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_defaultFactory = defaultFactory;
      this.m_random = randomProvider.GetSimRandomFor((object) this);
    }

    public StaticEntity Create(SettlementHousingModuleProto proto, TileTransform transform)
    {
      transform = new TileTransform(transform.Position, new Rotation90(this.m_random.NextInt(4)), this.m_random.NextBool());
      return this.m_defaultFactory.Create((StaticEntityProto) proto, transform);
    }

    public static void Serialize(SettlementHousingEntityFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementHousingEntityFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementHousingEntityFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IRandom>(this.m_random);
    }

    public static SettlementHousingEntityFactory Deserialize(BlobReader reader)
    {
      SettlementHousingEntityFactory housingEntityFactory;
      if (reader.TryStartClassDeserialization<SettlementHousingEntityFactory>(out housingEntityFactory))
        reader.EnqueueDataDeserialization((object) housingEntityFactory, SettlementHousingEntityFactory.s_deserializeDataDelayedAction);
      return housingEntityFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<SettlementHousingEntityFactory>(this, "m_defaultFactory", typeof (DefaultStaticEntityFactory), true);
      reader.SetField<SettlementHousingEntityFactory>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
    }

    static SettlementHousingEntityFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementHousingEntityFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SettlementHousingEntityFactory) obj).SerializeData(writer));
      SettlementHousingEntityFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SettlementHousingEntityFactory) obj).DeserializeData(reader));
    }
  }
}
