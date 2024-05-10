// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.ComputingConsumerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ComputingPower
{
  public static class ComputingConsumerExtensions
  {
    public static void ConsumeAndAssert(this IComputingConsumer consumer)
    {
      Assert.That<bool>(consumer.TryConsume()).IsTrue();
    }

    /// <summary>
    /// Will skip notification for entity when computing not available.
    /// Only used for transports as it is strange to notify when they still work no matter what.
    /// </summary>
    public static bool CanConsumeDoNotNotify(this IComputingConsumerReadonly consumer)
    {
      return consumer.ComputingCharged >= consumer.ComputingRequired;
    }

    public static bool CanConsume(this Option<IComputingConsumer> consumer)
    {
      IComputingConsumer valueOrNull = consumer.ValueOrNull;
      return valueOrNull == null || valueOrNull.CanConsume();
    }

    public static bool TryConsume(this Option<IComputingConsumer> consumer)
    {
      IComputingConsumer valueOrNull = consumer.ValueOrNull;
      return valueOrNull == null || valueOrNull.TryConsume();
    }
  }
}
