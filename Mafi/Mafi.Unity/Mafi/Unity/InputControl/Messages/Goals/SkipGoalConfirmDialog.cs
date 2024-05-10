// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.Goals.SkipGoalConfirmDialog
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages.Goals
{
  internal class SkipGoalConfirmDialog : DialogView
  {
    public SkipGoalConfirmDialog(UiBuilder builder, Action onConfirmClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      SkipGoalConfirmDialog goalConfirmDialog = this;
      this.SetMessage((LocStrFormatted) Tr.GoalSkip__Confirmation);
      this.AppendBtnDanger((LocStrFormatted) Tr.Skip).OnClick((Action) (() =>
      {
        goalConfirmDialog.Hide();
        onConfirmClick();
      }));
      this.AppendBtnGeneral((LocStrFormatted) Tr.Cancel).OnClick(new Action(((DialogView) this).Hide));
      this.HighlightAsGeneral();
    }
  }
}
