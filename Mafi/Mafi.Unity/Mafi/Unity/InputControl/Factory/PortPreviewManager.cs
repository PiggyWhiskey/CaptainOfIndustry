// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.PortPreviewManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Simulation;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class PortPreviewManager
  {
    private readonly ObjectPool2<PortPreview> m_pool;
    private readonly Lyst<PortPreview> m_activePreviews;
    private readonly Lyst<PortPreview> m_newPreviews;
    private readonly Lyst<PortPreview> m_removedPreviews;
    internal readonly Material PortPreviewMaterial;

    public PortPreviewManager(
      DependencyResolver resolver,
      ISimLoopEvents simLoopEvents,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_activePreviews = new Lyst<PortPreview>();
      this.m_newPreviews = new Lyst<PortPreview>();
      this.m_removedPreviews = new Lyst<PortPreview>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      PortPreviewManager portPreviewManager = this;
      this.PortPreviewMaterial = assetsDb.GetSharedMaterial("Assets/Core/Materials/BuildingBlueprint.mat");
      this.m_pool = new ObjectPool2<PortPreview>(20, (Func<ObjectPool2<PortPreview>, PortPreview>) (pool => resolver.Instantiate<PortPreview>(new object[1]
      {
        (object) portPreviewManager
      })), (Action<PortPreview>) (x => Assert.That<Option<IoPortShapeProto>>(x.ShapeProto).IsNone<IoPortShapeProto>("Returned item was not cleared.")));
      simLoopEvents.Sync.AddNonSaveable<PortPreviewManager>(this, new Action(this.syncUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<PortPreviewManager>(this, new Action(this.simUpdate));
    }

    private void simUpdate()
    {
      if (this.m_activePreviews.IsEmpty)
        return;
      foreach (PortPreview activePreview in this.m_activePreviews)
        activePreview.SimUpdate();
    }

    private void syncUpdate()
    {
      this.m_activePreviews.AddRange(this.m_newPreviews);
      this.m_newPreviews.Clear();
      Assert.That<int>(this.m_activePreviews.RemoveAll(new Predicate<PortPreview>(this.m_removedPreviews.Contains))).IsEqualTo(this.m_removedPreviews.Count);
      foreach (PortPreview removedPreview in this.m_removedPreviews)
        this.m_pool.ReturnInstance(ref removedPreview);
      this.m_removedPreviews.Clear();
      if (this.m_activePreviews.IsEmpty)
        return;
      foreach (PortPreview activePreview in this.m_activePreviews)
        activePreview.SyncUpdate();
    }

    /// <summary>
    /// Returns uninitialized port preview. Manager takes care of calling previews sim and sync events.
    /// </summary>
    public PortPreview GetPortPreviewPooled()
    {
      PortPreview instance = this.m_pool.GetInstance();
      this.m_newPreviews.Add(instance);
      return instance;
    }

    internal void ReturnToPool(PortPreview preview) => this.m_removedPreviews.Add(preview);
  }
}
