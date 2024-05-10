// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.SteepnessPathability
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.PathFinding
{
  /// <summary>Requirements for height clearance.</summary>
  public enum SteepnessPathability : uint
  {
    IgnoreSlope = 0,
    /// <summary>Can drive on slight slopes.</summary>
    SlightSlopeAllowed = 2,
    /// <summary>Cannot drive on slopes.</summary>
    NoSlopeAllowed = 3,
  }
}
