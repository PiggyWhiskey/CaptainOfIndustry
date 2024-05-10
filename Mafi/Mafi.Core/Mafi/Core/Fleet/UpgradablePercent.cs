// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.UpgradablePercent
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
  [GenerateSerializer(false, null, 0)]
  public class UpgradablePercent
  {
    public readonly Percent BaseValue;
    public readonly bool IsLimited0To100;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Percent BonusValue { get; private set; }

    public Percent BonusMultiplier { get; private set; }

    public UpgradablePercent(Percent baseValue, bool isLimited0To100)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseValue = baseValue;
      this.IsLimited0To100 = isLimited0To100;
      this.BonusValue = Percent.Zero;
      this.BonusMultiplier = Percent.Zero;
    }

    [Pure]
    public Percent GetValue()
    {
      Percent percent = (this.BonusMultiplier + Percent.Hundred).Apply(this.BaseValue + this.BonusValue);
      if (this.IsLimited0To100)
        percent = percent.Clamp0To100();
      return percent;
    }

    public void ApplyUpgrade(UpgradablePercentProto upgrade)
    {
      this.BonusValue += upgrade.BonusValue;
      this.BonusMultiplier += upgrade.BonusMultiplier;
    }

    public static void Serialize(UpgradablePercent value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpgradablePercent>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpgradablePercent.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Percent.Serialize(this.BaseValue, writer);
      Percent.Serialize(this.BonusMultiplier, writer);
      Percent.Serialize(this.BonusValue, writer);
      writer.WriteBool(this.IsLimited0To100);
    }

    public static UpgradablePercent Deserialize(BlobReader reader)
    {
      UpgradablePercent upgradablePercent;
      if (reader.TryStartClassDeserialization<UpgradablePercent>(out upgradablePercent))
        reader.EnqueueDataDeserialization((object) upgradablePercent, UpgradablePercent.s_deserializeDataDelayedAction);
      return upgradablePercent;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UpgradablePercent>(this, "BaseValue", (object) Percent.Deserialize(reader));
      this.BonusMultiplier = Percent.Deserialize(reader);
      this.BonusValue = Percent.Deserialize(reader);
      reader.SetField<UpgradablePercent>(this, "IsLimited0To100", (object) reader.ReadBool());
    }

    static UpgradablePercent()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpgradablePercent.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpgradablePercent) obj).SerializeData(writer));
      UpgradablePercent.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpgradablePercent) obj).DeserializeData(reader));
    }
  }
}
