// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BuilderPanelExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class BuilderPanelExtensions
  {
    public static Panel NewPanel(this UiBuilder builder, string name) => new Panel(builder, name);

    internal static Panel NewPanel(this UiBuilder builder, string name, IUiElement parent)
    {
      return new Panel(builder, name, parent?.GameObject);
    }

    public static PanelWithShadow NewPanelWithShadow(this UiBuilder builder, string name)
    {
      return new PanelWithShadow(builder, name);
    }

    internal static PanelWithShadow NewPanelWithShadow(
      this UiBuilder builder,
      string name,
      IUiElement parent)
    {
      return new PanelWithShadow(builder, name, parent?.GameObject);
    }
  }
}
