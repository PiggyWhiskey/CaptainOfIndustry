// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.FloatingUpointsCostPopup
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Population;
using Mafi.Unity.Camera;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar
{
  /// <summary>
  /// Popup that shows icon of entity which configuration is being copied.
  /// </summary>
  public class FloatingUpointsCostPopup
  {
    private Panel m_container;
    private readonly UpointsManager m_upointsManager;
    private readonly CameraController m_cameraController;
    private readonly UiBuilder m_builder;
    /// <summary>
    /// Whether the popup was visible before we entered into Free-look mode. If yes it needs to be restored after we
    /// leave that mode.
    /// </summary>
    private bool m_wasVisible;
    private Txt m_title;
    private TextWithIcon m_upointsCost;

    public FloatingUpointsCostPopup(
      UpointsManager upointsManager,
      CameraController cameraController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_upointsManager = upointsManager;
      this.m_cameraController = cameraController;
      this.m_builder = builder;
    }

    private void cameraFreeLookChanged(bool isInFreeLookMode)
    {
      if (isInFreeLookMode)
      {
        this.m_wasVisible = this.m_container.IsVisible();
        this.Hide();
      }
      else
      {
        this.m_container.SetVisibility<Panel>(this.m_wasVisible);
        this.m_wasVisible = false;
      }
    }

    private void buildIfNeeded()
    {
      if (this.m_container != null)
        return;
      UiStyle style = this.m_builder.Style;
      ColorRgba colorRgba = new ColorRgba(2829099, 237);
      this.m_container = this.m_builder.NewPanel(nameof (FloatingUpointsCostPopup)).PutToLeftBottomOf<Panel>((IUiElement) this.m_builder.MainCanvas, new Vector2(0.0f, 0.0f));
      Panel parent = this.m_builder.NewPanel("PriceBg").SetBackground(this.m_builder.AssetsDb.GetSharedSprite(style.Icons.WhiteBgBlackBorder), new ColorRgba?(colorRgba)).PutTo<Panel>((IUiElement) this.m_container);
      this.m_title = this.m_builder.NewTxt("Title").SetText("").SetTextStyle(style.Global.Text.Extend(new ColorRgba?(style.Global.UpointsTextColorForDark))).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.m_builder.Style.Global.Text).PutToTopOf<Txt>((IUiElement) parent, 35f);
      this.m_upointsCost = new TextWithIcon(this.m_builder).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetColor(style.Global.UpointsTextColorForDark).PutTo<TextWithIcon>((IUiElement) parent, Offset.Bottom(5f) + Offset.Top(35f));
      this.m_container.SetWidth<Panel>(100f);
      this.m_container.SetHeight<Panel>(65f);
      this.Hide();
      this.m_cameraController.FreeLookModeChanged += new Action<bool>(this.cameraFreeLookChanged);
    }

    public void Hide()
    {
      Panel container = this.m_container;
      if (container == null)
        return;
      container.Hide<Panel>();
    }

    public void SetUpointsCost(Upoints cost, string title)
    {
      this.buildIfNeeded();
      this.m_title.SetText(title);
      this.m_upointsCost.SetPrefixText(cost.Format());
      this.m_container.Show<Panel>();
      this.m_upointsCost.SetColor(this.m_upointsManager.CanConsume(cost) ? this.m_builder.Style.Global.UpointsTextColorForDark : this.m_builder.Style.Global.DangerClr);
    }

    public void UpdatePosition()
    {
      this.buildIfNeeded();
      this.m_container.SetPosition<Panel>(Input.mousePosition + new Vector3(30f, (float) (-(double) this.m_container.GetHeight() - 30.0)));
    }
  }
}
