// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ObjectIds.ObjectIdsGenerator
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;

#nullable disable
namespace Mafi.Serialization.ObjectIds
{
  /// <summary>Generates unique ids for objects.</summary>
  /// <remarks>
  /// We could use ObjectIDGenerator for ids generation, but it generates <see cref="T:System.Int64" /> ids, whereas we generate
  /// shorter <see cref="T:System.Int32" /> ids to save space. And even use have shorter ids for all or some object in the future.
  /// </remarks>
  public class ObjectIdsGenerator
  {
    public const int NULL_ID = 0;
    public const int FIRST_VALID_ID = 1;
    private int m_nextId;
    private readonly Dict<object, int> m_idsMap;

    /// <remarks>
    /// Type parameter is not limited to class, because of usage in methods in <see cref="T:Mafi.Serialization.BlobWriter" />, where their
    /// type parameter is not limited to class either.
    /// </remarks>
    public int GetOrAddIdFor<T>(T obj, out bool added)
    {
      Assert.That<bool>((object) obj != null).IsTrue();
      int orAddIdFor;
      if (this.m_idsMap.TryGetValue((object) obj, out orAddIdFor))
      {
        added = false;
        return orAddIdFor;
      }
      added = true;
      int nextId = this.getNextId();
      this.m_idsMap.Add((object) obj, nextId);
      return nextId;
    }

    public int GetIdFor<T>(T obj) => this.m_idsMap[(object) obj];

    private int getNextId()
    {
      int nextId = this.m_nextId;
      ++this.m_nextId;
      Assert.That<int>(this.m_nextId).IsGreater(0);
      return nextId;
    }

    public ObjectIdsGenerator()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_nextId = 1;
      this.m_idsMap = new Dict<object, int>(ReferenceEqualityComparer<object>.Instance);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
