// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.CostButton
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class CostButton : Btn
  {
    private readonly UiBuilder m_builder;
    private string m_text;
    private readonly TextWithIcon m_textWithIcon;
    private string m_cost;

    public CostButton(UiBuilder builder, string text, string iconPath, string btnName = "button")
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_cost = "";
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, btnName);
      this.m_builder = builder;
      this.m_text = text;
      this.SetText("");
      this.SetTextAlignment(TextAnchor.MiddleLeft);
      this.m_textWithIcon = new TextWithIcon(builder, 16).PutToCenterOf<TextWithIcon>((IUiElement) this, 0.0f, Offset.LeftRight(5f));
      this.m_textWithIcon.SetIcon(iconPath);
      TextStyle text1 = builder.Style.Global.UpointsBtn.Text;
      this.m_textWithIcon.SetTextStyle(text1);
      this.m_textWithIcon.SetColor(text1.Color);
      this.SetHeight<CostButton>(this.GetOptimalSize().y);
    }

    public void SetPrefixTextAndCost(LocStrFormatted text, string cost)
    {
      this.m_text = text.Value;
      this.SetCost(cost);
    }

    public void SetSuffix(string suffix)
    {
      this.m_textWithIcon.SetSuffixText(suffix);
      this.SetCost(this.m_cost);
    }

    public void SetCost(string cost)
    {
      this.m_cost = cost;
      this.m_textWithIcon.SetPrefixText(this.m_text + "  |  " + cost);
      this.SetWidth<CostButton>(this.m_textWithIcon.GetWidth() + 20f);
    }

    public override Btn SetEnabled(bool isEnabled)
    {
      this.m_textWithIcon.SetEnabled(isEnabled, this.m_builder.Style.Global.UpointsBtn.ForegroundClrWhenDisabled ?? this.m_builder.Style.Global.UpointsBtn.Text.Color);
      return base.SetEnabled(isEnabled);
    }
  }
}
