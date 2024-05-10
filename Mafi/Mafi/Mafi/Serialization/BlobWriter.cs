// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.BlobWriter
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization.Generators;
using Mafi.Serialization.ObjectIds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace Mafi.Serialization
{
  public class BlobWriter : IDisposable
  {
    internal const bool COLLECT_SERIALIZATION_STATS = false;
    protected readonly BinaryWriter Writer;
    /// <summary>Stream that was passed to the constructor.</summary>
    protected readonly Stream OutputStream;
    private ImmutableArray<ISpecialSerializerFactory> m_specialSerializers;
    private readonly Dict<Type, object> m_serializersCache;
    private readonly Dict<object, int> m_writtenObjects;
    private readonly Dict<Type, int> m_writtenTypes;
    private readonly Queueue<KeyValuePair<object, Action<object, BlobWriter>>> m_delayedDataSerializations;
    private readonly Assembly m_executingAssembly;
    private readonly Assembly m_mscorLibAssembly;

    public int DelayedSerializationsCount => this.m_delayedDataSerializations.Count;

    public BlobWriter(
      Stream outputStream,
      ImmutableArray<ISpecialSerializerFactory>? specialSerializers = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_serializersCache = new Dict<Type, object>();
      this.m_writtenObjects = new Dict<object, int>(ReferenceEqualityComparer<object>.Instance);
      this.m_writtenTypes = new Dict<Type, int>((IEqualityComparer<Type>) ReferenceEqualityComparer<object>.Instance);
      this.m_delayedDataSerializations = new Queueue<KeyValuePair<object, Action<object, BlobWriter>>>();
      this.m_executingAssembly = Assembly.GetExecutingAssembly();
      this.m_mscorLibAssembly = typeof (int).Assembly;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.OutputStream = outputStream;
      this.Writer = new BinaryWriter(outputStream, Encoding.UTF8, true);
      this.m_specialSerializers = specialSerializers ?? ImmutableArray<ISpecialSerializerFactory>.Empty;
      this.resetMap();
    }

    /// <summary>
    /// Returns whether given type can be serialized. This is quite slow. Consider caching results if needed.
    /// </summary>
    internal bool CanSerialize(Type type)
    {
      return SerializerGenerator.TryGetSerializeMethod(type, out MethodInfo _);
    }

    public void SetSpecialSerializers(
      ImmutableArray<ISpecialSerializerFactory> specialSerializers)
    {
      Assert.That<Queueue<KeyValuePair<object, Action<object, BlobWriter>>>>(this.m_delayedDataSerializations).IsEmpty<KeyValuePair<object, Action<object, BlobWriter>>>("Setting special serializers when there are still object for serialization pending.");
      this.m_specialSerializers = specialSerializers;
    }

    public void FinalizeSerialization()
    {
      while (this.m_delayedDataSerializations.IsNotEmpty)
      {
        KeyValuePair<object, Action<object, BlobWriter>> keyValuePair = this.m_delayedDataSerializations.Dequeue();
        keyValuePair.Value(keyValuePair.Key, this);
      }
    }

    public virtual void Flush() => this.Writer.Flush();

    public virtual void Dispose()
    {
      Assert.That<Queueue<KeyValuePair<object, Action<object, BlobWriter>>>>(this.m_delayedDataSerializations).IsEmpty<KeyValuePair<object, Action<object, BlobWriter>>>("There are some objects to serialize, did you call `FinalizeSerialization`?");
      this.Flush();
      this.Writer.Dispose();
      this.m_writtenObjects.Clear();
      this.m_writtenTypes.Clear();
    }

    public virtual void Reset()
    {
      this.Flush();
      this.resetMap();
    }

    private void resetMap()
    {
      this.m_writtenObjects.Clear();
      this.m_writtenTypes.Clear();
      this.m_writtenObjects.Add((object) null, 0);
      this.m_writtenTypes.Add((Type) null, 0);
    }

    public Action<T, BlobWriter> GetSerializerFor<T>()
    {
      object serializerFor1;
      if (this.m_serializersCache.TryGetValue(typeof (T), out serializerFor1))
        return (Action<T, BlobWriter>) serializerFor1;
      Action<T, BlobWriter> serializerFor2 = (Action<T, BlobWriter>) null;
      foreach (ISpecialSerializerFactory specialSerializer in this.m_specialSerializers)
      {
        if (specialSerializer.CanSerialize<T>())
          serializerFor2 = specialSerializer.GetSerializeAction<T>();
      }
      if (serializerFor2 == null)
        serializerFor2 = GenericSerializersFactory.GetSerializeAction<T>();
      this.m_serializersCache.Add(typeof (T), (object) serializerFor2);
      return serializerFor2;
    }

    public void WriteGeneric<T>(T obj)
    {
      Action<T, BlobWriter> serializerFor;
      try
      {
        serializerFor = this.GetSerializerFor<T>();
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to create generic serializer for '" + typeof (T).Name + "'.", ex);
      }
      serializerFor(obj, this);
    }

    public void WriteBool(bool value) => this.Writer.Write(value);

    public void WriteByte(byte value) => this.Writer.Write(value);

    public void WriteSByte(sbyte value) => this.Writer.Write(value);

    public void WriteChar(char value) => this.Writer.Write(value);

    public void WriteShort(short value) => this.Writer.Write(value);

    /// <summary>
    /// Variable encoding of short. Note that in order to make this worth it, the value must be small.
    /// Abs values larger than 8k will take 3 bytes to encode.
    /// </summary>
    public void WriteShortVariable(short value)
    {
      this.WriteUShortVariable((ushort) ((int) value << 1 ^ (int) value >> 15));
    }

    public void WriteUShort(ushort value) => this.Writer.Write(value);

    /// <summary>
    /// Variable encoding of ushort. Note that in order to make this worth it, the value must be small.
    /// Values larger than 16k will take 3 bytes to encode.
    /// </summary>
    public void WriteUShortVariable(ushort value)
    {
      if (value <= (ushort) sbyte.MaxValue)
      {
        this.Writer.Write((byte) value);
      }
      else
      {
        this.Writer.Write((byte) ((uint) value | 128U));
        if (value <= (ushort) 16383)
        {
          this.Writer.Write((byte) ((uint) value >> 7));
        }
        else
        {
          this.Writer.Write((byte) ((ulong) ((int) value >> 7) | 128UL));
          this.Writer.Write((byte) ((uint) value >> 14));
        }
      }
    }

    /// <summary>
    /// Non-variable encoded uint. Use in cases the number is expected to have 11 top set bits often (not counting
    /// sign bit).
    /// </summary>
    public void WriteIntNonVariable(int value) => this.Writer.Write(value);

    /// <summary>
    /// Variable encoding of integers. Small values encode in less bytes. The break-even is value ~+-13 M that still
    /// encodes to 4 bytes, values larger than that will have 5 bytes.
    /// </summary>
    public void WriteInt(int value) => this.WriteUInt((uint) (value << 1 ^ value >> 31));

    public void WriteIntNotNegative(int value)
    {
      if (value < 0)
        throw new CorruptedSaveException(string.Format("Value {0} is negative!", (object) value));
      this.WriteUInt((uint) value);
    }

    /// <summary>
    /// Non-variable encoded uint. Use in cases the number is expected to be have 11 top set bits often.
    /// </summary>
    public void WriteUIntNonVariable(uint value) => this.Writer.Write(value);

    /// <summary>
    /// Variable encoding of integers. Small values encode in less bytes. The break-even is value ~26 M that still
    /// encodes to 4 bytes, values larger than that will have 5 bytes.
    /// </summary>
    public void WriteUInt(uint value)
    {
      if (value <= (uint) sbyte.MaxValue)
      {
        this.Writer.Write((byte) value);
      }
      else
      {
        this.Writer.Write((byte) (value | 128U));
        if (value <= 16383U)
        {
          this.Writer.Write((byte) (value >> 7));
        }
        else
        {
          this.Writer.Write((byte) (value >> 7 | 128U));
          if (value <= 2097151U)
          {
            this.Writer.Write((byte) (value >> 14));
          }
          else
          {
            this.Writer.Write((byte) (value >> 14 | 128U));
            if (value <= 268435455U)
            {
              this.Writer.Write((byte) (value >> 21));
            }
            else
            {
              this.Writer.Write((byte) (value >> 21 | 128U));
              this.Writer.Write((byte) (value >> 28));
            }
          }
        }
      }
    }

    public void WriteLongNonVariable(long value) => this.Writer.Write(value);

    public void WriteLong(long value) => this.WriteULong((ulong) (value << 1 ^ value >> 63));

    public void WriteLongNotNegative(long value)
    {
      if (value < 0L)
        throw new CorruptedSaveException(string.Format("Value {0} is negative!", (object) value));
      this.WriteULong((ulong) value);
    }

    public void WriteULongNonVariable(ulong value) => this.Writer.Write(value);

    public void WriteULong(ulong value)
    {
      if (value <= (ulong) sbyte.MaxValue)
      {
        this.Writer.Write((byte) value);
      }
      else
      {
        this.Writer.Write((byte) (value | 128UL));
        if (value <= 16383UL)
        {
          this.Writer.Write((byte) (value >> 7));
        }
        else
        {
          this.Writer.Write((byte) (value >> 7 | 128UL));
          if (value <= 2097151UL)
          {
            this.Writer.Write((byte) (value >> 14));
          }
          else
          {
            this.Writer.Write((byte) (value >> 14 | 128UL));
            if (value <= 268435455UL)
            {
              this.Writer.Write((byte) (value >> 21));
            }
            else
            {
              this.Writer.Write((byte) (value >> 21 | 128UL));
              if (value <= 34359738367UL)
              {
                this.Writer.Write((byte) (value >> 28));
              }
              else
              {
                this.Writer.Write((byte) (value >> 28 | 128UL));
                if (value <= 4398046511103UL)
                {
                  this.Writer.Write((byte) (value >> 35));
                }
                else
                {
                  this.Writer.Write((byte) (value >> 35 | 128UL));
                  if (value <= 562949953421311UL)
                  {
                    this.Writer.Write((byte) (value >> 42));
                  }
                  else
                  {
                    this.Writer.Write((byte) (value >> 42 | 128UL));
                    if (value <= 72057594037927935UL)
                    {
                      this.Writer.Write((byte) (value >> 49));
                    }
                    else
                    {
                      this.Writer.Write((byte) (value >> 49 | 128UL));
                      this.Writer.Write((byte) (value >> 56));
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    public void WriteFloat(float value) => this.Writer.Write(value);

    public void WriteDouble(double value) => this.Writer.Write(value);

    public void WriteDateTime(DateTime value) => this.WriteLong(value.ToFileTimeUtc());

    public void WriteString(string value)
    {
      int num;
      if (this.m_writtenObjects.TryGetValue((object) value, out num))
      {
        this.WriteIntNotNegative(num);
      }
      else
      {
        int count = this.m_writtenObjects.Count;
        this.m_writtenObjects.Add((object) value, count);
        this.WriteIntNotNegative(count);
        this.Writer.Write(value);
      }
    }

    internal void WriteStringNoRef(string value)
    {
      if (value == null)
      {
        Log.Error("Serializing null of type 'string' (no ref).");
        this.Writer.Write("");
      }
      else
        this.Writer.Write(value);
    }

    public void WriteNullableStruct<T>(T? obj) where T : struct
    {
      this.WriteBool(obj.HasValue);
      if (!obj.HasValue)
        return;
      this.WriteGeneric<T>(obj.Value);
    }

    public void WriteKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
    {
      this.WriteGeneric<TKey>(pair.Key);
      this.WriteGeneric<TValue>(pair.Value);
    }

    public void WriteType(Type type, bool allowNulls = false)
    {
      if (type == (Type) null)
      {
        if (!allowNulls)
          Log.Error("Serializing null of type 'Type'.");
        this.WriteIntNotNegative(0);
      }
      else
      {
        int num;
        if (this.m_writtenTypes.TryGetValue(type, out num))
        {
          this.WriteIntNotNegative(num);
        }
        else
        {
          int count = this.m_writtenTypes.Count;
          this.m_writtenTypes.Add(type, count);
          this.WriteIntNotNegative(count);
          this.WriteTypeNameAsStrNoRef(type);
        }
      }
    }

    public void WriteTypeNameAsStrNoRef(Type type)
    {
      this.WriteStringNoRef(type.Assembly == this.m_executingAssembly || type.Assembly == this.m_mscorLibAssembly ? type.FullName : type.AssemblyQualifiedName);
    }

    public void WriteArray<T>(T[] arr)
    {
      if (!this.TryStartArraySerialization<T>(arr) || arr.Length == 0)
        return;
      Action<T, BlobWriter> serializerFor = this.GetSerializerFor<T>();
      for (int index = 0; index < arr.Length; ++index)
        serializerFor(arr[index], this);
    }

    public void WriteArray<T>(T[] arr, int length)
    {
      if (length > arr.Length)
      {
        Log.Error(string.Format("Trying to write {0} elements of array of length {1}.", (object) length, (object) arr.Length));
        length = arr.Length;
      }
      if (!this.TryStartArraySerialization<T>(arr) || arr.Length == 0)
        return;
      Action<T, BlobWriter> serializerFor = this.GetSerializerFor<T>();
      for (int index = 0; index < length; ++index)
        serializerFor(arr[index], this);
    }

    public void WriteByteArray(ImmutableArray<byte> immutableArray)
    {
      this.WriteIntNotNegative(immutableArray.Length);
      ImmutableArray<byte>.WriteDataTo(immutableArray, this.Writer);
    }

    public bool TryStartClassSerialization<T>(T obj) where T : class
    {
      if ((object) obj == null)
      {
        Log.Warning("Serializing null of type '" + typeof (T).Name + "'.");
        this.WriteIntNotNegative(0);
        return false;
      }
      int num;
      if (this.m_writtenObjects.TryGetValue((object) obj, out num))
      {
        this.WriteIntNotNegative(num);
        return false;
      }
      int count = this.m_writtenObjects.Count;
      this.m_writtenObjects.Add((object) obj, count);
      this.WriteIntNotNegative(count);
      this.WriteType(obj.GetType());
      return true;
    }

    public bool TryStartArraySerialization<T>(T[] arr)
    {
      if (arr == null)
      {
        Log.Error("Serializing null of type '" + typeof (T[]).Name + "'.");
        this.WriteIntNotNegative(0);
        return false;
      }
      int num;
      if (this.m_writtenObjects.TryGetValue((object) arr, out num))
      {
        this.WriteIntNotNegative(num);
        return false;
      }
      int count = this.m_writtenObjects.Count;
      this.m_writtenObjects.Add((object) arr, count);
      this.WriteIntNotNegative(count);
      this.WriteType(arr.GetType().GetElementType());
      this.WriteIntNotNegative(arr.Length);
      return true;
    }

    /// <summary>
    /// Enqueues object for data serialization. This is to break potentially too deep recursion when serializing
    /// large object graphs.
    /// </summary>
    public void EnqueueDataSerialization(object obj, Action<object, BlobWriter> serializeAction)
    {
      this.m_delayedDataSerializations.Enqueue(Make.Kvp<object, Action<object, BlobWriter>>(obj, serializeAction));
    }
  }
}
