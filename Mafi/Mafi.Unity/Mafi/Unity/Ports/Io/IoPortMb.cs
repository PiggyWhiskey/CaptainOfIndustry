// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ports.Io.IoPortMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ports.Io
{
  public class IoPortMb
  {
    /// <summary>
    /// Sets position and direction of this port to given absolute values.
    /// </summary>
    public static void SetTransform(GameObject portGo, Tile3i position, Direction90 direction)
    {
      Transform transform = portGo.transform;
      transform.position = position.ToGroundCenterVector3();
      transform.rotation = direction.ToRotation().Quaternion.ToUnityQuaternion();
    }

    /// <summary>
    /// Sets position and rotation of this port based on given layout port and layout data.
    /// </summary>
    public static void SetTransform(
      GameObject portGo,
      IoPortTemplate port,
      EntityLayout layout,
      TileTransform entityTransform)
    {
      IoPortMb.SetTransform(portGo, layout.Transform(port.RelativePosition, entityTransform), entityTransform.Transform(port.RelativeDirection));
    }

    public IoPortMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
