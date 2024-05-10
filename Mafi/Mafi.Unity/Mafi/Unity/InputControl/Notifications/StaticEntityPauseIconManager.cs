// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Notifications.StaticEntityPauseIconManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Gfx;
using Mafi.Unity.Entities;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Notifications
{
  /// <summary>
  /// Handles displaying pause icon whenever static entity is paused.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class StaticEntityPauseIconManager
  {
    private static readonly IconSpec ICON_SPEC;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly EntitiesIconRenderer m_iconRenderer;

    public StaticEntityPauseIconManager(
      IGameLoopEvents gameLoopEvents,
      IEntitiesManager entitiesManager,
      EntitiesIconRenderer iconRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_iconRenderer = iconRenderer;
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      entitiesManager.EntityPauseStateChanged.AddNonSaveable<StaticEntityPauseIconManager>(this, new Action<IEntity, bool>(this.pauseStateChanged));
    }

    private void initState()
    {
      foreach (IEntity entity in this.m_entitiesManager.Entities)
      {
        if (entity.IsPaused)
          this.pauseStateChanged(entity, true);
      }
    }

    private void pauseStateChanged(IEntity entity, bool isPaused)
    {
      if (!(entity is IRenderedEntity entity1))
        return;
      if (isPaused)
        this.m_iconRenderer.AddIcon(StaticEntityPauseIconManager.ICON_SPEC, entity1);
      else
        this.m_iconRenderer.RemoveIcon(StaticEntityPauseIconManager.ICON_SPEC, entity1);
    }

    static StaticEntityPauseIconManager()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StaticEntityPauseIconManager.ICON_SPEC = new IconSpec("Assets/Unity/UserInterface/EntityIcons/Pause.png", ColorRgba.Orange);
    }
  }
}
