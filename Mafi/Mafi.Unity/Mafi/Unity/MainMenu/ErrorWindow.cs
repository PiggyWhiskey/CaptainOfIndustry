// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.ErrorWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class ErrorWindow : Mafi.Unity.UiToolkit.Library.Window, IRootEscapeHandler
  {
    private readonly Queueue<(DateTime time, string message, string details)> m_pendingErrors;
    private ImmutableArray<(DateTime time, string message, string details)> m_errors;
    private int m_selectedIndex;
    private readonly ScrollColumn m_messageColumn;
    private readonly ScrollColumn m_detailColumn;
    private readonly Button m_clearButton;
    private readonly Button m_copyButton;
    private readonly UiComponent m_check;

    public bool HasErrors { get; private set; }

    public ErrorWindow(IEnumerable<(string, string)> errors = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_pendingErrors = new Queueue<(DateTime, string, string)>(100);
      this.m_selectedIndex = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector("Errors".AsLoc(), darkMask: true);
      Column body = this.Body;
      Row row1 = new Row(3.pt());
      row1.Add<Row>((Action<Row>) (c => c.Fill<Row>().AlignItemsStretch<Row>()));
      Column column1 = new Column(Outer.Panel);
      column1.Add<Column>((Action<Column>) (c => c.Width<Column>(36.Percent()).AlignItemsStretch<Column>().Padding<Column>(3.pt())));
      ScrollColumn component1 = new ScrollColumn();
      component1.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Fill<ScrollColumn>().Class<ScrollColumn>(Cls.listGroup)));
      ScrollColumn child1 = component1;
      this.m_messageColumn = component1;
      column1.Add((UiComponent) child1);
      Row row2 = new Row(Outer.EdgeShadowTop);
      row2.Add<Row>((Action<Row>) (c => c.FlexNoShrink<Row>().PaddingTop<Row>(3.pt())));
      row2.Add((UiComponent) (this.m_clearButton = (Button) new ButtonText("Clear errors".AsLoc(), new Action(this.clearErrors)).MarginLeft<ButtonText>(Px.Auto)));
      column1.Add((UiComponent) row2);
      row1.Add((UiComponent) column1);
      Column column2 = new Column(Outer.Panel);
      column2.Add<Column>((Action<Column>) (c => c.Fill<Column>().AlignItemsStretch<Column>().Padding<Column>(3.pt())));
      ScrollColumn component2 = new ScrollColumn();
      component2.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Fill<ScrollColumn>().PaddingBottom<ScrollColumn>(20.pt())));
      ScrollColumn child2 = component2;
      this.m_detailColumn = component2;
      column2.Add((UiComponent) child2);
      Row row3 = new Row(Outer.EdgeShadowTop, gap: new Px?(2.pt()));
      row3.Add<Row>((Action<Row>) (c => c.FlexNoShrink<Row>().PaddingTop<Row>(3.pt())));
      row3.Add((UiComponent) (this.m_copyButton = (Button) new ButtonText("Copy to clipboard".AsLoc(), new Action(this.copyError)).FlipNotches<ButtonText>().Enabled<ButtonText>(false)));
      row3.Add(this.m_check = (UiComponent) new Icon("Assets/Unity/UserInterface/General/Checkmark-v2.svg").Class<Icon>(Cls.fadeIn));
      column2.Add((UiComponent) row3);
      row1.Add((UiComponent) column2);
      body.Add((UiComponent) row1);
      if (errors != null)
      {
        foreach ((string, string) error in errors)
          this.AddErrorThreadSafe(error.Item1, error.Item2);
      }
      this.OnShow(new Action(this.handleShow));
      this.OnHide(new Action(this.handleHide));
    }

    public ErrorWindow AddErrorThreadSafe(LocStrFormatted message, LocStrFormatted details)
    {
      return this.AddErrorThreadSafe(message.ToString(), details.ToString());
    }

    public ErrorWindow AddErrorThreadSafe(string message, string details)
    {
      lock (this.m_pendingErrors)
      {
        for (int index = 0; index < this.m_pendingErrors.Count; ++index)
        {
          (DateTime _, string message1, string details1) = this.m_pendingErrors[index];
          if (message1 == message && details.Length == details1.Length)
          {
            this.m_pendingErrors.RemoveAt(index);
            break;
          }
        }
        if (this.m_pendingErrors.Count >= 100)
          this.m_pendingErrors.Dequeue();
        this.m_pendingErrors.Enqueue((DateTime.Now, message, details));
        this.HasErrors = true;
      }
      return this;
    }

    private void handleShow()
    {
      this.RunWithBuilder((Action<UiBuilder>) (b => b.SetOneTimeEscBlockingCallback((IRootEscapeHandler) this)));
      this.updateContents();
    }

    private void handleHide()
    {
      this.RunWithBuilder((Action<UiBuilder>) (b => b.ClearEscBlockingCallback((IRootEscapeHandler) this)));
    }

    private void updateContents()
    {
      lock (this.m_pendingErrors)
        this.m_errors = this.m_pendingErrors.AsEnumerable<(DateTime, string, string)>().Reverse<(DateTime, string, string)>().ToImmutableArray<(DateTime, string, string)>();
      this.m_messageColumn.SetChildren((IEnumerable<UiComponent>) this.m_errors.Select<ButtonRow>((Func<(DateTime, string, string), int, ButtonRow>) ((entry, idx) =>
      {
        ButtonRow buttonRow = new ButtonRow((Action) (() => this.selectIndex(idx)));
        buttonRow.Add<ButtonRow>((Action<ButtonRow>) (c => c.Variant<ButtonRow>(ButtonVariant.Area).Padding<ButtonRow>(2.pt(), 3.pt()).BorderBottom<ButtonRow>(idx != this.m_errors.Length - 1 ? 1 : 0)));
        buttonRow.Add((UiComponent) new Label(string.Format("{0:T}:  <b>{1}</b>", (object) entry.time, (object) entry.message).AsLoc()));
        return this.selectRow(buttonRow, this.m_selectedIndex == idx);
      })));
      this.selectIndex(this.m_errors.Length > 0 ? 0 : -1);
      this.m_clearButton.Enabled<Button>(this.m_errors.Length > 0);
    }

    private ButtonRow selectRow(ButtonRow row, bool selected)
    {
      if (selected && row.Count < 2)
      {
        ButtonRow buttonRow = row;
        Icon component = new Icon("Assets/Unity/UserInterface/General/ArrowRight.svg").Small();
        Px? nullable = new Px?(6.px());
        Px? top = new Px?();
        Px? right = nullable;
        Px? bottom = new Px?();
        Px? left = new Px?();
        Icon child = component.AbsolutePosition<Icon>(top, right, bottom, left).Color<Icon>(new ColorRgba?(Theme.PrimaryColor));
        buttonRow.Add((UiComponent) child);
      }
      else if (!selected && row.Count > 1)
        row.ChildAtOrNone(1).Value.RemoveFromHierarchy();
      return row.Selected<ButtonRow>(selected);
    }

    private void selectIndex(int index)
    {
      Option<ButtonRow> option = this.m_messageColumn.ChildAtOrNone<ButtonRow>(this.m_selectedIndex);
      if (option.HasValue)
        this.selectRow(option.Value, false);
      this.m_selectedIndex = index.Clamp(-1, this.m_errors.Length - 1);
      if (this.m_selectedIndex >= 0)
      {
        this.selectRow(this.m_messageColumn.ChildAtOrDefault<ButtonRow>(this.m_selectedIndex), true);
        (DateTime time, string message, string str) = this.m_errors[index];
        this.m_detailColumn.SetChildren((UiComponent) new Mafi.Unity.UiToolkit.Library.Title(string.Format("{0:T}:  {1}", (object) time, (object) message).AsLoc()).UpperCase(false).MarginTop<Label>(-1.pt()).MarginBottom<Label>(2.pt()), (UiComponent) new Label(str.AsLoc()));
        this.m_copyButton.Enabled<Button>(true);
      }
      else
      {
        this.m_detailColumn.Clear();
        this.m_copyButton.Enabled<Button>(false);
      }
    }

    private void copyError()
    {
      if (this.m_selectedIndex < 0)
        return;
      (DateTime time, string message, string details) error = this.m_errors[this.m_selectedIndex];
      DateTime time = error.time;
      GUIUtility.systemCopyBuffer = error.message + ":\n" + error.details;
      this.m_check.Class<UiComponent>(Cls.show);
      this.Schedule.Execute((Action) (() => this.m_check.ClassRemove<UiComponent>(Cls.show))).ExecuteLater(1000L);
    }

    private void clearErrors()
    {
      this.m_pendingErrors.Clear();
      this.updateContents();
      this.SetVisible(false);
    }

    bool IRootEscapeHandler.OnEscape()
    {
      if (!this.IsVisible())
        return false;
      this.SetVisible(false);
      return true;
    }
  }
}
