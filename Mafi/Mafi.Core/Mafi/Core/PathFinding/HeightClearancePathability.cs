// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.HeightClearancePathability
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.PathFinding
{
  /// <summary>Requirements for height clearance.</summary>
  public enum HeightClearancePathability : uint
  {
    IgnoreClearance = 0,
    /// <summary>
    /// Can pass under entities that have at least <see cref="F:Mafi.Core.PathFinding.ClearancePathabilityProvider.BASIC_HEIGHT_CLEARANCE" />
    /// tiles of clearance.
    /// </summary>
    CanPassUnder = 8,
    /// <summary>Cannot pass under entities.</summary>
    NoPassingUnder = 12, // 0x0000000C
  }
}
