// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.FloatingTextPopup
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar
{
  /// <summary>Popup that shows arbitrary text copied.</summary>
  public class FloatingTextPopup : IUiElement
  {
    private Panel m_container;
    private UiBuilder m_builder;
    private Txt m_title;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public void RegisterUi(UiBuilder builder, Canvass parentCanvas = null)
    {
      this.m_builder = builder;
      UiStyle style = builder.Style;
      ColorRgba colorRgba = new ColorRgba(2829099, 237);
      this.m_container = builder.NewPanel("FloatingUpointsCostPopup").PutToLeftBottomOf<Panel>((IUiElement) (parentCanvas ?? builder.MainCanvas), Vector2.zero);
      Panel parent = builder.NewPanel("PriceBg").SetBackground(builder.AssetsDb.GetSharedSprite(style.Icons.WhiteBgBlackBorder), new ColorRgba?(colorRgba)).PutTo<Panel>((IUiElement) this.m_container);
      this.m_title = this.m_builder.NewTxt("Title").SetText("").SetTextStyle(style.Global.Text.Extend(new ColorRgba?(style.Global.UpointsTextColorForDark))).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(builder.Style.Global.Text).PutToTopOf<Txt>((IUiElement) parent, 35f);
      this.m_container.SetWidth<Panel>(100f);
      this.m_container.SetHeight<Panel>(40f);
      this.Hide();
    }

    public void SetText(LocStrFormatted text)
    {
      this.m_title.SetText(text);
      this.Show<FloatingTextPopup>();
    }

    public void SetText(string text)
    {
      this.m_title.SetText(text);
      this.Show<FloatingTextPopup>();
    }

    public void UpdatePositionToCursor()
    {
      this.m_container.SetPosition<Panel>(Input.mousePosition + new Vector3(30f, (float) (-(double) this.m_container.GetHeight() - 30.0)));
    }

    public void Hide() => this.m_container.Hide<Panel>();

    public FloatingTextPopup()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
