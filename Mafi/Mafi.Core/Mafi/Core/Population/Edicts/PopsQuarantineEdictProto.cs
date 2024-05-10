// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.PopsQuarantineEdictProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  public class PopsQuarantineEdictProto : EdictProto
  {
    /// <summary>
    /// Reduced both health and mortality effect of a disease.
    /// </summary>
    public readonly Percent DiseaseReduction;
    /// <summary>Percentage of population to withhold from working.</summary>
    public readonly Percent WorkersToWithhold;

    public PopsQuarantineEdictProto(
      Proto.ID id,
      Proto.Str strings,
      EdictCategoryProto category,
      Upoints monthlyUpointsCost,
      Type edictImplementation,
      Percent diseaseReduction,
      Percent workersToWithhold,
      EdictProto.Gfx graphics,
      bool? isGeneratingUnity = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, category, monthlyUpointsCost, edictImplementation, graphics, isGeneratingUnity);
      this.DiseaseReduction = diseaseReduction;
      this.WorkersToWithhold = workersToWithhold;
    }
  }
}
