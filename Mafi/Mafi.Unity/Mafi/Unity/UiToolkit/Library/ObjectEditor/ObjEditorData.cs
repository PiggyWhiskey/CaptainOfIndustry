// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  public readonly struct ObjEditorData
  {
    public readonly Option<MemberInfo> Member;
    public readonly Type Type;
    public readonly object Value;
    public readonly int NestingLevel;
    public readonly string Label;
    public readonly string LabelForCollapse;
    public readonly string Tooltip;
    public readonly bool IsLabelHeader;
    public readonly bool IsErrorLabel;
    public readonly Option<MemberInfo> PassedThroughParentMember;
    /// <summary>Owner object of this member</summary>
    public readonly Option<object> Owner;
    /// <summary>
    /// If set, this property has options defined in the referenced member in form of a dropdown.
    /// </summary>
    public readonly Option<MemberInfo> DropdownSourceMember;

    public ObjEditorData(
      MemberInfo member,
      Type type,
      int nestingLevel,
      object owner,
      Option<MemberInfo> dropdownSourceMember,
      ObjEditorData? customInfo = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Member = (Option<MemberInfo>) member;
      this.Type = type;
      this.Value = ObjEditorUtils.GetValue(member, owner);
      this.NestingLevel = nestingLevel;
      EditorLabelAttribute customAttribute = member.GetCustomAttribute<EditorLabelAttribute>();
      this.Label = customInfo?.Label ?? ObjEditorData.processLabel(member, customAttribute, this.Type);
      this.LabelForCollapse = this.Label;
      this.Tooltip = customInfo?.Tooltip ?? customAttribute?.Tooltip ?? string.Empty;
      this.IsLabelHeader = customAttribute != null && customAttribute.IsHeader;
      this.IsErrorLabel = customAttribute != null && customAttribute.IsError;
      this.Owner = (Option<object>) owner;
      this.DropdownSourceMember = dropdownSourceMember;
      this.PassedThroughParentMember = customInfo.HasValue ? customInfo.GetValueOrDefault().Member : Option<MemberInfo>.None;
    }

    public ObjEditorData(
      Type type,
      object value,
      int nestingLevel,
      string label = null,
      string labelForCollapse = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.PassedThroughParentMember = new Option<MemberInfo>();
      this.Member = Option<MemberInfo>.None;
      this.Type = type;
      this.Value = value;
      this.NestingLevel = nestingLevel;
      this.Label = label ?? string.Empty;
      this.LabelForCollapse = labelForCollapse ?? this.Label;
      this.Tooltip = string.Empty;
      this.IsLabelHeader = false;
      this.IsErrorLabel = false;
      this.Owner = Option<object>.None;
      this.DropdownSourceMember = Option<MemberInfo>.None;
    }

    private static string processLabel(MemberInfo memberInfo, EditorLabelAttribute attr, Type type)
    {
      if (attr != null && attr.Label != null)
        return attr.Label;
      string str = ObjEditorConfig.GetUnitsSuffix(type);
      if (str.IsNotEmpty())
        str = " (" + str + ")";
      return ObjEditorUtils.ProcessCamelCase(memberInfo.Name) + str;
    }
  }
}
