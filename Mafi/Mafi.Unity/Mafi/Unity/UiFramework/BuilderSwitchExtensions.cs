// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BuilderSwitchExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class BuilderSwitchExtensions
  {
    public static SwitchBtn NewSwitchBtn(this UiBuilder builder) => new SwitchBtn(builder);

    internal static SwitchBtn NewSwitchBtn(this UiBuilder builder, IUiElement parent)
    {
      return new SwitchBtn(builder, parent);
    }
  }
}
