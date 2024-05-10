// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.QuickEntityTransformInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class QuickEntityTransformInputController
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly CursorPickingManager m_picker;
    private readonly InspectorController m_inspectorController;
    private readonly AudioSource m_rotateSound;
    private readonly AudioSource m_invalidSound;
    private Option<ILayoutEntity> m_entityToProcess;
    private Option<InputCommand> m_pendingCmd;
    private bool m_rotate;
    private bool m_flip;

    public QuickEntityTransformInputController(
      UiBuilder builder,
      EntitiesManager entitiesManager,
      IInputScheduler inputScheduler,
      IGameLoopEvents gameLoopEvents,
      ShortcutsManager shortcutsManager,
      CursorPickingManager picker,
      InspectorController inspectorController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_inputScheduler = inputScheduler;
      this.m_shortcutsManager = shortcutsManager;
      this.m_picker = picker;
      this.m_inspectorController = inspectorController;
      this.m_rotateSound = builder.AudioDb.GetSharedAudio(builder.Audio.Rotate);
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      gameLoopEvents.SyncUpdate.AddNonSaveable<QuickEntityTransformInputController>(this, new Action<GameTime>(this.onSync));
    }

    private void onSync(GameTime time)
    {
      if (this.m_pendingCmd.HasValue)
      {
        if (!this.m_pendingCmd.Value.IsProcessed)
          return;
        if (this.m_pendingCmd.Value.HasError)
          this.m_invalidSound.Play();
        else
          this.m_rotateSound.Play();
        this.m_pendingCmd = Option<InputCommand>.None;
      }
      if (!this.m_entityToProcess.HasValue)
        return;
      ILayoutEntity entity = this.m_entityToProcess.Value;
      if (this.m_entitiesManager.CanCutEntity((IStaticEntity) entity).IsSuccess)
        this.m_pendingCmd = (Option<InputCommand>) (InputCommand) this.m_inputScheduler.ScheduleInputCmd<TryTransformEntityCmd>(new TryTransformEntityCmd(entity.Id, this.m_rotate, this.m_flip));
      this.m_entityToProcess = (Option<ILayoutEntity>) Option.None;
    }

    public bool InputUpdate()
    {
      if (EventSystem.current.IsPointerOverGameObject() || this.m_pendingCmd.HasValue || this.m_entityToProcess.HasValue || this.m_inspectorController.ActiveInspector.HasValue)
        return false;
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.Rotate))
      {
        ILayoutEntity entity;
        if (this.m_picker.TryPickEntity<ILayoutEntity>(out entity))
        {
          this.m_entityToProcess = entity.SomeOption<ILayoutEntity>();
          this.m_rotate = true;
          this.m_flip = false;
          return true;
        }
      }
      else
      {
        ILayoutEntity entity;
        if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.Flip) && this.m_picker.TryPickEntity<ILayoutEntity>(out entity))
        {
          if (entity.Prototype.CannotBeReflected)
          {
            this.m_invalidSound.Play();
            return true;
          }
          this.m_entityToProcess = entity.SomeOption<ILayoutEntity>();
          this.m_rotate = false;
          this.m_flip = true;
          return true;
        }
      }
      return false;
    }
  }
}
