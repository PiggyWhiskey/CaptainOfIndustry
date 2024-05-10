// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.DestructibleFleetPart
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Fleet
{
  public abstract class DestructibleFleetPart : IBattleAware
  {
    public readonly DestructibleFleetPartProto DestructiblePartProto;
    public readonly UpgradableInt MaxHp;
    public readonly UpgradableInt HitWeight;

    public int RecoverableHp { get; private set; }

    public int CurrentHp { get; protected set; }

    public bool IsInBattle { get; private set; }

    public DestructibleFleetPart(DestructibleFleetPartProto proto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DestructiblePartProto = proto;
      this.MaxHp = new UpgradableInt(proto.MaxHp);
      this.HitWeight = new UpgradableInt(proto.HitWeight);
      this.CurrentHp = this.MaxHp.GetValue();
    }

    public int DamageTakenDuringCurrentBattle { get; private set; }

    public int ArmorDamageReductionDuringCurrentBattle { get; private set; }

    public Percent HpPercent => Percent.FromRatio(this.CurrentHp, this.MaxHp.GetValue());

    public Percent RecoverableHpPercent
    {
      get => Percent.FromRatio(this.RecoverableHp, this.MaxHp.GetValue());
    }

    public bool IsDestroyed => this.CurrentHp <= 0;

    public bool IsNotDestroyed => !this.IsDestroyed;

    public void ApplyMaxHpUpgrade(UpgradableIntProto bonus)
    {
      this.MaxHp.Apply(bonus);
      this.HitWeight.Apply(bonus);
    }

    public void RemoveMaxHpUpgrade(UpgradableIntProto bonus)
    {
      this.MaxHp.Remove(bonus);
      this.HitWeight.Remove(bonus);
    }

    public int TakeDamage(int rawDamage, int armor, IBattleSimConfig config)
    {
      Assert.That<bool>(this.IsNotDestroyed).IsTrue();
      int val = (rawDamage - armor).Max(rawDamage - config.MaxArmorReduction.Apply(rawDamage)).Min(this.CurrentHp);
      this.DamageTakenDuringCurrentBattle += val;
      this.ArmorDamageReductionDuringCurrentBattle += rawDamage - val;
      this.CurrentHp -= val;
      this.RecoverableHp += config.RecoverableHpMultiplier.Apply(val);
      Assert.That<int>(this.CurrentHp).IsNotNegative();
      Assert.That<int>(this.RecoverableHp).IsNotNegative();
      return val;
    }

    public virtual void InitializeBattle(BattleState state)
    {
      Assert.That<bool>(this.IsInBattle).IsFalse();
      Assert.That<int>(this.RecoverableHp).IsZero();
      this.IsInBattle = true;
    }

    public virtual void NextBattleRoundStarted(BattleState state)
    {
    }

    public virtual void FinalizeBattle(BattleState state)
    {
      Assert.That<bool>(this.IsInBattle).IsTrue();
      this.IsInBattle = false;
    }

    public virtual void ResetBattle(BattleState state)
    {
      Assert.That<bool>(this.IsInBattle).IsFalse();
      this.CurrentHp = (this.CurrentHp + this.RecoverableHp).Min(this.MaxHp.GetValue().ScaledByRounded(95.Percent()));
      this.RecoverableHp = 0;
      this.DamageTakenDuringCurrentBattle = 0;
      this.ArmorDamageReductionDuringCurrentBattle = 0;
    }

    public AssetValue GetRepairCost()
    {
      Assert.That<int>(this.RecoverableHp).IsZero("Repairs should not be attempted when recoverable HPs are not zero.");
      Assert.That<Percent>(this.HpPercent).IsNotNegative();
      if (this.CurrentHp >= this.MaxHp.GetValue())
        return AssetValue.Empty;
      Percent percent = Percent.Hundred - this.HpPercent;
      Assert.That<Percent>(percent).IsNotNegative();
      AssetValue assetValue = this.DestructiblePartProto.Value.ScaledByCeiled(percent);
      return this.IsDestroyed ? assetValue.ScaledByCeiled(FleetEntity.RepairCostForValueDestroyedPart) : assetValue.ScaledByCeiled(FleetEntity.RepairCostForValue);
    }

    public void Repair() => this.CurrentHp = this.MaxHp.GetValue();

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.ArmorDamageReductionDuringCurrentBattle);
      writer.WriteInt(this.CurrentHp);
      writer.WriteInt(this.DamageTakenDuringCurrentBattle);
      writer.WriteGeneric<DestructibleFleetPartProto>(this.DestructiblePartProto);
      UpgradableInt.Serialize(this.HitWeight, writer);
      writer.WriteBool(this.IsInBattle);
      UpgradableInt.Serialize(this.MaxHp, writer);
      writer.WriteInt(this.RecoverableHp);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ArmorDamageReductionDuringCurrentBattle = reader.ReadInt();
      this.CurrentHp = reader.ReadInt();
      this.DamageTakenDuringCurrentBattle = reader.ReadInt();
      reader.SetField<DestructibleFleetPart>(this, "DestructiblePartProto", (object) reader.ReadGenericAs<DestructibleFleetPartProto>());
      reader.SetField<DestructibleFleetPart>(this, "HitWeight", (object) UpgradableInt.Deserialize(reader));
      this.IsInBattle = reader.ReadBool();
      reader.SetField<DestructibleFleetPart>(this, "MaxHp", (object) UpgradableInt.Deserialize(reader));
      this.RecoverableHp = reader.ReadInt();
    }
  }
}
