// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.UndoRedoManager`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  internal class UndoRedoManager<T>
  {
    public readonly int MaxHistorySize;
    /// <summary>First item is the most recent state.</summary>
    private readonly Queueue<T> m_undoStates;

    public int CurrentStateIndex { get; private set; }

    /// <summary>
    /// All undo states from the most recent one to the oldest one.
    /// </summary>
    public IIndexable<T> UndoStates => (IIndexable<T>) this.m_undoStates;

    public T CurrentState => this.m_undoStates[this.CurrentStateIndex];

    public event Action OnHistoryChanged;

    public int PossibleUndoCount => this.m_undoStates.Count - this.CurrentStateIndex - 1;

    public int PossibleRedoCount => this.CurrentStateIndex;

    public UndoRedoManager(int maxHistorySize, T initialState)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_undoStates = new Queueue<T>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxHistorySize = maxHistorySize;
      this.m_undoStates.Enqueue(initialState);
    }

    public void Undo(int count)
    {
      this.CurrentStateIndex += count.Min(this.PossibleUndoCount);
      Assert.That<int>(this.CurrentStateIndex).IsValidIndexFor<T>((IIndexable<T>) this.m_undoStates);
      Action onHistoryChanged = this.OnHistoryChanged;
      if (onHistoryChanged == null)
        return;
      onHistoryChanged();
    }

    public void Redo(int count)
    {
      count = count.Min(this.PossibleRedoCount);
      this.CurrentStateIndex -= count;
      Assert.That<int>(this.CurrentStateIndex).IsValidIndexFor<T>((IIndexable<T>) this.m_undoStates);
      Action onHistoryChanged = this.OnHistoryChanged;
      if (onHistoryChanged == null)
        return;
      onHistoryChanged();
    }

    public void PushUndoState(T state)
    {
      if (this.CurrentStateIndex > 0)
      {
        for (int index = 0; index < this.CurrentStateIndex; ++index)
          this.m_undoStates.Dequeue();
        this.CurrentStateIndex = 0;
      }
      this.m_undoStates.EnqueueFirst(state);
      if (this.m_undoStates.Count > this.MaxHistorySize)
        this.m_undoStates.PopLast();
      Action onHistoryChanged = this.OnHistoryChanged;
      if (onHistoryChanged == null)
        return;
      onHistoryChanged();
    }
  }
}
