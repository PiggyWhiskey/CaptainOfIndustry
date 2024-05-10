// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.ButtonOpenInBrowser
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class ButtonOpenInBrowser : ButtonRow
  {
    public ButtonOpenInBrowser(LocStrFormatted text, Action onClick, bool lowerCase = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(onClick, Outer.ShadowCutCorner);
      this.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/ExportToString.svg").Small(), (UiComponent) new Label(text).UpperCase(!lowerCase));
      this.Gap<ButtonOpenInBrowser>(new Px?(2.pt()));
      this.Variant<ButtonOpenInBrowser>(ButtonVariant.Default);
    }
  }
}
