// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Title
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Themes;
using System.Text;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Title : Label
  {
    private static readonly Px EXTRA_TEXT_SPACE;
    private LocStrFormatted m_extraText;
    private Px m_extraTextSpace;

    public Title(LocStrFormatted title)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_extraTextSpace = Title.EXTRA_TEXT_SPACE;
      // ISSUE: explicit constructor call
      base.\u002Ector(title);
      this.Class<Title>(Cls.title);
      this.UpperCase();
    }

    public Title ExtraText(LocStrFormatted extraText, Px? space = null)
    {
      this.m_extraText = extraText;
      this.m_extraTextSpace = space ?? Title.EXTRA_TEXT_SPACE;
      this.Text<Title>(this.m_text);
      return this;
    }

    protected override void setText(LocStrFormatted text)
    {
      this.m_text = text;
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Element.style.unityFontStyleAndWeight != (StyleEnum<UnityEngine.FontStyle>) UnityEngine.FontStyle.Normal)
        stringBuilder.Append("<b>");
      stringBuilder.Append(this.m_upperCase ? this.m_text.Value.ToUpper(LocalizationManager.CurrentCultureInfo) : this.m_text.Value);
      if (this.Element.style.unityFontStyleAndWeight != (StyleEnum<UnityEngine.FontStyle>) UnityEngine.FontStyle.Normal)
        stringBuilder.Append("</b>");
      if (this.HasNonEmptyTooltipSet)
        stringBuilder.Append(this.getIconSuffix(TmpIcon.InfoIcon));
      if (this.m_extraText.IsNotEmpty)
      {
        stringBuilder.Append("<space=");
        stringBuilder.Append(this.m_extraTextSpace.Pixels);
        stringBuilder.Append("px><size=14px><color=");
        stringBuilder.Append(Theme.Text.ToHex());
        stringBuilder.Append(">");
        stringBuilder.Append(this.m_extraText.Value);
        stringBuilder.Append("</color></size>");
      }
      this.Element.text = stringBuilder.ToString();
    }

    static Title()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Title.EXTRA_TEXT_SPACE = 2.pt();
    }
  }
}
