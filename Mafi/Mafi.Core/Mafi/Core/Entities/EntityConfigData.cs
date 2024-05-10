// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityConfigData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>Temporary bag of properties to copy from entities.</summary>
  [ManuallyWrittenSerialization]
  public class EntityConfigData
  {
    private readonly EntityConfigData.Data m_data;
    private Option<Dict<string, ImmutableArray<byte>>> m_nonPersistentData;
    private readonly ConfigSerializationContext m_context;
    /// <summary>
    /// FYI: We don't remove config even if we don't find proto on load because we don't want to
    /// throw away blueprints just in case some mods are missing because blueprints are
    /// account wide and each save can have different mod setup.
    /// </summary>
    public readonly Option<Proto> Prototype;
    private EntityConfigData.EntityIdsHolder m_assignedOutputs;
    private EntityConfigData.EntityIdsHolder m_assignedInputs;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<Type> EntityType
    {
      get
      {
        return (Option<Type>) (this.Prototype.ValueOrNull is EntityProto valueOrNull ? valueOrNull.EntityType : (Type) null);
      }
    }

    /// <summary>
    /// Name of mod that registered the main proto. Null for base mod.
    /// </summary>
    public Option<string> ProtoModName
    {
      get => this.GetString(nameof (ProtoModName));
      set => this.SetString(nameof (ProtoModName), value);
    }

    internal Option<string> ProtoId => this.GetString("Prototype");

    /// <summary>
    /// Id of the original entity that this configs was made from.
    /// This is used to map entities assignments to the new IDs as they are recorded with the original ones.
    /// </summary>
    internal EntityId? OriginalEntityId
    {
      get => this.GetEntityId(nameof (OriginalEntityId));
      set => this.SetEntityId(nameof (OriginalEntityId), value);
    }

    public TileTransform? Transform
    {
      get
      {
        return this.Get<TileTransform>(nameof (Transform), EntityConfigData.\u003C\u003EO.\u003C0\u003E__Deserialize ?? (EntityConfigData.\u003C\u003EO.\u003C0\u003E__Deserialize = new Func<BlobReader, TileTransform>(TileTransform.Deserialize)));
      }
      set
      {
        this.Set<TileTransform>(nameof (Transform), value, EntityConfigData.\u003C\u003EO.\u003C1\u003E__Serialize ?? (EntityConfigData.\u003C\u003EO.\u003C1\u003E__Serialize = new Action<TileTransform, BlobWriter>(TileTransform.Serialize)));
      }
    }

    public Option<TransportTrajectory> Trajectory
    {
      get
      {
        return this.GetOption<TransportTrajectory>(nameof (Trajectory), (Func<BlobReader, TransportTrajectory>) (r => !this.Prototype.HasValue ? (TransportTrajectory) null : EntityConfigData.TrajectorySerializer.Deserialize((IProto) this.Prototype.Value, r)));
      }
      set
      {
        this.Set<TransportTrajectory>(nameof (Trajectory), value, EntityConfigData.\u003C\u003EO.\u003C2\u003E__Serialize ?? (EntityConfigData.\u003C\u003EO.\u003C2\u003E__Serialize = new Action<TransportTrajectory, BlobWriter>(EntityConfigData.TrajectorySerializer.Serialize)));
      }
    }

    public ImmutableArray<Tile2i>? Pillars
    {
      get
      {
        return this.GetArray<Tile2i>(nameof (Pillars), EntityConfigData.\u003C\u003EO.\u003C3\u003E__Deserialize ?? (EntityConfigData.\u003C\u003EO.\u003C3\u003E__Deserialize = new Func<BlobReader, Tile2i>(Tile2i.Deserialize)));
      }
      set
      {
        this.SetArray<Tile2i>(nameof (Pillars), value, EntityConfigData.\u003C\u003EO.\u003C4\u003E__Serialize ?? (EntityConfigData.\u003C\u003EO.\u003C4\u003E__Serialize = new Action<Tile2i, BlobWriter>(Tile2i.Serialize)));
      }
    }

    public EntityLogisticsMode? LogisticsInputMode
    {
      get
      {
        int? nullable = this.GetInt(nameof (LogisticsInputMode));
        return !nullable.HasValue ? new EntityLogisticsMode?() : new EntityLogisticsMode?((EntityLogisticsMode) nullable.Value);
      }
      set
      {
        this.SetInt(nameof (LogisticsInputMode), value.HasValue ? new int?((int) value.Value) : new int?());
      }
    }

    public EntityLogisticsMode? LogisticsOutputMode
    {
      get
      {
        int? nullable = this.GetInt(nameof (LogisticsOutputMode));
        return !nullable.HasValue ? new EntityLogisticsMode?() : new EntityLogisticsMode?((EntityLogisticsMode) nullable.Value);
      }
      set
      {
        this.SetInt(nameof (LogisticsOutputMode), value.HasValue ? new int?((int) value.Value) : new int?());
      }
    }

    public bool? IsLogisticsInputDisabled
    {
      get => this.GetBool(nameof (IsLogisticsInputDisabled));
      set => this.SetBool(nameof (IsLogisticsInputDisabled), value);
    }

    public bool? IsLogisticsOutputDisabled
    {
      get => this.GetBool(nameof (IsLogisticsOutputDisabled));
      set => this.SetBool(nameof (IsLogisticsOutputDisabled), value);
    }

    public int? GeneralPriority
    {
      get => this.GetInt(nameof (GeneralPriority));
      set => this.SetInt(nameof (GeneralPriority), value);
    }

    public int? ElectricityGenerationPriority
    {
      get => this.GetInt(nameof (ElectricityGenerationPriority));
      set => this.SetInt(nameof (ElectricityGenerationPriority), value);
    }

    public bool? IsElectricitySurplusGenerator
    {
      get => this.GetBool(nameof (IsElectricitySurplusGenerator));
      set => this.SetBool(nameof (IsElectricitySurplusGenerator), value);
    }

    public bool? IsElectricitySurplusConsumer
    {
      get => this.GetBool(nameof (IsElectricitySurplusConsumer));
      set => this.SetBool(nameof (IsElectricitySurplusConsumer), value);
    }

    public Option<string> CustomTitle
    {
      get => this.GetString(nameof (CustomTitle));
      set => this.SetString(nameof (CustomTitle), value);
    }

    public bool? IsPaused
    {
      get => this.GetBool(nameof (IsPaused));
      set => this.SetBool(nameof (IsPaused), value);
    }

    public bool? IsConstructionPaused
    {
      get => this.GetBoolNonPersistent(nameof (IsConstructionPaused));
      set => this.SetBoolNonPersistent(nameof (IsConstructionPaused), value);
    }

    public ImmutableArray<Proto>? AssignedVehicles
    {
      get => this.GetProtoArray<Proto>(nameof (AssignedVehicles), true);
      set => this.SetProtoArray<Proto>(nameof (AssignedVehicles), value);
    }

    public ImmutableArray<Option<ProductProto>>? MultipleBuffers
    {
      get => this.GetOptionProtoArray<ProductProto>(nameof (MultipleBuffers), false);
      set => this.SetOptionProtoArray<ProductProto>(nameof (MultipleBuffers), value);
    }

    public bool? NotifyOnFullBuffer
    {
      get => this.GetBool(nameof (NotifyOnFullBuffer));
      set => this.SetBool(nameof (NotifyOnFullBuffer), value);
    }

    public Percent? FuelImportUntilPercent
    {
      get => this.GetPercent(nameof (FuelImportUntilPercent));
      set => this.SetPercent(nameof (FuelImportUntilPercent), value);
    }

    public Percent? FuelExportFromPercent
    {
      get => this.GetPercent(nameof (FuelExportFromPercent));
      set => this.SetPercent(nameof (FuelExportFromPercent), value);
    }

    public ImmutableArray<RecipeProto>? Recipes
    {
      get => this.GetProtoArray<RecipeProto>(nameof (Recipes), true);
      set
      {
        if (!value.HasValue)
          this.m_data.StringArrays.Remove(nameof (Recipes));
        else
          this.SetProtoArray<RecipeProto>(nameof (Recipes), value);
      }
    }

    public bool? AllowNonAssignedOutput
    {
      get => this.GetBool(nameof (AllowNonAssignedOutput));
      set => this.SetBool(nameof (AllowNonAssignedOutput), value);
    }

    public ImmutableArray<EntityId>? AssignedOutputs
    {
      get => this.m_assignedOutputs.MappedIds;
      set => this.m_assignedOutputs.SetData(this, value);
    }

    public ImmutableArray<EntityId>? AssignedInputs
    {
      get => this.m_assignedInputs.MappedIds;
      set => this.m_assignedInputs.SetData(this, value);
    }

    public bool? NotifyOnLowReserve
    {
      get => this.GetBool(nameof (NotifyOnLowReserve));
      set => this.SetBool(nameof (NotifyOnLowReserve), value);
    }

    public ImmutableArray<KeyValuePair<string, ProductProto>>? AllowedProducts
    {
      get => this.GetProtoMapArray<ProductProto>(nameof (AllowedProducts), true);
      set => this.SetProtoMapArray<ProductProto>(nameof (AllowedProducts), value);
    }

    public EntityConfigData(
      EntityId entityId,
      IProto entityProto,
      ConfigSerializationContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedOutputs = new EntityConfigData.EntityIdsHolder(nameof (AssignedOutputs));
      this.m_assignedInputs = new EntityConfigData.EntityIdsHolder(nameof (AssignedInputs));
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_data = EntityConfigData.Data.GetNewInstance();
      this.m_context = context;
      switch (entityProto)
      {
        case TransportProto _:
        case TransportPillarProto _:
        case TreeProto _:
label_2:
          this.Prototype = (Option<Proto>) (entityProto as Proto);
          this.SetProto<Proto>(nameof (Prototype), this.Prototype);
          if (!(entityProto.Mod.Name != "Mafi-Base") || !(entityProto.Mod.Name != "Mafi-Core"))
            break;
          this.ProtoModName = (Option<string>) entityProto.Mod.Name;
          break;
        default:
          this.OriginalEntityId = new EntityId?(entityId);
          goto label_2;
      }
    }

    internal EntityConfigData(
      EntityConfigData.Data data,
      Option<Dict<string, ImmutableArray<byte>>> nonPersistentData,
      ConfigSerializationContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedOutputs = new EntityConfigData.EntityIdsHolder(nameof (AssignedOutputs));
      this.m_assignedInputs = new EntityConfigData.EntityIdsHolder(nameof (AssignedInputs));
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_data = data;
      this.m_nonPersistentData = nonPersistentData;
      this.m_context = context;
      this.Prototype = this.GetProto<Proto>(nameof (Prototype), false);
    }

    internal EntityConfigData(
      Proto customProto,
      EntityConfigData.Data data,
      Option<Dict<string, ImmutableArray<byte>>> nonPersistentData,
      ConfigSerializationContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedOutputs = new EntityConfigData.EntityIdsHolder(nameof (AssignedOutputs));
      this.m_assignedInputs = new EntityConfigData.EntityIdsHolder(nameof (AssignedInputs));
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_data = data;
      this.m_nonPersistentData = nonPersistentData;
      this.m_context = context;
      this.Prototype = (Option<Proto>) customProto;
      this.SetProto<Proto>(nameof (Prototype), customProto.CreateOption<Proto>());
    }

    private void initAfterLoad()
    {
      ReflectionUtils.SetField<EntityConfigData>(this, "Prototype", (object) this.GetProto<Proto>("Prototype", false));
    }

    public void MapEntitiesIds(Dict<EntityId, EntityId> map, Mafi.Collections.Set<EntityId> idsFailedToMap)
    {
      this.m_assignedInputs.MapGlobalIds(this, map, idsFailedToMap);
      this.m_assignedOutputs.MapGlobalIds(this, map, idsFailedToMap);
    }

    public void FinalizeAsBlueprint(Mafi.Collections.Set<EntityId> idsToKeep)
    {
      this.m_assignedInputs.FilterOutExternalIds(this, idsToKeep);
      this.m_assignedOutputs.FilterOutExternalIds(this, idsToKeep);
      this.m_nonPersistentData = (Option<Dict<string, ImmutableArray<byte>>>) Option.None;
    }

    public EntityConfigData CreateCopy()
    {
      Option<Dict<string, ImmutableArray<byte>>> nonPersistentData = this.m_nonPersistentData.HasValue ? this.m_nonPersistentData.Value.ToDict<string, ImmutableArray<byte>>().SomeOption<Dict<string, ImmutableArray<byte>>>() : (Option<Dict<string, ImmutableArray<byte>>>) Option.None;
      return new EntityConfigData(this.m_data.CreateCopyWithNewDictionaries(), nonPersistentData, this.m_context);
    }

    public EntityConfigData CreateCopyWithNewProto(Proto proto)
    {
      Option<Dict<string, ImmutableArray<byte>>> nonPersistentData = this.m_nonPersistentData.HasValue ? this.m_nonPersistentData.Value.ToDict<string, ImmutableArray<byte>>().SomeOption<Dict<string, ImmutableArray<byte>>>() : (Option<Dict<string, ImmutableArray<byte>>>) Option.None;
      return new EntityConfigData(proto, this.m_data.CreateCopyWithNewDictionaries(), nonPersistentData, this.m_context);
    }

    public void SetInt(string key, int? value)
    {
      if (!value.HasValue)
        this.m_data.Integers.Remove(key);
      else
        this.m_data.Integers[key] = value.Value;
    }

    public int? GetInt(string key)
    {
      int num;
      return !this.m_data.Integers.TryGetValue(key, out num) ? new int?() : new int?(num);
    }

    public void SetBool(string key, bool? value)
    {
      if (!value.HasValue)
        this.m_data.Booleans.Remove(key);
      else
        this.m_data.Booleans[key] = value.Value;
    }

    public bool? GetBool(string key)
    {
      bool flag;
      return !this.m_data.Booleans.TryGetValue(key, out flag) ? new bool?() : new bool?(flag);
    }

    public void SetBoolNonPersistent(string key, bool? value)
    {
      if (!value.HasValue)
        return;
      bool? nullable = value;
      bool flag = false;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        return;
      this.m_nonPersistentData = (Option<Dict<string, ImmutableArray<byte>>>) (this.m_nonPersistentData.ValueOrNull ?? new Dict<string, ImmutableArray<byte>>());
      this.m_context.Set<bool>(this.m_nonPersistentData.Value, key, value.Value, (Action<bool, BlobWriter>) ((v, w) => w.WriteBool(v)));
    }

    public bool? GetBoolNonPersistent(string key)
    {
      if (this.m_nonPersistentData.IsNone)
        return new bool?();
      ImmutableArray<byte> data;
      if (!this.m_nonPersistentData.Value.TryGetValue(key, out data))
        return new bool?();
      bool result;
      return !this.m_context.TryReadFromBytes<bool>(data, key, (Func<BlobReader, bool>) (r => r.ReadBool()), out result) ? new bool?() : new bool?(result);
    }

    public void SetPercent(string key, Percent? value)
    {
      if (!value.HasValue)
        this.m_data.Integers.Remove(key);
      else
        this.m_data.Integers[key] = value.Value.RawValue;
    }

    public Percent? GetPercent(string key)
    {
      int rawValue;
      return !this.m_data.Integers.TryGetValue(key, out rawValue) ? new Percent?() : new Percent?(Percent.FromRaw(rawValue));
    }

    public void SetThicknessTilesI(string key, ThicknessTilesI? value)
    {
      if (!value.HasValue)
        this.m_data.Integers.Remove(key);
      else
        this.m_data.Integers[key] = value.Value.Value;
    }

    public ThicknessTilesI? GetThicknessTilesI(string key)
    {
      int num;
      return !this.m_data.Integers.TryGetValue(key, out num) ? new ThicknessTilesI?() : new ThicknessTilesI?(new ThicknessTilesI(num));
    }

    public void SetEntityId(string key, EntityId? value)
    {
      if (!value.HasValue)
        this.m_data.Integers.Remove(key);
      else
        this.m_data.Integers[key] = value.Value.Value;
    }

    public EntityId? GetEntityId(string key)
    {
      int num;
      return !this.m_data.Integers.TryGetValue(key, out num) ? new EntityId?() : new EntityId?(new EntityId(num));
    }

    public void SetString(string key, Option<string> value)
    {
      if (!value.HasValue)
        this.m_data.Strings.Remove(key);
      else
        this.m_data.Strings[key] = value.Value;
    }

    public Option<string> GetString(string key)
    {
      string str;
      return !this.m_data.Strings.TryGetValue(key, out str) ? (Option<string>) Option.None : (Option<string>) str;
    }

    public void SetProto<T>(string key, Option<T> value) where T : Proto
    {
      if (!value.HasValue)
        this.m_data.Strings.Remove(key);
      else
        this.m_data.Strings[key] = value.Value.Id.Value;
    }

    public Option<T> GetProto<T>(string key, bool unlockedOnly) where T : Proto
    {
      string str;
      if (!this.m_data.Strings.TryGetValue(key, out str))
        return Option<T>.None;
      T proto;
      if (!this.m_context.ProtosDb.TryGetProto<T>(new Proto.ID(str), out proto))
        return (Option<T>) Option.None;
      return unlockedOnly && this.m_context.UnlockedProtosDb.IsLocked((IProto) proto) ? (Option<T>) Option.None : (Option<T>) proto;
    }

    public void SetProtoMapArray<T>(string key, ImmutableArray<KeyValuePair<string, T>>? value) where T : Proto
    {
      if (!value.HasValue)
        this.m_data.StringArrays.Remove(key);
      else
        this.m_data.StringArrays[key] = value.Value.Map<string>((Func<KeyValuePair<string, T>, string>) (x => x.Key + "::" + x.Value.Id.Value));
    }

    public ImmutableArray<KeyValuePair<string, T>>? GetProtoMapArray<T>(
      string key,
      bool unlockedOnly)
      where T : Proto
    {
      ImmutableArray<string> immutableArray;
      if (!this.m_data.StringArrays.TryGetValue(key, out immutableArray))
        return new ImmutableArray<KeyValuePair<string, T>>?();
      Lyst<KeyValuePair<string, T>> list = new Lyst<KeyValuePair<string, T>>(immutableArray.Length);
      foreach (string str in immutableArray)
      {
        string[] separator = new string[1]{ "::" };
        string[] strArray = str.Split(separator, StringSplitOptions.None);
        T proto;
        if (strArray.Length == 2 && this.m_context.ProtosDb.TryGetProto<T>(new Proto.ID(strArray[1]), out proto) && (!unlockedOnly || !this.m_context.UnlockedProtosDb.IsLocked((IProto) proto)))
          list.Add<string, T>(strArray[0], proto);
      }
      return new ImmutableArray<KeyValuePair<string, T>>?(list.ToImmutableArray());
    }

    public void SetProtoArray<T>(string key, ImmutableArray<T>? value) where T : Proto
    {
      if (!value.HasValue)
        this.m_data.StringArrays.Remove(key);
      else
        this.m_data.StringArrays[key] = value.Value.Map<string>((Func<T, string>) (x => x.Id.Value));
    }

    public ImmutableArray<T>? GetProtoArray<T>(string key, bool unlockedOnly) where T : Proto
    {
      ImmutableArray<string> immutableArray;
      if (!this.m_data.StringArrays.TryGetValue(key, out immutableArray))
        return new ImmutableArray<T>?();
      Lyst<T> lyst = new Lyst<T>(immutableArray.Length);
      foreach (string str in immutableArray)
      {
        T proto;
        if (this.m_context.ProtosDb.TryGetProto<T>(new Proto.ID(str), out proto) && (!unlockedOnly || !this.m_context.UnlockedProtosDb.IsLocked((IProto) proto)))
          lyst.Add(proto);
      }
      return new ImmutableArray<T>?(lyst.ToImmutableArray());
    }

    public void SetOptionProtoArray<T>(string key, ImmutableArray<Option<T>>? value) where T : Proto
    {
      if (!value.HasValue)
        this.m_data.StringArrays.Remove(key);
      else
        this.m_data.StringArrays[key] = value.Value.Map<string>((Func<Option<T>, string>) (x => !x.HasValue ? "" : x.Value.Id.Value));
    }

    public ImmutableArray<Option<T>>? GetOptionProtoArray<T>(string key, bool unlockedOnly) where T : Proto
    {
      ImmutableArray<string> immutableArray;
      if (!this.m_data.StringArrays.TryGetValue(key, out immutableArray))
        return new ImmutableArray<Option<T>>?();
      Lyst<Option<T>> lyst = new Lyst<Option<T>>();
      foreach (string str in immutableArray)
      {
        T proto;
        if (str.IsNotEmpty() && this.m_context.ProtosDb.TryGetProto<T>(new Proto.ID(str), out proto))
        {
          if (unlockedOnly && this.m_context.UnlockedProtosDb.IsLocked((IProto) proto))
            lyst.Add((Option<T>) Option.None);
          else
            lyst.Add((Option<T>) proto);
        }
        else
          lyst.Add((Option<T>) Option.None);
      }
      return new ImmutableArray<Option<T>>?(lyst.ToImmutableArray());
    }

    public void Set<T>(string key, T? value, Action<T, BlobWriter> writeFunc) where T : struct
    {
      if (!value.HasValue)
        this.m_data.ByteArrays.Remove(key);
      else
        this.m_context.Set<T>(this.m_data.ByteArrays, key, value.Value, writeFunc);
    }

    public void Set<T>(string key, Option<T> value, Action<T, BlobWriter> writeFunc) where T : class
    {
      if (value == default (T))
        this.m_data.ByteArrays.Remove(key);
      else
        this.m_context.Set<T>(this.m_data.ByteArrays, key, value.Value, writeFunc);
    }

    public T? Get<T>(string key, Func<BlobReader, T> readFunc) where T : struct
    {
      ImmutableArray<byte> data;
      if (!this.m_data.ByteArrays.TryGetValue(key, out data))
        return new T?();
      T result;
      return !this.m_context.TryReadFromBytes<T>(data, key, readFunc, out result) ? new T?() : new T?(result);
    }

    public void SetArray<T>(string key, ImmutableArray<T>? value, Action<T, BlobWriter> writeFunc)
    {
      if (!value.HasValue)
        this.m_data.ByteArrays.Remove(key);
      else
        this.m_context.SetArray<T>(this.m_data.ByteArrays, key, value.Value, writeFunc);
    }

    public ImmutableArray<T>? GetArray<T>(string key, Func<BlobReader, T> readFunc)
    {
      ImmutableArray<byte> data;
      return !this.m_data.ByteArrays.TryGetValue(key, out data) ? new ImmutableArray<T>?() : new ImmutableArray<T>?(this.m_context.ArrayFromBytes<T>(data, key, readFunc));
    }

    public Option<T> GetOption<T>(string key, Func<BlobReader, T> readFunc) where T : class
    {
      ImmutableArray<byte> data;
      if (!this.m_data.ByteArrays.TryGetValue(key, out data))
        return (Option<T>) Option.None;
      T result;
      return !this.m_context.TryReadFromBytes<T>(data, key, readFunc, out result) ? Option<T>.None : (Option<T>) result;
    }

    internal void SerializeForBlueprints(BlobWriter writer) => this.m_data.Serialize(writer);

    [MustUseReturnValue]
    internal static EntityConfigData DeserializeForBlueprints(
      BlobReader reader,
      ConfigSerializationContext context)
    {
      return new EntityConfigData(EntityConfigData.Data.Deserialize(reader), (Option<Dict<string, ImmutableArray<byte>>>) Option.None, context);
    }

    public static void Serialize(EntityConfigData value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EntityConfigData>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EntityConfigData.s_serializeDataDelayedAction);
    }

    protected void SerializeData(BlobWriter writer) => this.m_data.Serialize(writer);

    public static EntityConfigData Deserialize(BlobReader reader)
    {
      EntityConfigData entityConfigData;
      if (reader.TryStartClassDeserialization<EntityConfigData>(out entityConfigData))
        reader.EnqueueDataDeserialization((object) entityConfigData, EntityConfigData.s_deserializeDataDelayedAction);
      return entityConfigData;
    }

    protected void DeserializeData(BlobReader reader)
    {
      reader.SetField<EntityConfigData>(this, "m_data", (object) EntityConfigData.Data.Deserialize(reader));
      reader.RegisterResolvedMember<EntityConfigData>(this, "m_context", typeof (ConfigSerializationContext), true);
      reader.RegisterInitAfterLoad<EntityConfigData>(this, "initAfterLoad", InitPriority.High);
    }

    static EntityConfigData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityConfigData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EntityConfigData) obj).SerializeData(writer));
      EntityConfigData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EntityConfigData) obj).DeserializeData(reader));
    }

    /// <summary>
    /// Having specialized dictionaries instead of just a byte[] one, allows the
    /// BlobWriter (when saving) to cache strings for instance. It also reduces
    /// allocations of byte arrays was simple types.
    /// </summary>
    internal readonly struct Data
    {
      internal readonly Dict<string, int> Integers;
      internal readonly Dict<string, bool> Booleans;
      internal readonly Dict<string, string> Strings;
      internal readonly Dict<string, ImmutableArray<string>> StringArrays;
      internal readonly Dict<string, ImmutableArray<byte>> ByteArrays;

      internal static EntityConfigData.Data GetNewInstance()
      {
        return new EntityConfigData.Data(new Dict<string, int>(), new Dict<string, bool>(), new Dict<string, string>(), new Dict<string, ImmutableArray<string>>(), new Dict<string, ImmutableArray<byte>>());
      }

      private Data(
        Dict<string, int> integers,
        Dict<string, bool> booleans,
        Dict<string, string> strings,
        Dict<string, ImmutableArray<string>> stringArrays,
        Dict<string, ImmutableArray<byte>> byteArrays)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Integers = integers;
        this.Booleans = booleans;
        this.Strings = strings;
        this.StringArrays = stringArrays;
        this.ByteArrays = byteArrays;
      }

      [Pure]
      internal EntityConfigData.Data CreateCopyWithNewDictionaries()
      {
        return new EntityConfigData.Data(this.Integers.ToDict<string, int>(), this.Booleans.ToDict<string, bool>(), this.Strings.ToDict<string, string>(), this.StringArrays.ToDict<string, ImmutableArray<string>>(), this.ByteArrays.ToDict<string, ImmutableArray<byte>>());
      }

      internal void Serialize(BlobWriter writer)
      {
        writeDict<int>(this.Integers, (Action<BlobWriter, int>) ((w, v) => w.WriteInt(v)));
        writeDict<bool>(this.Booleans, (Action<BlobWriter, bool>) ((w, v) => w.WriteBool(v)));
        writeDict<string>(this.Strings, (Action<BlobWriter, string>) ((w, v) => w.WriteString(v)));
        writeDict<ImmutableArray<string>>(this.StringArrays, (Action<BlobWriter, ImmutableArray<string>>) ((w, v) =>
        {
          w.WriteIntNotNegative(v.Length);
          for (int index = 0; index < v.Length; ++index)
            w.WriteString(v[index]);
        }));
        writeDict<ImmutableArray<byte>>(this.ByteArrays, (Action<BlobWriter, ImmutableArray<byte>>) ((w, v) => w.WriteByteArray(v)));

        void writeDict<T>(Dict<string, T> data, Action<BlobWriter, T> valWriter)
        {
          writer.WriteIntNotNegative(data.Count);
          foreach (KeyValuePair<string, T> keyValuePair in data)
          {
            writer.WriteString(keyValuePair.Key);
            valWriter(writer, keyValuePair.Value);
          }
        }
      }

      [MustUseReturnValue]
      internal static EntityConfigData.Data Deserialize(BlobReader reader)
      {
        return new EntityConfigData.Data(readDict<int>((Func<BlobReader, int>) (r => r.ReadInt())), readDict<bool>((Func<BlobReader, bool>) (r => r.ReadBool())), readDict<string>((Func<BlobReader, string>) (r => r.ReadString())), readDict<ImmutableArray<string>>((Func<BlobReader, ImmutableArray<string>>) (r =>
        {
          int length = r.ReadIntNotNegative();
          ImmutableArrayBuilder<string> immutableArrayBuilder = new ImmutableArrayBuilder<string>(length);
          for (int i = 0; i < length; ++i)
            immutableArrayBuilder[i] = r.ReadString();
          return immutableArrayBuilder.GetImmutableArrayAndClear();
        })), readDict<ImmutableArray<byte>>((Func<BlobReader, ImmutableArray<byte>>) (r => r.ReadBytesArray())));

        Dict<string, T> readDict<T>(Func<BlobReader, T> valReader)
        {
          int capacity = reader.ReadIntNotNegative();
          Dict<string, T> dict = new Dict<string, T>(capacity);
          for (int index = 0; index < capacity; ++index)
            dict.Add(reader.ReadString(), valReader(reader));
          return dict;
        }
      }
    }

    public struct EntityIdsHolder
    {
      private readonly string m_key;

      public ImmutableArray<EntityId>? MappedIds { get; private set; }

      public EntityIdsHolder(string key)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_key = key;
        this.MappedIds = new ImmutableArray<EntityId>?();
      }

      public void SetData(EntityConfigData parent, ImmutableArray<EntityId>? data)
      {
        this.MappedIds = data;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        parent.SetArray<EntityId>(this.m_key, data, EntityConfigData.EntityIdsHolder.\u003C\u003EO.\u003C0\u003E__Serialize ?? (EntityConfigData.EntityIdsHolder.\u003C\u003EO.\u003C0\u003E__Serialize = new Action<EntityId, BlobWriter>(EntityId.Serialize)));
      }

      private ImmutableArray<EntityId>? getSavedIds(EntityConfigData parent)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return parent.GetArray<EntityId>(this.m_key, EntityConfigData.EntityIdsHolder.\u003C\u003EO.\u003C1\u003E__Deserialize ?? (EntityConfigData.EntityIdsHolder.\u003C\u003EO.\u003C1\u003E__Deserialize = new Func<BlobReader, EntityId>(EntityId.Deserialize)));
      }

      public void MapGlobalIds(
        EntityConfigData parent,
        Dict<EntityId, EntityId> map,
        Mafi.Collections.Set<EntityId> idsFailedToMap)
      {
        ImmutableArray<EntityId>? savedIds = this.getSavedIds(parent);
        if (!savedIds.HasValue)
          return;
        Lyst<EntityId> lyst = new Lyst<EntityId>();
        foreach (EntityId key in savedIds.Value)
        {
          EntityId entityId;
          if (map.TryGetValue(key, out entityId))
            lyst.Add(entityId);
          else if (!idsFailedToMap.Contains(key))
            lyst.Add(key);
        }
        this.MappedIds = new ImmutableArray<EntityId>?(lyst.ToImmutableArray());
      }

      public void FilterOutExternalIds(EntityConfigData parent, Mafi.Collections.Set<EntityId> idsToKeep)
      {
        ImmutableArray<EntityId>? savedIds = this.getSavedIds(parent);
        if (!savedIds.HasValue)
          return;
        Lyst<EntityId> lyst = new Lyst<EntityId>();
        foreach (EntityId entityId in savedIds.Value)
        {
          if (idsToKeep.Contains(entityId))
            lyst.Add(entityId);
        }
        this.SetData(parent, new ImmutableArray<EntityId>?(lyst.ToImmutableArray()));
      }
    }

    private class TrajectorySerializer
    {
      internal static void Serialize(TransportTrajectory trajectory, BlobWriter writer)
      {
        RelTile3i.Serialize(trajectory.StartDirection, writer);
        RelTile3i.Serialize(trajectory.EndDirection, writer);
        writer.WriteIntNotNegative(trajectory.Pivots.Length);
        foreach (Tile3i pivot in trajectory.Pivots)
          Tile3i.Serialize(pivot, writer);
      }

      internal static TransportTrajectory Deserialize(IProto proto, BlobReader reader)
      {
        RelTile3i relTile3i1 = RelTile3i.Deserialize(reader);
        RelTile3i relTile3i2 = RelTile3i.Deserialize(reader);
        int length = reader.ReadIntNotNegative();
        ImmutableArrayBuilder<Tile3i> immutableArrayBuilder = new ImmutableArrayBuilder<Tile3i>(length);
        for (int i = 0; i < length; ++i)
          immutableArrayBuilder[i] = Tile3i.Deserialize(reader);
        ImmutableArray<Tile3i> immutableArrayAndClear = immutableArrayBuilder.GetImmutableArrayAndClear();
        if (!(proto is TransportProto proto1))
          return (TransportTrajectory) null;
        TransportTrajectory trajectory;
        return TransportTrajectory.TryCreateFromPivots(proto1, immutableArrayAndClear, new RelTile3i?(relTile3i1), new RelTile3i?(relTile3i2), out trajectory, out string _, true) ? trajectory : (TransportTrajectory) null;
      }

      public TrajectorySerializer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
