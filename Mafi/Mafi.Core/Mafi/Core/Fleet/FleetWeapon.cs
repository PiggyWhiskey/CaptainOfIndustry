// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetWeapon
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>Actual weapon instance in the fleet entity.</summary>
  [GenerateSerializer(false, null, 0)]
  public class FleetWeapon : DestructibleFleetPart
  {
    public readonly FleetWeaponProto Proto;
    public readonly FleetEntity OwningEntity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int Damage => this.Proto.Damage;

    public int Range => this.Proto.Range;

    public int ReloadRounds => this.Proto.ReloadRounds;

    public Percent AccuracyAtMinRange => this.Proto.AccuracyAtMinRange;

    public Percent AccuracyAtMaxRange => this.Proto.AccuracyAtMaxRange;

    public FleetWeapon(FleetWeaponProto proto, FleetEntity owningEntity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((DestructibleFleetPartProto) proto);
      this.OwningEntity = owningEntity.CheckNotNull<FleetEntity>();
      this.Proto = proto.CheckNotNull<FleetWeaponProto>();
    }

    public int RoundsUntilReloaded { get; private set; }

    public bool IsReadyToFire => this.RoundsUntilReloaded <= 0 && this.IsNotDestroyed;

    public bool FiredLastSim { get; set; }

    public int DamageDuringCurrentBattle { get; private set; }

    public int MissedDmgDuringCurrentBattle { get; private set; }

    /// <summary>
    /// Average DPR is computed as: (Damage / ReloadTIme) * AvgAccuracy.
    /// </summary>
    public int AvgDamagePer10Rounds => this.Proto.AvgDamagePer10Rounds;

    public bool IsInRange(FleetEntity entity) => this.distanceToEnemy(entity) <= this.Range;

    private Fix32 distanceToEnemy(FleetEntity entity)
    {
      Assert.That<BattleFleet>(entity.Fleet).IsNotEqualTo<BattleFleet>(this.OwningEntity.Fleet);
      return (entity.Position - this.OwningEntity.Position).Abs();
    }

    private Percent getCurrentAccuracy(BattleState state, FleetEntity enemy)
    {
      Percent t = Percent.FromValueBetweenMinMax(this.distanceToEnemy(enemy).ToIntRounded(), this.Range / 2, this.Range);
      Assert.That<bool>(t.IsWithin0To100PercIncl).IsTrue();
      return this.AccuracyAtMinRange.Lerp(this.AccuracyAtMaxRange, t);
    }

    public override void NextBattleRoundStarted(BattleState state)
    {
      base.NextBattleRoundStarted(state);
      if (this.RoundsUntilReloaded <= 0)
        return;
      --this.RoundsUntilReloaded;
    }

    public void FireAt(FleetEntity enemy, BattleState state)
    {
      Assert.That<bool>(this.IsInBattle).IsTrue();
      Assert.That<bool>(this.IsReadyToFire).IsTrue();
      Assert.That<bool>(this.IsInRange(enemy)).IsTrue();
      Assert.That<bool>(enemy.IsNotDestroyed).IsTrue();
      this.RoundsUntilReloaded = this.ReloadRounds;
      int damage = this.Damage;
      Percent currentAccuracy = this.getCurrentAccuracy(state, enemy);
      if (state.Random.TestProbability(currentAccuracy))
      {
        int? impactFrom = enemy.TakeImpactFrom(damage, this, state);
        if (impactFrom.HasValue)
        {
          this.DamageDuringCurrentBattle += impactFrom.Value;
          return;
        }
      }
      this.MissedDmgDuringCurrentBattle += damage;
    }

    public override void ResetBattle(BattleState state)
    {
      base.ResetBattle(state);
      this.RoundsUntilReloaded = 0;
      this.DamageDuringCurrentBattle = 0;
      this.MissedDmgDuringCurrentBattle = 0;
    }

    public static void Serialize(FleetWeapon value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetWeapon>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetWeapon.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.DamageDuringCurrentBattle);
      writer.WriteBool(this.FiredLastSim);
      writer.WriteInt(this.MissedDmgDuringCurrentBattle);
      FleetEntity.Serialize(this.OwningEntity, writer);
      writer.WriteGeneric<FleetWeaponProto>(this.Proto);
      writer.WriteInt(this.RoundsUntilReloaded);
    }

    public static FleetWeapon Deserialize(BlobReader reader)
    {
      FleetWeapon fleetWeapon;
      if (reader.TryStartClassDeserialization<FleetWeapon>(out fleetWeapon))
        reader.EnqueueDataDeserialization((object) fleetWeapon, FleetWeapon.s_deserializeDataDelayedAction);
      return fleetWeapon;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.DamageDuringCurrentBattle = reader.ReadInt();
      this.FiredLastSim = reader.ReadBool();
      this.MissedDmgDuringCurrentBattle = reader.ReadInt();
      reader.SetField<FleetWeapon>(this, "OwningEntity", (object) FleetEntity.Deserialize(reader));
      reader.SetField<FleetWeapon>(this, "Proto", (object) reader.ReadGenericAs<FleetWeaponProto>());
      this.RoundsUntilReloaded = reader.ReadInt();
    }

    static FleetWeapon()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetWeapon.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((DestructibleFleetPart) obj).SerializeData(writer));
      FleetWeapon.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((DestructibleFleetPart) obj).DeserializeData(reader));
    }
  }
}
