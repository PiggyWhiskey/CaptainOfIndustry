// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForText
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForText : ObjEditorForString<string>
  {
    protected override string ExpectedFormatMsg => "string";

    public ObjEditorForText(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
    }

    public override void SetData(ObjEditorData data)
    {
      base.SetData(data);
      MemberInfo valueOrNull = data.Member.ValueOrNull;
      EditorTextAreaAttribute customAttribute = (object) valueOrNull != null ? valueOrNull.GetCustomAttribute<EditorTextAreaAttribute>() : (EditorTextAreaAttribute) null;
      if (customAttribute != null)
      {
        this.ForFieldSetWidth(new Px?());
        this.Multiline(labelOnTop: true);
        Px px = (30 + customAttribute.LinesCount * 14).px();
        this.SetTextAreaHeight(customAttribute.AutoScale ? new Px?() : new Px?(px));
        this.SetTextAreaMinHeight(customAttribute.AutoScale ? new Px?(px) : new Px?());
      }
      else
      {
        this.Multiline(false);
        this.SetTextAreaHeight(new Px?());
        this.SetTextAreaMinHeight(new Px?());
      }
    }

    protected override bool TryParse(string input, out string result)
    {
      result = input;
      return true;
    }

    protected override string ToString(string value) => value;
  }
}
