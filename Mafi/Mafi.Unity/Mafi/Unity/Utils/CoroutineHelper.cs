// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.CoroutineHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Allows invoking coroutines from outside of MonoBehaviors.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class CoroutineHelper
  {
    private readonly MonoBehaviour m_coroutineMb;

    public void StartCoroutine(IEnumerator routine) => this.m_coroutineMb.StartCoroutine(routine);

    public CoroutineHelper()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_coroutineMb = (MonoBehaviour) new GameObject(nameof (CoroutineHelper)).AddComponent<CoroutineHelper.CoroutineMb>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private class CoroutineMb : MonoBehaviour
    {
      public CoroutineMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
