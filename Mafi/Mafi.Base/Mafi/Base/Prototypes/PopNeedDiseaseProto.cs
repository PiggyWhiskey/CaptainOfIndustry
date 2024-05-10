// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.PopNeedDiseaseProto
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
  public class PopNeedDiseaseProto : DiseaseProto
  {
    public readonly PopNeedProto Need;

    public override Option<Type> CustomTrigger => (Option<Type>) typeof (PopNeedDiseaseTrigger);

    public PopNeedDiseaseProto(
      Proto.ID id,
      Proto.Str strings,
      Percent healthPenalty,
      Percent monthlyMortalityRate,
      int durationInMonths,
      PopNeedProto need,
      LocStr reason)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, reason, healthPenalty, monthlyMortalityRate, durationInMonths, 0);
      this.Need = need;
    }
  }
}
