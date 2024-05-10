// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.Settlements.ProductBufferViewWithPicker
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings.Settlements
{
  internal class ProductBufferViewWithPicker : IUiElementWithUpdater, IUiElement
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly Func<IEntityWithMultipleProductsToAssign> m_entityProvider;
    private readonly Panel m_container;
    private readonly ProtoPicker<ProductProto> m_protoPicker;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public float RequiredHeight { get; }

    public int Slot { get; set; }

    public IUiUpdater Updater { get; }

    public ProductBufferViewWithPicker(
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ItemDetailWindowView owner,
      Action<ProductProto, int> onSetProductToStore,
      Action<int> onProductRemove,
      Func<IEntityWithMultipleProductsToAssign> entityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductBufferViewWithPicker bufferViewWithPicker = this;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_entityProvider = entityProvider;
      this.RequiredHeight = builder.Style.BufferView.Height;
      this.m_container = builder.NewPanel(nameof (ProductBufferViewWithPicker));
      BufferView bufferView = builder.NewBufferView((IUiElement) this.m_container, (Action) (() => onProductRemove(bufferViewWithPicker.Slot)), new Action(this.plusBtnClicked)).PutTo<BufferView>((IUiElement) this.m_container);
      this.m_protoPicker = new ProtoPicker<ProductProto>((Action<ProductProto>) (proto =>
      {
        onSetProductToStore(proto, bufferViewWithPicker.Slot);
        bufferViewWithPicker.m_protoPicker.Hide();
      }));
      owner.SetupProductPickerWithBuffer(this.m_protoPicker, bufferView);
      owner.OnHide += (Action) (() => bufferViewWithPicker.m_protoPicker.Hide());
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() =>
      {
        IProductBuffer valueOrNull = entityProvider().GetBuffer(bufferViewWithPicker.Slot).ValueOrNull;
        return valueOrNull == null ? (Option<ProductProto>) Option.None : valueOrNull.Product.SomeOption<ProductProto>();
      })).Observe<Quantity>((Func<Quantity>) (() => entityProvider().GetCapacity(bufferViewWithPicker.Slot))).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBuffer valueOrNull = entityProvider().GetBuffer(bufferViewWithPicker.Slot).ValueOrNull;
        return valueOrNull == null ? 0.Quantity() : valueOrNull.Quantity;
      })).Do((Action<Option<ProductProto>, Quantity, Quantity>) ((product, capacity, quantity) => bufferView.UpdateState(product, capacity, quantity)));
      this.Updater = updaterBuilder.Build();
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_protoPicker.SetVisibleProtos(this.m_unlockedProtosDb.FilterUnlocked<ProductProto>(this.m_entityProvider().SupportedProducts.AsEnumerable()));
        this.m_protoPicker.Show();
      }
    }

    public void SetSlot(int slot) => this.Slot = slot;

    public class Cache : ViewsCacheHomogeneous<ProductBufferViewWithPicker>
    {
      public Cache(
        UiBuilder builder,
        UnlockedProtosDbForUi unlockedProtosDb,
        ItemDetailWindowView owner,
        Action<ProductProto, int> onSetProductToStore,
        Action<int> onProductRemove,
        Func<IEntityWithMultipleProductsToAssign> entityProvider)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Func<ProductBufferViewWithPicker>) (() => new ProductBufferViewWithPicker(builder, unlockedProtosDb, owner, onSetProductToStore, onProductRemove, entityProvider)));
      }
    }
  }
}
