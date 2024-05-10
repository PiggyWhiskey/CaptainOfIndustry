// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.TransportSnappingDisabledView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class TransportSnappingDisabledView : IUiElement
  {
    private readonly ShortcutsManager m_shortcutsManager;
    private Panel m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public TransportSnappingDisabledView(ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
    }

    public void BuildUi(UiBuilder builder)
    {
      EntitiesMenuUiStyle entitiesMenu = builder.Style.EntitiesMenu;
      this.m_container = builder.NewPanel("Container");
      builder.NewPanel("TopBorder").SetBackground((ColorRgba) 14399488).PutToBottomOf<Panel>((IUiElement) this, (float) (entitiesMenu.TopBorderSize + 2));
      string niceString = this.m_shortcutsManager.TransportSnapping.ToNiceString();
      Panel panel1 = builder.NewPanel("Container").SetBackground((ColorRgba) 14399488);
      Panel panel2 = builder.NewPanel("CenterHolder");
      IconContainer leftOf = builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/Toolbox/NoSnap.svg", (ColorRgba) 0).PutToLeftOf<IconContainer>((IUiElement) panel2, 24f);
      Txt txt = builder.NewTxt("Text").SetText(Tr.TransportSnappingOff__Title.TranslatedString + " [" + niceString + "]").SetTextStyle(builder.Style.Global.Title.Extend(new ColorRgba?((ColorRgba) 0))).SetAlignment(TextAnchor.MiddleCenter);
      txt.PutToRightOf<Txt>((IUiElement) panel2, txt.GetPreferedWidth());
      builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) panel2).SetText(Tr.TransportSnappingOff__Tooltip.Format(niceString));
      panel2.PutToCenterOf<Panel>((IUiElement) panel1, (float) ((double) leftOf.GetWidth() + (double) txt.GetWidth() + 5.0));
      panel1.PutToCenterBottomOf<Panel>((IUiElement) this, new Vector2(panel2.GetWidth() + 100f, 34f));
      this.SetHeight<TransportSnappingDisabledView>(panel1.GetHeight());
    }
  }
}
