// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.DropdownPopup`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.FloatingPanel;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class DropdownPopup<T> : FloatingContainer
  {
    private readonly Action<T, int> m_onOptionSelected;
    private ImmutableArray<T> m_options;
    private LystStruct<T> m_optionsComputed;
    private Option<UiComponent> m_header;
    private bool m_showClearOption;
    private T m_clearOptionValue;
    private Func<T, int, bool, UiComponent> m_optionViewFactory;
    private Option<TxtField> m_searchField;
    private Func<T, string> m_getSeachString;
    private string m_search;
    private bool m_isVisible;
    private bool m_hideCheckmarks;
    private T Value;

    public DropdownPopup(UiComponent target, Action<T, int> onOptionSelected, Action onClosed)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(target, onClosed);
      this.m_onOptionSelected = onOptionSelected;
      this.OnShow<DropdownPopup<T>>((Action) (() => this.m_isVisible = true));
      this.updateSearchStringLookup();
    }

    public DropdownPopup<T> SetHeader(UiComponent header)
    {
      this.m_header = (Option<UiComponent>) header;
      return this.UpdateView();
    }

    public DropdownPopup<T> SetOptions(IEnumerable<T> options)
    {
      this.m_options = options.ToImmutableArray<T>();
      return this.UpdateView();
    }

    public DropdownPopup<T> SetValue(T value)
    {
      this.Value = value;
      return this.UpdateView();
    }

    public DropdownPopup<T> SetOptionViewFactory(Func<T, int, bool, UiComponent> optionViewFactory)
    {
      this.m_optionViewFactory = optionViewFactory;
      return this.UpdateView();
    }

    public DropdownPopup<T> SetSearchStringLookup(Func<T, string> searchStringLookup)
    {
      this.updateSearchStringLookup(searchStringLookup);
      return this.UpdateView();
    }

    public DropdownPopup<T> SetClearOptionValue(T clearOption)
    {
      this.m_clearOptionValue = clearOption;
      this.m_showClearOption = true;
      return this.UpdateView();
    }

    public override void SetVisible(bool newVisible)
    {
      base.SetVisible(newVisible);
      bool focusSearch = !this.m_isVisible & newVisible;
      this.m_isVisible = newVisible;
      this.UpdateView(focusSearch);
    }

    public DropdownPopup<T> HideCheckmarks()
    {
      this.m_hideCheckmarks = true;
      this.UpdateView();
      return this;
    }

    public DropdownPopup<T> UpdateView(bool focusSearch = false)
    {
      if (!this.m_isVisible)
        return this;
      this.m_optionsComputed.Clear();
      this.m_optionsComputed.AddRange(this.m_options.AsEnumerable());
      this.updateMatchingOptions();
      LystStruct<UiComponent> lystStruct = new LystStruct<UiComponent>();
      foreach (T obj in this.m_optionsComputed)
      {
        T option = obj;
        int index = this.m_options.IndexOf(option);
        DropdownItem dropdownItem = new DropdownItem(this.m_optionViewFactory(option, index, true), (Action) (() => this.selectOption(option, index)), this.m_hideCheckmarks).Selected<DropdownItem>(!this.m_hideCheckmarks && this.isSelected(option));
        lystStruct.Add((UiComponent) dropdownItem);
      }
      UiComponent uiComponent1 = (UiComponent) null;
      if (this.m_getSeachString != null && this.m_options.Length > 8)
        uiComponent1 = (UiComponent) (this.m_searchField.HasValue ? this.m_searchField : (this.m_searchField = (Option<TxtField>) new SearchField().FlexNoShrink<SearchField>().Margin<SearchField>(0.px(), 2.px(), 2.px(), 2.px()).Text(this.m_search).OnValueChanged((Action<string>) (v =>
        {
          this.m_search = v;
          this.UpdateView();
        })))).Value;
      UiComponent[] uiComponentArray = new UiComponent[3]
      {
        this.m_header.ValueOrNull,
        uiComponent1,
        null
      };
      UiComponent uiComponent2;
      if (lystStruct.Count <= 0)
      {
        uiComponent2 = (UiComponent) new Label((LocStrFormatted) Tr.NoOptions).FontItalic<Label>().Padding<Label>(2.pt(), 3.pt());
      }
      else
      {
        uiComponent2 = (UiComponent) new ScrollColumn();
        uiComponent2.Add(lystStruct.AsEnumerable());
      }
      uiComponentArray[2] = uiComponent2;
      this.SetChildren(uiComponentArray);
      if (focusSearch)
        this.Schedule.Execute((Action) (() => this.m_searchField.ValueOrNull?.Focus()));
      return this;
    }

    private void selectOption(T option, int index)
    {
      this.m_onOptionSelected(option, this.isClearOption(option) ? -1 : index);
      this.m_searchField.ValueOrNull?.Text("");
      this.m_search = "";
    }

    private void updateMatchingOptions()
    {
      this.m_optionsComputed.Clear();
      if (this.m_getSeachString != null && !string.IsNullOrWhiteSpace(this.m_search))
      {
        foreach (T option in this.m_options)
        {
          if (this.m_getSeachString(option).Contains(this.m_search, StringComparison.CurrentCultureIgnoreCase))
            this.m_optionsComputed.Add(option);
        }
        if (this.m_optionsComputed.Count <= 1)
          return;
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_optionsComputed.Count; ++index2)
        {
          T obj = this.m_optionsComputed[index1];
          if (!this.m_getSeachString(obj).StartsWith(this.m_search, StringComparison.CurrentCultureIgnoreCase))
          {
            this.m_optionsComputed.RemoveAt(index1);
            this.m_optionsComputed.Add(obj);
          }
          else
            ++index1;
        }
      }
      else
      {
        if (this.m_showClearOption)
          this.m_optionsComputed.Add(this.m_clearOptionValue);
        this.m_optionsComputed.AddRange(this.m_options.AsEnumerable());
      }
    }

    private void updateSearchStringLookup(Func<T, string> getSearchString = null)
    {
      this.m_getSeachString = (Func<T, string>) null;
      if (getSearchString != null)
        this.m_getSeachString = getSearchString;
      else if (typeof (T).IsAssignableTo<LocStrFormatted>())
      {
        this.m_getSeachString = (Func<T, string>) (o => ((LocStrFormatted) (object) o).Value);
      }
      else
      {
        if (!typeof (T).IsAssignableTo<string>())
          return;
        this.m_getSeachString = (Func<T, string>) (o => (object) o as string);
      }
    }

    private bool isSelected(T option) => this.isSame(this.Value, option);

    private bool isClearOption(T option)
    {
      return this.m_showClearOption && this.isSame(this.m_clearOptionValue, option);
    }

    private bool isSame(T option1, T option2) => Dropdown<T>.IsSame(option1, option2);
  }
}
