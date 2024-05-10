// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPortData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  public readonly struct IoPortData
  {
    public static IoPortData Invalid;
    public readonly char Name;
    public readonly ProductType AllowedProductType;
    public readonly byte PortIndex;
    public readonly Option<IEntityWithPorts> ConnectedTo;
    private readonly IoPortToken m_connectedPortToken;

    public bool IsValid => this.Name > char.MinValue;

    /// <summary>Connected status of this port.</summary>
    public bool IsConnected => this.ConnectedTo.HasValue;

    /// <summary>Whether this port is not connected to any other port.</summary>
    public bool IsNotConnected => !this.ConnectedTo.HasValue;

    public IoPortData(IoPort port)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Name = port.Spec.Name;
      this.PortIndex = port.PortIndex;
      this.AllowedProductType = port.Spec.Shape.AllowedProductType;
      Option<IoPort> connectedPort = port.ConnectedPort;
      this.ConnectedTo = (connectedPort.ValueOrNull?.OwnerEntity ?? (IEntityWithPorts) null).CreateOption<IEntityWithPorts>();
      connectedPort = port.ConnectedPort;
      this.m_connectedPortToken = connectedPort.HasValue ? new IoPortToken(port.ConnectedPort.Value) : IoPortToken.Invalid;
    }

    /// <summary>
    /// Sends as much as possible from given buffer and returns sent amount.
    /// </summary>
    [Pure]
    public Quantity SendAsMuchAsFromBuffer(IProductBuffer buffer)
    {
      Quantity quantity = buffer.Quantity - this.SendAsMuchAs(buffer.Product.WithQuantity(buffer.Quantity));
      buffer.RemoveExactly(quantity);
      return quantity;
    }

    /// <summary>
    /// Sends as much of given product as possible to the connected port. Returns remaining non-sent product.
    /// </summary>
    /// <remarks>It is OK to try to send stuff when this port is not connected or connected as input.</remarks>
    [Pure]
    [MustUseReturnValue]
    public Quantity SendAsMuchAs(ProductQuantity pq)
    {
      if (pq.IsEmpty || !this.ConnectedTo.HasValue)
        return pq.Quantity;
      if ((Proto) pq.Product == (Proto) null)
      {
        Log.Error("Sending null product!");
        return pq.Quantity;
      }
      if (pq.Product.IsPhantom)
      {
        Log.Error("Sending phantom product!");
        return pq.Quantity;
      }
      if (pq.Product.Type != this.AllowedProductType)
      {
        Log.Error(string.Format("Sending incompatible product {0}", (object) pq.Product));
        return pq.Quantity;
      }
      if (this.ConnectedTo.Value.IsDestroyed)
      {
        Log.Error("Sending to destroyed entity!");
        return pq.Quantity;
      }
      Quantity newQuantity;
      Quantity quantity;
      if (pq.Quantity <= IoPort.MAX_TRANSFER_PER_TICK)
      {
        newQuantity = pq.Quantity;
        quantity = Quantity.Zero;
      }
      else
      {
        newQuantity = IoPort.MAX_TRANSFER_PER_TICK;
        quantity = pq.Quantity - IoPort.MAX_TRANSFER_PER_TICK;
      }
      Quantity expected = quantity + this.ConnectedTo.Value.ReceiveAsMuchAsFromPort(pq.WithNewQuantity(newQuantity), this.m_connectedPortToken);
      Assert.That<Quantity>(pq.Quantity).IsGreaterOrEqual(expected, "Receive returned more than was given to it.");
      return expected;
    }

    public void SendAsMuchAs(ref ProductQuantity pq)
    {
      pq = pq.WithNewQuantity(this.SendAsMuchAs(pq));
    }

    static IoPortData() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
  }
}
