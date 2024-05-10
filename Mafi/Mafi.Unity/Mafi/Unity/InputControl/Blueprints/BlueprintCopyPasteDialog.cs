// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.BlueprintCopyPasteDialog
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
  internal class BlueprintCopyPasteDialog : DialogView
  {
    private readonly Txt m_title;
    private readonly Txt m_txt;
    private readonly Btn m_importBtn;
    private readonly Btn m_closeBtn;
    private readonly Txt m_errorText;
    private string m_textToCopy;
    private readonly Btn m_exportBtn;
    private bool m_isInImportMode;
    private readonly UiBuilder m_builder;
    private readonly Func<string, bool> m_onImport;

    public BlueprintCopyPasteDialog(UiBuilder builder, Func<string, bool> onImport)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_textToCopy = "";
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      this.m_builder = builder;
      this.m_onImport = onImport;
      this.HideMessage();
      this.Width = 500f;
      this.m_title = builder.NewTitle("Title").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(builder.Style.Global.TitleBig);
      this.m_title.SetSize<Txt>(this.m_title.GetPreferedSize());
      this.AppendCustomElement((IUiElement) this.m_title);
      Panel panel1 = builder.NewPanel("TxtPanel").SetBackground((ColorRgba) 3487029).SetHeight<Panel>(30f);
      this.AppendCustomElement((IUiElement) panel1);
      this.m_txt = builder.NewTxt("Txt").SetTextStyle(builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) panel1, Offset.LeftRight(5f));
      Panel panel2 = builder.NewPanel("ErrorPanel").SetHeight<Panel>(20f);
      this.AppendCustomElement((IUiElement) panel2);
      this.m_errorText = builder.NewTxt("Error").SetTextStyle(builder.Style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) panel2).Hide<Txt>();
      string iconPath = "Assets/Unity/UserInterface/General/Clipboard.svg";
      this.m_importBtn = this.AppendBtnPrimary((LocStrFormatted) Tr.PasteString__Action, (Option<string>) iconPath).AddToolTip(Tr.PasteString__Tooltip).OnClick(new Action(this.importString));
      this.m_exportBtn = this.AppendBtnPrimary((LocStrFormatted) Tr.CopyString__Action, (Option<string>) iconPath).AddToolTip(Tr.CopyString__Tooltip).OnClick(new Action(this.exportString));
      this.m_closeBtn = this.AppendBtnGeneral((LocStrFormatted) Tr.Close).OnClick(new Action(((DialogView) this).Hide));
      this.HighlightAsSettings();
    }

    private void importString()
    {
      string systemCopyBuffer = GUIUtility.systemCopyBuffer;
      this.setStringToPreview(systemCopyBuffer);
      this.m_errorText.Show<Txt>();
      if (this.m_onImport(systemCopyBuffer))
      {
        this.m_errorText.SetText((LocStrFormatted) Tr.ImportBlueprint__Success);
        this.m_errorText.SetColor(this.m_builder.Style.Global.GreenForDark);
        this.m_importBtn.Hide<Btn>();
      }
      else
      {
        this.m_errorText.SetText((LocStrFormatted) Tr.ImportBlueprint__Fail);
        this.m_errorText.SetColor(this.m_builder.Style.Global.DangerClr);
      }
    }

    private void exportString()
    {
      GUIUtility.systemCopyBuffer = this.m_textToCopy;
      this.m_errorText.Show<Txt>();
      this.m_errorText.SetText((LocStrFormatted) Tr.CopyString__Success);
      this.m_errorText.SetColor(this.m_builder.Style.Global.GreenForDark);
    }

    public void ShowForStringImport()
    {
      this.m_isInImportMode = true;
      this.m_title.SetText((LocStrFormatted) Tr.ImportBlueprint__Title);
      this.m_txt.SetText("");
      this.SetBtnVisibility(this.m_exportBtn, false);
      this.SetBtnVisibility(this.m_importBtn, true);
      this.m_errorText.Hide<Txt>();
      this.Show();
    }

    public void ShowForStringExport(string text)
    {
      this.m_isInImportMode = false;
      this.m_textToCopy = text;
      this.m_title.SetText((LocStrFormatted) Tr.ExportBlueprint__Title);
      this.setStringToPreview(text);
      this.SetBtnVisibility(this.m_exportBtn, true);
      this.SetBtnVisibility(this.m_importBtn, false);
      this.m_errorText.Hide<Txt>();
      this.Show();
    }

    private void setStringToPreview(string text)
    {
      int length = text.Length;
      string str = Tr.CharactersCount.Format(length).Value;
      this.m_txt.SetText("\"" + text.Substring(0, length.Min(26)) + "...\" (" + str + ")");
    }

    public bool InputUpdate()
    {
      if (this.m_isInImportMode && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
      {
        this.importString();
        return true;
      }
      if (this.m_isInImportMode || !Input.GetKey(KeyCode.LeftControl) || !Input.GetKeyDown(KeyCode.C))
        return false;
      this.exportString();
      return true;
    }
  }
}
