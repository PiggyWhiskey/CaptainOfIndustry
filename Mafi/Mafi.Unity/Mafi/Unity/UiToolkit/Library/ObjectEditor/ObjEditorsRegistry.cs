// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorsRegistry
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  public class ObjEditorsRegistry
  {
    private readonly Dict<System.Type, ObjEditorsRegistry.RegisteredCustomEditor> m_registeredCustomEditors;
    private readonly Dict<System.Type, ObjEditorsRegistry.RegisteredAction> m_registeredActions;

    public ObjEditorsRegistry RegisterCustomEditor(
      string buttonName,
      System.Type typeToEdit,
      ICustomObjEditorFactory editorFactory,
      string tooltip = null)
    {
      this.m_registeredCustomEditors[typeToEdit] = new ObjEditorsRegistry.RegisteredCustomEditor(buttonName, tooltip ?? string.Empty, editorFactory);
      return this;
    }

    /// <summary>
    /// Function that will be appended as a button to the given type.
    /// The result you return from the function will be set back to the object.
    /// </summary>
    public void RegisterActionPerType<T>(Func<T, T> processingAction, ObjEditorIcon icon)
    {
      this.m_registeredActions.Add(typeof (T), new ObjEditorsRegistry.RegisteredAction("", icon, (Func<object, object>) (o => (object) processingAction((T) o)), (Option<string>) Option.None));
    }

    /// <summary>
    /// Function that will be appended as a button to the given type.
    /// The result you return from the function will be set back to the object.
    /// </summary>
    public void RegisterActionPerType<T>(
      Func<T, T> processingAction,
      string buttonName,
      string tooltip = null)
    {
      this.m_registeredActions.Add(typeof (T), new ObjEditorsRegistry.RegisteredAction(buttonName, ObjEditorIcon.None, (Func<object, object>) (o => (object) processingAction((T) o)), (Option<string>) tooltip));
    }

    public bool TryGetRegisteredAction(
      System.Type type,
      out ObjEditorsRegistry.RegisteredAction registeredAction)
    {
      return this.m_registeredActions.TryGetValue(type, out registeredAction);
    }

    internal Option<IObjEditor> CreateEditorFor(ObjEditor objEditor, ObjEditorData data)
    {
      ObjEditorsRegistry.RegisteredCustomEditor customEditor;
      if (this.m_registeredCustomEditors.TryGetValue(data.Type, out customEditor))
      {
        ObjEditorForAction editorInstance = objEditor.GetEditorInstance<ObjEditorForAction>();
        editorInstance.SetData(customEditor, data);
        return (Option<IObjEditor>) (IObjEditor) editorInstance;
      }
      Option<IObjEditor> editorIfCan = ObjEditorsRegistry.createEditorIfCan(objEditor, data);
      if (editorIfCan.HasValue)
      {
        editorIfCan.Value.SetData(data);
        return editorIfCan.As<IObjEditor>();
      }
      ObjEditorComposite editorInstance1 = objEditor.GetEditorInstance<ObjEditorComposite>();
      editorInstance1.SetData(data);
      return (Option<IObjEditor>) (IObjEditor) editorInstance1;
    }

    private static Option<IObjEditor> createEditorIfCan(ObjEditor editor, ObjEditorData data)
    {
      object obj1 = data.Value;
      System.Type type = data.Type;
      System.Type innerOptOrNullType = (System.Type) null;
      if (type.IsGenericType)
      {
        System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (Option<>))
        {
          innerOptOrNullType = type.GetGenericArguments()[0];
          if (innerOptOrNullType.IsAssignableTo(typeof (Proto)))
            return createEditorForProto(innerOptOrNullType, type);
        }
        else if (genericTypeDefinition == typeof (Nullable<>))
          innerOptOrNullType = Nullable.GetUnderlyingType(type);
        if (data.Value != null)
        {
          if (genericTypeDefinition == typeof (Lyst<>))
            return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForList>();
          if (genericTypeDefinition == typeof (Dict<,>))
            return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForDict>();
        }
      }
      if (data.DropdownSourceMember.HasValue)
      {
        Lyst<KeyValuePair<string, object>> options = (Lyst<KeyValuePair<string, object>>) null;
        object obj2 = ObjEditorUtils.GetValue(data.DropdownSourceMember.Value, data.Owner.Value);
        if (obj2 is Lyst<string> lyst)
          options = ((IEnumerable<KeyValuePair<string, object>>) lyst.Select<KeyValuePair<string, object>>((Func<string, KeyValuePair<string, object>>) (x => Make.Kvp<string, object>(x, (object) x)))).ToLyst<KeyValuePair<string, object>>();
        else if (obj2 is ImmutableArray<string> immutableArray)
          options = immutableArray.Select<KeyValuePair<string, object>>((Func<string, KeyValuePair<string, object>>) (x => Make.Kvp<string, object>(x, (object) x))).ToLyst<KeyValuePair<string, object>>();
        if (options != null)
        {
          ObjEditorDropdown editorInstance = editor.GetEditorInstance<ObjEditorDropdown>();
          editorInstance.SetOptions(options, (Func<object, object, bool>) ((f, s) => f.Equals(s)));
          return (Option<IObjEditor>) (IObjEditor) editorInstance;
        }
        Log.Error(string.Format("Member {0} has a dropdown target, but a wrong type!", (object) data.Member.ValueOrNull));
      }
      if (obj1 is bool)
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForBoolean>();
      if (isEditorFor<byte>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForByte>();
      if (isEditorFor<ushort>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForUShort>();
      if (isEditorFor<int>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForInt>();
      if (isEditorFor<uint>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForUInt>();
      if (isEditorFor<ulong>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForULong>();
      if (isEditorFor<AngleSlim>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForAngleSlim>();
      if (isEditorFor<Direction90>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForDirection90>();
      if (isEditorFor<float>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForFloat>();
      if (isEditorFor<Fix32>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForFix32>();
      if (isEditorFor<Fix64>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForFix64>();
      if (isEditorFor<Percent>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForPercent>();
      if (isEditorFor<string>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForText>();
      if (isEditorFor<ColorRgba>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForColor>();
      if (type.IsEnum)
      {
        Lyst<KeyValuePair<string, object>> options = new Lyst<KeyValuePair<string, object>>();
        foreach (object obj3 in Enum.GetValues(type))
          options.Add(Make.Kvp<string, object>(ObjEditorUtils.ProcessCamelCase(Enum.GetName(type, obj3)), obj3));
        ObjEditorDropdown editorInstance = editor.GetEditorInstance<ObjEditorDropdown>();
        editorInstance.SetOptions(options, (Func<object, object, bool>) ((f, s) => Convert.ToInt32(f) == Convert.ToInt32(s)));
        return (Option<IObjEditor>) (IObjEditor) editorInstance;
      }
      if (isEditorFor<Texture2D>())
        return (Option<IObjEditor>) (IObjEditor) editor.GetEditorInstance<ObjEditorForImage>();
      return type.IsAssignableTo<Proto>() ? createEditorForProto(type) : (Option<IObjEditor>) Option.None;

      Option<IObjEditor> createEditorForProto(System.Type protoType, System.Type optionType = null)
      {
        if (protoType.IsAssignableTo<TerrainMaterialProto>())
        {
          ObjEditorForProto<TerrainMaterialProto> editorInstance = editor.GetEditorInstance<ObjEditorForProto<TerrainMaterialProto>>();
          editorInstance.SetOptions((IEnumerable<TerrainMaterialProto>) editor.ProtosDb.All<TerrainMaterialProto>().Where<TerrainMaterialProto>((Func<TerrainMaterialProto, bool>) (x => !x.IgnoreInEditor)).OrderBy<TerrainMaterialProto, string>((Func<TerrainMaterialProto, string>) (x => x.Strings.Name.TranslatedString)));
          editorInstance.SetOptionType(optionType);
          return (Option<IObjEditor>) (IObjEditor) editorInstance;
        }
        ObjEditorForProto<Proto> editorInstance1 = editor.GetEditorInstance<ObjEditorForProto<Proto>>();
        editorInstance1.SetOptions((IEnumerable<Proto>) editor.ProtosDb.All(protoType).OrderBy<Proto, string>((Func<Proto, string>) (x => x.Strings.Name.TranslatedString)));
        editorInstance1.SetOptionType(optionType);
        return (Option<IObjEditor>) (IObjEditor) editorInstance1;
      }

      bool isEditorFor<T>() => type.IsAssignableTo<T>() || innerOptOrNullType.IsAssignableTo<T>();
    }

    public ObjEditorsRegistry()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_registeredCustomEditors = new Dict<System.Type, ObjEditorsRegistry.RegisteredCustomEditor>();
      this.m_registeredActions = new Dict<System.Type, ObjEditorsRegistry.RegisteredAction>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    internal readonly struct RegisteredCustomEditor
    {
      public readonly string Name;
      public readonly string Tooltip;
      public readonly ICustomObjEditorFactory EditorFactory;

      public RegisteredCustomEditor(
        string name,
        string tooltip,
        ICustomObjEditorFactory editorFactory)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Name = name;
        this.Tooltip = tooltip;
        this.EditorFactory = editorFactory;
      }
    }

    public readonly struct RegisteredAction
    {
      public readonly string Name;
      public readonly ObjEditorIcon Icon;
      public readonly Option<string> Tooltip;
      public readonly Func<object, object> ProcessingAction;

      public RegisteredAction(
        string name,
        ObjEditorIcon icon,
        Func<object, object> action,
        Option<string> tooltip)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Name = name;
        this.Icon = icon;
        this.Tooltip = tooltip;
        this.ProcessingAction = action;
      }
    }
  }
}
