// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.QuadTree`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Collections
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct QuadTree<T> where T : IQuadTreeComparable<T>
  {
    /// <summary>Side length.</summary>
    public readonly ushort Size;
    /// <summary>
    /// An array with one sub-array per level. The sub-array contains all the data for that level.
    /// </summary>
    private readonly ImmutableArray<T[]> m_dataLevels;

    [LoadCtor]
    private QuadTree(ushort size, ImmutableArray<T[]> dataLevels)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Size = size;
      this.m_dataLevels = dataLevels;
    }

    public QuadTree(ushort size)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<int>((int) size).IsLessOrEqual(256);
      Assert.That<int>((int) size).IsPowerOfTwo();
      this.Size = size;
      int length1 = ((uint) (((int) size).CeilToPowerOfTwoOrZero() - 1)).CountSetBits();
      ImmutableArrayBuilder<T[]> immutableArrayBuilder = new ImmutableArrayBuilder<T[]>(length1);
      for (int i = 0; i < length1; ++i)
      {
        int length2 = ((int) size >> i).Squared();
        if (length2 <= 0)
        {
          Log.Error("Level in QuadTree with 0 or fewer elements!");
          length2 = 1;
        }
        immutableArrayBuilder[i] = new T[length2];
      }
      this.m_dataLevels = immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    public int GetIndex(byte x, byte y) => this.getZOrderIndex((uint) x, (uint) y);

    public T GetComparableAt(byte x, byte y)
    {
      return this.GetComparableAt(this.getZOrderIndex((uint) x, (uint) y));
    }

    public T GetComparableAt(int index) => this.m_dataLevels[0][index];

    public bool IsSet(byte x, byte y) => this.IsSet(this.getZOrderIndex((uint) x, (uint) y));

    public bool IsSet(int index) => this.m_dataLevels[0][index].IsSet();

    public void SetAt(byte x, byte y, T value)
    {
      this.SetAt(this.getZOrderIndex((uint) x, (uint) y), value);
    }

    public void SetAt(int index, T value)
    {
      this.m_dataLevels[0][index] = value;
      for (int index1 = 1; index1 < this.m_dataLevels.Length; ++index1)
      {
        T[] dataLevel = this.m_dataLevels[index1 - 1];
        int index2 = index - (index & 3);
        ref T local1 = ref value;
        T obj = default (T);
        if ((object) obj == null)
        {
          obj = local1;
          local1 = ref obj;
        }
        T other1 = dataLevel[index2];
        if (!local1.CanMergeQuadTreeWith(other1))
          break;
        ref T local2 = ref value;
        obj = default (T);
        if ((object) obj == null)
        {
          obj = local2;
          local2 = ref obj;
        }
        T other2 = dataLevel[index2 + 1];
        if (!local2.CanMergeQuadTreeWith(other2))
          break;
        ref T local3 = ref value;
        obj = default (T);
        if ((object) obj == null)
        {
          obj = local3;
          local3 = ref obj;
        }
        T other3 = dataLevel[index2 + 2];
        if (!local3.CanMergeQuadTreeWith(other3))
          break;
        ref T local4 = ref value;
        obj = default (T);
        if ((object) obj == null)
        {
          obj = local4;
          local4 = ref obj;
        }
        T other4 = dataLevel[index2 + 3];
        if (!local4.CanMergeQuadTreeWith(other4))
          break;
        index >>= 2;
        this.m_dataLevels[index1][index] = value;
      }
    }

    public void ClearAt(byte x, byte y) => this.ClearAt(this.getZOrderIndex((uint) x, (uint) y));

    public void ClearAt(int index)
    {
      this.m_dataLevels[0][index].Clear();
      for (int index1 = 1; index1 < this.m_dataLevels.Length; ++index1)
      {
        index >>= 2;
        if (!this.m_dataLevels[index1][index].IsSet())
          break;
        this.m_dataLevels[index1][index].Clear();
      }
    }

    public int GetHighestSetLevel(byte x, byte y)
    {
      return this.GetHighestSetLevel(this.getZOrderIndex((uint) x, (uint) y));
    }

    public int GetHighestSetLevel(int index)
    {
      for (int index1 = this.m_dataLevels.Length - 1; index1 >= 0; --index1)
      {
        if (this.m_dataLevels[index1][index >> index1 * 2].IsSet())
          return index1;
      }
      Log.Error("GetHighestSetLevel can't find set level");
      return 0;
    }

    /// <summary>
    /// Computes the z-order curve (https://en.wikipedia.org/wiki/Z-order_curve) index.
    /// https://stackoverflow.com/questions/12157685/z-order-curve-coordinates
    /// </summary>
    /// 
    ///             TODO: Implemented this when the QuadTree used a bitmap for its set/unset representation.
    ///             Perhaps overkill now?
    private int getZOrderIndex(uint x, uint y)
    {
      x = (uint) (((int) x | (int) x << 4) & 3855);
      x = (uint) (((int) x | (int) x << 2) & 13107);
      x = (uint) (((int) x | (int) x << 1) & 21845);
      y = (uint) (((int) y | (int) y << 4) & 3855);
      y = (uint) (((int) y | (int) y << 2) & 13107);
      y = (uint) (((int) y | (int) y << 1) & 21845);
      return (int) x | (int) y << 1;
    }

    public static void Serialize(QuadTree<T> value, BlobWriter writer)
    {
      writer.WriteUShort(value.Size);
      ImmutableArray<T[]>.Serialize(value.m_dataLevels, writer);
    }

    public static QuadTree<T> Deserialize(BlobReader reader)
    {
      return new QuadTree<T>(reader.ReadUShort(), ImmutableArray<T[]>.Deserialize(reader));
    }
  }
}
