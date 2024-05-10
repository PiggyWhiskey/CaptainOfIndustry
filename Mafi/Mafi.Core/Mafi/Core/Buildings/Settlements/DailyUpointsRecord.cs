// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.DailyUpointsRecord
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct DailyUpointsRecord
  {
    public readonly Percent PercentSatisfied;
    public readonly Upoints Unity;
    public readonly Upoints PossibleMax;

    public DailyUpointsRecord(Percent percentSatisfied, Upoints unity, Upoints possibleMax)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.PercentSatisfied = percentSatisfied;
      this.Unity = unity;
      this.PossibleMax = possibleMax;
    }

    public static void Serialize(DailyUpointsRecord value, BlobWriter writer)
    {
      Percent.Serialize(value.PercentSatisfied, writer);
      Upoints.Serialize(value.Unity, writer);
      Upoints.Serialize(value.PossibleMax, writer);
    }

    public static DailyUpointsRecord Deserialize(BlobReader reader)
    {
      return new DailyUpointsRecord(Percent.Deserialize(reader), Upoints.Deserialize(reader), Upoints.Deserialize(reader));
    }
  }
}
