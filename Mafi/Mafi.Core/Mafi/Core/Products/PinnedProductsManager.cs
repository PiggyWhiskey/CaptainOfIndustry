// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.PinnedProductsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Products
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class PinnedProductsManager : 
    ICommandProcessor<PinToggleCmd>,
    IAction<PinToggleCmd>,
    ICommandProcessor<PinnedProductReorderCmd>,
    IAction<PinnedProductReorderCmd>
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly ProtosDb m_protosDb;
    public readonly Lyst<ProductProto> AllPinnedProducts;
    private readonly Set<ProductProto> m_unpinnedProducts;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public event Action OnPinnedProductsChanged;

    public PinnedProductsManager(UnlockedProtosDb unlockedProtosDb, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_unpinnedProducts = new Set<ProductProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_protosDb = protosDb;
      this.AllPinnedProducts = unlockedProtosDb.AllUnlocked<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (p => p.PinToHomeScreenByDefault)).ToLyst<ProductProto>();
      this.m_unlockedProtosDb.OnUnlockedSetChanged.Add<PinnedProductsManager>(this, new Action(this.onProtoUnlocked));
    }

    public bool IsPinned(ProductProto productProto)
    {
      return this.AllPinnedProducts.Contains(productProto);
    }

    private void onProtoUnlocked()
    {
      bool flag = false;
      foreach (ProductProto productProto in this.m_unlockedProtosDb.AllUnlocked<ProductProto>())
      {
        if (productProto.PinToHomeScreenByDefault && !this.m_unpinnedProducts.Contains(productProto))
          flag |= this.AllPinnedProducts.AddIfNotPresent(productProto);
      }
      if (!flag)
        return;
      Action pinnedProductsChanged = this.OnPinnedProductsChanged;
      if (pinnedProductsChanged == null)
        return;
      pinnedProductsChanged();
    }

    public void Invoke(PinToggleCmd cmd)
    {
      Option<ProductProto> option = this.m_protosDb.Get<ProductProto>((Proto.ID) cmd.ProductToToggle);
      if (option.IsNone)
      {
        cmd.SetResultError(string.Format("Could not find product with id {0}", (object) cmd.ProductToToggle));
      }
      else
      {
        ProductProto productProto = option.Value;
        if (this.AllPinnedProducts.Contains(productProto))
        {
          this.AllPinnedProducts.Remove(productProto);
          this.m_unpinnedProducts.Add(productProto);
        }
        else
        {
          this.AllPinnedProducts.Add(productProto);
          this.m_unpinnedProducts.Remove(productProto);
        }
        Action pinnedProductsChanged = this.OnPinnedProductsChanged;
        if (pinnedProductsChanged != null)
          pinnedProductsChanged();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(PinnedProductReorderCmd cmd)
    {
      ProductProto proto;
      if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductToMove, out proto))
      {
        cmd.SetResultError(string.Format("Could not find product with id {0}", (object) cmd.ProductToMove));
      }
      else
      {
        if (PinnedProductsManager.MoveProductInList(this.AllPinnedProducts, proto, cmd.NewIndex))
          cmd.SetResultSuccess();
        else
          cmd.SetResultError("Invalid indices");
        Action pinnedProductsChanged = this.OnPinnedProductsChanged;
        if (pinnedProductsChanged == null)
          return;
        pinnedProductsChanged();
      }
    }

    public static bool MoveProductInList(
      Lyst<ProductProto> listToUse,
      ProductProto productToMove,
      int indexToMoveTo)
    {
      int index = listToUse.IndexOf(productToMove);
      if (indexToMoveTo < 0 || index < 0)
        return false;
      if (indexToMoveTo == index)
        return true;
      listToUse.RemoveAt(index);
      if (index < indexToMoveTo && indexToMoveTo >= listToUse.Count)
      {
        listToUse.Add(productToMove);
        return true;
      }
      listToUse.Insert(indexToMoveTo, productToMove);
      return true;
    }

    public static void Serialize(PinnedProductsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PinnedProductsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PinnedProductsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<ProductProto>.Serialize(this.AllPinnedProducts, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      Set<ProductProto>.Serialize(this.m_unpinnedProducts, writer);
    }

    public static PinnedProductsManager Deserialize(BlobReader reader)
    {
      PinnedProductsManager pinnedProductsManager;
      if (reader.TryStartClassDeserialization<PinnedProductsManager>(out pinnedProductsManager))
        reader.EnqueueDataDeserialization((object) pinnedProductsManager, PinnedProductsManager.s_deserializeDataDelayedAction);
      return pinnedProductsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PinnedProductsManager>(this, "AllPinnedProducts", (object) Lyst<ProductProto>.Deserialize(reader));
      reader.RegisterResolvedMember<PinnedProductsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<PinnedProductsManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<PinnedProductsManager>(this, "m_unpinnedProducts", (object) Set<ProductProto>.Deserialize(reader));
    }

    static PinnedProductsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PinnedProductsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PinnedProductsManager) obj).SerializeData(writer));
      PinnedProductsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PinnedProductsManager) obj).DeserializeData(reader));
    }
  }
}
