// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ConstructionProduct
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class ConstructionProduct : IUiElement, IUiElementWithUpdater
  {
    public const int HEIGHT = 60;
    public const int WIDTH = 70;
    private readonly Panel m_container;
    private readonly IconContainer m_icon;
    private readonly TitleTooltip m_productName;
    private IProductBufferReadOnly m_buffer;
    private IConstructionProgress m_currentProgress;
    private Quantity m_requiredQuantity;
    private Txt m_quantityTxt;
    private IconContainer m_doneIcon;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public ConstructionProduct(IUiElement parent, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("ProductWithIcon", parent);
      this.m_icon = builder.NewIconContainer("Icon").PutTo<IconContainer>((IUiElement) this.m_container, Offset.Bottom(20f));
      this.m_doneIcon = builder.NewIconContainer("FullIcon").SetIcon("Assets/Unity/UserInterface/General/Tick128.png").SetColor(new ColorRgba(4498757)).PutToRightBottomOf<IconContainer>((IUiElement) this.m_icon, 26.Vector2());
      this.m_productName = new TitleTooltip(builder);
      this.m_quantityTxt = builder.NewTitle("Quantity").SetAlignment(TextAnchor.MiddleCenter).EnableRichText().PutToBottomOf<Txt>((IUiElement) this.m_container, 20f);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => this.m_buffer.Quantity)).Observe<Quantity>((Func<Quantity>) (() => this.m_currentProgress.AlreadyRemovedCost.GetQuantityOf(this.m_buffer.Product))).Observe<bool>((Func<bool>) (() => this.m_currentProgress.WasBlockedOnProductsLastSim)).Do(new Action<Quantity, Quantity, bool>(this.update));
      this.Updater = updaterBuilder.Build();
    }

    private void update(Quantity quantity, Quantity quantityReduction, bool wasBlocked)
    {
      this.m_quantityTxt.SetText(wasBlocked && (!this.m_currentProgress.IsDeconstruction && quantity < this.m_requiredQuantity || this.m_currentProgress.IsDeconstruction && quantity.IsPositive) ? string.Format("<color=#FF9900>{0}</color> / {1}", (object) quantity, (object) (this.m_requiredQuantity - quantityReduction)) : string.Format("{0} / {1}", (object) quantity, (object) (this.m_requiredQuantity - quantityReduction)));
      this.m_doneIcon.SetVisibility<IconContainer>(!this.m_currentProgress.IsDeconstruction && quantity >= this.m_requiredQuantity);
    }

    public void SetProduct(
      IConstructionProgress progress,
      IProductBufferReadOnly buffer,
      Quantity requiredQuantity)
    {
      this.m_currentProgress = progress;
      this.m_buffer = buffer;
      this.m_requiredQuantity = progress.IsDeconstruction ? Quantity.Zero : requiredQuantity;
      this.m_productName.SetText((LocStrFormatted) this.m_buffer.Product.Strings.Name);
      this.m_icon.SetIcon(this.m_buffer.Product.Graphics.IconPath);
      this.HideProductName();
      this.update(buffer.Quantity, this.m_currentProgress.AlreadyRemovedCost.GetQuantityOf(this.m_buffer.Product), progress.WasBlockedOnProductsLastSim);
    }

    public void ShowProductName() => this.m_productName.Show((IUiElement) this.m_container);

    public void HideProductName() => this.m_productName.Hide();
  }
}
