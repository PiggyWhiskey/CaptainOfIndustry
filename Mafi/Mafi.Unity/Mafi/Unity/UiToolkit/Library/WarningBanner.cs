// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.WarningBanner
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class WarningBanner : Row
  {
    public WarningBanner(LocStrFormatted message)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Gap<WarningBanner>(new Px?(3.pt())).Padding<WarningBanner>(6.px(), 10.px()).Border<WarningBanner>(1, new ColorRgba?(Theme.WarningText)).BorderRadius<WarningBanner>(2.pt());
      this.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Warning128.png").Large().Color<Icon>(new ColorRgba?(Theme.WarningText)).FlexNoShrink<Icon>(), (UiComponent) new Label(message).Class<Label>(Cls.warningText));
    }
  }
}
