using Mafi;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain;
using Mafi.Unity;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;

using System;
using System.Collections.Generic;

using TerrainTower.TTower;

using UnityEngine;

namespace TerrainTower.Extras
{
    //Used in TerrainTower, simulates the Farm Ouputs, and is used to list products for dumping
    internal class OutputBufferView : IUiElementWithUpdater, IUiElement
    {
        private readonly BufferView m_bufferView;

        public OutputBufferView(IUiElement parent, UiBuilder builder) : base()
        {
            m_bufferView = builder.NewBufferView(parent, isCompact: true).SetAsSuperCompact();
            UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
            updaterBuilder
                .Observe(() => Buffer.Product)
                .Observe(() => Buffer.Capacity)
                .Observe(() => Buffer.Quantity)
                .Do(new Action<ProductProto, Quantity, Quantity>(m_bufferView.UpdateState));
            Updater = updaterBuilder.Build();
            this.SetHeight(builder.Style.BufferView.SuperCompactHeight);
        }

        public GameObject GameObject => this.m_bufferView.GameObject;

        public RectTransform RectTransform => this.m_bufferView.RectTransform;

        public IUiUpdater Updater { get; }
        private IProductBuffer Buffer { get; set; }

        public OutputBufferView SetBuffer(IProductBuffer buffer)
        {
            Buffer = buffer;
            return this;
        }
    }

    internal class OutputSortedProductBufferView : IUiElementWithUpdater, IUiElement
    {
        public static float RequiredHeight;
        private readonly Panel m_container;
        private readonly IInputScheduler m_inputScheduler;
        private ProductProto m_product;

        static OutputSortedProductBufferView()
        {
            RequiredHeight = 50f;
        }

        public OutputSortedProductBufferView(UiBuilder builder, IInputScheduler inputScheduler) : base()
        {
            OutputSortedProductBufferView productBufferView = this;

            //Async Input Scheduler
            m_inputScheduler = inputScheduler;

            m_container = builder
                .NewPanel(nameof(OutputSortedProductBufferView))
                .SetBackground(builder.Style.Panel.ItemOverlay);

            BufferView bufferView = builder
                .NewBufferView(m_container, isCompact: true)
                .PutTo(m_container, Offset.Right(110f));

            TextWithIcon bufferDesc = new TextWithIcon(builder, 16)
                .SetTextStyle(builder.Style.Panel.Text)
                .SetIcon("Assets/Unity/UserInterface/General/Clock.svg")
                .PutToLeftBottomOf(bufferView, new Vector2(200f, 25f), Offset.Left(100f));

            IconContainer stuckIcon = builder
                .NewIconContainer("StuckIcon")
                .SetIcon("Assets/Unity/UserInterface/General/Warning-v2.svg", builder.Style.Global.OrangeText)
                .PutToLeftBottomOf(bufferView, 20.Vector2(), Offset.Left(65f) + Offset.Bottom(5f))
                .Hide();

            Tooltip tooltip = builder
                .AddTooltipFor(stuckIcon);

            _ = tooltip.SetErrorTextStyle();
            _ = tooltip.SetText("The sorter is not accepting more of this product because there is already too much of it.");

            UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();

            _ = updaterBuilder
                .Observe(() => productBufferView.Entity.GetSortedLastMonth(productBufferView.Buffer.Product))
                .Do(sorted => bufferDesc.SetPrefixText(string.Format("{0}: {1} / 60", Tr.ProducedLastMonth, sorted)));
            _ = updaterBuilder
                .Observe(() => productBufferView.Buffer.Product)
                .Observe(() => productBufferView.Buffer.Capacity)
                .Observe(() => productBufferView.Buffer.Quantity)
                .Do((product, capacity, quantity) => bufferView.UpdateState(product, capacity, quantity));

            //Port IDs to output to, with '-' as a default option to disable output
            List<string> options = new List<string> { "-" };
            for (int index = 0; index < TerrainTowerEntity.MAX_OUTPUTS; ++index)
            {
                char ch = (char)(65 + index);
                options.Add(ch.ToString());
            }

            Dropdwn dropdown = builder
                .NewDropdown("")
                .AddOptions(options)
                .AddTooltip(TrCore.OutputPort__Tooltip.TranslatedString)
                .OnValueChange(new Action<int>(onValueChanged));

            _ = dropdown.PutToRightTopOf(m_container, new Vector2(70f, 28f), Offset.Right(25f) + Offset.Top(2f));

            _ = updaterBuilder
                .Observe(() => productBufferView.Entity.GetPortIndexFor(productBufferView.Buffer.Product))
                .Do(index => dropdown.SetValueWithoutNotify(index));

            Btn toggleAlertBtn = builder
                .NewBtn("TogglePanelBtn")
                .SetButtonStyle(builder.Style.Global.GeneralBtnToToggle)
                .SetIcon("Assets/Unity/UserInterface/General/Bell128.png", new Mafi.Unity.UiFramework.Offset?(Offset.All(5f)))
                .AddToolTip(TrCore.OreSorter_BlockedAlert__Tooltip)
                .OnClick(() =>
                    productBufferView
                        .m_inputScheduler
                        .ScheduleInputCmd
                        (
                            new TerrainTowerCommands.SortingPlantSetBlockedProductAlertCmd
                            (
                                productBufferView.Entity.Id,
                                productBufferView.Buffer.Product.Id,
                                !productBufferView.Entity.IsProductBlockedAlertEnabled(productBufferView.Buffer.Product)
                            )
                        )
                    );

            _ = toggleAlertBtn.PutToRightTopOf(m_container, 26.Vector2(), Offset.Right(30f + dropdown.GetWidth()) + Offset.Top(3f));

            _ = updaterBuilder
                .Observe(() => productBufferView.Entity.IsProductBlockedAlertEnabled(productBufferView.Buffer.Product))
                .Do(alertsEnabled =>
                {
                    _ = alertsEnabled
                        ? toggleAlertBtn.SetButtonStyle(builder.Style.Global.GeneralBtnActive)
                        : toggleAlertBtn.SetButtonStyle(builder.Style.Global.GeneralBtnToToggle);
                });
            Updater = updaterBuilder.Build();
        }

