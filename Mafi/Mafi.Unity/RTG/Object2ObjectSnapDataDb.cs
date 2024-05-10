// Decompiled with JetBrains decompiler
// Type: RTG.Object2ObjectSnapDataDb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class Object2ObjectSnapDataDb : Singleton<Object2ObjectSnapDataDb>
  {
    private Dictionary<GameObject, Object2ObjectSnapData> _objectToSnapData;

    public Object2ObjectSnapData GetObject2ObjectSnapData(GameObject gameObject)
    {
      if (this._objectToSnapData.ContainsKey(gameObject))
        return this._objectToSnapData[gameObject];
      Object2ObjectSnapData object2ObjectSnapData = new Object2ObjectSnapData();
      if (!object2ObjectSnapData.Initialize(gameObject))
        return (Object2ObjectSnapData) null;
      this._objectToSnapData.Add(gameObject, object2ObjectSnapData);
      return object2ObjectSnapData;
    }

    public Object2ObjectSnapDataDb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._objectToSnapData = new Dictionary<GameObject, Object2ObjectSnapData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
