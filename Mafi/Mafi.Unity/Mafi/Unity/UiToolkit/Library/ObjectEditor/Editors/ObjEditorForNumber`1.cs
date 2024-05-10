// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForNumber`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal abstract class ObjEditorForNumber<T> : ObjEditorForString<T>, IObjEditorWithInlineSupport
  {
    private static readonly Percent INLINE_GAP;
    private static readonly Percent INLINE_GAP_COLUMN;

    public bool CanBeInlined => !this.IsOptional;

    protected ObjEditorForNumber(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
    }

    public void LayoutAsInline(bool useTwoLines, int editorsTotal)
    {
      Percent percent = (useTwoLines ? ObjEditorForNumber<T>.INLINE_GAP_COLUMN : ObjEditorForNumber<T>.INLINE_GAP) * (editorsTotal - 1);
      Percent width = (100.Percent() - percent) / editorsTotal;
      this.Class<ObjEditorForNumber<T>>(Cls.inline).Width<ObjEditorForNumber<T>>(width);
      this.ForFieldSetWidth(new Px?());
      this.ColumnDirection(useTwoLines);
      int num = editorsTotal > 2 ? 10 : 20;
      if (!useTwoLines || this.Data.Label.Length <= num)
        return;
      this.Label<ObjEditorForNumber<T>>(this.Data.Label.Replace(' ', '\n').AsLoc());
    }

    public override void SetData(ObjEditorData data)
    {
      this.ClassIff<ObjEditorForNumber<T>>(Cls.inline, false);
      this.ColumnDirection(false);
      this.Width<ObjEditorForNumber<T>>(new Px?());
      base.SetData(data);
    }

    public string GetLabel() => this.Data.Label;

    static ObjEditorForNumber()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjEditorForNumber<T>.INLINE_GAP = 4.Percent();
      ObjEditorForNumber<T>.INLINE_GAP_COLUMN = 12.Percent();
    }
  }
}
