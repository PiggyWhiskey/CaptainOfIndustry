// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.OceanAreaValidationMetadata
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public sealed class OceanAreaValidationMetadata : IAddRequestMetadata
  {
    public RectangleTerrainArea2i Area { get; private set; }

    public ColorRgba Color { get; private set; }

    public float StripesScale { get; private set; }

    public float StripesAngle { get; private set; }

    public OceanAreaValidationMetadata(
      RectangleTerrainArea2i area,
      ColorRgba color,
      float stripesScale,
      float stripesAngle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Area = area;
      this.Color = color;
      this.StripesScale = stripesScale;
      this.StripesAngle = stripesAngle;
    }

    public void Recycle()
    {
    }
  }
}