        public IProductBuffer Buffer { get; set; }
        public GameObject GameObject => m_container.GameObject;

        public RectTransform RectTransform => m_container.RectTransform;
        public IUiUpdater Updater { get; }
        private TerrainTowerEntity Entity { get; set; }

        public void SetOutputBuffer(IProductBuffer buffer, TerrainTowerEntity entity)
        {
            Buffer = buffer;
            Entity = entity;
            m_product = Buffer.Product;
        }

        private void onValueChanged(int index)
        {
            _ = m_inputScheduler.ScheduleInputCmd(new TerrainTowerCommands.SetProductPortCmd(Entity, m_product, index));
        }
    }

    /// <summary>
    /// UI Element for toggling between Mine/Dump/Flatten
    /// Covers the buttons and the icon
    /// </summary>
    internal class TerrainConfigPanel : IUiElement
    {
        public const float HEIGHT = 25f; //Object heights
        public const float SPACING = 5f; //Spacing between objects
        private readonly StackContainer m_buttonsContainer;
        private readonly Panel m_container;
        private readonly Btn m_dumpBtn;
        private readonly Btn m_editManagedArea;
        private readonly Btn m_flatBtn;
        private readonly IconContainer m_iconContainer;
        private readonly Txt m_managedAreaDescription;
        private readonly Btn m_mineBtn;
        private readonly PanelWithShadow m_panelWithShadow;

