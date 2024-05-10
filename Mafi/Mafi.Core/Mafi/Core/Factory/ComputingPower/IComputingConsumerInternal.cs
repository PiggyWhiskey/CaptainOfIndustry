// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.IComputingConsumerInternal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Factory.ComputingPower
{
  public interface IComputingConsumerInternal : 
    IComputingConsumer,
    IComputingConsumerReadonly,
    IComparable<IComputingConsumer>
  {
    /// <summary>
    /// Unique id of consumer Proto for fast collection of electricity stats.
    /// Set by ElectricityManager. Do not change, do not save.
    /// </summary>
    int ProtoToken { get; set; }

    /// <summary>
    /// Called by computing manager to recharge this consumer.
    /// </summary>
    void Recharge(Computing computingToAdd);

    /// <summary>
    /// Called by computing manager when recharging gets skipped.
    /// Either it was not needed or resources were missing.
    /// </summary>
    void RechargeSkipped();
  }
}
