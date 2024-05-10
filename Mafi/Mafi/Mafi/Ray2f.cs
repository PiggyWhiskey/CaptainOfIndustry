// Decompiled with JetBrains decompiler
// Type: Mafi.Ray2f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [DebuggerStepThrough]
  public readonly struct Ray2f
  {
    /// <summary>Ray origin.</summary>
    public readonly Vector2f Origin;
    /// <summary>Normalized direction of the ray.</summary>
    public readonly Vector2f Direction;

    /// <summary>Creates a ray starting at origin along direction.</summary>
    public Ray2f(Vector2f origin, Vector2f direction)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Origin = origin;
      this.Direction = direction.Normalized;
    }

    /// <summary>Returns a point at distance units along the ray.</summary>
    public Vector2f GetPoint(Percent distance) => this.Origin + distance * this.Direction;
  }
}
