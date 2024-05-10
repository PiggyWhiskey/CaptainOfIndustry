// Decompiled with JetBrains decompiler
// Type: RTG.GizmoBehaviourCollection
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoBehaviourCollection : IEnumerable
  {
    private List<IGizmoBehaviour> _behaviours;

    public int Count => this._behaviours.Count;

    public bool Add(IGizmoBehaviour behaviour)
    {
      if (this.Contains(behaviour))
        return false;
      this._behaviours.Add(behaviour);
      return true;
    }

    public bool Remove(IGizmoBehaviour behaviour) => this._behaviours.Remove(behaviour);

    public Type GetFirstBehaviourOfType<Type>() where Type : class, IGizmoBehaviour
    {
      List<Type> behavioursOfType = this.GetBehavioursOfType<Type>();
      return behavioursOfType.Count != 0 ? behavioursOfType[0] : default (Type);
    }

    public IGizmoBehaviour GetFirstBehaviourOfType(System.Type behaviourType)
    {
      List<IGizmoBehaviour> behavioursOfType = this.GetBehavioursOfType(behaviourType);
      return behavioursOfType.Count != 0 ? behavioursOfType[0] : (IGizmoBehaviour) null;
    }

    public List<Type> GetBehavioursOfType<Type>() where Type : class, IGizmoBehaviour
    {
      if (this.Count == 0)
        return new List<Type>();
      List<Type> behavioursOfType = new List<Type>(this.Count);
      System.Type c = typeof (Type);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        System.Type type = behaviour.GetType();
        if (type == c || type.IsSubclassOf(c))
          behavioursOfType.Add(behaviour as Type);
      }
      return behavioursOfType;
    }

    public List<IGizmoBehaviour> GetBehavioursOfType(System.Type behaviourType)
    {
      if (this.Count == 0)
        return new List<IGizmoBehaviour>();
      List<IGizmoBehaviour> behavioursOfType = new List<IGizmoBehaviour>(this.Count);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        System.Type type = behaviour.GetType();
        if (type == behaviourType || type.IsSubclassOf(behaviourType))
          behavioursOfType.Add(behaviour);
      }
      return behavioursOfType;
    }

    public bool Contains(IGizmoBehaviour behaviour) => this._behaviours.Contains(behaviour);

    public IEnumerator<IGizmoBehaviour> GetEnumerator()
    {
      return (IEnumerator<IGizmoBehaviour>) this._behaviours.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public GizmoBehaviourCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._behaviours = new List<IGizmoBehaviour>(10);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
