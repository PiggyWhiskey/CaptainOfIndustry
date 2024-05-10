// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.RainwaterHarvesterData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class RainwaterHarvesterData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      int count = 19;
      int num = 9;
      string str1 = ((IEnumerable<string>) "[3]".Repeat<string>(count - 1)).JoinStrings() + "   ";
      string str2 = ((IEnumerable<string>) "[3]".Repeat<string>(count)).JoinStrings();
      string[] array = ((IEnumerable<string>) (str1 + "   ").Repeat<string>(5)).Concat<string>((IEnumerable<string>) (str2 + "   ").Repeat<string>(num - 5)).Concat<string>((IEnumerable<string>) new string[1]
      {
        str2 + ">@A"
      }).Concat<string>(((IEnumerable<string>) (str2 + "   ").Repeat<string>(num - 5)).Concat<string>((IEnumerable<string>) (str1 + "   ").Repeat<string>(5))).ToArray<string>();
      registrator.RainwaterHarvesterProtoBuilder.Start("Rainwater harvester", Ids.Buildings.RainwaterHarvester).Description("Harvests rain water during rain. Has built-in small water tank.").SetCost(Costs.Buildings.RainwaterHarvester).SetWaterProto((Proto.ID) Ids.Products.Water).SetCapacity(30).SetWaterCollectedPerDay(1.0).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout(array).SetPrefabPath("Assets/Base/Machines/Water/RainwaterCollector.prefab").EnableInstancedRendering().BuildAndAdd();
    }

    public RainwaterHarvesterData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
