// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ReservedOceanAreaState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Notifications;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  public sealed class ReservedOceanAreaState
  {
    public const int MAX_AREAS_IN_SET = 32;
    [OnlyForSaveCompatibility(null)]
    public readonly IProtoWithReservedOcean Proto;
    private readonly IStaticEntityWithReservedOcean m_entity;
    public readonly ImmutableArray<ImmutableArray<RectangleTerrainArea2i>> AreasSets;
    [DoNotSave(0, null)]
    private readonly uint[] m_setsValidity;
    private readonly Event m_areasSetsValidityChanged;
    [NewInSaveVersion(99, null, null, null, null)]
    private EntityNotificator m_noValidAreasNotificator;
    [DoNotSave(99, null)]
    private EntityNotificator? m_noValidAreasNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Bit masks for sets validity, each bit represents an area. Bit is 0 when valid, 1 when invalid.
    /// Set is valid if its mask is equal to zero.
    /// </summary>
    public ReadOnlyArray<uint> AreasSetsValidity => this.m_setsValidity.AsReadOnlyArray<uint>();

    public bool HasAnyValidAreaSet => this.FirstValidAreasSetIndex >= 0;

    public int FirstValidAreasSetIndex { get; private set; }

    public IEvent AreasSetsValidityChanged => (IEvent) this.m_areasSetsValidityChanged;

    public bool IsNoValidAreasNotificationActive => this.m_noValidAreasNotificator.IsActive;

    public ReservedOceanAreaState(
      IProtoWithReservedOcean proto,
      IStaticEntityWithReservedOcean entity,
      EntityNotificationProto.ID? noValidAreasNotificationId = null,
      INotificationsManager notificationsManager = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_areasSetsValidityChanged = new Event();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = proto;
      this.m_entity = entity;
      this.AreasSets = proto.ReservedOceanAreasSets.Map<ImmutableArray<RectangleTerrainArea2i>>((Func<ImmutableArray<RectangleTerrainArea2iRelative>, ImmutableArray<RectangleTerrainArea2i>>) (x => x.Map<RectangleTerrainArea2i>((Func<RectangleTerrainArea2iRelative, RectangleTerrainArea2i>) (area => entity.Prototype.Layout.Transform(area, entity.Transform)))));
      this.m_setsValidity = new uint[this.Proto.ReservedOceanAreasSets.Length];
      if (!noValidAreasNotificationId.HasValue)
        return;
      if (notificationsManager != null)
        this.m_noValidAreasNotificator = notificationsManager.CreateNotificatorFor(noValidAreasNotificationId.Value);
      else
        Log.Warning(string.Format("Failed to create notification '{0}', ", (object) noValidAreasNotificationId) + "notifications manager was not provided.");
    }

    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void initSelf()
    {
      ReflectionUtils.SetField<ReservedOceanAreaState>(this, "m_setsValidity", (object) new uint[this.Proto.ReservedOceanAreasSets.Length]);
    }

    [InitAfterLoad(InitPriority.Lowest)]
    private void fixState(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 99 || !this.m_noValidAreasNotif.HasValue)
        return;
      this.m_noValidAreasNotif.Value.Deactivate((IEntity) this.m_entity);
      this.m_entity.Context.NotificationsManager.RemoveAllNotificationFor((IEntity) this.m_entity, this.m_noValidAreasNotif.Value.Prototype);
      this.m_noValidAreasNotificator = resolver.Resolve<INotificationsManager>().CreateNotificatorFor(new EntityNotificationProto.ID(this.m_noValidAreasNotif.Value.Prototype.Id.Value));
      this.m_noValidAreasNotificator.NotifyIff(this.FirstValidAreasSetIndex < 0, (IEntity) this.m_entity);
    }

    /// <summary>Called when ocean area is blocked or unblocked.</summary>
    /// <remarks>Note that this is being called during load.</remarks>
    public void NotifyOceanAreaStatusChanged(int setIndex, int areaIndex, bool isValid)
    {
      uint num = (uint) (1 << areaIndex);
      bool flag = this.m_setsValidity[setIndex] == 0U;
      if (isValid)
        this.m_setsValidity[setIndex] &= ~num;
      else
        this.m_setsValidity[setIndex] |= num;
      if (this.m_setsValidity[setIndex] == 0U == flag)
        return;
      this.FirstValidAreasSetIndex = -1;
      for (int index = 0; index < this.m_setsValidity.Length; ++index)
      {
        if (this.m_setsValidity[index] == 0U)
        {
          this.FirstValidAreasSetIndex = index;
          break;
        }
      }
      if (this.m_noValidAreasNotificator.IsValid)
        this.m_noValidAreasNotificator.NotifyIff(this.FirstValidAreasSetIndex < 0, (IEntity) this.m_entity);
      this.m_areasSetsValidityChanged.Invoke();
    }

    public bool IsAreaValid(int setIndex, int areaIndex)
    {
      return ((int) (this.m_setsValidity[setIndex] >> areaIndex) & 1) == 0;
    }

    public static void Serialize(ReservedOceanAreaState value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ReservedOceanAreaState>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ReservedOceanAreaState.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      ImmutableArray<ImmutableArray<RectangleTerrainArea2i>>.Serialize(this.AreasSets, writer);
      writer.WriteInt(this.FirstValidAreasSetIndex);
      Event.Serialize(this.m_areasSetsValidityChanged, writer);
      writer.WriteGeneric<IStaticEntityWithReservedOcean>(this.m_entity);
      EntityNotificator.Serialize(this.m_noValidAreasNotificator, writer);
      writer.WriteGeneric<IProtoWithReservedOcean>(this.Proto);
    }

    public static ReservedOceanAreaState Deserialize(BlobReader reader)
    {
      ReservedOceanAreaState reservedOceanAreaState;
      if (reader.TryStartClassDeserialization<ReservedOceanAreaState>(out reservedOceanAreaState))
        reader.EnqueueDataDeserialization((object) reservedOceanAreaState, ReservedOceanAreaState.s_deserializeDataDelayedAction);
      return reservedOceanAreaState;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<ReservedOceanAreaState>(this, "AreasSets", (object) ImmutableArray<ImmutableArray<RectangleTerrainArea2i>>.Deserialize(reader));
      this.FirstValidAreasSetIndex = reader.ReadInt();
      reader.SetField<ReservedOceanAreaState>(this, "m_areasSetsValidityChanged", (object) Event.Deserialize(reader));
      reader.SetField<ReservedOceanAreaState>(this, "m_entity", (object) reader.ReadGenericAs<IStaticEntityWithReservedOcean>());
      if (reader.LoadedSaveVersion < 99)
        this.m_noValidAreasNotif = reader.ReadNullableStruct<EntityNotificator>();
      this.m_noValidAreasNotificator = reader.LoadedSaveVersion >= 99 ? EntityNotificator.Deserialize(reader) : new EntityNotificator();
      reader.SetField<ReservedOceanAreaState>(this, "Proto", (object) reader.ReadGenericAs<IProtoWithReservedOcean>());
      this.initSelf();
      reader.RegisterInitAfterLoad<ReservedOceanAreaState>(this, "fixState", InitPriority.Lowest);
    }

    static ReservedOceanAreaState()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ReservedOceanAreaState.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ReservedOceanAreaState) obj).SerializeData(writer));
      ReservedOceanAreaState.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ReservedOceanAreaState) obj).DeserializeData(reader));
    }
  }
}
