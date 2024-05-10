// Decompiled with JetBrains decompiler
// Type: Mafi.Option`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// A generic Option type that allows for explicit distinguishing between an intentionally set value, and a default
  /// value of None. It is readonly struct. Storing mutable values is ok end expected.
  /// 
  /// The purpose of this class is not to completely prevent null reference exceptions but to introduce semantics to
  /// help distinguishing potentially null values and expected non-null references.
  /// </summary>
  /// <remarks>
  /// This struct has only one member - the pointer to the class. This is to minimize overhead. With only one pointer
  /// the memory overhead is actually 0.
  /// </remarks>
  [DebuggerDisplay("HasValue = {HasValue}, Value = {ValueOrNull}")]
  [ManuallyWrittenSerialization]
  public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>, IOptionNonGeneric where T : class
  {
    /// <summary>The Option indicating there is no value.</summary>
    public static readonly Option<T> None;
    /// <summary>
    /// Value of the option. Exception is raised when accessing an option that has no value so you better behave!
    /// </summary>
    /// <remarks>The access is checked in debug mode.</remarks>
    public readonly T Value;

    /// <summary>
    /// Creates an <see cref="T:Mafi.Option`1" /> from given value. The value may be null.
    /// </summary>
    public static Option<T> Create(T value) => new Option<T>(value);

    /// <summary>
    /// Creates an <see cref="T:Mafi.Option`1" /> from given value. The value is expected to be not null (yes, it will throw
    /// exception in your face if it is not).
    /// </summary>
    public static Option<T> Some(T value)
    {
      if ((object) value == null)
        Log.Error("Option<T>.Some called with value null.");
      return new Option<T>(value);
    }

    /// <summary>
    /// Private constructor to enforce usage of <see cref="M:Mafi.Option.Some``1(``0)" /> and <see cref="F:Mafi.Option.None" />.
    /// Default-construction option struct results in <see cref="F:Mafi.Option.None" /> value.
    /// </summary>
    private Option(T value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Value = value;
    }

    /// <summary>
    /// Bool indicating whether the option has a value (opposite to <see cref="P:Mafi.Option`1.IsNone" />).
    /// </summary>
    public bool HasValue => (object) this.Value != null;

    /// <summary>
    /// Bool indicating whether the option has no value (opposite to <see cref="P:Mafi.Option`1.HasValue" />).
    /// </summary>
    public bool IsNone => (object) this.Value == null;

    /// <summary>
    /// Returns this if it has a value or the given option otherwise.
    /// </summary>
    [Pure]
    public Option<T> Or(Option<T> other) => !this.HasValue ? other : this;

    /// <summary>
    /// Gets the value of the option if present, and the provided value otherwise. Keep in mind that the given
    /// default value gets evaluated every time even if it is not used, so do not put <cdoe>new</cdoe> statements or
    /// function calls there.
    /// </summary>
    [Pure]
    public T ValueOr(T val) => !this.HasValue ? val : this.Value;

    /// <summary>
    /// Gets raw reference of this option. This is useful for usage with Elvis operator ?.;
    /// </summary>
    public T ValueOrNull => this.Value;

    public T ValueOrThrow(string message)
    {
      if (this.HasValue)
        return this.Value;
      throw new NullReferenceException("Option<" + typeof (T).Name + "> is null: " + message);
    }

    /// <summary>
    /// Returns value of option cast as <typeparamref name="TResult" />. Returns cast value or
    /// <see cref="F:Mafi.Option.None" /> if this option has no value or the cast failed.
    /// </summary>
    public Option<TResult> As<TResult>() where TResult : class
    {
      return new Option<TResult>((object) this.Value as TResult);
    }

    bool IOptionNonGeneric.HasValue => this.HasValue;

    object IOptionNonGeneric.Value => (object) this.Value;

    object IOptionNonGeneric.ValueOrNull => (object) this.ValueOrNull;

    /// <summary>
    /// Implicitly converts an <see cref="T:Mafi.Option" /> to an <see cref="T:Mafi.Option`1" />.
    /// </summary>
    public static implicit operator Option<T>(Option option) => Option<T>.None;

    /// <summary>
    /// Implicitly converts given value to an <see cref="T:Mafi.Option`1" />. Due to C# spec limitations the implicit cast
    /// does not work for interfaces, use <see cref="M:Mafi.Option.Create``1(``0)" /> or <see cref="M:Mafi.Option.Some``1(``0)" />
    /// instead.
    /// </summary>
    public static implicit operator Option<T>(T value) => new Option<T>(value);

    /// <summary>
    /// Compares two options for equality based on their values.
    /// </summary>
    public static bool operator ==(Option<T> lhs, Option<T> rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Compares two options for inequality based on their values.
    /// </summary>
    public static bool operator !=(Option<T> lhs, Option<T> rhs) => !lhs.Equals(rhs);

    /// <summary>Compares option and raw value for equality.</summary>
    public static bool operator ==(Option<T> lhs, T rhs) => lhs.Equals(rhs);

    /// <summary>Compares option and raw value for inequality.</summary>
    public static bool operator !=(Option<T> lhs, T rhs) => !lhs.Equals(rhs);

    /// <summary>Compares option and raw value for equality.</summary>
    public static bool operator ==(T lhs, Option<T> rhs) => rhs.Equals(lhs);

    /// <summary>Compares option and raw value for inequality.</summary>
    public static bool operator !=(T lhs, Option<T> rhs) => !rhs.Equals(lhs);

    /// <summary>
    /// Operator that works like ?? for nullable types but does not short circuit.
    /// </summary>
    public static Option<T> operator |(Option<T> value1, Option<T> value2)
    {
      return !value1.HasValue ? value2 : value1;
    }

    /// <summary>
    /// Compares the option to another option for equality based on their values.
    /// </summary>
    public bool Equals(Option<T> other)
    {
      return EqualityComparer<T>.Default.Equals(this.Value, other.Value);
    }

    /// <summary>
    /// Compares the option to another option for equality based on their values.
    /// </summary>
    public bool Equals(T other) => EqualityComparer<T>.Default.Equals(this.Value, other);

    /// <summary>
    /// Compares the option to another object for equality based on their values.
    /// </summary>
    public override bool Equals(object other)
    {
      switch (other)
      {
        case null:
          return (object) this.Value == null;
        case Option<T> other1:
          return this.Equals(other1);
        case T other2:
          return this.Equals(other2);
        default:
          return false;
      }
    }

    public override int GetHashCode() => !this.IsNone ? this.Value.GetHashCode() : 0;

    public override string ToString()
    {
      return !this.IsNone ? "Some: " + this.Value?.ToString() : "None: " + typeof (T).Name;
    }

    public static void Serialize(Option<T> value, BlobWriter writer)
    {
      writer.WriteBool(value.HasValue);
      if (!value.HasValue)
        return;
      writer.WriteGeneric<T>(value.ValueOrNull);
    }

    public static Option<T> Deserialize(BlobReader reader)
    {
      return !reader.ReadBool() ? Option<T>.None : new Option<T>(reader.ReadGenericAs<T>());
    }

    static Option() => MBiHIp97M4MqqbtZOh.RFLpSOptx();
  }
}
