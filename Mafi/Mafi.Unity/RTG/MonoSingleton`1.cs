// Decompiled with JetBrains decompiler
// Type: RTG.MonoSingleton`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static object _singletonLock;
    private static T _instance;

    /// <summary>Returns the singleton instance.</summary>
    /// <remarks>
    /// If no instance of the required MonoBehaviour type exists in the scene, the property
    /// will return null. Null is also returned if more than one instance is found.
    /// </remarks>
    public static T Get
    {
      get
      {
        if ((Object) MonoSingleton<T>._instance == (Object) null)
        {
          lock (MonoSingleton<T>._singletonLock)
          {
            T[] objectsOfType = Object.FindObjectsOfType(typeof (T)) as T[];
            if (objectsOfType.Length == 0)
              return default (T);
            if (objectsOfType.Length > 1)
            {
              if (Application.isEditor)
                Debug.LogWarning((object) "MonoSingleton<T>.Instance: Only 1 singleton instance can exist in the scene. Null will be returned.");
              return default (T);
            }
            MonoSingleton<T>._instance = objectsOfType[0];
          }
        }
        return MonoSingleton<T>._instance;
      }
    }

    public MonoSingleton()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static MonoSingleton()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MonoSingleton<T>._singletonLock = new object();
    }
  }
}
