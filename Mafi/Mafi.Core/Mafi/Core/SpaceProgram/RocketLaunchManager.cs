// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.RocketLaunchManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class RocketLaunchManager : 
    IRocketLaunchManager,
    ICommandProcessor<LaunchRocketCmd>,
    IAction<LaunchRocketCmd>,
    ICommandProcessor<SetRocketAutoLaunchCmd>,
    IAction<SetRocketAutoLaunchCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly Upoints UNITY_PER_LAUNCH;
    public static readonly int UNITY_PER_LAUNCH_DURATION_MONTHS;
    private readonly Set<RocketLaunchPad> m_launchPads;
    private readonly Event<RocketEntity> m_rocketLaunched;
    private readonly IRandom m_random;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IUpointsManager m_upointsManager;
    [NewInSaveVersion(140, "m_unreachablesManager", null, typeof (UnreachableTerrainDesignationsManager), null)]
    private readonly UnreachableTerrainDesignationsManager m_unreachablesManager;
    private readonly ProtosDb m_protosDb;
    private readonly Queueue<Upoints> m_monthlyUpointsForLaunches;

    public static void Serialize(RocketLaunchManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketLaunchManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketLaunchManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.LaunchesFailuresCount);
      writer.WriteInt(this.LaunchesSuccessesCount);
      writer.WriteInt(this.LaunchExp);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Set<RocketLaunchPad>.Serialize(this.m_launchPads, writer);
      Queueue<Upoints>.Serialize(this.m_monthlyUpointsForLaunches, writer);
      writer.WriteGeneric<IRandom>(this.m_random);
      Event<RocketEntity>.Serialize(this.m_rocketLaunched, writer);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachablesManager, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
    }

    public static RocketLaunchManager Deserialize(BlobReader reader)
    {
      RocketLaunchManager rocketLaunchManager;
      if (reader.TryStartClassDeserialization<RocketLaunchManager>(out rocketLaunchManager))
        reader.EnqueueDataDeserialization((object) rocketLaunchManager, RocketLaunchManager.s_deserializeDataDelayedAction);
      return rocketLaunchManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.LaunchesFailuresCount = reader.ReadInt();
      this.LaunchesSuccessesCount = reader.ReadInt();
      this.LaunchExp = reader.ReadInt();
      reader.SetField<RocketLaunchManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<RocketLaunchManager>(this, "m_launchPads", (object) Set<RocketLaunchPad>.Deserialize(reader));
      reader.SetField<RocketLaunchManager>(this, "m_monthlyUpointsForLaunches", (object) Queueue<Upoints>.Deserialize(reader));
      reader.RegisterResolvedMember<RocketLaunchManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<RocketLaunchManager>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<RocketLaunchManager>(this, "m_rocketLaunched", (object) Event<RocketEntity>.Deserialize(reader));
      reader.SetField<RocketLaunchManager>(this, "m_unreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<RocketLaunchManager>(this, "m_unreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      reader.SetField<RocketLaunchManager>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
    }

    public int LaunchExp { get; private set; }

    public int LaunchesSuccessesCount { get; private set; }

    public int LaunchesFailuresCount { get; private set; }

    public int LaunchesCount => this.LaunchesSuccessesCount + this.LaunchesFailuresCount;

    public IEvent<RocketEntity> RocketLaunched => (IEvent<RocketEntity>) this.m_rocketLaunched;

    public RocketLaunchManager(
      RandomProvider randomProvider,
      IEntitiesManager entitiesManager,
      ICalendar calendar,
      IUpointsManager upointsManager,
      UnreachableTerrainDesignationsManager unreachablesManager,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_launchPads = new Set<RocketLaunchPad>();
      this.m_rocketLaunched = new Event<RocketEntity>();
      this.m_monthlyUpointsForLaunches = new Queueue<Upoints>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      this.m_entitiesManager = entitiesManager;
      this.m_upointsManager = upointsManager;
      this.m_unreachablesManager = unreachablesManager;
      this.m_protosDb = protosDb;
      entitiesManager.StaticEntityAdded.Add<RocketLaunchManager>(this, new Action<IStaticEntity>(this.staticEntityAdded));
      entitiesManager.StaticEntityRemoved.Add<RocketLaunchManager>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
      calendar.NewMonth.Add<RocketLaunchManager>(this, new Action(this.onNewMonth));
    }

    private void staticEntityAdded(IStaticEntity entity)
    {
      if (!(entity is RocketLaunchPad rocketLaunchPad))
        return;
      this.m_launchPads.AddAndAssertNew(rocketLaunchPad);
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      if (!(entity is RocketLaunchPad rocketLaunchPad))
        return;
      this.m_launchPads.RemoveAndAssert(rocketLaunchPad);
    }

    private void onNewMonth()
    {
      if (!this.m_monthlyUpointsForLaunches.IsNotEmpty)
        return;
      Upoints generated = this.m_monthlyUpointsForLaunches.Dequeue();
      this.m_upointsManager.GenerateUnity(IdsCore.UpointsCategories.Rockets, generated);
    }

    public Option<RocketLaunchPad> FindClosestAvailableLaunchPad(RocketTransporter transporter)
    {
      Option<RocketLaunchPad> availableLaunchPad = Option<RocketLaunchPad>.None;
      Fix32 fix32_1 = Fix32.MaxValue;
      Tile2i groundPositionTile2i = transporter.GroundPositionTile2i;
      IReadOnlySet<IEntity> unreachableEntitiesFor = this.m_unreachablesManager.GetUnreachableEntitiesFor((IPathFindingVehicle) transporter);
      foreach (RocketLaunchPad launchPad in this.m_launchPads)
      {
        if (!launchPad.IsNotEnabled && !launchPad.AttachedRocket.HasValue && launchPad.IncomingRocketsQueueLength <= 0 && !unreachableEntitiesFor.Contains((IEntity) launchPad))
        {
          Fix32 fix32_2 = launchPad.Transform.Position.Xy.DistanceTo(groundPositionTile2i);
          if (fix32_2 < fix32_1)
          {
            availableLaunchPad = (Option<RocketLaunchPad>) launchPad;
            fix32_1 = fix32_2;
          }
        }
      }
      return availableLaunchPad;
    }

    public void ReportRocketLaunched(RocketEntity rocket)
    {
      Assert.That<bool>(rocket.IsLaunched).IsTrue();
      Assert.That<bool?>(rocket.IsExploded).IsNone<bool>();
      this.m_rocketLaunched.Invoke(rocket);
    }

    public void ReportLaunchDone(RocketEntity rocket)
    {
      Assert.That<bool?>(rocket.IsExploded).HasValue<bool>();
      bool? isExploded = rocket.IsExploded;
      bool flag = false;
      if (isExploded.GetValueOrDefault() == flag & isExploded.HasValue)
      {
        this.LaunchExp += rocket.Prototype.LaunchExp;
        ++this.LaunchesSuccessesCount;
      }
      else
      {
        this.LaunchExp += rocket.Prototype.LaunchExp / 2;
        ++this.LaunchesFailuresCount;
      }
      Upoints[] array = this.m_monthlyUpointsForLaunches.ToArray();
      this.m_monthlyUpointsForLaunches.Clear();
      for (int index = 0; index < RocketLaunchManager.UNITY_PER_LAUNCH_DURATION_MONTHS; ++index)
      {
        Upoints unityPerLaunch = RocketLaunchManager.UNITY_PER_LAUNCH;
        if (index < array.Length)
          unityPerLaunch += array[index];
        this.m_monthlyUpointsForLaunches.Enqueue(unityPerLaunch);
      }
      Log.GameProgress(string.Format("Rocket #{0} launched! ", (object) (this.LaunchesSuccessesCount + this.LaunchesFailuresCount)));
    }

    void IAction<LaunchRocketCmd>.Invoke(LaunchRocketCmd cmd)
    {
      RocketLaunchPad entity;
      if (!this.m_entitiesManager.TryGetEntity<RocketLaunchPad>(cmd.LaunchPadId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find launch pad '{0}'.", (object) cmd.LaunchPadId));
      }
      else
      {
        Assert.That<Set<RocketLaunchPad>>(this.m_launchPads).Contains<RocketLaunchPad>(entity, "Unregistered launch pad.");
        if (!entity.TryStartLaunchCountdown())
          cmd.SetResultError("Failed to start rocket launch count-down.");
        else
          cmd.SetResultSuccess();
      }
    }

    void IAction<SetRocketAutoLaunchCmd>.Invoke(SetRocketAutoLaunchCmd cmd)
    {
      RocketLaunchPad entity;
      if (!this.m_entitiesManager.TryGetEntity<RocketLaunchPad>(cmd.LaunchPadId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find launch pad '{0}'.", (object) cmd.LaunchPadId));
      }
      else
      {
        Assert.That<Set<RocketLaunchPad>>(this.m_launchPads).Contains<RocketLaunchPad>(entity, "Unregistered launch pad.");
        entity.SetAutoLaunch(cmd.AutoLaunch);
        cmd.SetResultSuccess();
      }
    }

    static RocketLaunchManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketLaunchManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RocketLaunchManager) obj).SerializeData(writer));
      RocketLaunchManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RocketLaunchManager) obj).DeserializeData(reader));
      RocketLaunchManager.UNITY_PER_LAUNCH = 1.0.Upoints();
      RocketLaunchManager.UNITY_PER_LAUNCH_DURATION_MONTHS = 12;
    }
  }
}
