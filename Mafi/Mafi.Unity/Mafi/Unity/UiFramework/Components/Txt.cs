// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Txt
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Txt : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly TextMeshProUGUI m_text;
    private TextStyle m_textStyle;
    private Option<Panel> m_background;

    public GameObject GameObject { get; private set; }

    public RectTransform RectTransform { get; private set; }

    public Graphic Graphic => (Graphic) this.m_text;

    public string Text => this.m_text.text;

    public float X => this.RectTransform.position.x;

    public Txt(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      GameObject clonedGo = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = clonedGo.GetComponent<RectTransform>();
      this.m_text = clonedGo.AddComponent<TextMeshProUGUI>();
      this.GameObject = this.m_text.gameObject;
      this.m_text.richText = false;
      this.m_text.autoSizeTextContainer = false;
      this.m_text.textWrappingMode = TextWrappingModes.Normal;
    }

    public Txt SetTextStyle(TextStyle textStyle)
    {
      this.m_textStyle = textStyle;
      this.m_text.font = this.m_builder.Font.ValueOrNull;
      if (LocalizationManager.CurrentLangInfo.UsesSymbols)
        this.m_text.fontStyle = TMPro.FontStyles.Normal;
      else if (textStyle.FontStyle == FontStyle.Bold)
        this.m_text.fontStyle = TMPro.FontStyles.Bold;
      else if (textStyle.FontStyle == FontStyle.Italic)
        this.m_text.fontStyle = TMPro.FontStyles.Italic;
      else
        this.m_text.fontStyle = TMPro.FontStyles.Normal;
      this.m_text.color = this.m_textStyle.Color.ToColor();
      this.SetFontSize(this.m_textStyle.FontSize);
      if (this.m_textStyle.IsCapitalized && this.m_text.text != null)
        this.m_text.text = this.m_text.text.ToUpper(LocalizationManager.CurrentCultureInfo);
      return this;
    }

    public Txt SetText(LocStrFormatted text)
    {
      this.SetText(text.Value);
      return this;
    }

    public Txt SetText(string text)
    {
      if (this.m_textStyle.IsCapitalized && this.m_text.richText)
        text = text.Replace("<size=", "<SIZE=").Replace("</size>", "</SIZE>");
      if (this.m_textStyle.IsCapitalized)
        text = text.ToUpper(LocalizationManager.CurrentCultureInfo);
      if (text.Length > 18400)
      {
        Log.Warning(string.Format("Too long text, truncating from {0} to {1} characters.", (object) text.Length, (object) 18400));
        text = text.Substring(0, 18400);
      }
      this.m_text.text = text;
      return this;
    }

    public Txt BestFitEnabled(int maxFontSize = 16)
    {
      this.m_text.enableAutoSizing = true;
      this.m_text.fontSizeMin = 8f;
      this.m_text.fontSizeMax = (float) maxFontSize;
      return this;
    }

    public Txt SetAlignment(TextAnchor anchor)
    {
      this.applyLegacyAnchor(anchor);
      return this;
    }

    public Txt SetFontSize(int size)
    {
      this.m_text.fontSize = (float) size;
      return this;
    }

    internal Txt IncreaseFontForSymbols(int inc = 1)
    {
      if (LocalizationManager.CurrentLangInfo.UsesSymbols)
      {
        if (this.m_text.enableAutoSizing)
          this.m_text.fontSizeMax += (float) inc;
        else
          this.m_text.fontSize += (float) inc;
      }
      return this;
    }

    public Txt SetColor(ColorRgba color)
    {
      this.m_text.color = color.ToColor();
      return this;
    }

    public Txt EnableRichText()
    {
      this.m_text.richText = true;
      return this;
    }

    internal Txt AddOutline()
    {
      this.m_text.fontSharedMaterial = this.m_builder.AssetsDb.GetSharedMaterial("Assets/Unity/TextMeshPro/Fonts/Main-Regular/Roboto-Outline.mat");
      return this;
    }

    public Txt AllowVerticalOverflow()
    {
      this.m_text.textWrappingMode = TextWrappingModes.Normal;
      return this;
    }

    public Txt AllowHorizontalOverflow()
    {
      this.m_text.textWrappingMode = TextWrappingModes.NoWrap;
      return this;
    }

    public Txt SetLineSpacing(float spacing)
    {
      this.m_text.lineSpacing = spacing;
      return this;
    }

    public float GetPreferedWidth()
    {
      return this.getTempTxt().GetPreferredValues(this.m_text.text).x + 1f;
    }

    internal float GetPreferedHeight() => this.getTempTxt().GetPreferredValues(this.m_text.text).y;

    internal Vector2 GetPreferedSize()
    {
      return this.getTempTxt().GetPreferredValues(this.m_text.text) + new Vector2(1f, 0.0f);
    }

    public float GetPreferedHeight(float width, string text = null)
    {
      return this.getTempTxt().GetPreferredValues(text ?? this.m_text.text, width, float.MaxValue).y;
    }

    public Vector2 GetPreferredSize(float maxWidth, float maxHeight)
    {
      return this.getTempTxt().GetPreferredValues(this.m_text.text, maxWidth, maxHeight) + new Vector2(1f, 0.0f);
    }

    private TextMeshProUGUI getTempTxt()
    {
      object obj;
      if (!this.m_builder.ElementsCache.TryGetValue("tmpText", out obj))
      {
        GameObject clonedGo = this.m_builder.GetClonedGo("tmpText", this.m_builder.MainCanvas.GameObject);
        obj = (object) clonedGo.AddComponent<TextMeshProUGUI>();
        clonedGo.SetActive(false);
        this.m_builder.ElementsCache["tmpText"] = obj;
      }
      TextMeshProUGUI tempTxt = obj as TextMeshProUGUI;
      tempTxt.fontSize = this.m_text.fontSize;
      tempTxt.fontStyle = this.m_text.fontStyle;
      tempTxt.font = this.m_text.font;
      return tempTxt;
    }

    /// <summary>Sets size to preferred size.</summary>
    public Txt SetPreferredSize()
    {
      this.SetSize<Txt>(this.GetPreferedSize());
      return this;
    }

    public Txt SetFont(FontAsset font)
    {
      this.m_text.font = font;
      return this;
    }

    private void applyLegacyAnchor(TextAnchor anchor)
    {
      switch (anchor)
      {
        case TextAnchor.UpperLeft:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Top;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Left;
          break;
        case TextAnchor.UpperCenter:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Top;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Center;
          break;
        case TextAnchor.UpperRight:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Top;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Right;
          break;
        case TextAnchor.MiddleLeft:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Middle;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Left;
          break;
        case TextAnchor.MiddleCenter:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Middle;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Center;
          break;
        case TextAnchor.MiddleRight:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Middle;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Right;
          break;
        case TextAnchor.LowerLeft:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Bottom;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Left;
          break;
        case TextAnchor.LowerCenter:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Bottom;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Center;
          break;
        case TextAnchor.LowerRight:
          this.m_text.verticalAlignment = VerticalAlignmentOptions.Bottom;
          this.m_text.horizontalAlignment = HorizontalAlignmentOptions.Right;
          break;
      }
    }

    public Txt SetAsComponentOf(TMP_InputField field)
    {
      field.textComponent = (TMP_Text) this.m_text;
      return this;
    }

    public Txt SetAsPlaceholderOf(TMP_InputField field)
    {
      field.placeholder = (Graphic) this.m_text;
      return this;
    }

    public Txt SetBackground(ColorRgba color)
    {
      if (this.m_background.IsNone)
      {
        Panel panel = this.m_builder.NewPanel("TxtWithBg");
        this.m_background = (Option<Panel>) panel;
        panel.SetSize<Panel>(this.GetSize());
        this.PutTo<Txt>((IUiElement) panel);
        this.RectTransform = panel.RectTransform;
        this.GameObject = panel.GameObject;
      }
      this.m_background.Value.SetBackground(color);
      return this;
    }
  }
}
