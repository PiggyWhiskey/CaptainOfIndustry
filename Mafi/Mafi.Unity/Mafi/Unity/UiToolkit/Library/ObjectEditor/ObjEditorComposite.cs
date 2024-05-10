// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorComposite
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  internal class ObjEditorComposite : Column, IObjEditor, IEditorWithRenderUpdate
  {
    private static readonly Dict<Type, ImmutableArray<MemberInfo>> MEMBERS_CACHE;
    private static readonly Set<Type> SUPPORTED_READONLY_TYPES;
    private readonly ObjEditor m_editor;
    private readonly Lyst<MemberInfo> m_members;
    private readonly Lyst<IObjEditor> m_innerEditors;
    private Option<PropertyInfo> m_rebuildIfTrue;
    private ObjEditorData m_data;
    private object m_objToEdit;
    private bool m_isSelfDisabled;
    private Option<Type> m_optionType;
    private readonly Row m_labelContainer;
    private readonly ObjEditorLabel m_label;
    private readonly Row m_inlineContainer;
    private readonly Toggle m_optionToggle;
    private readonly Column m_rootItems;
    private Column m_itemsCurrent;
    private bool m_isRoot;

    public UiComponent Component => (UiComponent) this;

    public bool CanBeCollapsed => this.m_label.CanBeCollapsed;

    public bool IsCollapsed => this.m_label.IsCollapsed;

    public ObjEditorComposite(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_members = new Lyst<MemberInfo>();
      this.m_innerEditors = new Lyst<IObjEditor>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_editor = editor;
      this.Name<ObjEditorComposite>("EditorComposite").AlignItemsStretch<ObjEditorComposite>();
      UiComponent[] uiComponentArray = new UiComponent[2];
      Row component1 = new Row();
      component1.Add<Row>((Action<Row>) (c => c.IgnoreInputPicking<Row>().AlignItemsEnd<Row>()));
      component1.Add((UiComponent) (this.m_label = new ObjEditorLabel(new Action<bool>(this.onCollapse), new Action<bool>(this.onCollapseClick))));
      Px? gap = new Px?();
      component1.Add((UiComponent) (this.m_inlineContainer = new Row(gap: gap).FlexShrink<Row>(0.0f).JustifyItemsSpaceBetween<Row>().IgnoreInputPicking<Row>().AlignItemsEnd<Row>()));
      Row row = component1;
      this.m_labelContainer = component1;
      uiComponentArray[0] = (UiComponent) row;
      Column component2 = new Column(ObjEditor.GAP);
      component2.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>().PaddingLeftRight<Column>(ObjEditor.NESTING_OFFSET)));
      Column column = component2;
      this.m_rootItems = component2;
      uiComponentArray[1] = (UiComponent) column;
      this.Add(uiComponentArray);
      Toggle component3 = new Toggle(true).FlexGrow<Toggle>(1f).ReversedDirection<Toggle>().Label<Toggle>("Set value".AsLoc()).OnValueChanged(new Action<bool>(this.onOptionToggle));
      gap = new Px?(ObjEditor.NESTING_OFFSET);
      Px? top = new Px?(2.px());
      Px? right = gap;
      Px? bottom = new Px?();
      Px? left = new Px?();
      this.m_optionToggle = component3.AbsolutePosition<Toggle>(top, right, bottom, left);
    }

    public void SetCollapsed(bool isCollapsed) => this.m_label.SetCollapsed(isCollapsed);

    private void onCollapse(bool isCollapsed)
    {
      this.m_rootItems.SetVisible(!isCollapsed);
      this.m_optionToggle.SetVisible(!isCollapsed);
    }

    private void onCollapseClick(bool isCollapsed)
    {
      this.onCollapse(isCollapsed);
      this.m_editor.SetCollapsed(this.m_objToEdit, this.m_data.LabelForCollapse, isCollapsed);
    }

    public void SetAsRoot() => this.m_isRoot = true;

    public void SetData(ObjEditorData data)
    {
      this.m_data = data;
      this.m_objToEdit = data.Value;
      Type nullableType = data.Type;
      this.m_itemsCurrent = this.m_rootItems;
      this.m_rebuildIfTrue = (Option<PropertyInfo>) Option.None;
      this.m_innerEditors.Clear();
      this.m_members.Clear();
      this.m_rootItems.Clear();
      this.m_rootItems.PaddingTopBottom<Column>(0.px());
      this.SetChildren(this.m_isRoot ? (UiComponent) null : (UiComponent) this.m_labelContainer, (UiComponent) this.m_rootItems);
      this.m_optionToggle.RemoveFromHierarchy();
      this.m_inlineContainer.RemoveFromHierarchy();
      this.m_isSelfDisabled = false;
      this.m_optionType = Option<Type>.None;
      this.Enabled<ObjEditorComposite>(true);
      if (!this.m_isRoot)
        ObjEditor.ApplyClassForNesting((UiComponent) this.m_rootItems, data);
      bool canInlineEditors = true;
      Type underlyingType = Nullable.GetUnderlyingType(nullableType);
      if (underlyingType != (Type) null && underlyingType.IsValueType)
      {
        this.m_labelContainer.Add((UiComponent) this.m_optionToggle);
        this.m_optionToggle.Value(this.m_objToEdit != null);
        canInlineEditors = false;
        nullableType = underlyingType;
        if (this.m_objToEdit == null)
        {
          this.m_isSelfDisabled = true;
          this.m_objToEdit = Activator.CreateInstance(underlyingType);
        }
      }
      if (this.m_objToEdit == null)
      {
        this.setLabelText(data, "null");
      }
      else
      {
        Type key = this.m_objToEdit.GetType();
        if (key.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof (Option<>))
        {
          this.m_optionType = (Option<Type>) key;
          key = nullableType.GetGenericArguments()[0];
          this.m_objToEdit = ((IOptionNonGeneric) this.m_objToEdit).ValueOrNull;
          if (this.m_objToEdit == null)
          {
            this.setLabelText(data, "Option.None");
            return;
          }
        }
        if (data.NestingLevel > 8)
        {
          Log.Error("Nesting seems too deep. Infinite recursion?");
        }
        else
        {
          ImmutableArray<MemberInfo> immutableArray;
          if (!ObjEditorComposite.MEMBERS_CACHE.TryGetValue(key, out immutableArray))
          {
            immutableArray = ((IEnumerable<MemberInfo>) key.GetMembers(BindingFlags.Instance | BindingFlags.Public)).Where<MemberInfo>(new Func<MemberInfo, bool>(this.isEligibleMember)).OrderBy<MemberInfo, int>((Func<MemberInfo, int>) (x =>
            {
              EditorEnforceOrderAttribute customAttribute = x.GetCustomAttribute<EditorEnforceOrderAttribute>();
              return customAttribute == null ? int.MaxValue : customAttribute.Order;
            })).ToImmutableArray<MemberInfo>();
            ObjEditorComposite.MEMBERS_CACHE[key] = immutableArray;
          }
          int length = immutableArray.Length;
          if (length == 0)
          {
            this.setLabelText(data, "Object '" + key.Name + "' has no properties or fields'.");
          }
          else
          {
            ObjEditorData? customInfo = new ObjEditorData?();
            bool flag1 = length == 1 & canInlineEditors;
            int nestingLevel;
            if (flag1)
            {
              customInfo = new ObjEditorData?(data);
              this.m_label.SetCollapseDisabled(true);
              this.m_labelContainer.RemoveFromHierarchy();
              this.m_rootItems.RemoveFromHierarchy();
              nestingLevel = data.NestingLevel;
            }
            else
            {
              this.setLabelText(data);
              nestingLevel = data.NestingLevel + 1;
            }
            ObjEditorForAction previousAction = (ObjEditorForAction) null;
            foreach (MemberInfo memberInfo in immutableArray)
            {
              Option<MemberInfo> dropdownSourceIfCan = this.getDropdownSourceIfCan(memberInfo);
              bool flag2 = memberInfo.HasAttribute<EditorRebuildIfTrueAttribute>();
              bool flag3 = !memberInfo.HasAttribute<EditorReadonlyAttribute>() && !flag2 && this.canEditMember(memberInfo);
              Type memberType = ObjEditorUtils.GetMemberType(memberInfo);
              ObjEditorData data1 = new ObjEditorData(memberInfo, memberType, nestingLevel, this.m_objToEdit, dropdownSourceIfCan, customInfo);
              if (flag2)
              {
                this.setRebuildIfTrueProp(memberInfo, memberType);
              }
              else
              {
                if (this.createSectionLabelIfCan(memberInfo))
                  previousAction = (ObjEditorForAction) null;
                else if (!flag3)
                  this.createReadonlyFieldIfCan(data1);
                if (flag3 && memberType != typeof (Action))
                {
                  this.createAndAddEditorFor(data1, memberInfo, flag1 ? (Column) this : this.m_itemsCurrent);
                  previousAction = (ObjEditorForAction) null;
                }
                previousAction = this.createActionBtnIfCan(memberInfo, memberType, data1, previousAction);
              }
            }
            if (this.m_isSelfDisabled)
              this.setChildrenEnabled(false);
            if (length == 1 & canInlineEditors)
              return;
            bool isDisabled = this.inlineEditorsIfCan(data, length, canInlineEditors);
            this.m_label.SetCollapseDisabled(isDisabled);
            if (!isDisabled)
              this.m_rootItems.PaddingTopBottom<Column>(ObjEditor.NESTING_OFFSET);
            else
              this.m_rootItems.Hide<Column>();
            MemberInfo valueOrNull = data.Member.ValueOrNull;
            this.m_label.SetCollapsed(this.m_editor.IsCollapsed(this.m_objToEdit, this.m_data.LabelForCollapse, (object) valueOrNull != null && valueOrNull.HasAttribute<EditorCollapseObjectAttribute>()));
          }
        }
      }
    }

    private bool isEligibleMember(MemberInfo member)
    {
      if (member.MemberType != MemberTypes.Property && member.MemberType != MemberTypes.Field || member.GetCustomAttribute<EditorIgnoreAttribute>() != null || ObjEditorConfig.ShouldIgnoreMember(member) || member.HasAttribute<EditorClassNameAttribute>())
        return false;
      if (member.HasAttribute<EditorRebuildIfTrueAttribute>())
        return true;
      if (member.HasAttribute<EditorReadonlyAttribute>())
        return ObjEditorComposite.SUPPORTED_READONLY_TYPES.Contains(ObjEditorUtils.GetMemberType(member));
      return this.canEditMember(member) || ObjEditorUtils.GetMemberType(member) == typeof (string);
    }

    private bool canEditMember(MemberInfo member)
    {
      Type memberType = ObjEditorUtils.GetMemberType(member);
      if (memberType == typeof (Action))
        return true;
      PropertyInfo propertyInfo = member as PropertyInfo;
      if ((object) propertyInfo == null)
        return true;
      if (!propertyInfo.CanWrite)
        return false;
      if (!(propertyInfo.GetSetMethod(false) == (MethodInfo) null))
        return true;
      return !memberType.IsValueType && memberType != typeof (string);
    }

    private void setRebuildIfTrueProp(MemberInfo member, Type memberType)
    {
      if (!(memberType != typeof (bool)))
      {
        PropertyInfo propertyInfo = member as PropertyInfo;
        if ((object) propertyInfo != null && propertyInfo.CanWrite)
        {
          if (this.m_rebuildIfTrue.HasValue)
          {
            Log.Error("You have multiple RebuildIfTrue defined, only one per object is supported!");
            return;
          }
          this.m_rebuildIfTrue = (Option<PropertyInfo>) propertyInfo;
          return;
        }
      }
      Log.Error("RebuildIfTrue can only be defined on writeable boolean property.");
    }

    private bool inlineEditorsIfCan(ObjEditorData data, int itemsTotal, bool canInlineEditors)
    {
      if (itemsTotal < 2 || itemsTotal > 3 || !canInlineEditors || this.m_isRoot || !this.m_innerEditors.All<IObjEditor>((Func<IObjEditor, bool>) (x => x is IObjEditorWithInlineSupport withInlineSupport && withInlineSupport.CanBeInlined)))
        return false;
      int self = 0;
      foreach (IObjEditorWithInlineSupport innerEditor in this.m_innerEditors)
      {
        string label = innerEditor.GetLabel();
        self = self.Max(label.Length);
        if (label.Length > 2 && this.getLengthOfLongestWord(label) > 10)
          return false;
      }
      bool useTwoLines = self > 2;
      foreach (IObjEditor innerEditor in this.m_innerEditors)
      {
        innerEditor.Component.RemoveFromHierarchy();
        ((IObjEditorWithInlineSupport) innerEditor).LayoutAsInline(useTwoLines, itemsTotal);
        this.m_inlineContainer.Add(innerEditor.Component);
      }
      this.setLabelText(data);
      this.m_labelContainer.Add((UiComponent) this.m_inlineContainer);
      this.m_inlineContainer.Width<Row>(new Px?(ObjEditor.GetEditorWidth(data)));
      return true;
    }

    private int getLengthOfLongestWord(string str)
    {
      int self = 0;
      int num = 0;
      foreach (char ch in str)
      {
        if (ch == ' ')
        {
          self = self.Max(num);
          num = 0;
        }
        else
          ++num;
      }
      return self.Max(num);
    }

    private bool createSectionLabelIfCan(MemberInfo info)
    {
      EditorSectionAttribute customAttribute = info.GetCustomAttribute<EditorSectionAttribute>();
      if (customAttribute == null)
        return false;
      PropertyInfo propertyInfo = info as PropertyInfo;
      string label = (object) propertyInfo == null || propertyInfo.CanWrite || !(propertyInfo.PropertyType == typeof (string)) ? customAttribute.Label ?? "(empty)" : (string) propertyInfo.GetValue(this.m_objToEdit) ?? "(empty)";
      Column itemsCurrent = this.m_itemsCurrent = new Column(ObjEditor.GAP).AlignItemsStretch<Column>();
      object objToEdit = this.m_objToEdit;
      ObjEditorLabel objEditorLabel = new ObjEditorLabel((Action<bool>) (isCollapsed => itemsCurrent.SetVisible(!isCollapsed)), (Action<bool>) (isCollapsed =>
      {
        this.m_editor.SetCollapsed(objToEdit, label, isCollapsed);
        itemsCurrent.SetVisible(!isCollapsed);
      }));
      objEditorLabel.SetText(label, customAttribute.Tooltip, customAttribute.IsHeader, this.m_data.NestingLevel + 1);
      this.m_rootItems.Add((UiComponent) objEditorLabel, (UiComponent) itemsCurrent);
      objEditorLabel.SetCollapsed(this.m_editor.IsCollapsed(this.m_objToEdit, label, customAttribute.CollapsedByDefault));
      return true;
    }

    private bool createReadonlyFieldIfCan(ObjEditorData data)
    {
      if (!ObjEditorComposite.SUPPORTED_READONLY_TYPES.Contains(data.Type))
        return false;
      string text = data.Value?.ToString() ?? "";
      if (text.IsNullOrEmpty() && (data.Label.IsNullOrEmpty() || data.IsErrorLabel))
        return true;
      ObjEditorDisplay child = new ObjEditorDisplay();
      child.SetData(data.Label, text, data.Tooltip, data.IsErrorLabel, data.NestingLevel);
      this.m_itemsCurrent.Add((UiComponent) child);
      return true;
    }

    private ObjEditorForAction createActionBtnIfCan(
      MemberInfo info,
      Type memberType,
      ObjEditorData data,
      ObjEditorForAction previousAction = null)
    {
      object valueOrNull = data.Value;
      Type type = valueOrNull?.GetType();
      if (type != (Type) null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Option<>))
      {
        type = type.GetGenericArguments()[0];
        valueOrNull = ((IOptionNonGeneric) valueOrNull).ValueOrNull;
      }
      ObjEditorsRegistry.RegisteredAction registeredAction;
      if (valueOrNull != null && this.m_editor.EditorsRegistry.TryGetRegisteredAction(type, out registeredAction) && data.Owner.HasValue)
      {
        ObjEditorForAction editorInstance = this.m_editor.GetEditorInstance<ObjEditorForAction>();
        editorInstance.SetData(registeredAction, valueOrNull, (Action<object>) (v => ObjEditorUtils.SetValue(info, data.Owner.Value, v)));
        if (previousAction != null && previousAction.AppendActionIfCan(editorInstance))
          return previousAction;
        this.m_itemsCurrent.Add((UiComponent) editorInstance);
        return editorInstance;
      }
      if (valueOrNull is Action action)
      {
        ObjEditorForAction editorInstance = this.m_editor.GetEditorInstance<ObjEditorForAction>();
        editorInstance.SetData(info, action, data.NestingLevel);
        if (previousAction != null && previousAction.AppendActionIfCan(editorInstance))
          return previousAction;
        this.m_itemsCurrent.Add((UiComponent) editorInstance);
        return editorInstance;
      }
      return valueOrNull == null && memberType == typeof (Action) ? previousAction : (ObjEditorForAction) null;
    }

    private void createAndAddEditorFor(ObjEditorData data, MemberInfo member, Column container)
    {
      Option<IObjEditor> editorFor = this.m_editor.EditorsRegistry.CreateEditorFor(this.m_editor, data);
      if (editorFor.HasValue)
      {
        this.assignValidationSourceIfCan(editorFor.Value, data);
        this.m_innerEditors.Add(editorFor.Value);
        this.m_members.Add(member);
        container.Add(editorFor.Value.Component);
      }
      else
        Log.Error("Editor not found!");
    }

    private Option<MemberInfo> getDropdownSourceIfCan(MemberInfo member)
    {
      EditorDropdownAttribute customAttribute = member.GetCustomAttribute<EditorDropdownAttribute>();
      if (customAttribute == null)
        return Option<MemberInfo>.None;
      if (ObjEditorUtils.GetMemberType(member) != typeof (string))
      {
        Log.Error(string.Format("Member '{0}' must be string type!", (object) member));
        return Option<MemberInfo>.None;
      }
      MemberInfo[] member1 = this.m_objToEdit.GetType().GetMember(customAttribute.SourceDataMember, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (member1.Length != 1)
      {
        Log.Error(string.Format("Found '{0}' members of ", (object) member1.Length) + this.m_objToEdit.GetType().Name + "." + customAttribute.SourceDataMember + ", expected one.");
        return Option<MemberInfo>.None;
      }
      Type memberType = ObjEditorUtils.GetMemberType(member1[0]);
      if (!(memberType != typeof (Lyst<string>)) || !(memberType != typeof (ImmutableArray<string>)))
        return (Option<MemberInfo>) member1[0];
      Log.Error(string.Format("Source dropdown data member cannot be of type '{0}'! Use Lyst or ImmutableArray", (object) memberType));
      return Option<MemberInfo>.None;
    }

    private void assignValidationSourceIfCan(IObjEditor editor, ObjEditorData data)
    {
      MemberInfo valueOrNull = data.Member.ValueOrNull;
      string memberName = (object) valueOrNull != null ? valueOrNull.GetCustomAttribute<EditorValidationSourceAttribute>()?.MemberName : (string) null;
      if (memberName == null)
        return;
      MemberInfo memberInfo = ((IEnumerable<MemberInfo>) this.m_objToEdit.GetType().GetMember(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).FirstOrDefault<MemberInfo>();
      Func<string> source = (Func<string>) null;
      FieldInfo field = memberInfo as FieldInfo;
      if ((object) field != null && field.FieldType == typeof (string))
      {
        source = (Func<string>) (() => (string) field.GetValue(this.m_objToEdit));
      }
      else
      {
        PropertyInfo prop = memberInfo as PropertyInfo;
        if ((object) prop != null && prop.PropertyType == typeof (string))
          source = (Func<string>) (() => (string) prop.GetValue(this.m_objToEdit));
      }
      if (source == null)
        Log.Error("Failed to find validation source '" + memberName + "' for '" + data.Label + "'. Is it a string prop / field?");
      else if (editor is IEditorWithValidationSource validationSource)
        validationSource.SetValidationSource(source);
      else
        Log.Error("Validator '" + memberName + "' is not usable for a editor '" + editor.GetType().Name + "' of " + data.Label + ". Only editors implementing IEditorWithValidationSource are supported.");
    }

    private void setLabelText(ObjEditorData data, string editorText = null)
    {
      this.m_label.SetText(data.Label, data.Tooltip, data.IsLabelHeader, data.NestingLevel);
      if (editorText == null)
        return;
      Column itemsCurrent = this.m_itemsCurrent;
      Label label = new Label(editorText.AsLoc());
      label.Add<Label>((Action<Label>) (c => c.AlignText<Label>(TextAlign.CenterMiddle).PaddingTopBottom<Label>(2.pt())));
      itemsCurrent.Add((UiComponent) label);
    }

    public bool TryGetValue(out object value)
    {
      if (this.m_optionToggle.HasParent && !this.m_optionToggle.GetValue())
      {
        value = (object) null;
        return true;
      }
      for (int index = 0; index < this.m_innerEditors.Count; ++index)
      {
        IObjEditor innerEditor = this.m_innerEditors[index];
        MemberInfo member = this.m_members[index];
        object obj;
        if (innerEditor.TryGetValue(out obj))
        {
          try
          {
            this.setValueToMember(member, obj);
            if (innerEditor is IObjEditorWithValueUpdate editorWithValueUpdate)
              editorWithValueUpdate.UpdateValue(ObjEditorUtils.GetValue(member, this.m_objToEdit));
          }
          catch (Exception ex)
          {
            Log.Exception(ex, string.Format("Failed to set value to member '{0}' of '{1}'.", (object) member.Name, (object) this.m_objToEdit.GetType()));
            string str1 = ((IEnumerable<string>) this.m_innerEditors.Select<string>((Func<IObjEditor, string>) (x => x.GetType().Name))).JoinStrings("\n");
            string str2 = ((IEnumerable<string>) this.m_members.Select<string>((Func<MemberInfo, string>) (x => x.Name))).JoinStrings("\n");
            Log.Error(string.Format("Editors ({0}):\n{1}\n\n", (object) this.m_innerEditors.Count, (object) str1) + string.Format("Members ({0}):\n{1}", (object) this.m_members.Count, (object) str2));
          }
        }
      }
      value = this.m_optionType.HasValue ? ObjEditor.ObjToOption(this.m_optionType.Value, this.m_objToEdit) : this.m_objToEdit;
      return true;
    }

    private void setValueToMember(MemberInfo member, object value)
    {
      PropertyInfo propertyInfo = member as PropertyInfo;
      if ((object) propertyInfo != null)
      {
        propertyInfo.SetValue(this.m_objToEdit, value);
      }
      else
      {
        FieldInfo fieldInfo = member as FieldInfo;
        if ((object) fieldInfo != null)
          fieldInfo.SetValue(this.m_objToEdit, value);
        else
          Log.Error(string.Format("Failed to set value to member {0}", (object) member.GetType()));
      }
    }

    private void onOptionToggle(bool isValueSet)
    {
      this.m_isSelfDisabled = !isValueSet;
      this.setChildrenEnabled(isValueSet);
      if (isValueSet)
        return;
      this.m_editor.ReportValueChanged();
    }

    protected override void SetEnabledInternal(bool enabled)
    {
      base.SetEnabledInternal(enabled);
      if (this.m_isSelfDisabled)
        return;
      this.setChildrenEnabled(enabled);
    }

    private void setChildrenEnabled(bool enabled)
    {
      foreach (IObjEditor innerEditor in this.m_innerEditors)
        innerEditor.Component.Enabled<UiComponent>(enabled);
    }

    public void RenderUpdate()
    {
      if (!this.m_rebuildIfTrue.HasValue)
        return;
      bool? nullable = this.m_rebuildIfTrue.Value.GetValue(this.m_objToEdit) as bool?;
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.m_rebuildIfTrue.Value.SetValue(this.m_objToEdit, (object) false);
      this.m_editor.ReportValueChanged(rebuildUi: true);
    }

    static ObjEditorComposite()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjEditorComposite.MEMBERS_CACHE = new Dict<Type, ImmutableArray<MemberInfo>>();
      ObjEditorComposite.SUPPORTED_READONLY_TYPES = new Set<Type>()
      {
        typeof (byte),
        typeof (int),
        typeof (uint),
        typeof (long),
        typeof (ulong),
        typeof (short),
        typeof (ushort),
        typeof (Fix32),
        typeof (Fix64),
        typeof (string)
      };
    }
  }
}
