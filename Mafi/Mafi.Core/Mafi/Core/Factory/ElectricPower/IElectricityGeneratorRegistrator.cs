// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.IElectricityGeneratorRegistrator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  public interface IElectricityGeneratorRegistrator
  {
    /// <summary>
    /// Priority for generation. Lower number means that the entity will be prioritized more for generation.
    /// For example, solar panels should have higher priority than generators.
    /// </summary>
    int GenerationPriority { get; set; }

    bool IsSurplusGenerator { get; set; }

    /// <summary>
    /// Maximum generation capacity if the generator works on 100% of its max capacity.
    /// </summary>
    Electricity MaxGenerationCapacity { get; }

    /// <summary>
    /// Current generation capacity with respect to current conditions.
    /// </summary>
    Electricity GenerationCapacityThisTick { get; }

    /// <summary>Current generation.</summary>
    Electricity GeneratedThisTick { get; }
  }
}
