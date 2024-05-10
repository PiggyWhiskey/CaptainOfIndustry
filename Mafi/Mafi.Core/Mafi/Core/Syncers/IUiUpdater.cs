// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.IUiUpdater
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Syncers
{
  public interface IUiUpdater
  {
    void SyncUpdate();

    void RenderUpdate();

    void SetOneTimeAfterSyncCallback(Action action);

    /// <summary>
    /// Adds a child updater. The given updater will have its render, sync methods called by this updater. Keep in
    /// mind that if this updater is using lower <see cref="T:Mafi.Core.Syncers.SyncFrequency" /> it will affect the child updater. So
    /// setting the child updater with anything below critical might be too much (with a delay x in both it would
    /// lead to x*x delay for the child).
    /// </summary>
    void AddChildUpdater(IUiUpdater updater);

    void RemoveChildUpdater(IUiUpdater updater);

    void ClearAllChildUpdaters();

    void Invalidate();
  }
}
