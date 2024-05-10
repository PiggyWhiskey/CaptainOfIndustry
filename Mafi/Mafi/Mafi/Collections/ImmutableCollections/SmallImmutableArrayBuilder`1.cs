// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.ImmutableCollections.SmallImmutableArrayBuilder`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Collections.ImmutableCollections
{
  public struct SmallImmutableArrayBuilder<T>
  {
    private T m_singleElement;
    private ImmutableArrayBuilder<T> m_arrayBuilder;

    private bool IsSingleElement => this.m_arrayBuilder.IsNotValid;

    public int Length => !this.IsSingleElement ? this.m_arrayBuilder.Length : 1;

    public T this[int index]
    {
      get
      {
        if (!this.IsSingleElement)
          return this.m_arrayBuilder[index];
        Assert.That<int>(index).IsZero();
        return this.m_singleElement;
      }
      set
      {
        if (this.IsSingleElement)
        {
          Assert.That<int>(index).IsZero();
          this.m_singleElement = value;
        }
        else
          this.m_arrayBuilder[index] = value;
      }
    }

    public SmallImmutableArrayBuilder(int length)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<int>(length).IsNotNegative();
      this.m_singleElement = default (T);
      this.m_arrayBuilder = length == 1 ? new ImmutableArrayBuilder<T>() : new ImmutableArrayBuilder<T>(length);
    }

    /// <summary>
    /// Creates immutable array from the internal array and clear the builder. No further methods can be called on
    /// this builder.
    /// </summary>
    public SmallImmutableArray<T> GetImmutableArrayAndClear()
    {
      return this.IsSingleElement ? new SmallImmutableArray<T>(this.m_singleElement) : new SmallImmutableArray<T>(this.m_arrayBuilder.GetImmutableArrayAndClear());
    }
  }
}
