// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.IStaticEntityPreview
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Ports.Io;
using Mafi.Unity.InputControl.LayoutEntityPlacing;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  public interface IStaticEntityPreview
  {
    bool IsDestroyed { get; }

    IStaticEntityProto EntityProto { get; }

    AssetValue Price { get; }

    EntityValidationResult? ValidationResult { get; }

    bool ShowOceanAreas { get; }

    void ApplyTransformDelta(
      RelTile3i deltaPosition,
      Rotation90 deltaRotation,
      bool deltaReflection,
      Tile2i pivot);

    ThicknessTilesI GetEstPlacementHeight();

    void GetPlacementParams(
      out Tile3i basePosition,
      out bool canBeReflected,
      out ThicknessIRange allowedPlacementRange);

    void GetPortTypes(Set<ShapeTypePair> portsSet);

    void GetPortLocations(Dict<IoPortKey, IoPortType> ports);

    void HideConnectedPorts(Dict<IoPortKey, IoPortType> ports);

    bool CanMoveUpDownIfValid();

    void UpdateConfigWithTransform(EntityConfigData data);

    void SetPlacementPhase(EntityPlacementPhase placementPhase);

    void DestroyAndReturnToPool();
  }
}
