// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  public class ObjEditor : Column
  {
    public static readonly Px NESTING_OFFSET;
    public static readonly Px GAP;
    public static readonly Px LIST_GAP;
    private static readonly Px BOTTOM_PADDING;
    public readonly ObjEditorsRegistry EditorsRegistry;
    private readonly Dict<int, bool> m_collapsedItems;
    private readonly ObjEditorComposite m_editor;
    private Option<object> m_editedObject;
    private bool m_updateValuesPending;
    private Dict<object, ICustomObjEditor> m_customEditors;
    private Dict<object, ICustomObjEditor> m_customEditorsForDeletion;
    private Option<ObjEditorsDock> m_currentDock;
    private readonly Dict<Type, ObjEditor.CachePerType> m_editorsCache;
    private bool m_renderUpdateInProgress;

    public static void ApplyClassForNesting(UiComponent component, ObjEditorData data)
    {
      ObjEditor.ApplyClassForNesting(component, data.NestingLevel);
    }

    public static void ApplyClassForNesting(UiComponent component, int nesting)
    {
      component.ClassIff<UiComponent>(Cls.panel0, nesting % 2 == 0);
      component.ClassIff<UiComponent>(Cls.panel1, nesting % 2 != 0);
    }

    public static Px GetEditorWidth(ObjEditorData data)
    {
      return ObjEditor.GetEditorWidth(data.NestingLevel);
    }

    public static Px GetEditorWidth(int nestingLevel)
    {
      return Theme.EditorFixedWidth - nestingLevel * ObjEditor.NESTING_OFFSET;
    }

    internal static object ObjToOption(Type optionType, object value)
    {
      return optionType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(new object[1]
      {
        value
      });
    }

    public event Action IncActivate;

    public event Action DecActivate;

    public ProtosDb ProtosDb { get; }

    public LocStrFormatted Title { get; }

    public event Action<bool> OnValueChanged;

    public event Action OnClose;

    public Option<object> EditedObject => this.m_editedObject;

    public ObjEditor(ObjEditorsRegistry editorsRegistry, ProtosDb protosDb, LocStrFormatted title)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_collapsedItems = new Dict<int, bool>();
      this.m_customEditors = new Dict<object, ICustomObjEditor>();
      this.m_customEditorsForDeletion = new Dict<object, ICustomObjEditor>();
      this.m_editorsCache = new Dict<Type, ObjEditor.CachePerType>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EditorsRegistry = editorsRegistry;
      this.ProtosDb = protosDb;
      this.Title = title;
      this.Class<ObjEditor>(Cls.editor).AlignItemsStretch<ObjEditor>().PaddingBottom<ObjEditor>(ObjEditor.BOTTOM_PADDING);
      this.Add((UiComponent) (this.m_editor = new ObjEditorComposite(this)));
      this.m_editor.SetAsRoot();
    }

    public void Refresh() => this.SetObjectToEdit(this.m_editedObject);

    public void SetEditable(bool isEditable)
    {
      this.m_editor.Enabled<ObjEditorComposite>(isEditable);
    }

    private int getHashForCollapsed(object parent, string label)
    {
      int num1 = 17 * 31;
      int? nullable1 = parent?.GetHashCode();
      int? nullable2 = nullable1.HasValue ? new int?(num1 + nullable1.GetValueOrDefault()) : new int?();
      int num2 = nullable2.GetValueOrDefault() * 31;
      int? nullable3;
      if (label == null)
      {
        nullable2 = new int?();
        nullable3 = nullable2;
      }
      else
        nullable3 = new int?(label.GetHashCode());
      nullable1 = nullable3;
      int? nullable4;
      if (!nullable1.HasValue)
      {
        nullable2 = new int?();
        nullable4 = nullable2;
      }
      else
        nullable4 = new int?(num2 + nullable1.GetValueOrDefault());
      nullable2 = nullable4;
      return nullable2.GetValueOrDefault();
    }

    public bool IsCollapsed(object parent, string label, bool defaultValue = false)
    {
      bool flag;
      return this.m_collapsedItems.TryGetValue(this.getHashForCollapsed(parent, label), out flag) ? flag : defaultValue;
    }

    public void SetCollapsed(object parent, string label, bool isCollapsed)
    {
      this.m_collapsedItems[this.getHashForCollapsed(parent, label)] = isCollapsed;
    }

    /// <summary>May be called when already hidden.</summary>
    public void HideEditor()
    {
      this.m_currentDock.ValueOrNull?.TryRemoveTab((UiComponent) this);
      this.markCustomEditorsForDestroy();
      this.destroyPendingCustomEditors();
    }

    public void Close()
    {
      this.HideEditor();
      Action onClose = this.OnClose;
      if (onClose == null)
        return;
      onClose();
    }

    public void DockSelfTo(ObjEditorsDock newDock)
    {
      if (this.m_currentDock != newDock)
      {
        this.m_currentDock.ValueOrNull?.RemoveTab((UiComponent) this);
        this.m_currentDock = (Option<ObjEditorsDock>) newDock;
      }
      if (this.m_editedObject.HasValue)
        newDock.AddTab(this.Title, (UiComponent) this, switchTo: true);
      this.SetObjectToEdit(this.m_editedObject);
    }

    public void SetObjectToEdit(Option<object> editedObject)
    {
      this.m_editedObject = editedObject;
      if (editedObject.IsNone)
      {
        this.HideEditor();
      }
      else
      {
        this.returnAllEditors();
        this.markCustomEditorsForDestroy();
        object valueOrNull = editedObject.ValueOrNull;
        Type type = valueOrNull.GetType();
        this.m_editor.SetData(new ObjEditorData(type, valueOrNull, 0, type.Name));
        this.m_currentDock.ValueOrNull?.AddTab(this.Title, (UiComponent) this);
        this.destroyPendingCustomEditors();
      }
    }

    public void RenderUpdate(GameTime time)
    {
      if (this.m_updateValuesPending)
      {
        this.m_updateValuesPending = false;
        this.SetObjectToEdit(this.m_editedObject);
        foreach (ICustomObjEditor customObjEditor in this.m_customEditors.Values)
          customObjEditor.ValuesUpdated();
      }
      this.m_renderUpdateInProgress = true;
      this.m_editor.RenderUpdate();
      if (!this.m_renderUpdateInProgress)
        return;
      foreach (ObjEditor.CachePerType cachePerType in this.m_editorsCache.Values)
      {
        if (!cachePerType.InUse.IsEmpty && cachePerType.InUse.First<IObjEditor>() is IEditorWithRenderUpdate)
        {
          foreach (IObjEditor objEditor in cachePerType.InUse)
          {
            if (objEditor is IEditorWithRenderUpdate withRenderUpdate)
              withRenderUpdate.RenderUpdate();
            if (!this.m_renderUpdateInProgress)
              return;
          }
        }
      }
      this.m_renderUpdateInProgress = false;
    }

    public bool InputUpdate()
    {
      foreach (ICustomObjEditor customObjEditor in this.m_customEditors.Values)
      {
        if (customObjEditor.InputUpdate())
          return true;
      }
      return false;
    }

    /// <summary>May be called from any thread.</summary>
    public void UpdateValues() => this.m_updateValuesPending = true;

    public void ReportValueChanged(bool doNotSaveUndoState = false, bool rebuildUi = false)
    {
      this.m_renderUpdateInProgress = false;
      if (rebuildUi)
      {
        this.SetObjectToEdit(this.m_editedObject);
        this.invokeOnValueChangedSafe(!doNotSaveUndoState);
      }
      else
      {
        object obj;
        if (!this.m_editor.TryGetValue(out obj))
          return;
        this.m_editedObject = (Option<object>) obj;
        this.invokeOnValueChangedSafe(!doNotSaveUndoState);
      }
    }

    private void invokeOnValueChangedSafe(bool isFinal)
    {
      try
      {
        if (this.m_editedObject.ValueOrNull is IEditableObject valueOrNull && valueOrNull.ObjectWasEdited())
          this.UpdateValues();
        Action<bool> onValueChanged = this.OnValueChanged;
        if (onValueChanged == null)
          return;
        onValueChanged(isFinal);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Exception thrown in value changed callback.");
      }
    }

    internal void ReportActionCalled()
    {
      ObjEditorsDock valueOrNull = this.m_currentDock.ValueOrNull;
      this.ReportValueChanged(rebuildUi: valueOrNull != null && valueOrNull.ContainsTab((UiComponent) this));
      foreach (ICustomObjEditor customObjEditor in this.m_customEditors.Values)
        customObjEditor.ValuesUpdated();
    }

    private void onCustomEditChange(bool isFinal) => this.invokeOnValueChangedSafe(isFinal);

    internal T GetEditorInstance<T>() where T : IObjEditor
    {
      ObjEditor.CachePerType cachePerType;
      if (!this.m_editorsCache.TryGetValue(typeof (T), out cachePerType))
      {
        cachePerType = new ObjEditor.CachePerType();
        this.m_editorsCache.Add(typeof (T), cachePerType);
      }
      T editorInstance = !cachePerType.Unused.IsNotEmpty ? this.createEditorInstance<T>() : (T) cachePerType.Unused.PopLast();
      cachePerType.InUse.Add((IObjEditor) editorInstance);
      return editorInstance;
    }

    private T createEditorInstance<T>()
    {
      Type type = typeof (T);
      if (type.GetConstructor(Type.EmptyTypes) != (ConstructorInfo) null)
        return (T) Activator.CreateInstance(type);
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        typeof (ObjEditor)
      });
      return constructor != (ConstructorInfo) null ? (T) constructor.Invoke(new object[1]
      {
        (object) this
      }) : throw new InvalidOperationException("No suitable constructor found for type " + type.FullName);
    }

    private void returnAllEditors()
    {
      foreach (ObjEditor.CachePerType cachePerType in this.m_editorsCache.Values)
      {
        foreach (IObjEditor objEditor in cachePerType.InUse)
        {
          cachePerType.Unused.Add(objEditor);
          objEditor.Component.RemoveFromHierarchy();
        }
        cachePerType.Unused.Reverse();
        cachePerType.InUse.Clear();
      }
    }

    internal ICustomObjEditor CreateCustomEditor(ICustomObjEditorFactory factory, object obj)
    {
      ICustomObjEditor customEditor1;
      if (this.m_customEditorsForDeletion.TryRemove(obj, out customEditor1))
      {
        this.m_customEditors.Add(obj, customEditor1);
        return customEditor1;
      }
      ICustomObjEditor customEditor2 = factory.Create(obj);
      this.m_customEditors.Add(obj, customEditor2);
      customEditor2.OnChange += new Action<bool>(this.onCustomEditChange);
      Action incActivate = this.IncActivate;
      if (incActivate != null)
        incActivate();
      return customEditor2;
    }

    private void markCustomEditorsForDestroy()
    {
      if (this.m_customEditors.IsEmpty)
        return;
      Assert.That<Dict<object, ICustomObjEditor>>(this.m_customEditorsForDeletion).IsEmpty<object, ICustomObjEditor>();
      Swap.Them<Dict<object, ICustomObjEditor>>(ref this.m_customEditors, ref this.m_customEditorsForDeletion);
    }

    private void destroyPendingCustomEditors()
    {
      foreach (ICustomObjEditor customObjEditor in this.m_customEditorsForDeletion.Values)
      {
        customObjEditor.OnChange -= new Action<bool>(this.onCustomEditChange);
        customObjEditor.Destroy();
        Action decActivate = this.DecActivate;
        if (decActivate != null)
          decActivate();
      }
      this.m_customEditorsForDeletion.Clear();
    }

    static ObjEditor()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjEditor.NESTING_OFFSET = 2.pt();
      ObjEditor.GAP = 2.pt();
      ObjEditor.LIST_GAP = 2.px();
      ObjEditor.BOTTOM_PADDING = 40.px();
    }

    private readonly struct CachePerType
    {
      public readonly Lyst<IObjEditor> InUse;
      public readonly Lyst<IObjEditor> Unused;

      public CachePerType()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.InUse = new Lyst<IObjEditor>();
        this.Unused = new Lyst<IObjEditor>();
        this.InUse = new Lyst<IObjEditor>();
        this.Unused = new Lyst<IObjEditor>();
      }
    }
  }
}
