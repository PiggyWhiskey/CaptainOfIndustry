// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ConstructionProgressView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class ConstructionProgressView : IUiElement
  {
    public const int HEIGHT = 95;
    public const int HEIGHT_WITH_QUICKBUILD = 135;
    private static int BTN_HEIGHT;
    public readonly IUiUpdater Updater;
    private readonly UiBuilder m_builder;
    private readonly Panel m_container;
    private readonly StackContainer m_productsContainer;
    private readonly SimpleProgressBar m_progressBar;
    private readonly ViewsCacheHomogeneous<ConstructionProduct> m_viewsCache;
    private readonly ViewsCacheHomogeneous<IconContainer> m_plusCache;
    private Option<IConstructionProgress> m_currentInfo;
    private Panel m_progressBarHolder;
    private readonly StackContainer m_buttonsContainer;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ConstructionProgressView(
      IUiElement parent,
      UiBuilder builder,
      Func<Option<IConstructionProgress>> constructionInfoProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ConstructionProgressView constructionProgressView = this;
      this.m_builder = builder;
      this.m_plusCache = new ViewsCacheHomogeneous<IconContainer>((Func<IconContainer>) (() => builder.NewIconContainer("Plus").SetIcon(new IconStyle(builder.Style.Icons.Plus, new ColorRgba?(builder.Style.Panel.PlainIconColor)))));
      this.m_container = builder.NewPanel("ConstructionView", parent);
      this.m_buttonsContainer = builder.NewStackContainer("Btns", (IUiElement) this.m_container).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).PutToCenterBottomOf<StackContainer>((IUiElement) this.m_container, new Vector2(0.0f, 30f), Offset.Bottom(5f));
      this.m_productsContainer = builder.NewStackContainer("Container", (IUiElement) this.m_container).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).OnMouseEnter(new Action(this.onMouseEnter)).OnMouseLeave(new Action(this.onMouseLeave)).SetItemSpacing(5f);
      this.m_viewsCache = new ViewsCacheHomogeneous<ConstructionProduct>((Func<ConstructionProduct>) (() => new ConstructionProduct((IUiElement) constructionProgressView.m_productsContainer, builder)));
      this.m_progressBarHolder = builder.NewPanel("ProgressHolder", (IUiElement) this.m_container).SetBorderStyle(builder.Style.Panel.Border).PutToTopOf<Panel>((IUiElement) this.m_container, 12f, Offset.LeftRight(10f) + Offset.Top(80f));
      Txt progressTitle = builder.NewTxt("Description", (IUiElement) this.m_progressBarHolder).SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).EnableRichText().PutToTopOf<Txt>((IUiElement) this.m_progressBarHolder, 20f, Offset.Top(-20f));
      Txt percentValue = builder.NewTxt("Percent", (IUiElement) this.m_progressBarHolder).SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleRight).PutToTopOf<Txt>((IUiElement) this.m_progressBarHolder, 20f, Offset.Top(-20f));
      this.m_progressBar = new SimpleProgressBar((IUiElement) this.m_progressBarHolder, builder).AddBorder(new BorderStyle(ColorRgba.Black)).PutTo<SimpleProgressBar>((IUiElement) this.m_progressBarHolder);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<IConstructionProgress>>(constructionInfoProvider).Do((Action<Option<IConstructionProgress>>) (info =>
      {
        if (!info.HasValue)
          return;
        constructionProgressView.setProgress(info.Value);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => !constructionProgressView.m_currentInfo.HasValue ? Percent.Zero : constructionProgressView.m_currentInfo.Value.Progress)).Observe<bool>((Func<bool>) (() => constructionProgressView.m_currentInfo.HasValue && constructionProgressView.m_currentInfo.Value.WasBlockedOnProductsLastSim)).Observe<bool>((Func<bool>) (() => constructionProgressView.m_currentInfo.HasValue && constructionProgressView.m_currentInfo.Value.IsPaused)).Do((Action<Percent, bool, bool>) ((percent, wasBlocked, isPaused) =>
      {
        closure_0.m_progressBar.SetProgress(percent);
        if (isPaused)
        {
          progressTitle.SetText((LocStrFormatted) Tr.ConstructionState__Paused);
          progressTitle.SetColor(builder.Style.Global.OrangeText);
        }
        else if (wasBlocked)
        {
          if (closure_0.m_currentInfo.HasValue && closure_0.m_currentInfo.Value.IsDeconstruction)
          {
            progressTitle.SetText((LocStrFormatted) Tr.ConstructionState__WaitingForRemoval);
            progressTitle.SetColor(builder.Style.Global.OrangeText);
          }
          else
          {
            progressTitle.SetText((LocStrFormatted) Tr.ConstructionState__WaitingForDelivery);
            progressTitle.SetColor(builder.Style.Global.OrangeText);
          }
        }
        else if (percent.IsNearHundred)
        {
          progressTitle.SetText((LocStrFormatted) Tr.ConstructionState__Ready);
          progressTitle.SetColor(builder.Style.Global.GreenForDark);
        }
        else
        {
          progressTitle.SetText((LocStrFormatted) Tr.ConstructionState__InProgress);
          progressTitle.SetColor(builder.Style.Global.Text.Color);
        }
        percentValue.SetText(!closure_0.m_currentInfo.HasValue || !closure_0.m_currentInfo.Value.IsDeconstruction ? percent.ToStringRounded() : (Percent.Hundred - percent).ToStringRounded());
        closure_0.updateBarsColors(wasBlocked | isPaused);
      }));
      this.Updater = updaterBuilder.Build();
      this.Updater.AddChildUpdater(this.m_viewsCache.Updater);
    }

    public void AddCancelBtn(Action onCancel, Func<EntityValidationResult> validationFn)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Btn cancelBtn = this.m_builder.NewBtnDanger("Cancel").SetIcon("Assets/Unity/UserInterface/General/Cancel.svg").OnClick(onCancel);
      cancelBtn.AppendTo<Btn>(this.m_buttonsContainer, new float?((float) ConstructionProgressView.BTN_HEIGHT));
      Tooltip cancelTooltip = this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) cancelBtn);
      updaterBuilder.Observe<EntityValidationResult>(validationFn).Do((Action<EntityValidationResult>) (cancelCheckResult =>
      {
        cancelBtn.SetEnabled(cancelCheckResult.IsSuccess);
        cancelTooltip.SetText(cancelCheckResult.ErrorMessageForPlayer);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    public void AddPauseBtn(Action<bool> onToggle)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      UiStyle style = this.m_builder.Style;
      ToggleBtn pauseButton = this.m_builder.NewToggleBtn("PauseToggle", (IUiElement) this.m_buttonsContainer).SetButtonStyleWhenOn(style.Global.DangerBtn).SetButtonStyleWhenOff(style.Global.GeneralBtnToToggle).SetBtnIcon(style.Icons.Enable).SetOnToggleAction(onToggle);
      pauseButton.AppendTo<ToggleBtn>(this.m_buttonsContainer, new float?(28f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_currentInfo.HasValue && this.m_currentInfo.Value.IsPaused)).Do((Action<bool>) (isPaused => pauseButton.SetIsOn(isPaused)));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    public void AddQuickBuildBtn(
      IAssetTransactionManager assetManager,
      UpointsManager upointsManager,
      Action onQuickBuild)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      CostButton quickBuildBtn = new CostButton(this.m_builder, Tr.QuickBuild__Action.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg");
      quickBuildBtn.SetButtonStyle(this.m_builder.Style.Global.UpointsBtn).PlayErrorSoundWhenDisabled().OnClick(onQuickBuild).AddToolTip(Tr.QuickBuild__Tooltip).AppendTo<Btn>(this.m_buttonsContainer, new float?(0.0f));
      updaterBuilder.Observe<KeyValuePair<bool, Upoints>>((Func<KeyValuePair<bool, Upoints>>) (() =>
      {
        if (this.m_currentInfo.IsNone)
          return new KeyValuePair<bool, Upoints>(false, Upoints.Zero);
        bool hasProducts;
        Upoints upoints = this.m_currentInfo.Value.CostForQuickBuild(assetManager, out hasProducts);
        return new KeyValuePair<bool, Upoints>(hasProducts, upoints);
      })).Observe<Quantity>((Func<Quantity>) (() => upointsManager.Quantity)).Do((Action<KeyValuePair<bool, Upoints>, Quantity>) ((quickBuildPair, upointsAvailable) =>
      {
        bool key = quickBuildPair.Key;
        Upoints upoints = quickBuildPair.Value;
        if (upoints.IsPositive)
        {
          quickBuildBtn.SetCost(upoints.Value.ToStringRounded(1));
          this.m_buttonsContainer.UpdateItemWidth((IUiElement) quickBuildBtn, quickBuildBtn.GetWidth());
          quickBuildBtn.SetEnabled(key && upointsAvailable >= upoints.GetQuantityRounded());
        }
        this.m_buttonsContainer.SetItemVisibility((IUiElement) quickBuildBtn, upoints.IsPositive);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    public void AddQuickRemoveBtn(
      IAssetTransactionManager assetManager,
      UpointsManager upointsManager,
      Action onQuickRemove)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      CostButton quickRemoveBtn = new CostButton(this.m_builder, Tr.QuickRemove__Action.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg");
      quickRemoveBtn.SetButtonStyle(this.m_builder.Style.Global.UpointsBtn).PlayErrorSoundWhenDisabled().OnClick(onQuickRemove).AddToolTip(Tr.QuickRemove__Tooltip).AppendTo<Btn>(this.m_buttonsContainer, new float?(0.0f));
      updaterBuilder.Observe<Upoints?>((Func<Upoints?>) (() => !this.m_currentInfo.HasValue ? new Upoints?() : this.m_currentInfo.Value.CostForQuickRemove(assetManager))).Observe<Quantity>((Func<Quantity>) (() => upointsManager.Quantity)).Do((Action<Upoints?, Quantity>) ((quickRemoveCost, upointsAvailable) =>
      {
        if (quickRemoveCost.HasValue)
        {
          quickRemoveBtn.SetCost(quickRemoveCost.Value.Value.ToStringRounded(1));
          this.m_buttonsContainer.UpdateItemWidth((IUiElement) quickRemoveBtn, quickRemoveBtn.GetWidth());
          quickRemoveBtn.SetEnabled(upointsAvailable >= quickRemoveCost.Value.GetQuantityRounded());
        }
        this.m_buttonsContainer.SetItemVisibility((IUiElement) quickRemoveBtn, quickRemoveCost.HasValue);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    public void AddCustomBtn(Btn button) => button.AppendTo<Btn>(this.m_buttonsContainer);

    public ConstructionProgressView SetOnClick(Action onClick)
    {
      this.m_progressBarHolder.OnClick(onClick);
      return this;
    }

    public ConstructionProgressView SetBackground(ColorRgba color)
    {
      this.m_container.SetBackground(color);
      return this;
    }

    private void onMouseEnter()
    {
      foreach (ConstructionProduct allExistingOne in this.m_viewsCache.AllExistingOnes())
        allExistingOne.ShowProductName();
    }

    private void onMouseLeave()
    {
      foreach (ConstructionProduct allExistingOne in this.m_viewsCache.AllExistingOnes())
        allExistingOne.HideProductName();
    }

    private void setProgress(IConstructionProgress constructionProgress)
    {
      this.m_currentInfo = Option<IConstructionProgress>.Some(constructionProgress);
      this.m_productsContainer.ClearAll();
      this.m_plusCache.ReturnAll();
      this.m_viewsCache.ReturnAll();
      int num = 0;
      foreach (ProductQuantity product in constructionProgress.TotalCost.Products)
      {
        ProductQuantity pq = product;
        IProductBufferReadOnly buffer = constructionProgress.Buffers.FirstOrDefault((Func<IProductBufferReadOnly, bool>) (x => (Proto) x.Product == (Proto) pq.Product));
        if (buffer == null)
        {
          Assert.Fail(string.Format("Could not find buffer for product {0}", (object) pq.Product));
        }
        else
        {
          if (num > 0)
            this.m_plusCache.GetView().AppendTo<IconContainer>(this.m_productsContainer, new Vector2?(16.Vector2()), ContainerPosition.MiddleOrCenter);
          ++num;
          ConstructionProduct view = this.m_viewsCache.GetView();
          view.SetProduct(constructionProgress, buffer, pq.Quantity);
          view.AppendTo<ConstructionProduct>(this.m_productsContainer, new float?(70f));
        }
      }
      this.m_productsContainer.PutToCenterTopOf<StackContainer>((IUiElement) this.m_container, new Vector2(this.m_productsContainer.GetDynamicWidth(), 60f), Offset.Top(5f));
    }

    private void updateBarsColors(bool isBlocked)
    {
      if (this.m_currentInfo.IsNone)
        return;
      if (isBlocked)
        this.m_progressBar.SetColor((ColorRgba) 16750848);
      else
        this.m_progressBar.SetColor(this.m_currentInfo.Value.IsDeconstruction ? new ColorRgba(3564694) : new ColorRgba(4558930));
      this.m_progressBar.SetBackgroundColor(this.m_currentInfo.Value.IsDeconstruction ? new ColorRgba(6036765) : new ColorRgba(4408131));
    }

    static ConstructionProgressView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ConstructionProgressView.BTN_HEIGHT = 28;
    }
  }
}
