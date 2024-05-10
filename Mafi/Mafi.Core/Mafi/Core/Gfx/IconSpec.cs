// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Gfx.IconSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Gfx
{
  public readonly struct IconSpec : IEquatable<IconSpec>
  {
    public readonly string Path;
    public readonly ColorRgba Color;

    public IconSpec(string path, ColorRgba color)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Path = path;
      this.Color = color;
    }

    public bool Equals(IconSpec other) => this.Path == other.Path && this.Color.Equals(other.Color);

    public override bool Equals(object obj) => obj is IconSpec other && this.Equals(other);

    public override int GetHashCode() => Hash.Combine<string, ColorRgba>(this.Path, this.Color);
  }
}
