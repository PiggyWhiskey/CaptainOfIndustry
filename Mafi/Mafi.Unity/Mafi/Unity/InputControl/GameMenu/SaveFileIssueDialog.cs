// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.SaveFileIssueDialog
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.SaveGame;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  /// <summary>
  /// Shown when we detect that we can't load the current save file.
  /// </summary>
  public class SaveFileIssueDialog : Mafi.Unity.UiToolkit.Library.Window
  {
    public SaveFileIssueDialog(SaveFileInfo saveFileInfo, LoadFailInfo failInfo)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(LocStrFormatted.Empty, darkMask: true);
      this.MarkAsError();
      this.Title((TrCore.GameSaveLoad__CannotLoadFile.TranslatedString + " \"" + saveFileInfo.NameNoExtension + "\"").AsLoc());
      string str1 = "";
      bool flag = false;
      string str2;
      if (failInfo.FailReason == LoadFailInfo.Reason.ModsMissing)
        str2 = str1 + Tr.ContinueDisabled__NeedsModConfig.Format(Tr.ConfigureMods_Action).ToString();
      else if (failInfo.MessageForPlayer.HasValue)
        str2 = str1 + failInfo.MessageForPlayer.Value.ToString();
      else if (failInfo.FailReason != LoadFailInfo.Reason.Version || !failInfo.SaveVersion.HasValue)
      {
        str2 = str1 + Tr.LoadDisabled__Error.ToString();
      }
      else
      {
        int num = failInfo.SaveVersion.Value;
        if (num > 168)
        {
          str2 = str1 + TrCore.GameSaveLoad__VersionTooHigh.Format(num.ToString(), 168.ToString()).ToString();
        }
        else
        {
          str2 = str1 + TrCore.GameSaveLoad__VersionTooLow.Format(num.ToString()).ToString() + "\n\n";
          foreach (SaveVersion.MinBranchVersion branch in SaveVersion.BRANCH_MAP)
          {
            int minSaveVersion = branch.MinSaveVersion;
            int? saveVersion = failInfo.SaveVersion;
            int valueOrDefault = saveVersion.GetValueOrDefault();
            if (minSaveVersion <= valueOrDefault & saveVersion.HasValue)
            {
              str2 += TrCore.GameSaveLoad__SwitchSteamVersion.Format("<b>'" + branch.SteamBranchName + "'</b> (v" + branch.LatestGameVersion + ")").ToString();
              flag = true;
              break;
            }
          }
        }
      }
      Column body = this.Body;
      Column column = new Column(Outer.Panel);
      column.Add<Column>((Action<Column>) (c => c.Fill<Column>().AlignItemsStretch<Column>().Padding<Column>(4.pt())));
      column.Add((UiComponent) new Label(str2.AsLoc()).FontSize<Label>(16));
      column.Add(flag ? (UiComponent) new Img("Assets/Unity/UserInterface/MainMenu/SteamVersionGuide.png").Size<Img>(760.px(), 380.px()).MarginTop<Img>(6.pt()) : (UiComponent) null);
      body.Add((UiComponent) column);
      this.Size<SaveFileIssueDialog>(800.px(), flag ? 600.px() : 300.px());
    }
  }
}
