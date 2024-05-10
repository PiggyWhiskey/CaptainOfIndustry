// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.ProtosFilterEditor`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  /// <summary>
  /// A filtered products editor that is especially designed to work with inspector windows.
  /// </summary>
  public class ProtosFilterEditor<T> where T : class, IProtoWithIconAndName
  {
    private readonly ProtoPicker<T> m_protoPicker;
    private readonly BtnWithGridContainer m_filteredProducts;
    private readonly ProtoWithIcon<T>.Cache m_viewsCache;
    private readonly Func<IEnumerable<T>> m_availableProtosProvider;
    private readonly Func<IEnumerable<T>> m_assignedProtosProvider;
    private readonly Lyst<T> m_visibleProductsCache;
    private readonly Btn m_btn;
    private readonly Tooltip m_btnTooltip;

    public ProtosFilterEditor(
      UiBuilder builder,
      IWindowWithInnerWindowsSupport parentWindow,
      StackContainer parentContainer,
      Action<T> onProtoRemoved,
      Action<T> onProtoAdded,
      Func<IEnumerable<T>> availableProtosProvider,
      Func<IEnumerable<T>> assignedProtosProvider,
      int columnsCount = 5,
      bool usePrimaryBtnStyle = true)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_visibleProductsCache = new Lyst<T>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProtosFilterEditor<T> protosFilterEditor = this;
      this.m_availableProtosProvider = availableProtosProvider;
      this.m_assignedProtosProvider = assignedProtosProvider;
      this.m_protoPicker = new ProtoPicker<T>((Action<T>) (x =>
      {
        onProtoAdded(x);
        protosFilterEditor.m_protoPicker.Hide();
      }));
      this.m_btn = builder.NewBtn("PlusBtn").OnClick(new Action(this.plusBtnClicked)).SetButtonStyle(usePrimaryBtnStyle ? builder.Style.Global.PrimaryBtn : builder.Style.Global.GeneralBtn).SetIcon(builder.Style.Icons.Plus, new Offset?(Offset.All(10f)));
      this.m_btnTooltip = this.m_btn.AddToolTipAndReturn();
      this.m_filteredProducts = builder.NewBtnWithGridContainer(builder.Style.ProductWithIcon.Size).SetNumberOfColumns(columnsCount).AppendTo<BtnWithGridContainer>(parentContainer, new float?(0.0f));
      this.m_filteredProducts.AddBtn(this.m_btn);
      this.m_filteredProducts.SetupInnerWindowWithButton(parentWindow, (WindowView) this.m_protoPicker, new Action(this.plusBtnClicked), this.m_btn);
      this.m_viewsCache = new ProtoWithIcon<T>.Cache((IUiElement) this.m_filteredProducts.ItemsContainer, builder, onProtoRemoved);
      parentWindow.OnHide += (Action) (() => protosFilterEditor.m_protoPicker.Hide());
    }

    public void SetBtnEnabled(bool isEnabled, LocStrFormatted tooltip)
    {
      this.m_btn.SetEnabled(isEnabled);
      this.m_btnTooltip.SetText(tooltip);
    }

    public void SetTextToShowWhenEmpty(string text)
    {
      this.m_filteredProducts.SetTextToShowWhenEmpty(text);
    }

    public float GetRequiredWidth() => this.m_filteredProducts.GetRequiredWidth();

    public void UpdateFilteredProtos(Lyst<T> filteredProducts)
    {
      this.m_filteredProducts.StartBatchOperation();
      this.m_filteredProducts.ItemsContainer.ClearAllAndHide();
      foreach (T filteredProduct in filteredProducts)
        this.m_viewsCache.GetView(filteredProduct).EnableRemoveHoverEffect().AppendTo<ProtoWithIcon<T>>(this.m_filteredProducts.ItemsContainer);
      this.m_filteredProducts.FinishBatchOperation();
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_visibleProductsCache.Clear();
        this.m_visibleProductsCache.AddRange(this.m_availableProtosProvider());
        foreach (T obj in this.m_assignedProtosProvider())
          this.m_visibleProductsCache.Remove(obj);
        this.m_protoPicker.SetVisibleProtos((IEnumerable<T>) this.m_visibleProductsCache);
        this.m_protoPicker.Show();
      }
    }
  }
}
