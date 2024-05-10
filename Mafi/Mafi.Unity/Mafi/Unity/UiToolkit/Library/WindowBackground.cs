// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.WindowBackground
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class WindowBackground : Outer
  {
    public WindowBackground()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(Cls.diamondPattern);
      float num = (float) (Theme.WindowBackgroundSize / UiScaleHelper.GetCurrentScaleFloat());
      this.Element.style.backgroundSize = (StyleBackgroundSize) new BackgroundSize((Length) num, (Length) num);
      this.Add(new UiComponent().Class<UiComponent>(Cls.windowBorder));
      this.WrapClassName = (Option<string>) Cls.windowBorder;
    }
  }
}
