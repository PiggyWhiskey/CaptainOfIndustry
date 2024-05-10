// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForColor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForColor : ObjEditorForString<ColorRgba>
  {
    private readonly UiComponent m_colorPreview;

    protected override string ExpectedFormatMsg => "expected color value in format RGB(A)";

    public ObjEditorForColor(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
      this.m_colorPreview = new UiComponent().Border<UiComponent>(2, new ColorRgba?(ColorRgba.Black)).Size<UiComponent>(22.px()).AbsolutePositionMiddle<UiComponent>(new Px?(4.px()));
      this.ForFieldAddComponent(this.m_colorPreview);
      this.ForFieldAddComponent((UiComponent) new Label("#".AsLoc()).Size<Label>(10.px()).AlignText<Label>(TextAlign.CenterMiddle).Opacity<Label>(0.6f).AbsolutePositionMiddle<Label>(new Px?(22.px() + 6.px())));
      this.ForFieldSetPaddingLeft(new Px?(36.px()));
    }

    protected override bool TryParse(string input, out ColorRgba result)
    {
      Color color;
      if (ColorUtility.TryParseHtmlString("#" + input.Trim('#'), out color))
      {
        result = new ColorRgba(color.r, color.g, color.b, color.a);
        this.m_colorPreview.Background<UiComponent>(new ColorRgba?(result));
        return true;
      }
      result = ColorRgba.Empty;
      return false;
    }

    protected override string ToString(ColorRgba value) => value.ToHexNoHash();

    protected override void OnValueSet(object value)
    {
      this.m_colorPreview.Background<UiComponent>(new ColorRgba?((ColorRgba) value));
    }
  }
}
