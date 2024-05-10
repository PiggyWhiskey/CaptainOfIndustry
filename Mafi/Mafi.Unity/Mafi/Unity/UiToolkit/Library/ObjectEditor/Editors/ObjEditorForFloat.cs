// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForFloat
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
  internal class ObjEditorForFloat : ObjEditorForNumber<float>
  {
    protected override string ExpectedFormatMsg => "expected number (float).";

    public ObjEditorForFloat(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
    }

    protected override bool TryParse(string input, out float result)
    {
      return float.TryParse(input, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    }

    protected override string ToString(float value)
    {
      return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
