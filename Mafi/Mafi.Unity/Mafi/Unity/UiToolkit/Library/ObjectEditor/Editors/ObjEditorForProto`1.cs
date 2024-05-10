// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForProto`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForProto<T> : Dropdown<T>, IObjEditor where T : Proto
  {
    private ObjEditorData m_data;
    private Type m_optionType;

    public UiComponent Component => (UiComponent) this;

    public ObjEditorForProto(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ValueChanged((Action<T, int>) ((_1, _2) => editor.ReportValueChanged()));
      this.SetSearchStringLookup((Func<T, string>) (p => p.Strings.Name.TranslatedString));
      this.ConstrainMenuWidth(false);
      if (typeof (T).IsAssignableTo<TerrainMaterialProto>())
        this.SetOptionViewFactory((Func<T, int, bool, UiComponent>) ((p, _3, _4) =>
        {
          TerrainMaterialProto terrainMaterialProto = (object) p as TerrainMaterialProto;
          return (UiComponent) new Row(2.pt())
          {
            (UiComponent) new Img(terrainMaterialProto.MinedProduct.IconPath).Size<Img>(32.px()).FlexNoShrink<Img>(),
            (UiComponent) new Label((LocStrFormatted) p.Strings.Name)
          };
        }));
      else
        this.SetOptionViewFactory((Func<T, int, bool, UiComponent>) ((p, _5, _6) => (UiComponent) new Label((LocStrFormatted) p.Strings.Name)));
    }

    public void SetOptionType(Type optionType)
    {
      this.m_optionType = optionType;
      if (!(optionType != (Type) null))
        return;
      this.IncludeClearOption();
    }

    public virtual void SetData(ObjEditorData data)
    {
      this.m_data = data;
      this.ForEditorSetWidth(ObjEditor.GetEditorWidth(data));
      this.Enabled<ObjEditorForProto<T>>(true).Label<ObjEditorForProto<T>>(data.Label.AsLoc()).Tooltip<ObjEditorForProto<T>>(new LocStrFormatted?(data.Tooltip.AsLoc()));
      this.setValue(data.Value);
    }

    private void setValue(object value)
    {
      if (this.m_optionType != (Type) null)
        value = ((IOptionNonGeneric) value).ValueOrNull;
      this.SetValue(value as T);
    }

    public bool TryGetValue(out object value)
    {
      value = (object) this.SelectedValue;
      if (this.m_optionType != (Type) null)
        value = ObjEditor.ObjToOption(this.m_optionType, value);
      return true;
    }
  }
}
