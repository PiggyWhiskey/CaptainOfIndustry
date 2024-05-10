// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.TooltipBuilderExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class TooltipBuilderExtensions
  {
    public static Tooltip AddTooltipFor<T>(this UiBuilder builder, IUiElementWithHover<T> element)
    {
      Tooltip tooltip = new Tooltip(builder);
      tooltip.AttachTo<T>(element);
      return tooltip;
    }

    public static Tooltip AddTooltipForTitle(
      this UiBuilder builder,
      Txt title,
      LocStrFormatted text,
      bool fixDensity = false)
    {
      return builder.AddTooltipForTitle(title, text.Value, fixDensity);
    }

    public static Tooltip AddTooltipForTitle(
      this UiBuilder builder,
      Txt title,
      string text = null,
      bool fixDensity = false)
    {
      Panel leftMiddleOf = builder.NewPanel("Info", (IUiElement) title).SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) title, 15.Vector2(), Offset.Left((float) ((fixDensity ? (double) title.GetPreferedWidth() / (double) builder.MainCanvas.ScaleFactor : (double) title.GetPreferedWidth()) + 5.0)));
      Tooltip tooltip = builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) leftMiddleOf);
      if (text != null)
        tooltip.SetText(text);
      return tooltip;
    }

    public static Tooltip AddTooltipForElement(
      this UiBuilder builder,
      IUiElement element,
      string text = null)
    {
      Panel rightMiddleOf = builder.NewPanel("Info").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToRightMiddleOf<Panel>(element, 15.Vector2(), Offset.Right(-20f));
      Tooltip tooltip = builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) rightMiddleOf);
      if (text != null)
        tooltip.SetText(text);
      return tooltip;
    }

    public static Tooltip CreateTooltipIcon(this UiBuilder builder, out Panel icon, string text = null)
    {
      icon = builder.NewPanel("Info").SetBackground("Assets/Unity/UserInterface/General/Info128.png").SetSize<Panel>(15.Vector2());
      Tooltip tooltipIcon = builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) icon);
      if (text != null)
        tooltipIcon.SetText(text);
      return tooltipIcon;
    }
  }
}
