﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.BirthRateCategoryProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Population
{
  /// <summary>Used for statistics and overview table in UI.</summary>
  public class BirthRateCategoryProto : Proto
  {
    public readonly BirthRateCategoryProto.Gfx Graphics;
    private Proto.ID? m_otherProtoString;

    public LocStr Title { get; private set; }

    public BirthRateCategoryProto(
      Proto.ID id,
      Proto.Str strings,
      BirthRateCategoryProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Graphics = graphics;
      this.Title = this.Strings.Name;
    }

    public BirthRateCategoryProto(Proto.ID otherProtoStrings, string iconPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Proto.ID("BirthRateCat_" + otherProtoStrings.Value), Proto.Str.Empty);
      this.Graphics = new BirthRateCategoryProto.Gfx(iconPath);
      this.m_otherProtoString = new Proto.ID?(otherProtoStrings);
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      if (!this.m_otherProtoString.HasValue)
        return;
      this.Title = protosDb.GetOrThrow<Proto>(this.m_otherProtoString.Value).Strings.Name;
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly BirthRateCategoryProto.Gfx Empty;

      /// <summary>Icon asset path to be used in UI.</summary>
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
        BirthRateCategoryProto.Gfx.Empty = new BirthRateCategoryProto.Gfx("EMPTY");
      }
    }
  }
}
