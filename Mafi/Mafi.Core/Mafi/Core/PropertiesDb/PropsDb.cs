// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropsDb
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  /// <summary>
  /// See <see cref="T:Mafi.Core.PropertiesDb.IPropertiesDb" />.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class PropsDb : IPropertiesDb, IPropertiesDbInternal
  {
    [NewInSaveVersion(140, null, null, typeof (ProtosDb), null)]
    private readonly ProtosDb m_protosDb;
    [NewInSaveVersion(140, null, null, typeof (UnlockedProtosDb), null)]
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    [NewInSaveVersion(140, null, null, typeof (IEntitiesManager), null)]
    private readonly IEntitiesManager m_entitiesManager;
    private readonly Dict<string, IProperty> m_properties;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PropsDb(
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IEntitiesManager entitiesManager,
      IGameLoopEvents gameLoop)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_properties = new Dict<string, IProperty>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_entitiesManager = entitiesManager;
      foreach (PropertyProto propertyProto in protosDb.All<PropertyProto>())
      {
        IProperty property = propertyProto.CreateProperty((IPropertiesDbInternal) this);
        this.m_properties.Add(property.Id, property);
      }
      gameLoop.RegisterNewGameCreated((object) this, new Action(this.newGameCreated));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelf(DependencyResolver resolver)
    {
      foreach (PropertyProto propertyProto in resolver.Resolve<ProtosDb>().All<PropertyProto>())
      {
        if (!this.m_properties.ContainsKey(propertyProto.PropertyId))
        {
          IProperty property = propertyProto.CreateProperty((IPropertiesDbInternal) this);
          this.m_properties.Add(property.Id, property);
        }
      }
      foreach (IProperty property in this.m_properties.Values)
        this.updateProtosOnChange(property, true);
    }

    private void newGameCreated()
    {
      foreach (IProperty property in this.m_properties.Values)
        this.updateProtosOnChange(property, false);
    }

    private void updateProtosOnChange(IProperty property, bool isGameLoad)
    {
      if (!this.m_protosDb.PropertyIdsToTrack.Contains(property.Id))
        return;
      foreach (Proto proto in this.m_protosDb.All())
      {
        if (proto is IProtoWithPropertiesUpdate propertiesUpdate)
        {
          bool isAvailable = proto.IsAvailable;
          propertiesUpdate.OnPropertyUpdated(property);
          if (!isGameLoad && isAvailable != proto.IsAvailable)
            this.m_unlockedProtosDb.OnProtoAvailabilityChanged(proto);
        }
      }
    }

    private Property<T> getPropertyContainer<T>(string idValue) where T : IEquatable<T>
    {
      return (Property<T>) this.m_properties[idValue];
    }

    public IProperty<T> GetProperty<T>(PropertyId<T> id) where T : IEquatable<T>
    {
      return (IProperty<T>) this.getPropertyContainer<T>(id.Value);
    }

    /// <summary>
    /// Throws exceptions if parsing fails! Only use in debug/testing code!
    /// </summary>
    private void setPropertyValueFromString(string propertyName, string value)
    {
      this.m_properties[propertyName].SetValueFromString(value);
    }

    void IPropertiesDbInternal.OnPropertyChanged(
      IProperty property,
      ImmutableArray<Type> subscribedEntities)
    {
      if (subscribedEntities.IsNotEmpty)
        this.m_entitiesManager.UpdatePropertiesForAllEntities(subscribedEntities);
      this.updateProtosOnChange(property, false);
    }

    public static void Serialize(PropsDb value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropsDb>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropsDb.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Dict<string, IProperty>.Serialize(this.m_properties, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
    }

    public static PropsDb Deserialize(BlobReader reader)
    {
      PropsDb propsDb;
      if (reader.TryStartClassDeserialization<PropsDb>(out propsDb))
        reader.EnqueueDataDeserialization((object) propsDb, PropsDb.s_deserializeDataDelayedAction);
      return propsDb;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<PropsDb>(this, "m_entitiesManager", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IEntitiesManager>() : (object) (IEntitiesManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<PropsDb>(this, "m_entitiesManager", typeof (IEntitiesManager), true);
      reader.SetField<PropsDb>(this, "m_properties", (object) Dict<string, IProperty>.Deserialize(reader));
      reader.RegisterResolvedMember<PropsDb>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<PropsDb>(this, "m_unlockedProtosDb", reader.LoadedSaveVersion >= 140 ? (object) UnlockedProtosDb.Deserialize(reader) : (object) (UnlockedProtosDb) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<PropsDb>(this, "m_unlockedProtosDb", typeof (UnlockedProtosDb), true);
      reader.RegisterInitAfterLoad<PropsDb>(this, "initSelf", InitPriority.High);
    }

    static PropsDb()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropsDb.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PropsDb) obj).SerializeData(writer));
      PropsDb.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PropsDb) obj).DeserializeData(reader));
    }
  }
}
