// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.IElectricityConsumerReadonly
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  public interface IElectricityConsumerReadonly
  {
    bool IsEnabled { get; }

    int Priority { get; }

    bool NotEnoughPower { get; }

    bool DidConsumeLastTick { get; }

    IElectricityConsumingEntity Entity { get; }

    bool IsSurplusConsumer { get; }

    Electricity PowerCharged { get; }

    Electricity PowerRequired { get; }
  }
}
