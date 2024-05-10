// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.AnimalFarmConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  public static class AnimalFarmConfigExtensions
  {
    public static int? GetSlaughterLimit(this EntityConfigData data)
    {
      return data.GetInt("SlaughterLimit");
    }

    public static void SetSlaughterLimit(this EntityConfigData data, int? value)
    {
      data.SetInt("SlaughterLimit", value);
    }

    public static bool? GetIsGrowthPaused(this EntityConfigData data)
    {
      return data.GetBool("IsGrowthPaused");
    }

    public static void SetIsGrowthPaused(this EntityConfigData data, bool? value)
    {
      data.SetBool("IsGrowthPaused", value);
    }
  }
}
