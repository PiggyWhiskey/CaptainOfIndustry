// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ports.Io.PortHighlightData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ports.Io
{
  public readonly struct PortHighlightData
  {
    public readonly Option<GameObject> IconGo;
    public readonly Option<GameObject> ColliderGo;
    public readonly IoPortType PortType;
    public readonly int ArrowId1;
    public readonly int ArrowId2;
    public readonly uint HighlightInstanceId;

    public PortHighlightData(
      Option<GameObject> iconGo,
      Option<GameObject> colliderGo,
      IoPortType portType,
      int arrowId1,
      int arrowId2,
      uint highlightInstanceId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.IconGo = iconGo;
      this.PortType = portType;
      this.ArrowId1 = arrowId1;
      this.ArrowId2 = arrowId2;
      this.HighlightInstanceId = highlightInstanceId;
      this.ColliderGo = colliderGo;
    }
  }
}
