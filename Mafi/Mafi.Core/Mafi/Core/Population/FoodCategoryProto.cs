// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.FoodCategoryProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Population
{
  public class FoodCategoryProto : Proto
  {
    public readonly bool HasHealthBenefit;

    public FoodCategoryProto(Proto.ID id, Proto.Str strings, bool hasHealthBenefit)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.HasHealthBenefit = hasHealthBenefit;
    }
  }
}
