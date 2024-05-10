// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.BetterTextField
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Linq;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>Adds placeholder support as it is only in Unity 2023.</summary>
  public class BetterTextField : TextField
  {
    private Option<Label> m_placeholderLabel;
    private readonly VisualElement m_textInput;

    public override string value
    {
      get => base.value;
      set
      {
        base.value = value;
        this.updatePlaceholderVisibility();
      }
    }

    public BetterTextField()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RemoveFromClassList("unity-base-field--no-label");
      this.RemoveFromClassList("unity-base-text-field");
      this.RemoveFromClassList("unity-text-field");
      this.RemoveFromClassList("unity-base-field");
      this.AddToClassList(Cls.inputText);
      this.AddToClassList(Cls.clickable);
      this.m_textInput = this.Children().FirstOrDefault<VisualElement>((Func<VisualElement, bool>) (x => x.name == "unity-text-input"));
      this.m_textInput.AddToClassList(Cls.inputText_field);
      this.m_textInput.RemoveFromClassList("unity-base-text-field__input");
      this.m_textInput.RemoveFromClassList("unity-base-field__input");
      this.m_textInput.RemoveFromClassList("unity-text-field__input");
    }

    public void SetPlaceholder(LocStrFormatted txt)
    {
      this.getOrCreatePlaceholder().Text<Label>(txt);
    }

    private Label getOrCreatePlaceholder()
    {
      if (this.m_placeholderLabel.HasValue)
        return this.m_placeholderLabel.Value;
      this.m_placeholderLabel = (Option<Label>) new Label().Class<Label>(Cls.inputText_field_placeholder).IgnoreInputPicking<Label>();
      this.m_textInput.Add(this.m_placeholderLabel.Value.RootElement);
      this.RegisterCallback<FocusInEvent>((EventCallback<FocusInEvent>) (e => this.hidePlaceholder()));
      this.RegisterCallback<FocusOutEvent>((EventCallback<FocusOutEvent>) (e =>
      {
        if (!string.IsNullOrEmpty(this.text))
          return;
        this.showPlaceholder();
      }));
      return this.m_placeholderLabel.Value;
    }

    private void updatePlaceholderVisibility()
    {
      if (this.m_placeholderLabel.IsNone)
        return;
      if (string.IsNullOrEmpty(this.value))
      {
        if (this.focusController?.focusedElement == this)
          return;
        this.showPlaceholder();
      }
      else
        this.hidePlaceholder();
    }

    private void hidePlaceholder()
    {
      Label valueOrNull = this.m_placeholderLabel.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Hide<Label>();
    }

    private void showPlaceholder()
    {
      Label valueOrNull = this.m_placeholderLabel.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Show<Label>();
    }

    public override void SetValueWithoutNotify(string newValue)
    {
      base.SetValueWithoutNotify(newValue);
      this.updatePlaceholderVisibility();
    }
  }
}
