// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.UpgradableInt
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
  public class UpgradableInt
  {
    public readonly int BaseValue;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int BonusValue { get; private set; }

    public Percent BonusMultiplier { get; private set; }

    public UpgradableInt(int baseValue)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BaseValue = baseValue;
      this.BonusValue = 0;
      this.BonusMultiplier = Percent.Zero;
    }

    [Pure]
    public int GetValue()
    {
      return (this.BonusMultiplier + Percent.Hundred).Apply(this.BaseValue + this.BonusValue);
    }

    public void Apply(UpgradableIntProto upgrade)
    {
      this.BonusValue += upgrade.BonusValue;
      this.BonusMultiplier += upgrade.BonusMultiplier;
    }

    public void Remove(UpgradableIntProto upgrade)
    {
      this.BonusValue -= upgrade.BonusValue;
      this.BonusMultiplier -= upgrade.BonusMultiplier;
    }

    public static void Serialize(UpgradableInt value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpgradableInt>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpgradableInt.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.BaseValue);
      Percent.Serialize(this.BonusMultiplier, writer);
      writer.WriteInt(this.BonusValue);
    }

    public static UpgradableInt Deserialize(BlobReader reader)
    {
      UpgradableInt upgradableInt;
      if (reader.TryStartClassDeserialization<UpgradableInt>(out upgradableInt))
        reader.EnqueueDataDeserialization((object) upgradableInt, UpgradableInt.s_deserializeDataDelayedAction);
      return upgradableInt;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UpgradableInt>(this, "BaseValue", (object) reader.ReadInt());
      this.BonusMultiplier = Percent.Deserialize(reader);
      this.BonusValue = reader.ReadInt();
    }

    static UpgradableInt()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpgradableInt.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UpgradableInt) obj).SerializeData(writer));
      UpgradableInt.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UpgradableInt) obj).DeserializeData(reader));
    }
  }
}
