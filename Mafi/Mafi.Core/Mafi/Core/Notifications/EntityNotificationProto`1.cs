// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.EntityNotificationProto`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Gfx;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Notifications
{
  public class EntityNotificationProto<TParam> : NotificationProto<TParam>
  {
    public EntityNotificationProto(
      EntityNotificationProto<TParam>.ID id,
      Proto.Str strings,
      Option<NotificationProto<TParam>.NotificationMessageFormatter> messageFormatter,
      NotificationType type,
      NotificationStyle style,
      Option<string> iconAssetPath,
      IconSpec entityIconSpec,
      bool hideInNotificationPanel = false,
      bool hideInInspector = false,
      Duration timeToLive = default (Duration))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((NotificationProto<TParam>.ID) id, strings, messageFormatter, type, style, iconAssetPath, hideInNotificationPanel, hideInInspector, new IconSpec?(entityIconSpec), timeToLive);
    }

    public EntityNotificationProto<TParam>.ID Id
    {
      get => new EntityNotificationProto<TParam>.ID(base.Id.Value);
    }

    [ManuallyWrittenSerialization]
    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    public new readonly struct ID : 
      IEquatable<EntityNotificationProto<TParam>.ID>,
      IComparable<EntityNotificationProto<TParam>.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      public static implicit operator NotificationProto<TParam>.ID(
        EntityNotificationProto<TParam>.ID id)
      {
        return new NotificationProto<TParam>.ID(id.Value);
      }

      public static bool operator ==(
        NotificationProto<TParam>.ID lhs,
        EntityNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        EntityNotificationProto<TParam>.ID lhs,
        NotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        NotificationProto<TParam>.ID lhs,
        EntityNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        EntityNotificationProto<TParam>.ID lhs,
        NotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Notifications.NotificationProto.ID" />.
      /// </summary>
      public static implicit operator NotificationProto.ID(EntityNotificationProto<TParam>.ID id)
      {
        return new NotificationProto.ID(id.Value);
      }

      public static bool operator ==(
        NotificationProto.ID lhs,
        EntityNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        EntityNotificationProto<TParam>.ID lhs,
        NotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        NotificationProto.ID lhs,
        EntityNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        EntityNotificationProto<TParam>.ID lhs,
        NotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(EntityNotificationProto<TParam>.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, EntityNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(EntityNotificationProto<TParam>.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, EntityNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityNotificationProto<TParam>.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        EntityNotificationProto<TParam>.ID lhs,
        EntityNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        EntityNotificationProto<TParam>.ID lhs,
        EntityNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is EntityNotificationProto<TParam>.ID other1 && this.Equals(other1);
      }

      public bool Equals(EntityNotificationProto<TParam>.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(EntityNotificationProto<TParam>.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(EntityNotificationProto<TParam>.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static EntityNotificationProto<TParam>.ID Deserialize(BlobReader reader)
      {
        return new EntityNotificationProto<TParam>.ID(reader.ReadString());
      }
    }
  }
}
