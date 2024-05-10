// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.LiftWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Factory.Lifts;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class LiftWindowView : StaticEntityInspectorBase<Lift>
  {
    private readonly LiftInspector m_inspector;
    private BufferView.Cache m_buffersCache;
    private readonly Lyst<KeyValuePair<ProductProto, Quantity>> m_aggregatedProducts;
    private Quantity m_capacitySync;
    private StackContainer m_buffersContainer;
    private Panel m_emptyInfoContainer;

    protected override Lift Entity => this.m_inspector.SelectedEntity;

    public LiftWindowView(LiftInspector inspector)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_aggregatedProducts = new Lyst<KeyValuePair<ProductProto, Quantity>>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) inspector);
      this.m_inspector = inspector;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.TransportedProducts);
      this.m_buffersContainer = this.Builder.NewStackContainer("Buffers", (IUiElement) itemContainer).SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer, new float?(0.0f));
      this.m_emptyInfoContainer = this.Builder.NewPanel("EmptyInfo", (IUiElement) itemContainer).SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(this.Builder.Style.BufferView.Height));
      int num = 30;
      StackContainer container = this.AddButtonsSection(this.ItemsContainer, num + 10, Offset.Top(10f));
      this.Builder.NewBtnGeneral("ReverseBtn").SetIcon("Assets/Unity/UserInterface/General/Rotate128.png", new Offset?(Offset.All(5f))).AddToolTip(Tr.ToggleDirection.ToString() + " [" + this.m_inspector.Context.ShortcutsManager.Flip.ToNiceString() + "]").OnClick((Action) (() => this.m_inspector.InputScheduler.ScheduleInputCmd<ReverseLiftCmd>(new ReverseLiftCmd(this.Entity.Id)))).AppendTo<Btn>(container, new float?((float) num), Offset.TopBottom(5f));
      this.Builder.NewTxt("Empty", (IUiElement) this.m_emptyInfoContainer).SetText((LocStrFormatted) Tr.Empty).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_emptyInfoContainer);
      this.m_buffersCache = new BufferView.Cache((IUiElement) this.m_buffersContainer, this.Builder);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_aggregatedProducts.Clear();
      this.Entity.GetAllBufferedProducts(this.m_aggregatedProducts);
      this.m_capacitySync = this.Entity.MaxBufferSize;
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.updateProducts();
    }

    private void updateProducts()
    {
      if (this.m_buffersCache.AllExistingOnes().Count == this.m_aggregatedProducts.Count)
      {
        for (int index = 0; index < this.m_aggregatedProducts.Count; ++index)
        {
          KeyValuePair<ProductProto, Quantity> aggregatedProduct = this.m_aggregatedProducts[index];
          this.m_buffersCache.AllExistingOnes()[index].UpdateState(aggregatedProduct.Key, this.m_capacitySync.Max(aggregatedProduct.Value), aggregatedProduct.Value);
        }
      }
      else
      {
        this.m_buffersContainer.StartBatchOperation();
        this.m_buffersContainer.ClearAll();
        this.m_buffersCache.ReturnAll();
        foreach (KeyValuePair<ProductProto, Quantity> aggregatedProduct in this.m_aggregatedProducts)
        {
          ProductProto key = aggregatedProduct.Key;
          BufferView view = this.m_buffersCache.GetView();
          view.Show<BufferView>();
          view.AppendTo<BufferView>(this.m_buffersContainer, new float?(this.Builder.Style.BufferView.Height));
          view.UpdateState(key, this.m_capacitySync.Max(aggregatedProduct.Value), aggregatedProduct.Value);
        }
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_emptyInfoContainer, this.m_aggregatedProducts.IsEmpty<KeyValuePair<ProductProto, Quantity>>());
        this.m_buffersContainer.FinishBatchOperation();
      }
    }
  }
}
