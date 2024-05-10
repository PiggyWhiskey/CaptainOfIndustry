// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.Settings.KeyBindingField
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.Settings
{
  public class KeyBindingField : Label
  {
    private Option<Icon> m_errorIcon;

    public KeyBindingField()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<KeyBindingField>(Cls.inputText).Padding<KeyBindingField>(5.px(), 6.px());
    }

    public KeyBindingField MarkAsError(bool isError, LocStrFormatted tooltip = default (LocStrFormatted))
    {
      this.ClassIff<KeyBindingField>(Cls.error, isError);
      if (isError && this.m_errorIcon.IsNone)
      {
        this.m_errorIcon = (Option<Icon>) new Icon().Class<Icon>(Cls.errorIcon, Cls.attachedIconLeft);
        this.Add((UiComponent) this.m_errorIcon.Value);
        this.RunWithBuilder((Action<UiBuilder>) (builder => this.m_errorIcon.Value.Build(builder)));
      }
      Icon valueOrNull = this.m_errorIcon.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.Tooltip<Icon>(new LocStrFormatted?(tooltip));
      this.m_errorIcon.ValueOrNull?.SetVisible(isError);
      return this;
    }
  }
}
