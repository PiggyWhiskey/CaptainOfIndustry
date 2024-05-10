// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.StaticEntityRemovalHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Input;
using Mafi.Localization;
using Mafi.Unity.Entities;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal sealed class StaticEntityRemovalHandler : IStaticEntityRemovalHandler
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly IConstructionManager m_constructionManager;
    private readonly EntityHighlighter m_entityHighlighter;
    private Option<IStaticEntity> m_entityToValidate;
    private EntityValidationResult? m_lastValidationResult;
    private AssetValue m_assetValue;

    public int Priority => 100;

    public bool CanBeInterrupted => true;

    public StaticEntityRemovalHandler(
      EntitiesManager entitiesManager,
      IConstructionManager constructionManager,
      NewInstanceOf<EntityHighlighter> entityHighlighter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_constructionManager = constructionManager;
      this.m_entityHighlighter = entityHighlighter.Instance;
    }

    public bool TryHandleHover(
      Option<IStaticEntity> entityUnderCursor,
      Tile3f pickCoord,
      out AssetValue deconstructionValue,
      out LocStrFormatted errorMsg)
    {
      if (entityUnderCursor.IsNone)
      {
        deconstructionValue = AssetValue.Empty;
        errorMsg = LocStrFormatted.Empty;
        this.m_entityHighlighter.ClearAllHighlights();
        return false;
      }
      IStaticEntity staticEntity = entityUnderCursor.Value;
      if (staticEntity != this.m_entityToValidate)
      {
        this.m_entityToValidate = staticEntity.SomeOption<IStaticEntity>();
        this.m_lastValidationResult = new EntityValidationResult?();
        this.m_assetValue = this.m_constructionManager.GetDeconstructionValueFor(staticEntity);
        this.m_entityHighlighter.HighlightOnly((IRenderedEntity) staticEntity, DeleteEntityInputController.COLOR_HIGHLIGHT);
        errorMsg = LocStrFormatted.Empty;
      }
      else
        errorMsg = !this.m_lastValidationResult.HasValue ? LocStrFormatted.Empty : new LocStrFormatted(this.m_lastValidationResult.Value.ErrorMessageForPlayer);
      deconstructionValue = this.m_assetValue;
      return true;
    }

    public bool TryHandleMouseDown(Option<IStaticEntity> entityUnderCursor)
    {
      return entityUnderCursor == this.m_entityToValidate;
    }

    public bool TryHandleMouseUp(
      Option<IStaticEntity> entityUnderCursor,
      Action<IInputCommand> scheduleCommand,
      bool useQuickRemove)
    {
      if (this.m_entityToValidate.IsNone || entityUnderCursor != this.m_entityToValidate)
        return false;
      scheduleCommand((IInputCommand) new StartDeconstructionOfStaticEntityCmd(this.m_entityToValidate.Value, EntityRemoveReason.Remove));
      if (useQuickRemove)
        scheduleCommand((IInputCommand) new FinishBuildOfStaticEntityCmd(this.m_entityToValidate.Value.Id, true));
      return false;
    }

    public bool TryHandleCancel() => false;

    public void Sync()
    {
      if (this.m_lastValidationResult.HasValue || this.m_entityToValidate.IsNone)
        return;
      this.m_lastValidationResult = new EntityValidationResult?(this.m_entitiesManager.CanRemoveEntity((IEntity) this.m_entityToValidate.Value, EntityRemoveReason.Remove));
    }

    public void Deactivate()
    {
      this.m_entityToValidate = Option<IStaticEntity>.None;
      this.m_lastValidationResult = new EntityValidationResult?();
      this.m_assetValue = AssetValue.Empty;
      this.m_entityHighlighter.ClearAllHighlights();
    }
  }
}
