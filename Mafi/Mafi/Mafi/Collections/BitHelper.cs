// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.BitHelper
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.Security;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  ///  Note: Duplicated because the one in System.Collections.Generic is internal.
  /// 
  /// ABOUT:
  ///  Helps with operations that rely on bit marking to indicate whether an item in the
  ///  collection should be added, removed, visited already, etc.
  /// 
  ///  BitHelper doesn't allocate the array; you must pass in an array or ints allocated on the
  ///  stack or heap. ToIntArrayLength() tells you the int array size you must allocate.
  /// 
  ///  USAGE:
  ///  Suppose you need to represent a bit array of length (i.e. logical bit array length)
  ///  BIT_ARRAY_LENGTH. Then this is the suggested way to instantiate BitHelper:
  ///  ***************************************************************************
  ///  int intArrayLength = BitHelper.ToIntArrayLength(BIT_ARRAY_LENGTH);
  ///  BitHelper bitHelper;
  ///  if (intArrayLength less than stack alloc threshold)
  ///      int* m_arrayPtr = stackalloc int[intArrayLength];
  ///      bitHelper = new BitHelper(m_arrayPtr, intArrayLength);
  ///  else
  ///      int[] m_arrayPtr = new int[intArrayLength];
  ///      bitHelper = new BitHelper(m_arrayPtr, intArrayLength);
  ///  ***************************************************************************
  /// 
  ///  IMPORTANT:
  ///  The second ctor args, length, should be specified as the length of the int array, not
  ///  the logical bit array. Because length is used for bounds checking into the int array,
  ///  it's especially important to get this correct for the stackalloc version. See the code
  ///  samples above; this is the value gotten from ToIntArrayLength().
  /// 
  ///  The length ctor argument is the only exception; for other methods -- MarkBit and
  ///  IsMarked -- pass in values as indices into the logical bit array, and it will be mapped
  ///  to the position within the array of ints.
  /// 
  ///  </summary>
  internal class BitHelper
  {
    private int m_length;
    [SecurityCritical]
    private unsafe int* m_arrayPtr;
    private int[] m_array;
    private bool useStackAlloc;

    /// <summary>
    /// Instantiates a BitHelper with a heap alloc'd array of ints
    /// </summary>
    /// <param name="bitArray">int array to hold bits</param>
    /// <param name="length">length of int array</param>
    [SecurityCritical]
    internal unsafe BitHelper(int* bitArrayPtr, int length)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_arrayPtr = bitArrayPtr;
      this.m_length = length;
      this.useStackAlloc = true;
    }

    /// <summary>
    /// Instantiates a BitHelper with a heap alloc'd array of ints
    /// </summary>
    /// <param name="bitArray">int array to hold bits</param>
    /// <param name="length">length of int array</param>
    internal BitHelper(int[] bitArray, int length)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_array = bitArray;
      this.m_length = length;
    }

    /// <summary>Mark bit at specified position</summary>
    /// <param name="bitPosition"></param>
    [SecuritySafeCritical]
    internal unsafe void MarkBit(int bitPosition)
    {
      if (this.useStackAlloc)
      {
        int num = bitPosition / 32;
        if (num >= this.m_length || num < 0)
          return;
        int* numPtr = this.m_arrayPtr + num;
        *numPtr = *numPtr | 1 << bitPosition % 32;
      }
      else
      {
        int index = bitPosition / 32;
        if (index >= this.m_length || index < 0)
          return;
        this.m_array[index] |= 1 << bitPosition % 32;
      }
    }

    /// <summary>Is bit at specified position marked?</summary>
    /// <param name="bitPosition"></param>
    /// <returns></returns>
    [SecuritySafeCritical]
    internal unsafe bool IsMarked(int bitPosition)
    {
      if (this.useStackAlloc)
      {
        int index = bitPosition / 32;
        return index < this.m_length && index >= 0 && (this.m_arrayPtr[index] & 1 << bitPosition % 32) != 0;
      }
      int index1 = bitPosition / 32;
      return index1 < this.m_length && index1 >= 0 && (this.m_array[index1] & 1 << bitPosition % 32) != 0;
    }

    /// <summary>
    /// How many ints must be allocated to represent n bits. Returns (n+31)/32, but avoids overflow
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    internal static int ToIntArrayLength(int n) => n <= 0 ? 0 : (n - 1) / 32 + 1;
  }
}
