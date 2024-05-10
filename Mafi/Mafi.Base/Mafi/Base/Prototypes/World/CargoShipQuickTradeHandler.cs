// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.World.CargoShipQuickTradeHandler
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Products;
using Mafi.Core.World.QuickTrade;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.World
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class CargoShipQuickTradeHandler : IVirtualProductQuickTradeHandler
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly CargoDepotManager m_cargoDepotManager;

    public static void Serialize(CargoShipQuickTradeHandler value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShipQuickTradeHandler>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShipQuickTradeHandler.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      CargoDepotManager.Serialize(this.m_cargoDepotManager, writer);
    }

    public static CargoShipQuickTradeHandler Deserialize(BlobReader reader)
    {
      CargoShipQuickTradeHandler quickTradeHandler;
      if (reader.TryStartClassDeserialization<CargoShipQuickTradeHandler>(out quickTradeHandler))
        reader.EnqueueDataDeserialization((object) quickTradeHandler, CargoShipQuickTradeHandler.s_deserializeDataDelayedAction);
      return quickTradeHandler;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CargoShipQuickTradeHandler>(this, "m_cargoDepotManager", (object) CargoDepotManager.Deserialize(reader));
    }

    public LocStrFormatted MessageOnDelivery
    {
      get => (LocStrFormatted) TrCore.TradeOfferDelivered__CargoShip;
    }

    public LocStrFormatted DescriptionOfTrade
    {
      get => (LocStrFormatted) TrCore.TradeStatus__Info_CargoShip;
    }

    public CargoShipQuickTradeHandler(CargoDepotManager cargoDepotManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cargoDepotManager = cargoDepotManager;
    }

    public bool IsProductManaged(ProductProto product) => product.Id == Ids.Products.CargoShip;

    public bool CanBuy(ProductProto product, Quantity quantityToBuy, out LocStrFormatted error)
    {
      error = LocStrFormatted.Empty;
      return true;
    }

    public void StoreBoughtProduct(ProductProto product, Quantity quantityToBuy)
    {
      if (quantityToBuy.IsNotPositive)
      {
        Log.Error("Cargo ship count is not positive!");
      }
      else
      {
        this.m_cargoDepotManager.ReportNewCargoShipFound(quantityToBuy.Value);
        this.m_cargoDepotManager.ReportNewCargoShipRepaired(quantityToBuy.Value);
      }
    }

    static CargoShipQuickTradeHandler()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      CargoShipQuickTradeHandler.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CargoShipQuickTradeHandler) obj).SerializeData(writer));
      CargoShipQuickTradeHandler.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CargoShipQuickTradeHandler) obj).DeserializeData(reader));
    }
  }
}
