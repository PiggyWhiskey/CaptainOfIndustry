// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.IElectricityGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  public interface IElectricityGenerator
  {
    Electricity MaxGenerationCapacity { get; }

    /// <summary>
    /// Returns current maximum electricity generation. If <paramref name="canGenerate" /> is <c>false</c>, the
    /// <see cref="M:Mafi.Core.Factory.ElectricPower.IElectricityGenerator.GenerateAsMuchAs(Mafi.Electricity,Mafi.Electricity)" /> should not be called and returned capacity is ignored (set to zero).
    /// This can happen for example when the generator is disabled or broken. If it is set to <c>true</c>,
    /// the <see cref="M:Mafi.Core.Factory.ElectricPower.IElectricityGenerator.GenerateAsMuchAs(Mafi.Electricity,Mafi.Electricity)" /> will be called even if returned max generation is zero.
    /// </summary>
    Electricity GetCurrentMaxGeneration(out bool canGenerate);

    /// <summary>
    /// Generates as much electricity as requested. If this entity cannot scale production, it may return more than
    /// requested.
    /// The <see cref="!:currentMaxGeneration" /> is returned value from <see cref="M:Mafi.Core.Factory.ElectricPower.IElectricityGenerator.GetCurrentMaxGeneration(System.Boolean@)" />.
    /// </summary>
    /// <remarks>
    /// Note that this generation is requested when the internal electricity buffer is not full, despite this entity
    /// may not need to be generating because other higher-priority entities are already covering all electricity
    /// consumption.
    /// 
    /// This means that entity should NOT keep consuming some material and not returning any power because it can
    /// get stuck in this state indefinitely.
    /// </remarks>
    Electricity GenerateAsMuchAs(Electricity freeCapacity, Electricity currentMaxGeneration);
  }
}
