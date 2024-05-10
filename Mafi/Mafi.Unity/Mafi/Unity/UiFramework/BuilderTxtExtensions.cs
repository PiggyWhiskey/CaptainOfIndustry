// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BuilderTxtExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class BuilderTxtExtensions
  {
    public static Txt NewTxt(this UiBuilder builder, string name) => new Txt(builder, name);

    internal static Txt NewTxt(this UiBuilder builder, string name, IUiElement parent)
    {
      return new Txt(builder, name, parent?.GameObject);
    }

    public static Txt NewTitle(this UiBuilder builder, string name)
    {
      return new Txt(builder, name).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    internal static Txt NewTitle(this UiBuilder builder, string name, IUiElement parent)
    {
      return new Txt(builder, name, parent?.GameObject).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    public static Txt NewTitleBig(this UiBuilder builder, string name)
    {
      return new Txt(builder, name).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleLeft);
    }

    internal static Txt NewTitleBigCentered(this UiBuilder builder, IUiElement parent)
    {
      return new Txt(builder, "Title", parent?.GameObject).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleCenter);
    }

    public static Txt NewTitle(this UiBuilder builder, LocStr name)
    {
      return new Txt(builder, "title").SetText((LocStrFormatted) name).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }

    internal static Txt NewTitle(this UiBuilder builder, LocStr name, IUiElement parent)
    {
      return new Txt(builder, "title", parent?.GameObject).SetText((LocStrFormatted) name).SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
    }
  }
}
