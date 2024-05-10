// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.IElectricityConsumerInternal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  public interface IElectricityConsumerInternal : 
    IElectricityConsumer,
    IElectricityConsumerReadonly,
    IComparable<IElectricityConsumer>
  {
    /// <summary>
    /// Unique id of consumer Proto for fast collection of electricity stats.
    /// Set by ElectricityManager. Do not change, do not save.
    /// </summary>
    int ProtoToken { get; set; }

    /// <summary>
    /// Called by electricity manager to recharge this consumer.
    /// </summary>
    void Recharge(Electricity powerToAdd);

    /// <summary>
    /// Called by electricity manager when recharging gets skipped.
    /// Either it was not needed or resources were missing.
    /// </summary>
    void RechargeSkipped();

    /// <summary>Do not set without ElectricityManager.</summary>
    void SetIsSurplusConsumer(bool isSurplusConsumer);
  }
}
