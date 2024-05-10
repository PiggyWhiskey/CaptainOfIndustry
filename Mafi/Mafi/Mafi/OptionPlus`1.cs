// Decompiled with JetBrains decompiler
// Type: Mafi.OptionPlus`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// OptionPlus is an option that can store both classes and structs. This should be used only in generic classes that
  /// have to store its parameter in some kind of Option type.
  /// </summary>
  [DebuggerDisplay("HasValue = {HasValue}, Value = {ValueOrDefault}")]
  [DebuggerStepThrough]
  public readonly struct OptionPlus<T> : IEquatable<OptionPlus<T>>, IEquatable<T>
  {
    /// <summary>The Option indicating there is no value.</summary>
    public static readonly OptionPlus<T> None;
    /// <summary>
    /// Value of the option. Exception is raised when accessing an option that has no value so you better behave!
    /// </summary>
    /// <remarks>The access is checked in debug mode.</remarks>
    public readonly T Value;
    public readonly bool HasValue;

    /// <summary>
    /// Creates an <see cref="T:Mafi.OptionPlus`1" /> from given value. The value may be null.
    /// </summary>
    public static OptionPlus<T> Create(T value) => new OptionPlus<T>(value);

    /// <summary>
    /// Creates an <see cref="T:Mafi.OptionPlus`1" /> from given value. The value is expected to be not null (yes, it will
    /// throw exception in your face if it is not).
    /// </summary>
    public static OptionPlus<T> Some(T value)
    {
      return (object) value != null ? new OptionPlus<T>(value) : throw new ArgumentNullException(nameof (value), "Option<T>.Some called with value null.");
    }

    /// <summary>
    /// Private constructor to enforce usage of <see cref="M:Mafi.OptionPlus.Some``1(``0)" /> and <see cref="F:Mafi.OptionPlus.None" />.
    /// Default-construction option struct results in <see cref="F:Mafi.OptionPlus.None" /> value.
    /// </summary>
    private OptionPlus(T value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Value = value;
      this.HasValue = true;
    }

    /// <summary>
    /// Bool indicating whether the option has no value (opposite to <see cref="F:Mafi.OptionPlus`1.HasValue" />).
    /// </summary>
    public bool IsNone => !this.HasValue;

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
    public T ValueOrDefault => !this.HasValue ? default (T) : this.Value;

    /// <summary>
    /// Returns value of option casted as <typeparamref name="TResult" />. Returns casted value or <see cref="F:Mafi.OptionPlus.None" /> if this option has no value or the cast failed.
    /// </summary>
    public OptionPlus<TResult> As<TResult>() where TResult : class
    {
      return new OptionPlus<TResult>((object) this.Value as TResult);
    }

    /// <summary>
    /// Implicitly converts an <see cref="T:Mafi.OptionPlus" /> to an <see cref="T:Mafi.OptionPlus`1" />.
    /// </summary>
    public static implicit operator OptionPlus<T>(OptionPlus option) => OptionPlus<T>.None;

    /// <summary>
    /// Implicitly converts given value to an <see cref="T:Mafi.OptionPlus`1" />. Due to C# spec limitations the implicit
    /// cast does not work for interfaces, use <see cref="M:Mafi.OptionPlus.Create``1(``0)" /> or <see cref="M:Mafi.OptionPlus.Some``1(``0)" /> instead.
    /// </summary>
    public static implicit operator OptionPlus<T>(T value) => new OptionPlus<T>(value);

    /// <summary>
    /// Compares two options for equality based on their values.
    /// </summary>
    public static bool operator ==(OptionPlus<T> lhs, OptionPlus<T> rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Compares two options for inequality based on their values.
    /// </summary>
    public static bool operator !=(OptionPlus<T> lhs, OptionPlus<T> rhs) => !lhs.Equals(rhs);

    /// <summary>Compares option and raw value for equality.</summary>
    public static bool operator ==(OptionPlus<T> lhs, T rhs) => lhs.Equals(rhs);

    /// <summary>Compares option and raw value for inequality.</summary>
    public static bool operator !=(OptionPlus<T> lhs, T rhs) => !lhs.Equals(rhs);

    /// <summary>Compares option and raw value for equality.</summary>
    public static bool operator ==(T lhs, OptionPlus<T> rhs) => rhs.Equals(lhs);

    /// <summary>Compares option and raw value for inequality.</summary>
    public static bool operator !=(T lhs, OptionPlus<T> rhs) => !rhs.Equals(lhs);

    /// <summary>
    /// Compares the option to another option for equality based on their values.
    /// </summary>
    public bool Equals(OptionPlus<T> other)
    {
      if (this.IsNone && other.IsNone)
        return true;
      return this.HasValue && other.HasValue && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
    }

    /// <summary>
    /// Compares the option to another option for equality based on their values.
    /// </summary>
    public bool Equals(T other)
    {
      return !this.IsNone && EqualityComparer<T>.Default.Equals(this.Value, other);
    }

    /// <summary>
    /// Compares the option to another object for equality based on their values.
    /// </summary>
    public override bool Equals(object other)
    {
      return other is OptionPlus<T> other1 && this.Equals(other1);
    }

    public override int GetHashCode()
    {
      if (this.IsNone)
        return 0;
      return (object) this.Value != null ? this.Value.GetHashCode() : 1;
    }

    static OptionPlus() => MBiHIp97M4MqqbtZOh.RFLpSOptx();
  }
}
