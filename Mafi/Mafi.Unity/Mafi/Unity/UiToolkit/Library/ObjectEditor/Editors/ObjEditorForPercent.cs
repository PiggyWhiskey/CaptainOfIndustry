// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors.ObjEditorForPercent
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
  internal class ObjEditorForPercent : ObjEditorForNumber<Percent>
  {
    protected override string ExpectedFormatMsg => "expected Percent value.";

    public ObjEditorForPercent(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(editor);
    }

    protected override bool TryParse(string input, out Percent result)
    {
      double result1;
      if (double.TryParse(input.TrimEnd('%'), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      {
        result = Percent.FromDouble(result1 / 100.0);
        return true;
      }
      result = Percent.Zero;
      return false;
    }

    protected override string ToString(Percent value)
    {
      return (value.ToDouble() * 100.0).Round(2).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "%";
    }
  }
}
