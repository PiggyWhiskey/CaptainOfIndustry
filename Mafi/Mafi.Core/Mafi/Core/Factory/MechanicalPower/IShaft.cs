// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.MechanicalPower.IShaft
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Ports;

#nullable disable
namespace Mafi.Core.Factory.MechanicalPower
{
  public interface IShaft
  {
    bool IsDefaultNoCapacityShaft { get; }

    IProductBufferReadOnly InertiaBuffer { get; }

    /// <summary>
    /// Total available power incl. input, output, and inertia buffer.
    /// </summary>
    MechPower TotalAvailablePower { get; }

    Percent CurrentInertia { get; }

    Percent CurrentInputScale { get; }

    Percent CurrentOutputScale { get; }

    Percent ThroughputUtilization { get; }

    bool OutputAllowed { get; }

    bool InputAllowed { get; }

    bool IsDestroyed { get; }

    Quantity GetRemoveAmount(Quantity quantity, Quantity maxOutput);

    IReadOnlySet<IEntityWithPorts> ConnectedEntities { get; }
  }
}
