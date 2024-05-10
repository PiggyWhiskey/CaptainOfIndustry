// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.EdictProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  public class EdictProto : Proto, IProtoWithIcon, IProto
  {
    public static readonly string[] ROMAN_NUMERALS;
    public readonly EdictCategoryProto Category;
    /// <summary>If negative, the edict actually generates unity.</summary>
    public readonly Upoints MonthlyUpointsCost;
    public readonly Type Implementation;
    public readonly EdictProto.Gfx Graphics;
    public readonly Option<EdictProto> PreviousTier;
    public readonly bool IsGeneratingUnity;

    public string IconPath => this.Graphics.IconPath;

    public Option<EdictProto> NextTier { get; private set; }

    public bool IsAdvanced => this.PreviousTier.HasValue;

    public EdictProto(
      Proto.ID id,
      Proto.Str strings,
      EdictCategoryProto category,
      Upoints monthlyUpointsCost,
      Type edictImplementation,
      EdictProto.Gfx graphics,
      bool? isGeneratingUnity,
      Option<EdictProto>? previousTier = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Category = category;
      this.MonthlyUpointsCost = monthlyUpointsCost;
      this.Implementation = edictImplementation.CheckNotNull<Type>();
      this.IsGeneratingUnity = ((int) isGeneratingUnity ?? (monthlyUpointsCost.IsNegative ? 1 : 0)) != 0;
      this.PreviousTier = previousTier ?? Option<EdictProto>.None;
      this.Graphics = graphics;
      if (isGeneratingUnity.HasValue || !this.MonthlyUpointsCost.IsZero)
        return;
      Log.Error(string.Format("You should specify if edict '{0}' is Unity positive or not.", (object) id));
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      EdictProto[] array = protosDb.All<EdictProto>().Where<EdictProto>((Func<EdictProto, bool>) (x => x.PreviousTier == this)).ToArray<EdictProto>();
      this.NextTier = array.Length <= 1 ? (Option<EdictProto>) ((IEnumerable<EdictProto>) array).FirstOrDefault<EdictProto>() : throw new ProtoInitException(string.Format("Edict '{0}' cannot have multiple next tiers, you can only chain one by one!", (object) this.Id));
    }

    static EdictProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EdictProto.ROMAN_NUMERALS = new string[10]
      {
        "I",
        "II",
        "III",
        "IV",
        "V",
        "VI",
        "VII",
        "VIII",
        "IX",
        "X"
      };
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly EdictProto.Gfx Empty;

      public string IconPath { get; private set; }

      public Gfx(string iconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconPath = iconPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        EdictProto.Gfx.Empty = new EdictProto.Gfx(string.Empty);
      }
    }
  }
}
