// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.NeedsList
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  internal class NeedsList : IDynamicSizeElement, IUiElement
  {
    private readonly StackContainer m_needsContainer;

    public IUiUpdater Updater { get; }

    public GameObject GameObject => this.m_needsContainer.GameObject;

    public RectTransform RectTransform => this.m_needsContainer.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public NeedsList(
      IUiElement parent,
      UiBuilder builder,
      Func<ImmutableArray<PopNeed>> needsProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      NeedsList needsList = this;
      this.m_needsContainer = builder.NewStackContainer("ServicesContainer", parent).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f);
      ViewsCacheHomogeneous<NeedsList.NeedStatusInfoView> needsViewsCache = new ViewsCacheHomogeneous<NeedsList.NeedStatusInfoView>((Func<NeedsList.NeedStatusInfoView>) (() => new NeedsList.NeedStatusInfoView(builder, needsList.m_needsContainer)));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Lyst<PopNeed> needsCache = new Lyst<PopNeed>();
      updaterBuilder.Observe<PopNeed>((Func<IIndexable<PopNeed>>) (() =>
      {
        needsCache.Clear();
        foreach (PopNeed popNeed in needsProvider())
        {
          if (popNeed.ShouldBeShown)
            needsCache.Add(popNeed);
        }
        return (IIndexable<PopNeed>) needsCache;
      }), (ICollectionComparator<PopNeed, IIndexable<PopNeed>>) CompareFixedOrder<PopNeed>.Instance).Do((Action<Lyst<PopNeed>>) (needs =>
      {
        closure_0.m_needsContainer.StartBatchOperation();
        closure_0.m_needsContainer.ClearAll();
        needsViewsCache.ReturnAll();
        foreach (PopNeed need in needs)
        {
          NeedsList.NeedStatusInfoView view = needsViewsCache.GetView();
          view.SetNeed(need);
          closure_0.m_needsContainer.Append((IUiElement) view, new float?(30f));
        }
        closure_0.m_needsContainer.FinishBatchOperation();
      }));
      this.Updater = updaterBuilder.Build();
      this.Updater.AddChildUpdater(needsViewsCache.Updater);
      this.m_needsContainer.SizeChanged += (Action<IUiElement>) (el =>
      {
        Action<IUiElement> sizeChanged = needsList.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) needsList);
      });
    }

    internal class NeedStatusInfoView : IUiElementWithUpdater, IUiElement
    {
      private readonly BalanceInfoView m_balanceView;

      public GameObject GameObject => this.m_balanceView.GameObject;

      public RectTransform RectTransform => this.m_balanceView.RectTransform;

      public PopNeed Need { get; private set; }

      public IUiUpdater Updater { get; }

      public NeedStatusInfoView(UiBuilder builder, StackContainer parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        this.m_balanceView = new BalanceInfoView(builder, "Assets/Unity/UserInterface/General/UnitySmall.svg");
        updaterBuilder.Observe<Upoints>((Func<Upoints>) (() => this.Need.UnityAfterLastUpdate)).Observe<Upoints>((Func<Upoints>) (() => this.Need.PossibleMaxAfterLastUpdate)).Do((Action<Upoints, Upoints>) ((unity, max) =>
        {
          this.m_balanceView.SetUnityDiff(unity, new Upoints?(max));
          if (this.Need.UnityAfterLastUpdate >= this.Need.PossibleMaxAfterLastUpdate)
            this.m_balanceView.SetPositiveClr();
          else
            this.m_balanceView.SetWarningClr();
        }));
        this.Updater = updaterBuilder.Build();
      }

      public void SetNeed(PopNeed need)
      {
        this.Need = need;
        this.m_balanceView.SetIcon(need.Proto.Graphics.IconPath);
        this.m_balanceView.SetTitle(this.Need.Proto.Strings.Name.TranslatedString);
      }
    }
  }
}
