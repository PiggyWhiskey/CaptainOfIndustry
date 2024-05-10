// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Previews.EntityTesterPreviewController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Previews
{
  public class EntityTesterPreviewController : IUnityInputController, IRegisterInMapEditor
  {
    private static readonly ColorRgba COLOR_FAIL;
    private static readonly ColorRgba COLOR_PASS;
    public readonly ImmutableArray<LayoutEntityProto> TestableProtos;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ProtosDb m_protosDb;
    private readonly LayoutEntityPreviewManager m_previewManager;
    private readonly CameraController m_cameraController;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly TerrainCursor m_terrainCursor;
    private readonly EntityTesterToolbox m_toolbox;
    private LayoutEntityProto m_proto;
    private Option<LayoutEntityPreview> m_preview;
    private readonly AudioSource m_rotateSound;
    private readonly AudioSource m_invalidSound;

    public ControllerConfig Config => ControllerConfig.Tool;

    public bool IsEnabled { get; private set; }

    public Option<LayoutEntityProto> ProtoBeingTested
    {
      get
      {
        return !this.IsEnabled ? (Option<LayoutEntityProto>) Option.None : (Option<LayoutEntityProto>) this.m_proto;
      }
    }

    public EntityTesterPreviewController(
      IUnityInputMgr inputManager,
      NewInstanceOf<TerrainCursor> terrainCursor,
      ProtosDb protosDb,
      LayoutEntityPreviewManager previewManager,
      CameraController cameraController,
      ShortcutsManager shortcutsManager,
      NewInstanceOf<EntityTesterToolbox> toolbox,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputManager = inputManager;
      this.m_protosDb = protosDb;
      this.m_previewManager = previewManager;
      this.m_cameraController = cameraController;
      this.m_shortcutsManager = shortcutsManager;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_toolbox = toolbox.Instance;
      this.m_rotateSound = builder.AudioDb.GetSharedAudio(builder.Audio.Rotate);
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_toolbox.SetOnRotate(new Action(this.rotate));
      this.m_toolbox.SetOnFlip(new Action(this.flip));
      this.TestableProtos = new ImmutableArrayBuilder<LayoutEntityProto>(3)
      {
        [0] = this.m_protosDb.GetOrThrow<LayoutEntityProto>((Proto.ID) Ids.Buildings.FarmT1),
        [1] = this.m_protosDb.GetOrThrow<LayoutEntityProto>((Proto.ID) Ids.Buildings.CargoDepotT4),
        [2] = this.m_protosDb.GetOrThrow<LayoutEntityProto>((Proto.ID) Ids.Buildings.TradeDock)
      }.GetImmutableArrayAndClear();
    }

    public void Activate()
    {
      if (this.IsEnabled)
        return;
      this.IsEnabled = true;
      this.m_terrainCursor.Activate();
      this.m_toolbox.Show();
    }

    public void Deactivate()
    {
      if (!this.IsEnabled)
        return;
      this.IsEnabled = false;
      this.clear();
      this.m_terrainCursor.Deactivate();
      this.m_toolbox.Hide();
    }

    public void DeactivateIfCan()
    {
      if (!this.IsEnabled)
        return;
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public void StartTesting(LayoutEntityProto proto)
    {
      this.clear();
      this.activateSelfIfNeeded();
      this.m_proto = proto;
      if (!this.m_proto.IsNotPhantom)
        return;
      if (this.m_preview.HasValue)
        this.m_preview.Value.DestroyAndReturnToPool();
      this.m_preview = (Option<LayoutEntityPreview>) this.m_previewManager.CreatePreview((ILayoutEntityProto) proto, EntityPlacementPhase.FirstAndFinal, new TileTransform(this.m_terrainCursor.Tile3i), disablePortPreviews: true);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (EventSystem.current.IsPointerOverGameObject() || this.m_preview.IsNone)
        return false;
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Rotate))
      {
        this.rotate();
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Flip))
      {
        this.flip();
        return true;
      }
      TileTransform transform = this.m_preview.Value.Transform;
      this.m_preview.Value.SetTransform(new TileTransform(this.m_terrainCursor.Tile3i, transform.Rotation, transform.IsReflected));
      return false;
    }

    private void clear()
    {
      this.m_preview.ValueOrNull?.DestroyAndReturnToPool();
      this.m_preview = Option<LayoutEntityPreview>.None;
    }

    private void activateSelfIfNeeded()
    {
      if (this.IsEnabled)
        return;
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    private void rotate()
    {
      if (this.m_preview.IsNone)
        return;
      TileTransform transform = this.m_preview.Value.Transform;
      this.m_preview.Value.SetTransform(new TileTransform(transform.Position, transform.Rotation.RotatedMinus90, transform.IsReflected));
      this.m_rotateSound.Play();
    }

    private void flip()
    {
      if (this.m_preview.IsNone)
        return;
      if (this.m_proto.CannotBeReflected)
      {
        this.m_invalidSound.Play();
      }
      else
      {
        TileTransform transform = this.m_preview.Value.Transform;
        AngleDegrees1f normalized = this.m_cameraController.CameraModel.State.YawAngle.Normalized;
        bool flag = (!(normalized > 45.Degrees()) || !(normalized < 135.Degrees()) ? (!(normalized > 225.Degrees()) ? 0 : (normalized < 315.Degrees() ? 1 : 0)) : 1) != (transform.Rotation.Is90Or270Deg ? 1 : 0);
        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
          flag = !flag;
        this.m_preview.Value.SetTransform(new TileTransform(transform.Position, transform.Rotation, !transform.IsReflected));
        if (flag)
          this.m_preview.Value.SetTransform(new TileTransform(transform.Position, transform.Rotation.Rotated180, !transform.IsReflected));
        this.m_rotateSound.Play();
      }
    }

    static EntityTesterPreviewController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      EntityTesterPreviewController.COLOR_FAIL = 16724787.ToColorRgba();
      EntityTesterPreviewController.COLOR_PASS = 3407667.ToColorRgba();
    }
  }
}
