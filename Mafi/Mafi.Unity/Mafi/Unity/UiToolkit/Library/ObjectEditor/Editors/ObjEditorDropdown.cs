// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorDropdown
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorDropdown : Dropdown<LocStrFormatted>, IObjEditor
  {
    private ObjEditorData m_data;
    private Lyst<KeyValuePair<string, object>> m_options;
    private Func<object, object, bool> m_equalsFunc;
    private Type m_optionType;

    public UiComponent Component => (UiComponent) this;

    public ObjEditorDropdown(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ValueChanged((Action<LocStrFormatted, int>) ((_1, _2) => editor.ReportValueChanged()));
    }

    public void SetOptions(
      Lyst<KeyValuePair<string, object>> options,
      Func<object, object, bool> equalsFunc,
      Type optionType = null)
    {
      this.m_options = options;
      this.m_equalsFunc = equalsFunc;
      this.m_optionType = optionType;
      this.SetOptions((IEnumerable<LocStrFormatted>) options.Select<LocStrFormatted>((Func<KeyValuePair<string, object>, LocStrFormatted>) (x => x.Key.AsLoc()))).ToLyst<UiComponent>();
    }

    public virtual void SetData(ObjEditorData data)
    {
      this.m_data = data;
      this.ForEditorSetWidth(ObjEditor.GetEditorWidth(data));
      this.Enabled<ObjEditorDropdown>(true).Label<ObjEditorDropdown>(data.Label.AsLoc()).Tooltip<ObjEditorDropdown>(new LocStrFormatted?(data.Tooltip.AsLoc()));
      this.setValue(data.Value);
    }

    private void setValue(object value)
    {
      if (this.m_optionType != (Type) null)
        value = ((IOptionNonGeneric) value).ValueOrNull;
      for (int index = 0; index < this.m_options.Count; ++index)
      {
        if (this.m_equalsFunc(this.m_options[index].Value, value))
        {
          this.SetValueIndex(index);
          return;
        }
      }
      Log.Error(string.Format("Failed to found matching item for {0} in {1}", value, (object) this.m_data.Label));
    }

    public bool TryGetValue(out object value)
    {
      value = this.m_options[this.SelectedIndex].Value;
      if (this.m_optionType != (Type) null)
        value = ObjEditor.ObjToOption(this.m_optionType, value);
      return true;
    }
  }
}
