// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.Utilities
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public static class Utilities
  {
    public static Func<StartingLocationPreview, int, bool, UiComponent> StartingLocationViewFactory(
      bool showLocationDifficulty)
    {
      return (Func<StartingLocationPreview, int, bool, UiComponent>) ((st, idx, inMenu) =>
      {
        string str1 = string.Format("#{0}", (object) (idx + 1));
        string str2 = string.Empty;
        if (showLocationDifficulty & inMenu)
          str1 = string.Format("{0} ({1})", (object) str1, (object) st.Difficulty.ToLabel());
        if (!inMenu)
          str2 = string.Format("{0}: {1}", (object) Tr.StartingLocation_Title, (object) str1);
        return (UiComponent) new Label(str1.AsLoc()).TextOverflow<Label>(inMenu ? TextOverflow.Wrap : TextOverflow.Ellipsis);
      });
    }
  }
}
