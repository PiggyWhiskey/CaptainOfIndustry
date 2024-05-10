// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.BlueprintDescriptionDialog
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Blueprints
{
  internal class BlueprintDescriptionDialog : DialogView
  {
    private readonly TxtField m_txtField;

    public BlueprintDescriptionDialog(UiBuilder builder, Action<string> onApplyChanges)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      BlueprintDescriptionDialog descriptionDialog = this;
      this.HideMessage();
      this.Width = 400f;
      Txt element = builder.NewTitle("Title").SetText((LocStrFormatted) Tr.UpdateDescription__Title).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(builder.Style.Global.TitleBig);
      element.SetSize<Txt>(element.GetPreferedSize());
      this.AppendCustomElement((IUiElement) element);
      this.m_txtField = builder.NewTxtField("TxtField").SetStyle(builder.Style.Global.LightTxtFieldStyle).SetPlaceholderText(string.Format("{0} ...", (object) Tr.UpdateDescription__Placeholder)).SetCharLimit(0).EnableSelectionOnFocus().MakeMultiline();
      this.m_txtField.SetHeight<TxtField>(250f);
      this.AppendCustomElement((IUiElement) this.m_txtField);
      this.AppendBtnPrimary((LocStrFormatted) Tr.ApplyChanges).OnClick((Action) (() =>
      {
        onApplyChanges(descriptionDialog.m_txtField.GetText());
        descriptionDialog.Hide();
      }));
      this.AppendBtnGeneral((LocStrFormatted) Tr.Cancel).OnClick(new Action(((DialogView) this).Hide));
      this.HighlightAsSettings();
    }

    public void ShowForEdit(string description)
    {
      this.m_txtField.SetText(description);
      this.Show();
    }
  }
}
