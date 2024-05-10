// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CropScheduleView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Farms;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class CropScheduleView : IUiElementWithUpdater, IUiElement
  {
    private readonly StackContainer m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public CropScheduleView(
      UiBuilder builder,
      IInputScheduler inputScheduler,
      Func<Farm> farmProvider,
      Action<int> onSlotClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_container = builder.NewStackContainer("Slots").SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(5f).SetSizeMode(StackContainer.SizeMode.Dynamic).SetHeight<StackContainer>(60f);
      this.Updater = UpdaterBuilder.Start().Build();
      for (int slotIndex = 0; slotIndex < 4; ++slotIndex)
        this.Updater.AddChildUpdater(new CropScheduleView.SlotView(builder, farmProvider, slotIndex, onSlotClick).AppendTo<CropScheduleView.SlotView>(this.m_container).Updater);
      builder.NewBtnDanger("ForwardBtn").OnClick((Action) (() => inputScheduler.ScheduleInputCmd<FarmForceNextSlotCmd>(new FarmForceNextSlotCmd(farmProvider())))).SetIcon("Assets/Unity/UserInterface/Toolbar/FastForward128.png").AddToolTip(Tr.CropScheduleSkip__Tooltip).AppendTo<Btn>(this.m_container, new Vector2?(new Vector2(28f, 46f)), ContainerPosition.LeftOrTop);
    }

    public class SlotView : IUiElementWithUpdater, IUiElement
    {
      public const int SLOT_HEIGHT = 60;
      public const int SLOT_WIDTH = 46;
      public const int BTN_SIZE = 46;
      private readonly Panel m_container;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public IUiUpdater Updater { get; private set; }

      public SlotView(
        UiBuilder builder,
        Func<Farm> farmProvider,
        int slotIndex,
        Action<int> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        UiStyle style = builder.Style;
        this.m_container = builder.NewPanel("PlusBtnContainer").SetSize<Panel>(new Vector2(46f, 60f));
        Btn plusBtn = builder.NewBtnGeneral("PlusBtn").OnClick((Action) (() => onClick(slotIndex))).SetIcon(builder.Style.Icons.Plus, new Offset?(Offset.All(12f))).PutToTopOf<Btn>((IUiElement) this.m_container, 46f);
        Btn plusBtnWithIcon = builder.NewBtn("PlusBtnWithIcon").OnClick((Action) (() => onClick(slotIndex))).SetButtonStyle(style.Global.GeneralBtn).PutToTopOf<Btn>((IUiElement) this.m_container, 46f);
        IconContainer plusBtnProductIcon = builder.NewIconContainer("Icon").PutTo<IconContainer>((IUiElement) plusBtnWithIcon, Offset.All(4f));
        IconContainer activeIndicator = builder.NewIconContainer("Arrow").SetIcon(builder.AssetsDb.GetSharedSprite(builder.Style.Icons.WhiteBgGrayBorder)).PutToLeftBottomOf<IconContainer>((IUiElement) this.m_container, new Vector2(10f, 10f), Offset.Left(10f));
        Txt duration = builder.NewTxt("Duration").SetTextStyle(style.Global.Title).SetAlignment(TextAnchor.MiddleLeft).SetText("").PutToRightBottomOf<Txt>((IUiElement) this.m_container, new Vector2(15f, 15f), Offset.Right(5f) + Offset.Bottom(-3f));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<Option<CropProto>>((Func<Option<CropProto>>) (() => farmProvider().Schedule[slotIndex])).Observe<int>((Func<int>) (() => farmProvider().ActiveScheduleIndex)).Do((Action<Option<CropProto>, int>) ((cropMaybe, activeIndex) =>
        {
          plusBtn.SetVisibility<Btn>(cropMaybe.IsNone);
          plusBtnWithIcon.SetVisibility<Btn>(cropMaybe.HasValue);
          if (cropMaybe.HasValue)
          {
            plusBtnProductIcon.SetIcon(cropMaybe.Value.Graphics.IconPath);
            duration.SetText(cropMaybe.Value.MonthsToGrow.ToString());
          }
          else
            duration.SetText("");
          activeIndicator.SetColor(slotIndex == activeIndex ? builder.Style.Global.OrangeText : (ColorRgba) 3487029);
        }));
        this.Updater = updaterBuilder.Build();
      }
    }
  }
}
