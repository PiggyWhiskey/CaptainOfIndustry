// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ElectricityConsumerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  public static class ElectricityConsumerExtensions
  {
    public static void ConsumeAndAssert(this IElectricityConsumer consumer)
    {
      Assert.That<bool>(consumer.TryConsume()).IsTrue();
    }

    public static bool CanConsume(this Option<IElectricityConsumer> consumer, bool doNotNotify = false)
    {
      IElectricityConsumer valueOrNull = consumer.ValueOrNull;
      return valueOrNull == null || valueOrNull.CanConsume(doNotNotify);
    }

    public static bool TryConsume(this Option<IElectricityConsumer> consumer, bool doNotNotify = false)
    {
      IElectricityConsumer valueOrNull = consumer.ValueOrNull;
      return valueOrNull == null || valueOrNull.TryConsume(doNotNotify);
    }
  }
}
