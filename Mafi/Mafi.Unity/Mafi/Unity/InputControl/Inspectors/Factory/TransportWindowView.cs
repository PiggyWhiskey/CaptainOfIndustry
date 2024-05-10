// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.TransportWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class TransportWindowView : StaticEntityInspectorBase<Mafi.Core.Factory.Transports.Transport>
  {
    private readonly TransportInspector m_controller;
    private readonly Dict<ProductProto, Quantity> m_aggregatedProducts;
    private ViewsCacheHomogeneous<BufferView> m_buffersCache;
    private StackContainer m_buffersContainer;
    private StatusPanel m_statusInfo;
    private Panel m_emptyInfoContainer;
    private readonly ProductsSlimIdManager m_productsSlimIdManager;

    protected override Mafi.Core.Factory.Transports.Transport Entity
    {
      get => this.m_controller.SelectedEntity;
    }

    public TransportWindowView(
      TransportInspector controller,
      IAssetTransactionManager assetsManager,
      ProductsSlimIdManager productsSlimIdManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_aggregatedProducts = new Dict<ProductProto, Quantity>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_productsSlimIdManager = productsSlimIdManager;
      this.m_controller = controller.CheckNotNull<TransportInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_statusInfo = this.AddStatusInfoPanel();
      Panel parent = this.AddOverlayPanel(itemContainer, offset: Offset.Top(10f));
      TextWithIcon throughput = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.TextControlsBold).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent, 0.0f, Offset.Left(15f));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.TransportedProducts);
      this.m_buffersContainer = this.Builder.NewStackContainer("Buffers").SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer, new float?(0.0f));
      this.m_buffersCache = new ViewsCacheHomogeneous<BufferView>((Func<BufferView>) (() => this.Builder.NewBufferView((IUiElement) this.m_buffersContainer, isCompact: true).SetAsSuperCompact()));
      this.m_emptyInfoContainer = this.Builder.NewPanel("EmptyInfo").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(this.Builder.Style.BufferView.SuperCompactHeight));
      this.Builder.NewTxt("Empty").SetText((LocStrFormatted) Tr.Empty).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_emptyInfoContainer);
      int num = 30;
      StackContainer container = this.AddButtonsSection(this.ItemsContainer, num + 10, Offset.Top(10f));
      this.Builder.NewBtnGeneral("ReverseBtn").SetIcon("Assets/Unity/UserInterface/General/Rotate128.png", new Offset?(Offset.All(5f))).AddToolTip(Tr.ToggleDirection.ToString() + " [" + this.m_controller.Context.ShortcutsManager.Flip.ToNiceString() + "]").OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<ReverseTransportCmd>(new ReverseTransportCmd(this.Entity.Id)))).AppendTo<Btn>(container, new float?((float) num), Offset.TopBottom(5f));
      Btn clearBtn = this.Builder.NewBtn("ClearBtn").SetButtonStyle(this.Builder.Style.Global.DangerBtn).SetIcon("Assets/Unity/UserInterface/General/Trash128.png", new Offset?(Offset.All(5f))).PlayErrorSoundWhenDisabled().OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<ClearTransportCmd>(new ClearTransportCmd(this.Entity.Id))));
      clearBtn.AppendTo<Btn>(container, new float?((float) num), Offset.TopBottom(5f));
      Tooltip clearTooltip = clearBtn.AddToolTipAndReturn();
      clearTooltip.SetText((LocStrFormatted) Tr.RemoveProducts__Tooltip);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsProductsRemovalInProgress)).Observe<bool>((Func<bool>) (() => this.Entity.TransportedProducts.IsNotEmpty<TransportedProductMutable>())).Do((Action<bool, bool>) ((isClearingInProgress, hasProducts) =>
      {
        clearTooltip.SetText((LocStrFormatted) (isClearingInProgress ? Tr.RemoveProducts__Stop : Tr.RemoveProducts__Tooltip));
        clearBtn.SetButtonStyle(isClearingInProgress ? this.Builder.Style.Global.DangerBtnActive : this.Builder.Style.Global.DangerBtn);
        clearBtn.SetEnabled(isClearingInProgress | hasProducts);
      }));
      updaterBuilder.Observe<TransportProto>((Func<TransportProto>) (() => this.Entity.Prototype)).Observe<bool>((Func<bool>) (() => this.Builder.DurationNormalizer.IsNormalizationOn)).Do((Action<TransportProto, bool>) ((proto, normalize) =>
      {
        Duration duration = normalize ? 60.Seconds() : 1.Seconds();
        string str = this.Builder.DurationNormalizer.NormalizeThroughput(proto.ThroughputPerTick);
        throughput.SetPrefixText(Tr.ThroughputWithParam.Format(string.Format("{0} / {1}", (object) str, (object) duration.Seconds.IntegerPart)).Value.ToUpper(LocalizationManager.CurrentCultureInfo));
      }));
      updaterBuilder.Observe<Mafi.Core.Factory.Transports.Transport.Status>((Func<Mafi.Core.Factory.Transports.Transport.Status>) (() => this.Entity.GetStatus())).Do(new Action<Mafi.Core.Factory.Transports.Transport.Status>(this.updateStatusInfo));
      this.AddUpdater(updaterBuilder.Build());
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_aggregatedProducts.Clear();
      foreach (TransportedProductMutable transportedProduct in this.Entity.TransportedProducts)
      {
        ProductProto key = this.m_productsSlimIdManager.ResolveOrPhantom(transportedProduct.SlimId);
        Quantity quantity;
        if (this.m_aggregatedProducts.TryGetValue(key, out quantity))
          this.m_aggregatedProducts[key] = quantity + transportedProduct.Quantity;
        else
          this.m_aggregatedProducts.Add(key, transportedProduct.Quantity);
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.updateProducts();
    }

    private void updateProducts()
    {
      Quantity quantity = this.Entity.Trajectory.MaxProducts * this.Entity.Prototype.MaxQuantityPerTransportedProduct;
      if (this.m_buffersCache.AllExistingOnes().Count == this.m_aggregatedProducts.Count)
      {
        int index = 0;
        foreach (KeyValuePair<ProductProto, Quantity> aggregatedProduct in this.m_aggregatedProducts)
        {
          ProductProto key = aggregatedProduct.Key;
          this.m_buffersCache.AllExistingOnes()[index].UpdateState(key, quantity.Max(aggregatedProduct.Value), aggregatedProduct.Value);
          ++index;
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
          view.AppendTo<BufferView>(this.m_buffersContainer, new float?(this.Builder.Style.BufferView.SuperCompactHeight));
          view.UpdateState(key, quantity.Max(aggregatedProduct.Value), aggregatedProduct.Value);
        }
        this.m_buffersContainer.FinishBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_emptyInfoContainer, this.m_aggregatedProducts.IsEmpty<KeyValuePair<ProductProto, Quantity>>());
      }
    }

    private void updateStatusInfo(Mafi.Core.Factory.Transports.Transport.Status status)
    {
      switch (status)
      {
        case Mafi.Core.Factory.Transports.Transport.Status.Idle:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
          break;
        case Mafi.Core.Factory.Transports.Transport.Status.NotConnected:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__NotConnected, StatusPanel.State.Warning);
          break;
        case Mafi.Core.Factory.Transports.Transport.Status.Moving:
          this.m_statusInfo.SetStatusWorking();
          break;
        case Mafi.Core.Factory.Transports.Transport.Status.Paused:
          this.m_statusInfo.SetStatusPaused();
          break;
        case Mafi.Core.Factory.Transports.Transport.Status.PowerLow:
          this.m_statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
          break;
      }
    }
  }
}
