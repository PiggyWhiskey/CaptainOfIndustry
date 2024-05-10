// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForFix32
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Globalization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors
{
  internal class ObjEditorForFix32 : ObjEditorForNumber<Fix32>
  {
    protected override string ExpectedFormatMsg => "expected number (double, fix32).";

    public ObjEditorForFix32(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
    }

    protected override bool TryParse(string input, out Fix32 result)
    {
      double result1;
      if (double.TryParse(input, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1) && Fix32.TryCreateFromDouble(result1, out result))
        return true;
      result = new Fix32();
      return false;
    }

    protected override string ToString(Fix32 value)
    {
      return value.ToDouble().ToString("0.####", (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
