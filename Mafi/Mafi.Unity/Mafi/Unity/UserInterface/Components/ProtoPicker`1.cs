// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ProtoPicker`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class ProtoPicker<T> : WindowView where T : class, IProtoWithIcon
  {
    private static readonly char[] SEARCH_QUERY_SEPARATOR;
    private GridContainer m_gridContainer;
    private TxtField m_searchBox;
    private Txt m_nothingFoundInfo;
    private ScrollableContainer m_scrollCont;
    private float m_windowWidth;
    private float m_minWindowWidth;
    private float m_maxWindowHeight;
    private ProtoWithIcon<T>.Cache m_viewsCache;
    private readonly Action<T> m_onProtoClick;
    private readonly Func<T, LocStrFormatted> m_tooltipProvider;
    private readonly bool m_useBigIcons;
    private readonly Lyst<ProtoWithIcon<T>> m_visibleProtos;

    public ProtoPicker(
      Action<T> onProtoClick,
      Func<T, LocStrFormatted> tooltipProvider = null,
      bool useBigIcons = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_visibleProtos = new Lyst<ProtoWithIcon<T>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ProtoPicker<T>));
      this.m_onProtoClick = onProtoClick;
      this.m_tooltipProvider = tooltipProvider;
      this.m_useBigIcons = useBigIcons;
    }

    protected override void BuildWindowContent()
    {
      this.SetHeaderAlignment(TextAnchor.MiddleLeft, Offset.Left(20f));
      this.SetHeaderHeight(38f);
      this.SetTitle((LocStrFormatted) Tr.ProductSelectorTitle);
      this.m_scrollCont = this.Builder.NewScrollableContainer("ScrollableCont").AddVerticalScrollbar().SetScrollSensitivity(this.Style.ProductWithIcon.Size.y);
      this.m_gridContainer = this.Builder.NewGridContainer("GridContainer").SetDynamicHeightMode(this.m_useBigIcons ? 6 : 8).SetCellSize(this.m_useBigIcons ? 90.Vector2() : this.Style.ProductWithIcon.Size).SetCellSpacing(4f).SetInnerPadding(Offset.All(this.Style.Panel.Padding));
      this.m_viewsCache = new ProtoWithIcon<T>.Cache((IUiElement) this.m_gridContainer, this.Builder, this.m_onProtoClick, true);
      this.m_windowWidth = this.m_gridContainer.GetRequiredWidth() + this.m_scrollCont.ScrollbarWidth;
      this.m_minWindowWidth = this.m_gridContainer.ComputeHeightFor(2);
      this.m_maxWindowHeight = this.m_gridContainer.ComputeHeightFor(4);
      this.m_scrollCont.AddItemTop((IUiElement) this.m_gridContainer);
      this.m_scrollCont.PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel());
      Txt txt = this.Builder.NewTxt("NothingFound").SetAlignment(TextAnchor.MiddleCenter);
      TextStyle text = this.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable1 = new int?(16);
      FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = nullable2;
      int? fontSize = nullable1;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_nothingFoundInfo = txt.SetTextStyle(textStyle).PutToTopOf<Txt>((IUiElement) this.GetContentPanel(), 100f, Offset.Top(20f)).Hide<Txt>();
      this.m_searchBox = this.AddSearchBoxToHeader();
      this.m_searchBox.SetDelayedOnEditEndListener(new Action<string>(this.search));
      this.OnShowDone += (Action) (() => this.m_searchBox.Focus());
      this.OnHide += (Action) (() => this.m_searchBox.ClearInput());
    }

    public void SetVisibleProtos(IEnumerable<T> protos, bool alreadySorted = false)
    {
      this.m_nothingFoundInfo.Hide<Txt>();
      this.m_visibleProtos.Clear();
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.ClearAllAndHide();
      foreach (T data in alreadySorted ? protos : (IEnumerable<T>) protos.OrderBy<T, string>((Func<T, string>) (x => x.Strings.Name.TranslatedString)))
      {
        ProtoWithIcon<T> view = this.m_viewsCache.GetView(data);
        if (this.m_tooltipProvider != null)
          view.SetTooltip(this.m_tooltipProvider(data));
        this.m_visibleProtos.Add(view);
        this.m_gridContainer.Append((IUiElement) view);
      }
      this.m_gridContainer.FinishBatchOperation();
      this.SetContentSize(this.m_windowWidth, this.m_gridContainer.GetHeight().Clamp(this.m_minWindowWidth, this.m_maxWindowHeight));
    }

    private void search(string text)
    {
      this.m_nothingFoundInfo.Hide<Txt>();
      string[] query = text.Trim().ToLower(LocalizationManager.CurrentCultureInfo).Split(ProtoPicker<T>.SEARCH_QUERY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
      this.m_gridContainer.StartBatchOperation();
      this.m_gridContainer.HideAllItems();
      foreach (ProtoWithIcon<T> visibleProto in this.m_visibleProtos)
      {
        if (visibleProto.Matches(query))
          this.m_gridContainer.ShowItem((IUiElement) visibleProto);
      }
      this.m_gridContainer.FinishBatchOperation();
      if (this.m_gridContainer.VisibleItemsCount != 0)
        return;
      this.m_nothingFoundInfo.SetText(Tr.NothingFoundFor.Format(text)).Show<Txt>();
    }

    static ProtoPicker()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ProtoPicker<T>.SEARCH_QUERY_SEPARATOR = new char[1]
      {
        ' '
      };
    }
  }
}
