// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.QuickTrade.AnimalQuickTradeHandler
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.World.QuickTrade
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class AnimalQuickTradeHandler : IVirtualProductQuickTradeHandler
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IProductsManager m_productsManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public static void Serialize(AnimalQuickTradeHandler value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnimalQuickTradeHandler>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnimalQuickTradeHandler.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
    }

    public static AnimalQuickTradeHandler Deserialize(BlobReader reader)
    {
      AnimalQuickTradeHandler quickTradeHandler;
      if (reader.TryStartClassDeserialization<AnimalQuickTradeHandler>(out quickTradeHandler))
        reader.EnqueueDataDeserialization((object) quickTradeHandler, AnimalQuickTradeHandler.s_deserializeDataDelayedAction);
      return quickTradeHandler;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<AnimalQuickTradeHandler>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<AnimalQuickTradeHandler>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.RegisterResolvedMember<AnimalQuickTradeHandler>(this, "m_protosDb", typeof (ProtosDb), true);
    }

    public LocStrFormatted MessageOnDelivery
    {
      get => (LocStrFormatted) TrCore.TradeOfferDelivered__Animal;
    }

    public LocStrFormatted DescriptionOfTrade => (LocStrFormatted) TrCore.TradeStatus__Info_Animal;

    public AnimalQuickTradeHandler(
      IProductsManager productsManager,
      IEntitiesManager entitiesManager,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productsManager = productsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public bool IsProductManaged(ProductProto product)
    {
      return this.m_protosDb.All<AnimalFarmProto>().Any<AnimalFarmProto>((Func<AnimalFarmProto, bool>) (x => (Proto) x.Animal == (Proto) product));
    }

    public bool CanBuy(ProductProto product, Quantity quantityToBuy, out LocStrFormatted error)
    {
      int num = this.m_entitiesManager.GetAllEntitiesOfType<AnimalFarm>().Sum<AnimalFarm>((Func<AnimalFarm, int>) (x => !x.IsEnabled || !((Proto) x.Prototype.Animal == (Proto) product) ? 0 : x.CapacityLeft));
      if (quantityToBuy.Value > num)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__NoSpaceInFarm;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    public void StoreBoughtProduct(ProductProto product, Quantity quantityToBuy)
    {
      IEnumerable<AnimalFarm> allEntitiesOfType = this.m_entitiesManager.GetAllEntitiesOfType<AnimalFarm>();
      int count = quantityToBuy.Value;
      foreach (AnimalFarm animalFarm in allEntitiesOfType)
      {
        if (animalFarm.IsEnabled && !((Proto) animalFarm.Prototype.Animal != (Proto) product) && animalFarm.CapacityLeft > 0)
        {
          if (count > 0)
            count -= animalFarm.AddAnimals(count);
          else
            break;
        }
      }
      Quantity quantity = quantityToBuy - count.Quantity();
      this.m_productsManager.ProductCreated(product, quantity, CreateReason.QuickTrade);
    }

    static AnimalQuickTradeHandler()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnimalQuickTradeHandler.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AnimalQuickTradeHandler) obj).SerializeData(writer));
      AnimalQuickTradeHandler.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AnimalQuickTradeHandler) obj).DeserializeData(reader));
    }
  }
}
