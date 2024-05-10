// Decompiled with JetBrains decompiler
// Type: RTG.RTUndoRedo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTUndoRedo : MonoSingleton<RTUndoRedo>
  {
    [SerializeField]
    private bool _isEnabled;
    [SerializeField]
    private int _actionLimit;
    private List<RTUndoRedo.ActionGroup> _actionGroupStack;
    private int _stackPointer;

    public event UndoStartHandler UndoStart;

    public event UndoEndHandler UndoEnd;

    public event RedoStartHandler RedoStart;

    public event RedoEndHandler RedoEnd;

    public event CanUndoRedoHandler CanUndoRedo;

    public bool IsEnabled => this._isEnabled;

    public int ActionLimit
    {
      get => this._actionLimit;
      set
      {
        this.ClearActions();
        this._actionLimit = Mathf.Max(value, 1);
      }
    }

    public void SetEnabled(bool isEnabled) => this._isEnabled = isEnabled;

    public void ClearActions()
    {
      this.RemoveGroups(0, this._actionGroupStack.Count);
      this._stackPointer = -1;
    }

    public void RecordAction(IUndoRedoAction action)
    {
      if (!this._isEnabled)
        return;
      if (this._actionGroupStack.Count != 0 && this._stackPointer < this._actionGroupStack.Count - 1)
      {
        int startIndex = this._stackPointer + 1;
        int count = this._actionGroupStack.Count - startIndex;
        this.RemoveGroups(startIndex, count);
      }
      this._actionGroupStack.Add(new RTUndoRedo.ActionGroup(action));
      if (this._actionGroupStack.Count > this._actionLimit)
        this.RemoveGroups(0, 1);
      this._stackPointer = this._actionGroupStack.Count - 1;
    }

    public void Update_SystemCall()
    {
      if (!this._isEnabled)
        return;
      if (!Application.isEditor)
      {
        if (RTInput.WasKeyPressedThisFrame(KeyCode.Z) && RTInput.IsKeyPressed(KeyCode.LeftControl))
        {
          this.Undo();
        }
        else
        {
          if (!RTInput.WasKeyPressedThisFrame(KeyCode.Y) || !RTInput.IsKeyPressed(KeyCode.LeftControl))
            return;
          this.Redo();
        }
      }
      else if (RTInput.WasKeyPressedThisFrame(KeyCode.Z) && RTInput.IsKeyPressed(KeyCode.LeftControl) && RTInput.IsKeyPressed(KeyCode.LeftShift))
      {
        this.Undo();
      }
      else
      {
        if (!RTInput.WasKeyPressedThisFrame(KeyCode.Y) || !RTInput.IsKeyPressed(KeyCode.LeftControl) || !RTInput.IsKeyPressed(KeyCode.LeftShift))
          return;
        this.Redo();
      }
    }

    private void Undo()
    {
      if (!this._isEnabled || this._stackPointer < 0)
        return;
      RTUndoRedo.ActionGroup actionGroup = this._actionGroupStack[this._stackPointer];
      YesNoAnswer answer = new YesNoAnswer();
      if (this.CanUndoRedo != null)
        this.CanUndoRedo(UndoRedoOpType.Undo, answer);
      if (answer.HasNo)
        return;
      --this._stackPointer;
      foreach (IUndoRedoAction action in actionGroup.Actions)
      {
        if (this.UndoStart != null)
          this.UndoStart(action);
        action.Undo();
        if (this.UndoEnd != null)
          this.UndoEnd(action);
      }
    }

    private void Redo()
    {
      if (!this._isEnabled || this._actionGroupStack.Count == 0 || this._stackPointer == this._actionGroupStack.Count - 1)
        return;
      RTUndoRedo.ActionGroup actionGroup = this._actionGroupStack[this._stackPointer + 1];
      YesNoAnswer answer = new YesNoAnswer();
      if (this.CanUndoRedo != null)
        this.CanUndoRedo(UndoRedoOpType.Redo, answer);
      if (answer.HasNo)
        return;
      ++this._stackPointer;
      foreach (IUndoRedoAction action in actionGroup.Actions)
      {
        if (this.RedoStart != null)
          this.RedoStart(action);
        action.Redo();
        if (this.RedoEnd != null)
          this.RedoEnd(action);
      }
    }

    private void RemoveGroups(int startIndex, int count)
    {
      List<RTUndoRedo.ActionGroup> range = this._actionGroupStack.GetRange(startIndex, count);
      this._actionGroupStack.RemoveRange(startIndex, count);
      foreach (RTUndoRedo.ActionGroup actionGroup in range)
      {
        foreach (IUndoRedoAction action in actionGroup.Actions)
          action.OnRemovedFromUndoRedoStack();
      }
    }

    private void OnValidate() => this.ActionLimit = this.ActionLimit;

    public RTUndoRedo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isEnabled = true;
      this._actionLimit = 50;
      this._actionGroupStack = new List<RTUndoRedo.ActionGroup>();
      this._stackPointer = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private class ActionGroup
    {
      public List<IUndoRedoAction> Actions;

      public ActionGroup(IUndoRedoAction action)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Actions = new List<IUndoRedoAction>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Actions.Add(action);
      }
    }
  }
}
