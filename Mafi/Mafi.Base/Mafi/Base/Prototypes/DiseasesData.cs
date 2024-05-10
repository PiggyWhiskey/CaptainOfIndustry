// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.DiseasesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class DiseasesData : IModData
  {
    public static readonly LocStr1 DiseaseSeverity__1;
    public static readonly LocStr1 DiseaseSeverity__2;
    public static readonly LocStr1 DiseaseSeverity__3;
    public static readonly LocStr1 DiseaseSeverity__4;
    public static readonly LocStr1 DiseaseSeverity__5;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      Proto.ID id1 = new Proto.ID("NoWaterDisease");
      ProtosDb protosDb = prototypesDb;
      Proto.ID id2 = id1;
      Proto.Str str1 = Proto.CreateStr(id1, "Cholera", "Bacterial disease spread through contaminated water. To prevent this disease in the future, make sure your settlement has stable supply of clean water.");
      LocStr locStr1 = Loc.Str("NoWaterDisease_Reason", "low water supply", "explains that settlement has the current disease due to its low water supply");
      Percent healthPenalty = 20.Percent();
      Percent monthlyMortalityRate = 0.4.Percent();
      PopNeedProto orThrow = prototypesDb.GetOrThrow<PopNeedProto>(IdsCore.PopNeeds.WaterNeed);
      LocStr reason = locStr1;
      PopNeedDiseaseProto proto = new PopNeedDiseaseProto(id2, str1, healthPenalty, monthlyMortalityRate, 4, orThrow, reason);
      protosDb.Add<PopNeedDiseaseProto>(proto);
      Proto.ID id3 = new Proto.ID("InfectionDisease");
      prototypesDb.Add<TrashDiseaseProto>(new TrashDiseaseProto(id3, Proto.CreateStr(id3, "Infection", "To prevent this illness in the future, prevent trash from piling up in your settlement."), Loc.Str("InfectionDisease_Reason", "settlement full of waste", "explains that settlement has the current disease because its waste collection is poor"), 15.Percent(), 0.3.Percent(), 5));
      Proto.ID id4 = new Proto.ID("Cold");
      LocStr descShort = Loc.Str(id4.ToString() + "__desc", "This disease happens naturally and cannot be completely avoided. However the impact of it can be significantly reduced by having a hospital provided with medical supplies.", "");
      Proto.Str str2 = Proto.CreateStr(id4, "Cold", descShort);
      prototypesDb.Add<DiseaseProto>(new DiseaseProto(id4, str2, LocStr.Empty, 5.Percent(), 0.Percent(), 8, 0));
      Proto.ID id5 = new Proto.ID("Disease1");
      LocStr locStr2 = Loc.Str(id5.Value + "__name", "Flu", "disease");
      prototypesDb.Add<DiseaseProto>(new DiseaseProto(id5, Proto.CreateStrFromLocalized(id5, DiseasesData.DiseaseSeverity__1.Format(locStr2), descShort), LocStr.Empty, 8.Percent(), 0.Percent(), 6, 600));
      Proto.ID id6 = new Proto.ID("Disease2");
      prototypesDb.Add<DiseaseProto>(new DiseaseProto(id6, Proto.CreateStrFromLocalized(id6, DiseasesData.DiseaseSeverity__2.Format(locStr2), descShort), LocStr.Empty, 12.Percent(), 0.1.Percent(), 6, 900));
      Proto.ID id7 = new Proto.ID("Disease3");
      prototypesDb.Add<DiseaseProto>(new DiseaseProto(id7, Proto.CreateStrFromLocalized(id7, DiseasesData.DiseaseSeverity__3.Format(locStr2), descShort), LocStr.Empty, 15.Percent(), 0.2.Percent(), 7, 1100));
      Proto.ID id8 = new Proto.ID("Disease4");
      prototypesDb.Add<DiseaseProto>(new DiseaseProto(id8, Proto.CreateStrFromLocalized(id8, DiseasesData.DiseaseSeverity__4.Format(locStr2), descShort), LocStr.Empty, 20.Percent(), 0.3.Percent(), 8, 1430));
      Proto.ID id9 = new Proto.ID("Disease5");
      prototypesDb.Add<DiseaseProto>(new DiseaseProto(id9, Proto.CreateStrFromLocalized(id9, DiseasesData.DiseaseSeverity__5.Format(locStr2), descShort), LocStr.Empty, 25.Percent(), 0.5.Percent(), 8, 1700));
    }

    public DiseasesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static DiseasesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      DiseasesData.DiseaseSeverity__1 = Loc.Str1(nameof (DiseaseSeverity__1), "{0} (mild)", "disease severity, this is the lowest severity 1/5, {0} is disease name");
      DiseasesData.DiseaseSeverity__2 = Loc.Str1(nameof (DiseaseSeverity__2), "{0} (moderate)", "disease severity, this is second lowest severity 2/5, {0} is disease name");
      DiseasesData.DiseaseSeverity__3 = Loc.Str1(nameof (DiseaseSeverity__3), "{0} (severe)", "disease severity, this is third lowest severity, 3/5, {0} is disease name");
      DiseasesData.DiseaseSeverity__4 = Loc.Str1(nameof (DiseaseSeverity__4), "{0} (very severe)", "disease severity, this is second highest severity 4/5, {0} is disease name");
      DiseasesData.DiseaseSeverity__5 = Loc.Str1(nameof (DiseaseSeverity__5), "{0} (deadly)", "disease severity, this is the highest severity 5/5, {0} is disease name");
    }
  }
}
