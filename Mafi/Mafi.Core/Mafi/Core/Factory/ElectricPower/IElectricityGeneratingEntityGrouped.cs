// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.IElectricityGeneratingEntityGrouped
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  /// <summary>
  /// Multiple entities with the same Proto grouped into one generator (e.g. solar panels).
  /// </summary>
  public interface IElectricityGeneratingEntityGrouped : IElectricityGenerator
  {
    /// <summary>Number of enabled entities in this group.</summary>
    int GeneratorsTotal { get; }

    /// <summary>The proto that is aggregated in this group.</summary>
    IEntityProto Prototype { get; }
  }
}
