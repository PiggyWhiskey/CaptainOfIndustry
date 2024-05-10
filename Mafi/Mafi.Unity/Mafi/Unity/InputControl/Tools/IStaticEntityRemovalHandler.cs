// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.IStaticEntityRemovalHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [MultiDependency]
  public interface IStaticEntityRemovalHandler
  {
    int Priority { get; }

    bool CanBeInterrupted { get; }

    bool TryHandleHover(
      Option<IStaticEntity> entityUnderCursor,
      Tile3f pickCoord,
      out AssetValue deconstructionValue,
      out LocStrFormatted errorMsg);

    bool TryHandleMouseDown(Option<IStaticEntity> entityUnderCursor);

    bool TryHandleMouseUp(
      Option<IStaticEntity> entityUnderCursor,
      Action<IInputCommand> scheduleCommand,
      bool useQuickRemove);

    bool TryHandleCancel();

    void Sync();

    void Deactivate();
  }
}
