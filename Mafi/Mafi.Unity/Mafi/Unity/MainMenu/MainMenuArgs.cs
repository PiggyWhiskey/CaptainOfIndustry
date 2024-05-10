// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.MainMenuArgs
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class MainMenuArgs
  {
    public readonly LocStrFormatted? LoadError;
    public readonly bool DoNotOfferBugReport;
    public readonly bool ShowCredits;

    public MainMenuArgs(LocStrFormatted? loadError = null, bool doNotOfferBugReport = false, bool showCredits = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.LoadError = loadError;
      this.DoNotOfferBugReport = doNotOfferBugReport;
      this.ShowCredits = showCredits;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
