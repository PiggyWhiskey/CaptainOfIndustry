// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForBoolean
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
  internal class ObjEditorForBoolean : Toggle, IObjEditor
  {
    public UiComponent Component => (UiComponent) this;

    public ObjEditorForBoolean(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.OnValueChanged((Action<bool>) (_ => editor.ReportValueChanged()));
    }

    public bool TryGetValue(out object value)
    {
      value = (object) this.GetValue();
      return true;
    }

    public virtual void SetData(ObjEditorData data)
    {
      this.ForEditorSetWidth(ObjEditor.GetEditorWidth(data));
      this.Enabled<ObjEditorForBoolean>(true).Label<ObjEditorForBoolean>(data.Label.AsLoc()).Tooltip<ObjEditorForBoolean>(new LocStrFormatted?(data.Tooltip.AsLoc())).Value((bool) data.Value);
    }
  }
}
