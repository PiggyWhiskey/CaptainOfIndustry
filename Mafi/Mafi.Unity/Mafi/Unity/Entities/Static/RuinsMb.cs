// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.RuinsMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.RuinedBuildings;
using Mafi.Core.Entities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class RuinsMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private Ruins m_ruins;
    private ImmutableArray<GameObject> m_parts;
    private Percent m_scrapProgressSync;
    private Percent m_scrapProgressRender;

    public void Initialize(Ruins ruins)
    {
      this.Initialize((ILayoutEntity) ruins);
      this.m_ruins = ruins;
      this.m_parts = ((IEnumerable<Transform>) this.GetComponentsInChildren<Transform>()).Where<Transform>((Func<Transform, bool>) (x => x.gameObject.name.StartsWith("RadioTowerDamage"))).Select<Transform, GameObject>((Func<Transform, GameObject>) (x => x.gameObject)).OrderByDescending<GameObject, string>((Func<GameObject, string>) (x => x.name)).ToImmutableArray<GameObject>();
      Assert.That<ImmutableArray<GameObject>>(this.m_parts).IsNotEmpty<GameObject>("No ruin parts found.");
      this.m_scrapProgressSync = this.m_scrapProgressRender = this.m_ruins.ScrapProgress.Progress;
      this.updatePartsVisibility();
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      this.m_scrapProgressSync = this.m_ruins.ScrapProgress.Progress;
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (this.m_scrapProgressSync == this.m_scrapProgressRender || this.m_parts.Length == 0)
        return;
      this.m_scrapProgressRender = this.m_scrapProgressSync;
      this.updatePartsVisibility();
    }

    private void updatePartsVisibility()
    {
      if (this.m_parts.Length == 0)
        return;
      Percent percent = Percent.Hundred / this.m_parts.Length;
      int num = 0;
      foreach (GameObject part in this.m_parts)
      {
        part.SetActive(num * percent < this.m_scrapProgressRender);
        ++num;
      }
    }

    public RuinsMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
