// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.TechnologyProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Research
{
  [DebuggerDisplay("TechnologyProto: {Id}")]
  public class TechnologyProto : Proto, IProtoWithIcon, IProto
  {
    public readonly TechnologyProto.Gfx Graphics;

    public string IconPath => this.Graphics.IconPath;

    public TechnologyProto(Proto.ID id, Proto.Str strings, TechnologyProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Graphics = graphics;
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly TechnologyProto.Gfx Empty;

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
        TechnologyProto.Gfx.Empty = new TechnologyProto.Gfx("");
      }
    }
  }
}
