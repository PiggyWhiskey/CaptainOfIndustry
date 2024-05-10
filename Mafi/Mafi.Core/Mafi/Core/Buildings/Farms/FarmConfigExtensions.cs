// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  public static class FarmConfigExtensions
  {
    public static Percent? GetFertilityTarget(this EntityConfigData data)
    {
      return data.GetPercent("FertilityTarget");
    }

    public static void SetFertilityTarget(this EntityConfigData data, Percent value)
    {
      data.SetPercent("FertilityTarget", new Percent?(value));
    }

    public static ImmutableArray<Option<CropProto>>? GetCropSchedule(this EntityConfigData data)
    {
      return data.GetOptionProtoArray<CropProto>("CropSchedule", true);
    }

    public static void SetCropSchedule(
      this EntityConfigData data,
      ImmutableArray<Option<CropProto>>? value)
    {
      data.SetOptionProtoArray<CropProto>("CropSchedule", value);
    }
  }
}
