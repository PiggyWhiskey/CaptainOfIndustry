// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Logistics.JobStatsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Logistics
{
  /// <summary>Displays a grid of products and quantities.</summary>
  internal class JobStatsView : IUiElement, IDynamicSizeElement
  {
    private readonly VehicleJobStatsManager m_jobStatsManager;
    private readonly ViewsCacheHomogeneous<ProductQuantityWithIcon> m_viewsCache;
    private readonly GridContainer m_itemsContainer;
    private readonly Btn m_container;
    private readonly Txt m_noItemsText;
    private bool m_dataChanged;
    public static readonly int[] MonthRanges;
    private bool m_includeMiningJobs;
    private bool m_includeGeneralJobs;
    private bool m_includeRefuelingJobs;
    private int m_numberOfMonthsToShow;
    private readonly Dict<ProductProto, JobStatsView.StatsData> m_sumData;
    private int m_lastSimStepSync;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public bool IncludeMiningJobs
    {
      get => this.m_includeMiningJobs;
      set
      {
        this.m_includeMiningJobs = value;
        this.m_dataChanged = true;
      }
    }

    public bool IncludeGeneralJobs
    {
      get => this.m_includeGeneralJobs;
      set
      {
        this.m_includeGeneralJobs = value;
        this.m_dataChanged = true;
      }
    }

    public bool IncludeRefuelingJobs
    {
      get => this.m_includeRefuelingJobs;
      set
      {
        this.m_includeRefuelingJobs = value;
        this.m_dataChanged = true;
      }
    }

    public int NumberOfMonthsToShow
    {
      get => this.m_numberOfMonthsToShow;
      set
      {
        this.m_numberOfMonthsToShow = value;
        this.m_dataChanged = true;
      }
    }

    public JobStatsView(
      IUiElement parent,
      UiBuilder builder,
      VehicleJobStatsManager jobStatsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_includeMiningJobs = true;
      this.m_includeGeneralJobs = true;
      this.m_includeRefuelingJobs = true;
      this.m_numberOfMonthsToShow = 3;
      this.m_sumData = new Dict<ProductProto, JobStatsView.StatsData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      JobStatsView jobStatsView = this;
      this.m_jobStatsManager = jobStatsManager;
      this.m_container = builder.NewBtn("Products", parent).SetButtonStyle(new BtnStyle(backgroundClr: new ColorRgba?(builder.Style.Panel.ItemOverlay), normalMaskClr: new ColorRgba?((ColorRgba) 16777215), hoveredMaskClr: new ColorRgba?((ColorRgba) 16777215), pressedMaskClr: new ColorRgba?((ColorRgba) 16777215)));
      this.m_noItemsText = builder.NewTxt("NoItemsInfo", (IUiElement) this.m_container).SetTextStyle(builder.Style.Global.Text).SetText((LocStrFormatted) Tr.Empty).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_container).Hide<Txt>();
      this.m_itemsContainer = builder.NewGridContainer("Grid").SetCellSize(75.Vector2()).SetCellSpacing(5f).SetDynamicHeightMode(7).PutToLeftTopOf<GridContainer>((IUiElement) this.m_container, 0.Vector2(), Offset.Top(5f));
      this.m_viewsCache = new ViewsCacheHomogeneous<ProductQuantityWithIcon>((Func<ProductQuantityWithIcon>) (() => new ProductQuantityWithIcon((IUiElement) jobStatsView.m_itemsContainer, builder)));
    }

    public void SyncUpdate(GameTime gameTime)
    {
      if (!((Environment.TickCount - this.m_lastSimStepSync).Abs() > 2.Seconds().Millis) && !this.m_dataChanged)
        return;
      this.m_lastSimStepSync = Environment.TickCount;
      this.m_dataChanged = false;
      this.m_sumData.Clear();
      if (this.IncludeGeneralJobs)
        addDataFrom(this.m_jobStatsManager.GeneralJobsStats);
      if (this.IncludeMiningJobs)
        addDataFrom(this.m_jobStatsManager.MiningJobsStats);
      if (this.IncludeRefuelingJobs)
        addDataFrom(this.m_jobStatsManager.RefuelingJobsStats);
      this.setData((IEnumerable<JobStatsView.StatsData>) this.m_sumData.Values.OrderByDescending<JobStatsView.StatsData, Quantity>((Func<JobStatsView.StatsData, Quantity>) (x => x.QuantityTotal)));

      void addDataFrom(
        IIndexable<Dict<ProductProto, JobStatistics>> stats)
      {
        int num = stats.Count - this.m_numberOfMonthsToShow.Min(stats.Count);
        for (int index = stats.Count - 1; index >= num; --index)
        {
          foreach (JobStatistics jobStatistics in stats[index].Values)
          {
            if (!jobStatistics.Quantity.IsNotPositive)
            {
              ref JobStatsView.StatsData local = ref this.m_sumData.GetRefValue(jobStatistics.Product, out bool _);
              local.Product = jobStatistics.Product;
              local.JobsCount += jobStatistics.JobsCount;
              local.QuantityTotal += jobStatistics.Quantity;
            }
          }
        }
      }
    }

    public void RenderUpdate(GameTime gameTime)
    {
    }

    private void setData(IEnumerable<JobStatsView.StatsData> data)
    {
      this.m_itemsContainer.StartBatchOperation();
      this.m_itemsContainer.ClearAll();
      this.m_viewsCache.ReturnAll();
      bool flag = false;
      foreach (JobStatsView.StatsData statsData in data)
      {
        flag = true;
        ProductQuantityWithIcon view = this.m_viewsCache.GetView();
        view.SetProduct(statsData.Product.WithQuantity(statsData.QuantityTotal));
        view.SetQuantityText(statsData.QuantityTotal.ToString() + "\n" + Tr.TrucksStats__JobsCnt.Format(statsData.JobsCount).ToString());
        view.EnableShowNameOnHover();
        this.m_itemsContainer.Append((IUiElement) view);
      }
      this.m_itemsContainer.FinishBatchOperation();
      this.m_noItemsText.SetVisibility<Txt>(!flag);
      Vector2 size = this.m_itemsContainer.GetSize();
      this.m_container.SetSize<Btn>(new Vector2(size.x, (size.y + 10f).Max(40f)));
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    static JobStatsView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      JobStatsView.MonthRanges = new int[4]{ 1, 3, 6, 12 };
    }

    private struct StatsData
    {
      public ProductProto Product;
      public int JobsCount;
      public Quantity QuantityTotal;
    }
  }
}
