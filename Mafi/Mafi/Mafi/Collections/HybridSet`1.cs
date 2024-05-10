// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.HybridSet`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Immutable set that is optimized for small number of items (0-10). It is also optimized for repeated queries of
  /// the same item.
  /// </summary>
  public abstract class HybridSet<T> : ICollectionWithCount
  {
    private static readonly string s_pcKey;
    private static readonly string s_pcKey_ContainsEmpty;
    private static readonly string s_pcKey_CreatedOne;
    private static readonly string s_pcKey_ContainsOne;
    private static readonly string s_pcKey_CreatedTwo;
    private static readonly string s_pcKey_ContainsTwo;
    private static readonly string s_pcKey_CreatedMany;
    private static readonly string s_pcKey_ContainsMany;
    private static readonly string s_pcKey_ContainsMany_CachedQueries;
    public static readonly HybridSet<T> Empty;

    public static HybridSet<T> From(params T[] items)
    {
      switch (items.Length)
      {
        case 0:
          return HybridSet<T>.Empty;
        case 1:
          return (HybridSet<T>) new HybridSet<T>.HybridSetOne(items[0]);
        case 2:
          return (HybridSet<T>) new HybridSet<T>.HybridSetTwo(items[0], items[1]);
        default:
          return (HybridSet<T>) new HybridSet<T>.HybridSetMany(items);
      }
    }

    public static HybridSet<T> From(IEnumerable<T> items)
    {
      return HybridSet<T>.From(items.Distinct<T>().ToArray<T>());
    }

    public abstract int Count { get; }

    public abstract bool Contains(T item);

    public abstract IEnumerable<T> All();

    public bool IsEmpty => this.Count == 0;

    public bool IsNotEmpty => this.Count > 0;

    /// <summary>
    /// Creates a new HybridSet as a union of current and new items.
    /// </summary>
    [Pure]
    public HybridSet<T> Add(params T[] items) => this.Add((IEnumerable<T>) items);

    /// <summary>
    /// Creates a new HybridSet as a union of current and new items.
    /// </summary>
    [Pure]
    public HybridSet<T> Add(IEnumerable<T> items) => HybridSet<T>.From(this.All().Concat<T>(items));

    protected HybridSet()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static HybridSet()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      HybridSet<T>.s_pcKey = "HybridSet<" + typeof (T).Name + ">: ";
      HybridSet<T>.s_pcKey_ContainsEmpty = HybridSet<T>.s_pcKey + "Contains empty";
      HybridSet<T>.s_pcKey_CreatedOne = HybridSet<T>.s_pcKey + "Created one";
      HybridSet<T>.s_pcKey_ContainsOne = HybridSet<T>.s_pcKey + "Contains one";
      HybridSet<T>.s_pcKey_CreatedTwo = HybridSet<T>.s_pcKey + "Created two";
      HybridSet<T>.s_pcKey_ContainsTwo = HybridSet<T>.s_pcKey + "Contains two";
      HybridSet<T>.s_pcKey_CreatedMany = HybridSet<T>.s_pcKey + "Created many";
      HybridSet<T>.s_pcKey_ContainsMany = HybridSet<T>.s_pcKey + "Contains many";
      HybridSet<T>.s_pcKey_ContainsMany_CachedQueries = HybridSet<T>.s_pcKey + "Contains many (cached queries)";
      HybridSet<T>.Empty = (HybridSet<T>) new HybridSet<T>.HybridSetNone();
    }

    /// <summary>Used to store items in the HashSet.</summary>
    private class HybridSetMany : HybridSet<T>
    {
      private readonly EqualityComparer<T> m_comparer;
      private readonly Set<T> m_set;
      private T m_lastQuery;
      private bool m_lastResult;

      public HybridSetMany(T[] items)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_comparer = EqualityComparer<T>.Default;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Assert.That<T[]>(items).IsNotEmpty<T>();
        this.m_set = new Set<T>((IEnumerable<T>) items);
        this.m_lastQuery = items.First<T>();
        this.m_lastResult = true;
      }

      public override int Count => this.m_set.Count;

      public override IEnumerable<T> All() => (IEnumerable<T>) this.m_set;

      public override bool Contains(T item)
      {
        if (this.m_comparer.Equals(item, this.m_lastQuery))
          return this.m_lastResult;
        this.m_lastQuery = item;
        this.m_lastResult = this.m_set.Contains(item);
        return this.m_lastResult;
      }
    }

    private class HybridSetNone : HybridSet<T>
    {
      public override int Count => 0;

      public override IEnumerable<T> All() => Enumerable.Empty<T>();

      public override bool Contains(T item) => false;

      public HybridSetNone()
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private class HybridSetOne : HybridSet<T>
    {
      private readonly EqualityComparer<T> m_comparer;
      private readonly T m_item;

      public HybridSetOne(T item)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_comparer = EqualityComparer<T>.Default;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_item = item;
      }

      public override int Count => 1;

      public override IEnumerable<T> All()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerable<T>) new HybridSet<T>.HybridSetOne.\u003CAll\u003Ed__5(-2)
        {
          \u003C\u003E4__this = this
        };
      }

      public override bool Contains(T product) => this.m_comparer.Equals(product, this.m_item);
    }

    private class HybridSetTwo : HybridSet<T>
    {
      private readonly EqualityComparer<T> m_comparer;
      private readonly T m_item1;
      private readonly T m_item2;

      public HybridSetTwo(T item1, T item2)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_comparer = EqualityComparer<T>.Default;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_item1 = item1;
        this.m_item2 = item2;
      }

      public override int Count => 2;

      public override IEnumerable<T> All()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerable<T>) new HybridSet<T>.HybridSetTwo.\u003CAll\u003Ed__6(-2)
        {
          \u003C\u003E4__this = this
        };
      }

      public override bool Contains(T item)
      {
        return this.m_comparer.Equals(this.m_item1, item) || this.m_comparer.Equals(this.m_item2, item);
      }
    }
  }
}
