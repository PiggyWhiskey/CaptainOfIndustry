﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ButtonBold
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class ButtonBold : ButtonText
  {
    public ButtonBold(LocStrFormatted text, Action onClick = null, bool lowerCase = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(text, onClick, lowerCase: lowerCase);
      this.Class<ButtonBold>(Cls.bold);
    }
  }
}
