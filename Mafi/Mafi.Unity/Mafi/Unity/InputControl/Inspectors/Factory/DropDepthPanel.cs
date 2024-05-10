// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.DropDepthPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Inspectors.Vehicles;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  public class DropDepthPanel : IUiElementWithUpdater, IUiElement
  {
    private readonly Panel m_container;
    private readonly Tooltip m_tooltip;
    private readonly Dropdwn m_dropdown;
    private readonly IInputScheduler m_inputScheduler;
    private readonly Func<Stacker> m_provider;
    private readonly MbBasedEntitiesRenderer m_entitiesRenderer;
    private Option<Transform> m_renderedEntityTransform;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public LineOverlayRendererHelper GoalLineRenderer { get; }

    public DropDepthPanel(
      IUiElement parent,
      IInputScheduler inputScheduler,
      Func<Stacker> provider,
      UiBuilder builder,
      LinesFactory linesFactory,
      MbBasedEntitiesRenderer entitiesRenderer,
      int numOffsets,
      string title)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      DropDepthPanel dropDepthPanel = this;
      this.m_inputScheduler = inputScheduler;
      this.m_provider = provider;
      this.m_entitiesRenderer = entitiesRenderer;
      this.GoalLineRenderer = new LineOverlayRendererHelper(linesFactory);
      this.GoalLineRenderer.SetWidth(1f);
      this.GoalLineRenderer.SetColor(new Color(0.0f, 0.8f, 0.0f, 0.5f));
      this.m_container = builder.NewPanel(nameof (DropDepthPanel), parent).SetBorderStyle(new BorderStyle(ColorRgba.Black)).SetBackground((ColorRgba) 3815994);
      this.m_container.SetHeight<Panel>(70f);
      this.m_container.SetWidth<Panel>(120f);
      Txt topOf = builder.NewTxt("txt", (IUiElement) this.m_container).SetText(title).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.m_container, 30f, Offset.Left(5f) + Offset.Top(2f));
      this.m_tooltip = builder.AddTooltipForTitle(topOf, "");
      this.m_tooltip.SetText(Tr.DropDepth__OrderingExplanation.TranslatedString);
      this.m_dropdown = builder.NewDropdown("Dropdown", (IUiElement) this.m_container).PutToLeftTopOf<Dropdwn>((IUiElement) this.m_container, new Vector2(96f, (float) Dropdwn.HEIGHT), Offset.Left(10f) + Offset.Top(32f));
      this.m_dropdown.OnValueChange(new Action<int>(onValueChangedInternal));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<ThicknessTilesI>(new Func<ThicknessTilesI>(this.getCurrentDumpHeightMinOffset)).Do((Action<ThicknessTilesI>) (minOffset =>
      {
        dropDepthPanel.m_dropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int index = minOffset.Value; index <= numOffsets; ++index)
          options.Add(string.Format("-{0}", (object) index));
        dropDepthPanel.m_dropdown.AddOptions(options);
      }));
      updaterBuilder.Observe<ThicknessTilesI>(new Func<ThicknessTilesI>(this.getCurrentDumpHeightOffset)).ObserveNoCompare<ThicknessTilesI>(new Func<ThicknessTilesI>(this.getCurrentDumpHeightMinOffset)).Do((Action<ThicknessTilesI, ThicknessTilesI>) ((offset, minOffset) =>
      {
        dropDepthPanel.m_dropdown.SetValueWithoutNotify((offset - minOffset).Value);
        dropDepthPanel.updateGoalLine(offset.Value);
      }));
      this.SetVisibility<DropDepthPanel>(true);
      this.m_dropdown.SetEnabled(true);
      this.Updater = updaterBuilder.Build();

      void onValueChangedInternal(int index) => dropDepthPanel.OnValueChanged(index);
    }

    private ThicknessTilesI getCurrentDumpHeightOffset() => this.m_provider().DumpHeightOffset;

    private ThicknessTilesI getCurrentDumpHeightMinOffset()
    {
      return this.m_provider().Prototype.MinDumpOffset;
    }

    private void OnValueChanged(int index)
    {
      this.m_inputScheduler.ScheduleInputCmd<SetDumpHeightOffsetCmd>(new SetDumpHeightOffsetCmd(this.m_provider().Id, new ThicknessTilesI(this.getCurrentDumpHeightMinOffset().Value + index)));
    }

    public void OnDeactivated()
    {
      this.GoalLineRenderer.HideLine();
      this.m_renderedEntityTransform = Option<Transform>.None;
    }

    private void updateGoalLine(int offset)
    {
      Stacker entity = this.m_provider();
      if (this.m_renderedEntityTransform.IsNone)
      {
        EntityMb entityMb;
        if (this.m_entitiesRenderer.TryGetMbFor((IRenderedEntity) entity, out entityMb))
        {
          if (entityMb.gameObject.IsNullOrDestroyed())
          {
            Log.Error(string.Format("Received destroyed mb for {0}", (object) entity));
            return;
          }
          this.m_renderedEntityTransform = (Option<Transform>) entityMb.transform;
        }
        else
        {
          Log.Warning(string.Format("Failed to find rendered entity for {0}", (object) entity));
          return;
        }
      }
      Tile3f from = entity.DumpPositionXy.CenterTile2f.ExtendZ(entity.Position3f.Z + (Fix32) entity.Prototype.DumpHeadRelPos.Z);
      this.GoalLineRenderer.ShowLine(from, from - offset.TilesThick().ThicknessTilesF);
    }
  }
}
