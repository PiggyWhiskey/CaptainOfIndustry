// Decompiled with JetBrains decompiler
// Type: RTG.ObjectCloning
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class ObjectCloning
  {
    private static ObjectCloning.Config _defaultConfig;

    static ObjectCloning()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjectCloning._defaultConfig = new ObjectCloning.Config();
      ObjectCloning._defaultConfig.TransformFlags = ObjectCloning.TransformFlags.All;
      ObjectCloning._defaultConfig.Layer = 0;
    }

    public static ObjectCloning.Config DefaultConfig => ObjectCloning._defaultConfig;

    public static List<GameObject> CloneHierarchies(
      List<GameObject> roots,
      ObjectCloning.Config cloneConfig)
    {
      if (roots.Count == 0)
        return new List<GameObject>();
      List<GameObject> gameObjectList = new List<GameObject>(roots.Count);
      foreach (GameObject root in roots)
      {
        GameObject gameObject = ObjectCloning.CloneHierarchy(root, cloneConfig);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          gameObjectList.Add(gameObject);
      }
      return gameObjectList;
    }

    public static GameObject CloneHierarchy(GameObject root, ObjectCloning.Config cloneConfig)
    {
      if ((UnityEngine.Object) root == (UnityEngine.Object) null)
        return (GameObject) null;
      Transform transform1 = root.transform;
      Vector3 position = Vector3.zero;
      Quaternion rotation = Quaternion.identity;
      Vector3 vector3 = Vector3.one;
      if ((cloneConfig.TransformFlags & ObjectCloning.TransformFlags.Position) != ObjectCloning.TransformFlags.None)
        position = transform1.position;
      if ((cloneConfig.TransformFlags & ObjectCloning.TransformFlags.Rotation) != ObjectCloning.TransformFlags.None)
        rotation = transform1.rotation;
      if ((cloneConfig.TransformFlags & ObjectCloning.TransformFlags.Scale) != ObjectCloning.TransformFlags.None)
        vector3 = transform1.lossyScale;
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(root, position, rotation);
      gameObject.name = root.name;
      gameObject.layer = cloneConfig.Layer;
      Transform transform2 = gameObject.transform;
      transform2.localScale = vector3;
      transform2.parent = cloneConfig.Parent;
      return gameObject;
    }

    [Flags]
    public enum TransformFlags
    {
      None = 0,
      Position = 1,
      Rotation = 2,
      Scale = 4,
      All = Scale | Rotation | Position, // 0x00000007
    }

    public struct Config
    {
      public Transform Parent;
      public ObjectCloning.TransformFlags TransformFlags;
      public int Layer;
    }
  }
}
