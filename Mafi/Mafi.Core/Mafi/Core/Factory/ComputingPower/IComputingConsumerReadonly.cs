// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.IComputingConsumerReadonly
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Factory.ComputingPower
{
  public interface IComputingConsumerReadonly
  {
    bool IsEnabled { get; }

    int Priority { get; }

    bool NotEnoughComputing { get; }

    IComputingConsumingEntity Entity { get; }

    bool DidConsumeLastTick { get; }

    Computing ComputingCharged { get; }

    Computing ComputingRequired { get; }
  }
}
