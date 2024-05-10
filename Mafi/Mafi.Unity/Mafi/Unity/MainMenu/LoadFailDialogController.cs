// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.LoadFailDialogController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  internal class LoadFailDialogController
  {
    private readonly UiBuilder m_builder;

    internal LoadFailDialogController(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
    }

    internal void BuildUi(LocStrFormatted errorMessage, bool showReportButton)
    {
      DialogView failDialog = new DialogView(this.m_builder);
      failDialog.SetLargeMessageOnce(errorMessage);
      failDialog.HighlightAsDanger();
      failDialog.AppendBtnGeneral((LocStrFormatted) Tr.Dismiss).OnClick((Action) (() => failDialog.Hide()));
      failDialog.AppendBtnGeneral((LocStrFormatted) Tr.CopyString__Action, (Option<string>) "Assets/Unity/UserInterface/General/Clipboard.svg").OnClick((Action) (() => GUIUtility.systemCopyBuffer = errorMessage.Value));
      if (showReportButton)
        failDialog.AppendBtnPrimary((LocStrFormatted) TrCore.ReportIssue).OnClick((Action) (() => Application.OpenURL(MainMenuScreen.REPORT_ISSUE_URL)));
      failDialog.Show();
    }
  }
}
