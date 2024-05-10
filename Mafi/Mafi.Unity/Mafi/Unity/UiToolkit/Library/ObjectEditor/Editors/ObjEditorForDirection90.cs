// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForDirection90
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForDirection90 : Dropdown<int>, IObjEditor
  {
    public UiComponent Component => (UiComponent) this;

    public ObjEditorForDirection90(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetOptionViewFactory((Func<int, int, bool, UiComponent>) ((item, index, isInDropdown) => (UiComponent) new Label(new Direction90(item).ToString().AsLoc())));
      this.AddOption(0);
      this.AddOption(1);
      this.AddOption(2);
      this.AddOption(3);
      this.ValueChanged((Action<int, int>) ((_1, _2) => editor.ReportValueChanged()));
    }

    public virtual void SetData(ObjEditorData data)
    {
      this.ForEditorSetWidth(ObjEditor.GetEditorWidth(data));
      this.Enabled<ObjEditorForDirection90>(true).Label<ObjEditorForDirection90>(data.Label.AsLoc()).Tooltip<ObjEditorForDirection90>(new LocStrFormatted?(data.Tooltip.AsLoc()));
      this.setValue(data.Value);
    }

    private void setValue(object value) => this.SetValueIndex(((Direction90) value).DirectionIndex);

    public bool TryGetValue(out object value)
    {
      value = (object) new Direction90(this.SelectedIndex);
      return true;
    }
  }
}
