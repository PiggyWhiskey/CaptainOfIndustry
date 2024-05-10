// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Label
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Themes;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Label : UiComponent<TextElement>, IComponentWithText
  {
    protected LocStrFormatted m_text;
    protected bool m_upperCase;

    public Label(LocStrFormatted locStr = default (LocStrFormatted))
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((TextElement) new UnityEngine.UIElements.Label());
      this.ClassRemove<Label>("unity-label", "unity-text-element");
      this.m_text = LocStrFormatted.Empty;
      if (locStr.IsNotEmpty)
        this.setText(locStr);
      HACK_TextLayoutFix.Apply((VisualElement) this.Element);
    }

    public override void SetTooltipOrCreate(LocStrFormatted tooltip)
    {
      base.SetTooltipOrCreate(tooltip);
      this.rebuildText();
    }

    void IComponentWithText.SetText(LocStrFormatted text) => this.setText(text);

    void IComponentWithText.SetTextOverflow(Mafi.Unity.UiToolkit.Component.TextOverflow overflow)
    {
      overflow.ApplyTo((VisualElement) this.Element);
    }

    protected virtual void setText(LocStrFormatted text)
    {
      this.m_text = text;
      string str = !this.m_upperCase ? text.Value : text.Value.ToUpper(LocalizationManager.CurrentCultureInfo);
      if (this.HasNonEmptyTooltipSet)
        this.Element.text = str + this.getIconSuffix(TmpIcon.InfoIcon);
      else
        this.Element.text = str;
    }

    private void rebuildText() => this.setText(this.m_text);

    public LocStrFormatted GetText() => this.m_text;

    public Label UpperCase(bool upperCase = true)
    {
      if (this.m_upperCase != upperCase)
      {
        this.m_upperCase = upperCase;
        this.rebuildText();
      }
      return this;
    }

    protected string getIconSuffix(TmpIcon icon) => "<nobr> " + icon.SpriteTag + "</nobr>";
  }
}
