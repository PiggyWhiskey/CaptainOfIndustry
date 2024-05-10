// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForInt
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForInt : ObjEditorForNumber<int>
  {
    protected override string ExpectedFormatMsg
    {
      get => "expected number (-2,147,483,648 to 2,147,483,647).";
    }

    public ObjEditorForInt(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
    }

    protected override bool TryParse(string input, out int result)
    {
      return int.TryParse(input, out result);
    }

    protected override string ToString(int value) => value.ToStringCached();
  }
}
