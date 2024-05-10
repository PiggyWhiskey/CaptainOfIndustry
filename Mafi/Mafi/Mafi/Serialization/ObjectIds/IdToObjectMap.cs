// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ObjectIds.IdToObjectMap
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;

#nullable disable
namespace Mafi.Serialization.ObjectIds
{
  /// <summary>
  /// Keeps track of map of loaded ids to corresponding objects.
  /// It is a counterpart for <see cref="T:Mafi.Serialization.ObjectIds.ObjectIdsGenerator" />
  /// </summary>
  public class IdToObjectMap
  {
    private int m_nextId;
    private readonly Dict<int, object> m_idToObject;

    public int GenerateNextId()
    {
      int nextId = this.m_nextId;
      ++this.m_nextId;
      Assert.That<int>(this.m_nextId).IsNotEqualTo(0);
      return nextId;
    }

    public void Add<T>(int id, T obj) => this.m_idToObject.Add(id, (object) obj);

    public bool TryGetObject<T>(int id, out T result)
    {
      object obj;
      if (this.m_idToObject.TryGetValue(id, out obj))
      {
        result = (T) obj;
        return true;
      }
      result = default (T);
      return false;
    }

    public bool GetObjectUntyped(int id, out object o) => this.m_idToObject.TryGetValue(id, out o);

    public bool Contains(int id) => this.m_idToObject.ContainsKey(id);

    public IdToObjectMap()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_nextId = 1;
      this.m_idToObject = new Dict<int, object>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
