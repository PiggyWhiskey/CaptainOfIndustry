// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiObjectEditor.EditorMaxLengthAttribute
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.UiObjectEditor
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorMaxLengthAttribute : Attribute, IEditorValidationAttribute
  {
    public readonly int MaxLength;

    public EditorMaxLengthAttribute(int maxLength)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxLength = maxLength;
    }

    public string ValidateObj(object obj)
    {
      string str = (string) obj;
      return str.Length > this.MaxLength ? string.Format("Max chars allowed {0} (is {1})", (object) this.MaxLength, (object) str.Length) : string.Empty;
    }
  }
}
