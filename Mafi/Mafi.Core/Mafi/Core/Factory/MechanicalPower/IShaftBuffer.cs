// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.MechanicalPower.IShaftBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static;

#nullable disable
namespace Mafi.Core.Factory.MechanicalPower
{
  public interface IShaftBuffer : IProductBuffer, IProductBufferReadOnly
  {
    IShaft Shaft { get; }

    bool IsDestroyed { get; }

    MechPower RemoveAsMuchAs(MechPower mechPower);

    /// <summary>
    /// Returns amount that will be returned with <see cref="M:Mafi.Core.Entities.Static.IProductBuffer.RemoveAsMuchAs(Mafi.Quantity)" /> method.
    /// This takes shaft inertia into an account.
    /// </summary>
    MechPower GetRemoveAmount(MechPower maxMechPower);
  }
}
