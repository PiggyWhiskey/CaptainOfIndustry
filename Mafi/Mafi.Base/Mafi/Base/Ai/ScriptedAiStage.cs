// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Ai.ScriptedAiStage
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

#nullable disable
namespace Mafi.Base.Ai
{
  public enum ScriptedAiStage
  {
    Latest = -1, // 0xFFFFFFFF
    Start = 0,
    InitialIronAndLab = 1,
    FirstFarm = 2,
    VehiclesAndCp = 3,
    PowerAndMaintenance = 4,
    IronOreMiningAndSmelting = 5,
    CoalMiningAndBeacon = 6,
    ScalingUp = 7,
    ConcreteProduction = 8,
    Diesel = 9,
    CopperRefinement = 10, // 0x0000000A
    MoreIron = 11, // 0x0000000B
    Cp2Assembly = 12, // 0x0000000C
    CementProduction = 13, // 0x0000000D
    ConveyorsAndFuel = 14, // 0x0000000E
    ShipRepair = 15, // 0x0000000F
    SettlementWater = 16, // 0x00000010
  }
}
