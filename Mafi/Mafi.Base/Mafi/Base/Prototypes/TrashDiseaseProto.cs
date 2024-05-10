// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.TrashDiseaseProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes
{
  public class TrashDiseaseProto : DiseaseProto
  {
    public override Option<Type> CustomTrigger => (Option<Type>) typeof (TrashDiseaseTrigger);

    public TrashDiseaseProto(
      Proto.ID id,
      Proto.Str strings,
      LocStr reason,
      Percent healthPenalty,
      Percent monthlyMortalityRate,
      int durationInMonths)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, reason, healthPenalty, monthlyMortalityRate, durationInMonths, 0);
    }
  }
}
