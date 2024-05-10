// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Button
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Audio;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>
  /// Does not offer API to set text, for that use ButtonText.
  /// </summary>
  public class Button : UiComponentDecorated<UnityEngine.UIElements.Button>, IButtonComponent, IUiComponent
  {
    private Option<Action> m_onClick;
    private Option<Action> m_onDoubleClick;
    private bool m_clicked;
    private Option<AudioSource> m_customClickSound;
    protected ButtonVariant m_variant;

    /// <summary>
    /// If there is no shadow the ButtonElement == "outer element" and there is no overhead.
    /// </summary>
    protected UnityEngine.UIElements.Button ButtonElement => this.InnerElement;

    public Button(Action onClick = null, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new UnityEngine.UIElements.Button(), outer, inner);
      this.m_onClick = (Option<Action>) onClick;
      this.ClassRemove<Button>("unity-button", "unity-text-element");
      this.Class<Button>(Cls.clickable);
      this.ButtonElement.clicked += new Action(this.clicked);
      this.ButtonElement.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.handleMouseUp));
      this.Element.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.clickedAlways));
    }

    void IButtonComponent.SetVariant(ButtonVariant variant)
    {
      this.ClassRemove<Button>(this.m_variant.ToClass());
      this.m_variant = variant;
      this.Class<Button>(this.m_variant.ToClass());
      this.SetOuterDecor(this.m_variant.ToOuter());
    }

    private void clickedAlways(MouseDownEvent evt)
    {
      if (this.ButtonElement.enabledSelf)
        return;
      this.Builder.ValueOrNull?.InvalidOpSound.Play();
    }

    private void handleMouseUp(MouseUpEvent evt)
    {
      if (!this.m_clicked && ((double) evt.localMousePosition.y == -1.0 || (double) evt.localMousePosition.x == -1.0))
        this.clicked();
      this.m_clicked = false;
    }

    private void clicked()
    {
      this.m_clicked = true;
      if (this.m_onClick.IsNone)
        return;
      if (this.m_customClickSound.HasValue)
        this.m_customClickSound.Value.Play();
      else
        this.Builder.ValueOrNull?.ClickSound.Play();
      this.m_onClick.Value();
    }

    private void handleDoubleClick(ClickEvent evt)
    {
      if (evt.clickCount != 2 || !this.m_onDoubleClick.HasValue)
        return;
      this.m_onDoubleClick.Value();
    }

    void IButtonComponent.SetCustomClickSound(string pathToSound)
    {
      this.RunWithBuilder((Action<UiBuilder>) (builder => this.m_customClickSound = (Option<AudioSource>) builder.AudioDb.GetSharedAudio(pathToSound, AudioChannel.UserInterface)));
    }

    void IButtonComponent.SetOnClickAction([CanBeNull] Action onClick)
    {
      this.m_onClick = (Option<Action>) onClick;
    }

    void IButtonComponent.SetOnDoubleClickAction(Action onClick)
    {
      if (this.m_onDoubleClick.IsNone)
        this.ButtonElement.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.handleDoubleClick));
      this.m_onDoubleClick = (Option<Action>) onClick;
    }

    IButtonDecorator IButtonComponent.GetButtonDecorator()
    {
      return (IButtonDecorator) ButtonDecorator.GetSharedInstance(this.ButtonElement);
    }

    protected override void SetEnabledInternal(bool enabled)
    {
      this.ButtonElement.SetEnabled(enabled);
      if (!this.OuterDecor.HasValue)
        return;
      this.OuterDecor.Value.SetVisible(enabled);
    }
  }
}
