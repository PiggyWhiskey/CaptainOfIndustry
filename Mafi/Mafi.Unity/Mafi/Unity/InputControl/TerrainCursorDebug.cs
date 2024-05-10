// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TerrainCursorDebug
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [GlobalDependency(RegistrationMode.AsSelf, true, false)]
  internal class TerrainCursorDebug
  {
    public TerrainCursorDebug(NewInstanceOf<TerrainCursor> cursor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      new GameObject("DEBUG: terrain cursor.").AddComponent<TerrainCursorDebugMb>().Initialize(cursor.Instance);
    }
  }
}
