// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ButtonText
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class ButtonText : Button, IComponentWithText
  {
    public ButtonText(
      LocStrFormatted text = default (LocStrFormatted),
      Action onClick = null,
      Outer outer = null,
      Inner inner = null,
      bool lowerCase = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(onClick, outer ?? Outer.ShadowCutCorner, inner);
      this.Variant<ButtonText>(ButtonVariant.Default);
      this.AlignTextCenter<ButtonText>();
      if (!text.IsNotEmpty)
        return;
      this.InnerElement.text = lowerCase ? text.Value : text.Value.ToUpper();
      HACK_TextLayoutFix.Apply((VisualElement) this.InnerElement);
    }

    void IComponentWithText.SetText(LocStrFormatted text)
    {
      this.ButtonElement.text = text.Value.ToUpper();
    }

    void IComponentWithText.SetTextOverflow(Mafi.Unity.UiToolkit.Component.TextOverflow overflow)
    {
      overflow.ApplyTo((VisualElement) this.ButtonElement);
    }
  }
}
