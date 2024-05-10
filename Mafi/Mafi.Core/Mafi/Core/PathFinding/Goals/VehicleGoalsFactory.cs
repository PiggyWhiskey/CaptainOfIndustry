// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.VehicleGoalsFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  /// <summary>Simplifies creation of all vehicle goals.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class VehicleGoalsFactory
  {
    private readonly TilePositionVehicleGoal.Factory m_tileGoalFactory;
    private readonly StaticEntityVehicleGoal.Factory m_staticEntityGoalFactory;
    private readonly DynamicEntityVehicleGoal.Factory m_dynEntityGoalFactory;

    public VehicleGoalsFactory(
      TilePositionVehicleGoal.Factory tileGoalFactory,
      StaticEntityVehicleGoal.Factory staticEntityGoalFactory,
      DynamicEntityVehicleGoal.Factory dynEntityGoalFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_tileGoalFactory = tileGoalFactory;
      this.m_staticEntityGoalFactory = staticEntityGoalFactory;
      this.m_dynEntityGoalFactory = dynEntityGoalFactory;
    }

    public TilePositionVehicleGoal CreateGoal(Tile2i position, RelTile1i tolerance)
    {
      return this.m_tileGoalFactory.Create(position, new RelTile1i?(tolerance));
    }

    public StaticEntityVehicleGoal CreateGoal(IStaticEntity staticEntity, bool useCustomTarget = false)
    {
      return this.m_staticEntityGoalFactory.Create(staticEntity, useCustomTarget);
    }

    public DynamicEntityVehicleGoal CreateGoal(Vehicle vehicle)
    {
      return this.m_dynEntityGoalFactory.Create(vehicle);
    }
  }
}
