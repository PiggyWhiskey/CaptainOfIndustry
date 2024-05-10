// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays.MaintenanceDisplay
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Maintenance;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays
{
  internal class MaintenanceDisplay : IUiElementWithUpdater, IUiElement
  {
    public const float WIDTH = 90f;
    private readonly Panel m_container;

    public IUiUpdater Updater { get; }

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ProductProto Product { get; set; }

    public MaintenanceDisplay(
      IMaintenanceBufferReadonly buffer,
      UiBuilder builder,
      StackContainer parent)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      MaintenanceDisplay maintenanceDisplay = this;
      this.Product = buffer.Product;
      this.m_container = builder.NewPanel("ProductDisplay").SetWidth<Panel>(90f).SetHeight<Panel>(30f);
      builder.NewIconContainer("ProductIcon").SetIcon(buffer.Product.Graphics.IconPath).PutToLeftBottomOf<IconContainer>((IUiElement) this.m_container, new Vector2(24f, 24f));
      SimpleProgressBar storageState = new SimpleProgressBar((IUiElement) this.m_container, builder).SetColor(builder.Style.Global.GreenForDark).SetBackgroundColor(new ColorRgba(4473924)).PutToLeftBottomOf<SimpleProgressBar>((IUiElement) this.m_container, new Vector2(60f, 18f), Offset.Left(30f) + Offset.Bottom(2f));
      Txt amount = builder.NewTxt("Amount").SetText("0").AllowHorizontalOverflow().AllowVerticalOverflow().SetTextStyle(new TextStyle()
      {
        FontStyle = FontStyle.Bold,
        FontSize = 14,
        Color = new ColorRgba(14277081)
      }).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) storageState);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => buffer.Quantity)).Do((Action<Quantity, Quantity>) ((capacity, stored) =>
      {
        Percent progress = capacity.IsPositive ? Percent.FromRatio(stored.Value, capacity.Value) : Percent.Zero;
        amount.SetText(progress.ToStringRounded());
        storageState.SetProgress(progress);
        storageState.SetColor((ColorRgba) (progress < 50.Percent() ? 10431790 : 1274889));
      }));
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => buffer.Quantity)).Do((Action<Quantity, Quantity>) ((capacity, stored) =>
      {
        Percent progress = capacity.IsPositive ? Percent.FromRatio(stored.Value, capacity.Value) : Percent.Zero;
        amount.SetText(progress.ToStringRounded());
        storageState.SetProgress(progress);
        storageState.SetColor((ColorRgba) (progress < 50.Percent() ? 10431790 : 1274889));
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => buffer.ShouldShowInUi)).Do((Action<bool>) (show => parent.SetItemVisibility((IUiElement) maintenanceDisplay, show)));
      this.Updater = updaterBuilder.Build();
      this.SetSize<MaintenanceDisplay>(new Vector2(90f, 30f));
    }
  }
}
