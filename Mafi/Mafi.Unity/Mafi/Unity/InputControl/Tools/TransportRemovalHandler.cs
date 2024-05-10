// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.TransportRemovalHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Factory;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal sealed class TransportRemovalHandler : IStaticEntityRemovalHandler
  {
    private readonly TransportTrajectoryHighlighter m_transportHighlighter;
    private readonly EntityHighlighter m_entityHighlighter;
    private readonly EntitiesManager m_entitiesManager;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IConstructionManager m_constructionManager;
    private readonly AudioSource m_setStartPointSound;
    private readonly AudioSource m_cancelStartPointSound;
    private Option<Mafi.Core.Factory.Transports.Transport> m_selectedTransport;
    private Tile3i? m_transportDeleteStartPosition;
    private Tile3i m_hoverClosestPosition;
    private bool m_isCutValid;
    private Option<Mafi.Core.Factory.Transports.Transport> m_transportToValidate;
    private EntityValidationResult? m_lastValidationResult;
    private TransportRemovalHandler.ValidatedTransportInfo? m_validatedData;
    private AssetValue m_deconstructionValue;
    private LocStrFormatted m_errorMsg;
    private AssetValue m_decValueForTransportCache;
    private Option<Mafi.Core.Factory.Transports.Transport> m_decValueForTransport;

    public int Priority => 50;

    public bool CanBeInterrupted => !this.m_transportDeleteStartPosition.HasValue;

    public TransportRemovalHandler(
      EntitiesManager entitiesManager,
      ShortcutsManager shortcutsManager,
      NewInstanceOf<TransportTrajectoryHighlighter> transportHighlighter,
      NewInstanceOf<EntityHighlighter> entityHighlighter,
      IConstructionManager constructionManager,
      AudioDb audioDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_shortcutsManager = shortcutsManager;
      this.m_constructionManager = constructionManager;
      this.m_transportHighlighter = transportHighlighter.Instance;
      this.m_entityHighlighter = entityHighlighter.Instance;
      this.m_setStartPointSound = audioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/TransportBind.prefab", AudioChannel.UserInterface);
      this.m_cancelStartPointSound = audioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/TransportUnbind.prefab", AudioChannel.UserInterface);
    }

    public bool TryHandleHover(
      Option<IStaticEntity> entityUnderCursor,
      Tile3f pickCoord,
      out AssetValue deconstructionValue,
      out LocStrFormatted errorMsg)
    {
      Mafi.Core.Factory.Transports.Transport transport1;
      if (this.m_selectedTransport.IsNone)
      {
        if (entityUnderCursor.HasValue && entityUnderCursor.Value is Mafi.Core.Factory.Transports.Transport transport2)
        {
          transport1 = transport2;
        }
        else
        {
          deconstructionValue = AssetValue.Empty;
          errorMsg = LocStrFormatted.Empty;
          return false;
        }
      }
      else
        transport1 = this.m_selectedTransport.Value;
      if (transport1 != this.m_transportToValidate)
      {
        this.m_transportToValidate = (Option<Mafi.Core.Factory.Transports.Transport>) transport1;
        this.m_lastValidationResult = new EntityValidationResult?();
      }
      if (this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteEntireTransport) || transport1.Trajectory.Pivots.Length == 1)
      {
        this.m_validatedData = new TransportRemovalHandler.ValidatedTransportInfo?();
        this.m_transportHighlighter.ClearAllHighlights();
        this.m_entityHighlighter.HighlightOnly((IRenderedEntity) transport1, DeleteEntityInputController.COLOR_HIGHLIGHT);
        deconstructionValue = this.getDeconstructionValueFor(transport1);
        errorMsg = LocStrFormatted.Empty;
        return true;
      }
      this.m_entityHighlighter.ClearAllHighlights();
      this.m_hoverClosestPosition = transport1.GetClosestTransportPosition(pickCoord.Tile3i);
      this.m_isCutValid = this.validateTransportCut(transport1, this.m_transportDeleteStartPosition ?? this.m_hoverClosestPosition, this.m_hoverClosestPosition, out errorMsg, out deconstructionValue);
      if (this.m_lastValidationResult.HasValue && this.m_lastValidationResult.Value.IsError)
      {
        this.m_isCutValid = false;
        errorMsg = new LocStrFormatted(this.m_lastValidationResult.Value.ErrorMessageForPlayer);
      }
      return true;
    }

    public bool TryHandleMouseDown(Option<IStaticEntity> entityUnderCursor)
    {
      if (this.m_selectedTransport.HasValue)
        return true;
      return entityUnderCursor.HasValue && entityUnderCursor.Value is Mafi.Core.Factory.Transports.Transport;
    }

    public bool TryHandleMouseUp(
      Option<IStaticEntity> entityUnderCursor,
      Action<IInputCommand> scheduleCommand,
      bool useQuickRemove)
    {
      if (this.m_selectedTransport.IsNone)
      {
        if (entityUnderCursor.IsNone || !(entityUnderCursor.Value is Mafi.Core.Factory.Transports.Transport entity))
          return false;
        if (this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteEntireTransport) || entity.Trajectory.Pivots.Length == 1)
        {
          scheduleCommand((IInputCommand) new StartDeconstructionOfStaticEntityCmd((IStaticEntity) entity, EntityRemoveReason.Remove));
          if (useQuickRemove)
            scheduleCommand((IInputCommand) new FinishBuildOfStaticEntityCmd(entity.Id, true));
          return false;
        }
        this.m_selectedTransport = (Option<Mafi.Core.Factory.Transports.Transport>) entity;
        this.m_transportDeleteStartPosition = new Tile3i?(this.m_hoverClosestPosition);
        this.m_setStartPointSound.Play();
        return true;
      }
      if (this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteEntireTransport))
      {
        scheduleCommand((IInputCommand) new StartDeconstructionOfStaticEntityCmd((IStaticEntity) this.m_selectedTransport.Value, EntityRemoveReason.Remove));
        if (useQuickRemove)
          scheduleCommand((IInputCommand) new FinishBuildOfStaticEntityCmd(this.m_selectedTransport.Value.Id, true));
        return false;
      }
      if (!this.m_isCutValid)
        return true;
      scheduleCommand((IInputCommand) new DeconstructTransportSegmentCmd(this.m_selectedTransport.Value, this.m_transportDeleteStartPosition.Value, this.m_hoverClosestPosition, useQuickRemove));
      return false;
    }

    public bool TryHandleCancel()
    {
      if (!this.m_selectedTransport.HasValue)
        return false;
      this.m_selectedTransport = Option<Mafi.Core.Factory.Transports.Transport>.None;
      this.m_transportDeleteStartPosition = new Tile3i?();
      this.m_cancelStartPointSound.Play();
      return true;
    }

    public void Sync()
    {
      if (this.m_lastValidationResult.HasValue || this.m_transportToValidate.IsNone)
        return;
      this.m_lastValidationResult = new EntityValidationResult?(this.m_entitiesManager.CanRemoveEntity((IEntity) this.m_transportToValidate.Value, EntityRemoveReason.Remove));
    }

    public void Deactivate()
    {
      this.m_selectedTransport = Option<Mafi.Core.Factory.Transports.Transport>.None;
      this.m_transportDeleteStartPosition = new Tile3i?();
      this.m_decValueForTransport = Option<Mafi.Core.Factory.Transports.Transport>.None;
      this.m_transportHighlighter.ClearAllHighlights();
      this.m_entityHighlighter.ClearAllHighlights();
      this.m_errorMsg = LocStrFormatted.Empty;
      this.m_deconstructionValue = AssetValue.Empty;
      this.m_validatedData = new TransportRemovalHandler.ValidatedTransportInfo?();
    }

    private bool validateTransportCut(
      Mafi.Core.Factory.Transports.Transport transport,
      Tile3i from,
      Tile3i to,
      out LocStrFormatted errorMsg,
      out AssetValue deconstructionValue)
    {
      TransportRemovalHandler.ValidatedTransportInfo validatedTransportInfo = new TransportRemovalHandler.ValidatedTransportInfo(transport, from, to);
      if (this.m_validatedData.HasValue && this.m_validatedData.Equals((object) validatedTransportInfo))
      {
        errorMsg = this.m_errorMsg;
        deconstructionValue = this.m_deconstructionValue;
        return this.m_isCutValid;
      }
      CanCutOutTransportResult result;
      LocStrFormatted error;
      bool flag = TransportsConstructionHelper.CanCutOutTransport(transport, from, to, true, true, out result, out error);
      errorMsg = error;
      this.m_transportHighlighter.ClearAllHighlights();
      if (flag)
      {
        deconstructionValue = this.m_constructionManager.GetSellValue(result.ComputeCutOutValue());
        if (result.CutOutSubTransport.HasValue)
          this.m_transportHighlighter.HighlightTrajectory(result.CutOutSubTransport.Value, DeleteEntityInputController.COLOR_HIGHLIGHT);
      }
      else
        deconstructionValue = AssetValue.Empty;
      this.m_errorMsg = errorMsg;
      this.m_deconstructionValue = deconstructionValue;
      this.m_validatedData = new TransportRemovalHandler.ValidatedTransportInfo?(validatedTransportInfo);
      this.m_isCutValid = flag;
      return flag;
    }

    private AssetValue getDeconstructionValueFor(Mafi.Core.Factory.Transports.Transport t)
    {
      if (this.m_decValueForTransport != t)
      {
        this.m_decValueForTransport = (Option<Mafi.Core.Factory.Transports.Transport>) t;
        this.m_decValueForTransportCache = this.m_constructionManager.GetDeconstructionValueFor((IStaticEntity) t);
      }
      return this.m_decValueForTransportCache;
    }

    private readonly struct ValidatedTransportInfo : 
      IEquatable<TransportRemovalHandler.ValidatedTransportInfo>
    {
      public readonly Mafi.Core.Factory.Transports.Transport Transport;
      public readonly Tile3i From;
      public readonly Tile3i To;

      public ValidatedTransportInfo(Mafi.Core.Factory.Transports.Transport transport, Tile3i from, Tile3i to)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Transport = transport;
        this.From = from;
        this.To = to;
      }

      public bool Equals(
        TransportRemovalHandler.ValidatedTransportInfo other)
      {
        return this.Transport.Equals((Entity) other.Transport) && this.From == other.From && this.To == other.To;
      }

      public override bool Equals(object obj)
      {
        return obj is TransportRemovalHandler.ValidatedTransportInfo other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return Hash.Combine<Mafi.Core.Factory.Transports.Transport, Tile3i, Tile3i>(this.Transport, this.From, this.To);
      }
    }
  }
}
