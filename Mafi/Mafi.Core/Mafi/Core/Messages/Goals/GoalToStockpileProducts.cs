// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToStockpileProducts
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToStockpileProducts : Goal
  {
    private readonly GoalToStockpileProducts.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private Quantity m_storedCurrently;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToStockpileProducts(
      GoalToStockpileProducts.Proto goalProto,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      Quantity quantity = this.m_entitiesManager.GetAllEntitiesOfType<Storage>((Predicate<Storage>) (x => x.IsConstructed && x.StoredProduct.HasValue && x.StoredProduct.Value.Id == this.m_goalProto.ProductToStoreId)).Sum<Storage>((Func<Storage, int>) (x => x.CurrentQuantity.Value)).Quantity();
      if (quantity != this.m_storedCurrently)
      {
        this.m_storedCurrently = quantity;
        this.updateTitle();
      }
      return this.m_storedCurrently >= this.m_goalProto.QuantityToStockpile;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.Value + string.Format("{0} / {1}", (object) this.m_storedCurrently, (object) this.m_goalProto.QuantityToStockpile);
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToStockpileProducts value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToStockpileProducts>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToStockpileProducts.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToStockpileProducts.Proto>(this.m_goalProto);
      Quantity.Serialize(this.m_storedCurrently, writer);
    }

    public static GoalToStockpileProducts Deserialize(BlobReader reader)
    {
      GoalToStockpileProducts stockpileProducts;
      if (reader.TryStartClassDeserialization<GoalToStockpileProducts>(out stockpileProducts))
        reader.EnqueueDataDeserialization((object) stockpileProducts, GoalToStockpileProducts.s_deserializeDataDelayedAction);
      return stockpileProducts;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToStockpileProducts>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToStockpileProducts>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToStockpileProducts.Proto>());
      this.m_storedCurrently = Quantity.Deserialize(reader);
    }

    static GoalToStockpileProducts()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToStockpileProducts.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToStockpileProducts.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public static readonly LocStr2 TITLE_STORE;
      public readonly LocStrFormatted Title;
      public readonly Quantity QuantityToStockpile;
      public readonly ProductProto.ID ProductToStoreId;

      public override Type Implementation => typeof (GoalToStockpileProducts);

      public Proto(
        string id,
        LocStrFormatted title,
        Quantity quantityToStockpile,
        ProductProto.ID productToStoreId,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
        this.Title = title;
        this.QuantityToStockpile = quantityToStockpile;
        this.ProductToStoreId = productToStoreId;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToStockpileProducts.Proto.TITLE_STORE = Loc.Str2("Goal__FillStorage", "Store {0} in {1} ", "goal text, {0} - product name, {1} - storage");
      }
    }
  }
}
