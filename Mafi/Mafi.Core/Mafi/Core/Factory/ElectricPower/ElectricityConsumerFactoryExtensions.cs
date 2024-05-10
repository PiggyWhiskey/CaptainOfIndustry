// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ElectricityConsumerFactoryExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  public static class ElectricityConsumerFactoryExtensions
  {
    public static Option<IElectricityConsumer> CreateConsumerIfNeeded(
      this IElectricityConsumerFactory factory,
      IElectricityConsumingEntity entity,
      Option<IElectricityConsumer> currentConsumer)
    {
      if (currentConsumer.HasValue)
        return currentConsumer;
      return entity.PowerRequired.IsPositive ? factory.CreateConsumer(entity).SomeOption<IElectricityConsumer>() : Option<IElectricityConsumer>.None;
    }
  }
}
