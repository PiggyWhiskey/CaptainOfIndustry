// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleStats
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public struct VehicleStats : IEquatable<VehicleStats>
  {
    /// <summary>Number of owned vehicles.</summary>
    public int Owned;
    /// <summary>Number of vehicles that can be assigned.</summary>
    public int Assignable;

    public bool Equals(VehicleStats other)
    {
      return this.Owned == other.Owned && this.Assignable == other.Assignable;
    }

    public override bool Equals(object obj)
    {
      return obj != null && obj is VehicleStats other && this.Equals(other);
    }

    public override int GetHashCode() => this.Owned * 397 ^ this.Assignable;
  }
}
