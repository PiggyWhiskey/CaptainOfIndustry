// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.BuildingsAssigner
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Unity.Audio;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  /// <summary>
  /// Graphical tool to visualize and update assignments of static entities between each other.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class BuildingsAssigner
  {
    private static readonly Fix32 LINES_OFFSET_TILES;
    private static readonly ColorRgba COLOR_ENTITY_HOVERED;
    private static readonly ColorRgba COLOR_INVALID;
    private static readonly Color LINE_COLOR_OUTPUT_ACTIVE;
    private static readonly Color LINE_COLOR_OUTPUT;
    private static readonly Color LINE_COLOR_INPUT_ACTIVE;
    private static readonly Color LINE_COLOR_INPUT;
    private static readonly Color LINE_COLOR_INVALID;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly CursorPickingManager m_picker;
    private readonly EntityHighlighter m_entityHighlighter;
    private readonly TerrainCursor m_terrainCursor;
    private readonly CameraController m_cameraController;
    private readonly LinesFactory m_linesFactory;
    private readonly LineMb m_linePreview;
    private Option<LineMb> m_hiddenLine;
    private readonly Material m_movingArrowsLineMaterialShared;
    private readonly Cursoor m_unassignCursor;
    private readonly Cursoor m_assignCursor;
    private readonly Cursoor m_assignTool;
    private readonly AudioSource m_assignClickAudio;
    private readonly AudioSource m_unassignClickAudio;
    private readonly AudioSource m_invalidClickAudio;
    private readonly Lyst<IRenderedEntity> m_highlightedEntities;
    private readonly Dict<IRenderedEntity, LineMb> m_entitiesInputsLines;
    private readonly Dict<IRenderedEntity, LineMb> m_entitiesOutputsLines;
    private Option<IEntityAssignedAsInput> m_inputEntity;
    private Option<IEntityAssignedAsOutput> m_outputEntity;
    private Option<ILayoutEntity> m_hoveredEntity;
    private Action m_onDeactivate;
    private Option<IInputCommand> m_processedCmd;
    private readonly IReadOnlySet<IEntityAssignedAsInput> m_dummyEmptyInputSet;
    private readonly IReadOnlySet<IEntityAssignedAsOutput> m_dummyEmptyOutputSet;
    /// <summary>
    /// Whether this tool is currently assigning inputs to an output entity.
    /// </summary>
    private bool m_isForInputs;

    public IUiUpdater Updater { get; }

    public bool IsToolActive { get; private set; }

    public BuildingsAssigner(
      ShortcutsManager shortcutsManager,
      CursorPickingManager picker,
      NewInstanceOf<EntityHighlighter> entityHighlighter,
      TerrainCursor terrainCursor,
      CursorManager cursorManager,
      CameraController cameraController,
      LinesFactory linesFactory,
      AudioDb audioDb,
      UiStyle style,
      UiAudio audio,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlightedEntities = new Lyst<IRenderedEntity>();
      this.m_entitiesInputsLines = new Dict<IRenderedEntity, LineMb>();
      this.m_entitiesOutputsLines = new Dict<IRenderedEntity, LineMb>();
      this.m_dummyEmptyInputSet = (IReadOnlySet<IEntityAssignedAsInput>) new Set<IEntityAssignedAsInput>();
      this.m_dummyEmptyOutputSet = (IReadOnlySet<IEntityAssignedAsOutput>) new Set<IEntityAssignedAsOutput>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_picker = picker;
      this.m_entityHighlighter = entityHighlighter.Instance;
      this.m_terrainCursor = terrainCursor;
      this.m_cameraController = cameraController;
      this.m_linesFactory = linesFactory;
      this.m_unassignCursor = cursorManager.RegisterCursor(style.Cursors.Unassign);
      this.m_assignCursor = cursorManager.RegisterCursor(style.Cursors.Assign);
      this.m_assignTool = cursorManager.RegisterCursor(style.Cursors.AssignGeneric);
      this.m_assignClickAudio = audioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/AssignStructure.prefab");
      this.m_unassignClickAudio = audioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/UnassignStructure.prefab");
      this.m_invalidClickAudio = audioDb.GetSharedAudio(audio.InvalidOp);
      this.m_movingArrowsLineMaterialShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/MovingArrowsLine.mat");
      this.m_linePreview = linesFactory.CreateLine(Vector3.zero, Vector3.zero, 1.5f, Color.white, this.m_movingArrowsLineMaterialShared);
      this.m_linePreview.SetTextureMode(LineTextureMode.Tile);
      this.m_linePreview.Hide();
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<IEntityAssignedAsInput>((Func<IEnumerable<IEntityAssignedAsInput>>) (() => (IEnumerable<IEntityAssignedAsInput>) this.m_outputEntity.ValueOrNull?.AssignedInputs ?? (IEnumerable<IEntityAssignedAsInput>) this.m_dummyEmptyInputSet), (ICollectionComparator<IEntityAssignedAsInput, IEnumerable<IEntityAssignedAsInput>>) CompareFixedOrder<IEntityAssignedAsInput>.Instance).Observe<IEntityAssignedAsOutput>((Func<IReadOnlyCollection<IEntityAssignedAsOutput>>) (() => (IReadOnlyCollection<IEntityAssignedAsOutput>) this.m_inputEntity.ValueOrNull?.AssignedOutputs ?? (IReadOnlyCollection<IEntityAssignedAsOutput>) this.m_dummyEmptyOutputSet), (ICollectionComparator<IEntityAssignedAsOutput, IReadOnlyCollection<IEntityAssignedAsOutput>>) CompareByCount<IEntityAssignedAsOutput>.Instance).Observe<ILayoutEntity>((Func<ILayoutEntity>) (() => (ILayoutEntity) this.m_inputEntity.ValueOrNull ?? (ILayoutEntity) this.m_outputEntity.ValueOrNull)).Do((Action<Lyst<IEntityAssignedAsInput>, Lyst<IEntityAssignedAsOutput>, ILayoutEntity>) ((inputs, outputs, entity) => this.updateHighlightOfAssignedEntities(entity, inputs, outputs)));
      this.Updater = updaterBuilder.Build();
    }

    public void SetEntity(IEntity entity)
    {
      this.m_inputEntity = (entity as IEntityAssignedAsInput).CreateOption<IEntityAssignedAsInput>();
      this.m_outputEntity = (entity as IEntityAssignedAsOutput).CreateOption<IEntityAssignedAsOutput>();
    }

    public void ActivateTool(Action onDeactivate, bool isForInputs)
    {
      ILayoutEntity entity = (ILayoutEntity) this.m_inputEntity.ValueOrNull ?? (ILayoutEntity) this.m_outputEntity.ValueOrNull;
      if (entity == null)
      {
        Log.Error("Invalid state");
      }
      else
      {
        this.m_isForInputs = isForInputs;
        this.IsToolActive = true;
        this.m_terrainCursor.Activate();
        this.m_onDeactivate = onDeactivate.CheckNotNull<Action>();
        Vector3 position = this.m_isForInputs ? BuildingsAssigner.getCenterForInput(entity) : BuildingsAssigner.getCenterForOutput(entity);
        this.m_linePreview.SetStartPoint(position);
        this.m_linePreview.SetEndPoint(position);
        this.m_linePreview.Show();
      }
    }

    public void DeactivateTool()
    {
      this.clearHighlightOfAssignedEntities();
      this.m_entityHighlighter.ClearAllHighlights();
      this.m_linePreview.Hide();
      this.m_unassignCursor.Hide();
      this.m_assignCursor.Hide();
      this.m_assignTool.Hide();
      this.m_hiddenLine = Option<LineMb>.None;
      this.m_processedCmd = Option<IInputCommand>.None;
      this.m_hoveredEntity = Option<ILayoutEntity>.None;
      if (this.IsToolActive)
        this.m_terrainCursor.Deactivate();
      this.IsToolActive = false;
    }

    public void RenderUpdate()
    {
      if (!this.IsToolActive)
        return;
      if (this.m_processedCmd.HasValue && this.m_processedCmd.Value.IsProcessedAndSynced)
      {
        this.m_processedCmd = Option<IInputCommand>.None;
        this.clearHoveredEntityIfAny();
      }
      if (!this.m_isForInputs && this.m_inputEntity.HasValue)
      {
        IEntityAssignedAsOutput valueOrNull = this.m_picker.PickEntity<IEntityAssignedAsOutput>().ValueOrNull;
        if (valueOrNull == this.m_inputEntity.Value)
          this.clearHoveredEntityIfAny();
        else if (valueOrNull != null)
        {
          if ((ILayoutEntity) valueOrNull == this.m_hoveredEntity)
            return;
          this.clearHoveredEntityIfAny();
          this.setHoveredEntityOutputsMode(valueOrNull);
        }
        else
          updateNothingHovered();
      }
      else
      {
        if (!this.m_isForInputs || !this.m_outputEntity.HasValue)
          return;
        IEntityAssignedAsInput valueOrNull = this.m_picker.PickEntity<IEntityAssignedAsInput>().ValueOrNull;
        if (valueOrNull == this.m_outputEntity.Value)
          this.clearHoveredEntityIfAny();
        else if (valueOrNull != null)
        {
          if ((ILayoutEntity) valueOrNull == this.m_hoveredEntity)
            return;
          this.clearHoveredEntityIfAny();
          this.setHoveredEntityInputsMode(valueOrNull);
        }
        else
          updateNothingHovered();
      }

      void updateNothingHovered()
      {
        this.m_assignTool.Show();
        this.clearHoveredEntityIfAny();
        this.m_linePreview.SetColor(this.m_isForInputs ? BuildingsAssigner.LINE_COLOR_INPUT_ACTIVE : BuildingsAssigner.LINE_COLOR_OUTPUT_ACTIVE);
        if (!this.m_terrainCursor.HasValue || this.m_cameraController.IsInFreeLookMode)
          return;
        Vector3 vector3 = this.m_terrainCursor.Tile3f.AddZ(BuildingsAssigner.LINES_OFFSET_TILES + (Fix32) 2).ToVector3();
        if (this.m_isForInputs)
          this.m_linePreview.SetEndPoint(vector3);
        else
          this.m_linePreview.SetStartPoint(vector3);
      }
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.IsToolActive)
        return false;
      if (this.m_shortcutsManager.IsSecondaryActionUp)
      {
        this.m_onDeactivate();
        return true;
      }
      if (this.m_processedCmd.HasValue || !this.m_shortcutsManager.IsPrimaryActionDown)
        return false;
      IRenderedEntity valueOrNull = this.m_picker.PickEntity<IRenderedEntity>().ValueOrNull;
      if (valueOrNull == null)
        return true;
      if (this.m_isForInputs && this.m_outputEntity.HasValue)
      {
        IEntityAssignedAsOutput assignedAsOutput = this.m_outputEntity.Value;
        if (valueOrNull is IEntityAssignedAsInput entityAssignedAsInput)
        {
          if (assignedAsOutput.AssignedInputs.Contains(entityAssignedAsInput))
          {
            this.m_processedCmd = (Option<IInputCommand>) (IInputCommand) new UnassignStaticEntityCmd(assignedAsOutput, entityAssignedAsInput);
            this.m_unassignClickAudio.Play();
          }
          else if (assignedAsOutput.CanBeAssignedWithInput(entityAssignedAsInput) && entityAssignedAsInput.CanBeAssignedWithOutput(assignedAsOutput))
          {
            this.m_processedCmd = (Option<IInputCommand>) (IInputCommand) new AssignStaticEntityCmd(assignedAsOutput, entityAssignedAsInput);
            this.m_assignClickAudio.Play();
          }
        }
        else
        {
          this.m_invalidClickAudio.Play();
          return true;
        }
      }
      else if (!this.m_isForInputs && this.m_inputEntity.HasValue)
      {
        IEntityAssignedAsInput entityAssignedAsInput = this.m_inputEntity.Value;
        if (valueOrNull is IEntityAssignedAsOutput assignedAsOutput)
        {
          if (entityAssignedAsInput.AssignedOutputs.Contains(assignedAsOutput))
          {
            this.m_processedCmd = (Option<IInputCommand>) (IInputCommand) new UnassignStaticEntityCmd(assignedAsOutput, entityAssignedAsInput);
            this.m_unassignClickAudio.Play();
          }
          else if (assignedAsOutput.CanBeAssignedWithInput(entityAssignedAsInput) && entityAssignedAsInput.CanBeAssignedWithOutput(assignedAsOutput))
          {
            this.m_processedCmd = (Option<IInputCommand>) (IInputCommand) new AssignStaticEntityCmd(assignedAsOutput, entityAssignedAsInput);
            this.m_assignClickAudio.Play();
          }
        }
        else
        {
          this.m_invalidClickAudio.Play();
          return true;
        }
      }
      if (this.m_processedCmd.HasValue)
        inputScheduler.ScheduleInputCmd<IInputCommand>(this.m_processedCmd.Value);
      else
        this.m_invalidClickAudio.Play();
      return true;
    }

    private void clearHoveredEntityIfAny()
    {
      if (this.m_hoveredEntity.IsNone)
        return;
      ILayoutEntity entity = this.m_hoveredEntity.Value;
      this.m_entityHighlighter.RemoveHighlight((IRenderedEntity) entity);
      if (this.m_isForInputs && this.m_outputEntity.HasValue && this.m_outputEntity.Value.AssignedInputs.Contains(entity as IEntityAssignedAsInput))
      {
        this.m_entityHighlighter.Highlight((IRenderedEntity) entity, BuildingsAssigner.COLOR_ENTITY_HOVERED);
        if (this.m_hiddenLine.HasValue && this.m_entitiesInputsLines.ContainsValue(this.m_hiddenLine.Value))
          this.m_hiddenLine.Value.gameObject.SetActive(true);
        this.m_hiddenLine = (Option<LineMb>) Option.None;
      }
      if (!this.m_isForInputs && this.m_inputEntity.HasValue && this.m_inputEntity.Value.AssignedOutputs.Contains(entity as IEntityAssignedAsOutput))
      {
        this.m_entityHighlighter.Highlight((IRenderedEntity) entity, BuildingsAssigner.COLOR_ENTITY_HOVERED);
        if (this.m_hiddenLine.HasValue && this.m_entitiesOutputsLines.ContainsValue(this.m_hiddenLine.Value))
          this.m_hiddenLine.Value.gameObject.SetActive(true);
        this.m_hiddenLine = (Option<LineMb>) Option.None;
      }
      this.m_hoveredEntity = (Option<ILayoutEntity>) Option.None;
    }

    private void setHoveredEntityInputsMode(IEntityAssignedAsInput hoveredEntity)
    {
      if (this.m_outputEntity.IsNone)
      {
        Log.Error("Unexpected state");
      }
      else
      {
        IEntityAssignedAsOutput assignedAsOutput = this.m_outputEntity.Value;
        this.clearHoveredEntityIfAny();
        this.m_hoveredEntity = Option.Some<ILayoutEntity>((ILayoutEntity) hoveredEntity);
        Vector3 centerForInput = BuildingsAssigner.getCenterForInput((ILayoutEntity) hoveredEntity);
        if (this.m_isForInputs)
          this.m_linePreview.SetEndPoint(centerForInput);
        else
          this.m_linePreview.SetStartPoint(centerForInput);
        if (assignedAsOutput.AssignedInputs.Contains(hoveredEntity))
        {
          this.m_entityHighlighter.Highlight((IRenderedEntity) hoveredEntity, BuildingsAssigner.COLOR_ENTITY_HOVERED);
          this.m_unassignCursor.Show();
          LineMb lineMb;
          if (!this.m_entitiesInputsLines.TryGetValue((IRenderedEntity) hoveredEntity, out lineMb))
            return;
          this.m_hiddenLine = (Option<LineMb>) lineMb;
          lineMb.gameObject.SetActive(false);
        }
        else if (!assignedAsOutput.CanBeAssignedWithInput(hoveredEntity))
        {
          this.m_entityHighlighter.Highlight((IRenderedEntity) hoveredEntity, BuildingsAssigner.COLOR_INVALID);
          this.m_linePreview.SetColor(BuildingsAssigner.LINE_COLOR_INVALID);
          this.m_assignTool.Show();
        }
        else
        {
          this.m_entityHighlighter.Highlight((IRenderedEntity) hoveredEntity, BuildingsAssigner.COLOR_ENTITY_HOVERED);
          this.m_assignCursor.Show();
        }
      }
    }

    private void setHoveredEntityOutputsMode(IEntityAssignedAsOutput hoveredEntity)
    {
      if (this.m_inputEntity.IsNone)
      {
        Log.Error("Unexpected state");
      }
      else
      {
        IEntityAssignedAsInput entityAssignedAsInput = this.m_inputEntity.Value;
        this.clearHoveredEntityIfAny();
        this.m_hoveredEntity = Option.Some<ILayoutEntity>((ILayoutEntity) hoveredEntity);
        Vector3 centerForOutput = BuildingsAssigner.getCenterForOutput((ILayoutEntity) hoveredEntity);
        if (this.m_isForInputs)
          this.m_linePreview.SetEndPoint(centerForOutput);
        else
          this.m_linePreview.SetStartPoint(centerForOutput);
        if (entityAssignedAsInput.AssignedOutputs.Contains(hoveredEntity))
        {
          this.m_entityHighlighter.Highlight((IRenderedEntity) hoveredEntity, BuildingsAssigner.COLOR_ENTITY_HOVERED);
          this.m_unassignCursor.Show();
          LineMb lineMb;
          if (!this.m_entitiesOutputsLines.TryGetValue((IRenderedEntity) hoveredEntity, out lineMb))
            return;
          this.m_hiddenLine = (Option<LineMb>) lineMb;
          lineMb.gameObject.SetActive(false);
        }
        else if (!entityAssignedAsInput.CanBeAssignedWithOutput(hoveredEntity))
        {
          this.m_entityHighlighter.Highlight((IRenderedEntity) hoveredEntity, BuildingsAssigner.COLOR_INVALID);
          this.m_linePreview.SetColor(BuildingsAssigner.LINE_COLOR_INVALID);
          this.m_assignTool.Show();
        }
        else
        {
          this.m_entityHighlighter.Highlight((IRenderedEntity) hoveredEntity, BuildingsAssigner.COLOR_ENTITY_HOVERED);
          this.m_assignCursor.Show();
        }
      }
    }

    private void updateHighlightOfAssignedEntities(
      ILayoutEntity entity,
      Lyst<IEntityAssignedAsInput> assignedInputs,
      Lyst<IEntityAssignedAsOutput> assignedOutputs)
    {
      this.clearHighlightOfAssignedEntities();
      foreach (IEntityAssignedAsInput assignedInput in assignedInputs)
      {
        this.m_highlightedEntities.Add((IRenderedEntity) assignedInput);
        this.m_entityHighlighter.Highlight((IRenderedEntity) assignedInput, BuildingsAssigner.COLOR_ENTITY_HOVERED);
        LineMb line = this.m_linesFactory.CreateLine(BuildingsAssigner.getCenterForInput(entity), BuildingsAssigner.getCenterForInput((ILayoutEntity) assignedInput), 1.5f, BuildingsAssigner.LINE_COLOR_INPUT, this.m_movingArrowsLineMaterialShared);
        line.SetTextureMode(LineTextureMode.Tile);
        this.m_entitiesInputsLines.Add((IRenderedEntity) assignedInput, line);
      }
      foreach (IEntityAssignedAsOutput assignedOutput in assignedOutputs)
      {
        if (!this.m_highlightedEntities.Contains((IRenderedEntity) assignedOutput))
        {
          this.m_highlightedEntities.Add((IRenderedEntity) assignedOutput);
          this.m_entityHighlighter.Highlight((IRenderedEntity) assignedOutput, BuildingsAssigner.COLOR_ENTITY_HOVERED);
        }
        LineMb line = this.m_linesFactory.CreateLine(BuildingsAssigner.getCenterForOutput((ILayoutEntity) assignedOutput), BuildingsAssigner.getCenterForOutput(entity), 1.5f, BuildingsAssigner.LINE_COLOR_OUTPUT, this.m_movingArrowsLineMaterialShared);
        line.SetTextureMode(LineTextureMode.Tile);
        this.m_entitiesOutputsLines.Add((IRenderedEntity) assignedOutput, line);
      }
    }

    private void clearHighlightOfAssignedEntities()
    {
      foreach (IRenderedEntity highlightedEntity in this.m_highlightedEntities)
      {
        this.m_entityHighlighter.RemoveHighlight(highlightedEntity);
        LineMb lineMb1;
        if (this.m_entitiesInputsLines.TryGetValue(highlightedEntity, out lineMb1))
          lineMb1.gameObject.Destroy();
        LineMb lineMb2;
        if (this.m_entitiesOutputsLines.TryGetValue(highlightedEntity, out lineMb2))
          lineMb2.gameObject.Destroy();
      }
      this.m_entitiesInputsLines.Clear();
      this.m_entitiesOutputsLines.Clear();
      this.m_highlightedEntities.Clear();
    }

    private static Vector3 getCenterForInput(ILayoutEntity entity)
    {
      return entity.GetCenter().AddZ(BuildingsAssigner.LINES_OFFSET_TILES + (Fix32) 3).ToVector3();
    }

    private static Vector3 getCenterForOutput(ILayoutEntity entity)
    {
      return entity.GetCenter().AddZ(BuildingsAssigner.LINES_OFFSET_TILES + (Fix32) 1).ToVector3();
    }

    static BuildingsAssigner()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BuildingsAssigner.LINES_OFFSET_TILES = Fix32.One;
      BuildingsAssigner.COLOR_ENTITY_HOVERED = 1612277.ToColorRgba();
      BuildingsAssigner.COLOR_INVALID = 3355443.ToColorRgba();
      BuildingsAssigner.LINE_COLOR_OUTPUT_ACTIVE = 16711680.ToColor();
      BuildingsAssigner.LINE_COLOR_OUTPUT = 12528684.ToColor();
      BuildingsAssigner.LINE_COLOR_INPUT_ACTIVE = 53514.ToColor();
      BuildingsAssigner.LINE_COLOR_INPUT = 2203175.ToColor();
      BuildingsAssigner.LINE_COLOR_INVALID = BuildingsAssigner.COLOR_INVALID.ToColor();
    }
  }
}