        //
        public TerrainConfigPanel(UiBuilder builder, float width, Action<TerrainTowerEntity.TerrainTowerConfigState> onModeClick, Action onEditAreaClick) : base()
        {
            string icon = "Assets/Unity/UserInterface/Toolbar/Flatten.svg";
            m_container = builder
                .NewPanel("TerrainConfigPanel") //.NewPanel("TerrainConfigPanel", parent)
                .SetSize(new Vector2(width, HEIGHT));

            //Build Icon Panel and put to parent
            m_iconContainer = builder
                .NewIconContainer("Icon", m_container)
                .SetIcon(icon)
                .SetSize(new Vector2(HEIGHT, HEIGHT));

            //Add tooltip for Icon Panel
            builder
                .AddTooltipFor(m_iconContainer)
                .SetText("Configures whether Terrain Tower can <b>Mine</b> or <b>Dump</b> terrain (Or both)");

            //Add Panel with Shadow to hold buttons
            m_panelWithShadow = builder
                .NewPanelWithShadow("ToggleShadow", m_container)
                .AddShadowRightBottom();

            //Create Buttons Container
            m_buttonsContainer = builder
                .NewStackContainer("ToggleCont", m_container)
                .SetSizeMode(StackContainer.SizeMode.Dynamic)
                .SetStackingDirection(StackContainer.Direction.LeftToRight)
                .SetInnerPadding(Offset.All(0))
                .SetItemSpacing(1f)
                .SetBackground(0)
                .PutToLeftOf(m_panelWithShadow, 0.0f);

            //Add buttons
            m_dumpBtn = addToggleBtn("btnDump", "Dump", "Terrain Tower is allowed to DUMP only", builder.Style.Global.ToggleBtnOn, () => onModeClick(TerrainTowerEntity.TerrainTowerConfigState.Dumping));
            m_flatBtn = addToggleBtn("m_flatBtn", "Flatten", "Terrain Tower is allowed to Flatten", builder.Style.Global.ToggleBtnAuto, () => onModeClick(TerrainTowerEntity.TerrainTowerConfigState.Flatten));
            m_mineBtn = addToggleBtn("m_mineBtn", "Mine", "Terrain Tower is allowed to MINE only", builder.Style.Global.ToggleBtnOn, () => onModeClick(TerrainTowerEntity.TerrainTowerConfigState.Mining));

            //Create Edit Area Button
            m_editManagedArea = builder
                .NewBtnGeneral("Btn", m_container)
                .EnableDynamicSize()
                .SetText("Edit Area")
                .SetIcon("Assets/Unity/UserInterface/Toolbox/SelectArea.svg", 21.Vector2())
                .OnClick(onEditAreaClick);

            //Create Area Info Panel
            //Managed area: 100m x 100m
            m_managedAreaDescription = builder
                .NewTxt("AreaInfo")
                .SetTextStyle(builder.Style.Global.Text)
                .SetAlignment(TextAnchor.LowerRight)
                .SetText("Managed area: 1000m x 1000m")
                .SetHeight(HEIGHT);

            #region Place Objects

            //Left Anchored Items - Outwards in
            float leftAnchor = SPACING * 2; //Start with double the spacing

            _ = m_iconContainer.PutToLeftMiddleOf(m_container, m_iconContainer.GetSize(), Offset.Left(leftAnchor));
            leftAnchor += m_iconContainer.GetWidth() + SPACING;

            _ = m_panelWithShadow.PutToLeftMiddleOf(m_container, new Vector2(m_buttonsContainer.GetDynamicWidth(), HEIGHT), Offset.Left(leftAnchor));
            leftAnchor += m_panelWithShadow.GetWidth() + SPACING;

            //Right Anchored Items - Outwards in
            float rightAnchor = SPACING * 2; //Start with double the spacing

            _ = m_editManagedArea.PutToRightMiddleOf(m_container, m_editManagedArea.GetSize(), Offset.Right(rightAnchor));
            rightAnchor += m_editManagedArea.GetWidth() + SPACING;

            _ = m_managedAreaDescription.PutToRightMiddleOf(m_container, m_managedAreaDescription.GetSize(), Offset.Right(rightAnchor));
            rightAnchor += m_managedAreaDescription.GetWidth() + SPACING;

            #endregion Place Objects

            this.SetSize(new Vector2(width - (2 * SPACING), HEIGHT + (2 * SPACING)));

            Btn addToggleBtn(string name, string text, string tooltip, BtnStyle style, Action onClick)
            {
                Btn objectToPlace = builder
                    .NewBtn(name, m_buttonsContainer)
                    .SetButtonStyle(style)
                    .OnClick(onClick)
                    .AddToolTip(tooltip)
                    .SetText(text)
                    .SetHeight(HEIGHT);
                return objectToPlace.AppendTo(m_buttonsContainer, new Vector2(objectToPlace.GetOptimalWidth().CeilToInt(), HEIGHT), ContainerPosition.LeftOrTop);
            }
        }

        public GameObject GameObject => m_container.GameObject;

        public RectTransform RectTransform => m_container.RectTransform;

        public void SetAreaDescription(RectangleTerrainArea2i area)
        {
            SetAreaDescription(area.Size.X * 2, area.Size.Y * 2);
        }

        public void SetAreaDescription(float x, float y)
        {
            m_managedAreaDescription.SetText(string.Format("Managed area: {0}m x {1}m", x, y));
        }

        public TerrainConfigPanel SetBackground(ColorRgba color)
        {
            m_container.SetBackground(color);
            return this;
        }

        public void SetMode(TerrainTowerEntity.TerrainTowerConfigState state)
        {
            m_dumpBtn.SetEnabled(state != TerrainTowerEntity.TerrainTowerConfigState.Dumping);
            m_flatBtn.SetEnabled(state != TerrainTowerEntity.TerrainTowerConfigState.Flatten);
            m_mineBtn.SetEnabled(state != TerrainTowerEntity.TerrainTowerConfigState.Mining);
        }
    }
}