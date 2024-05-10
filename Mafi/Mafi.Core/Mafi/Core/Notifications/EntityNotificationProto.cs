// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.EntityNotificationProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Gfx;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Notifications
{
  public class EntityNotificationProto : NotificationProto
  {
    public LocStrFormatted ExtraMessageForInspector;
    public readonly Proto.ID? Tutorial;

    public EntityNotificationProto(
      NotificationProto.ID id,
      Proto.Str strings,
      NotificationType type,
      NotificationStyle style,
      Option<string> iconAssetPath,
      IconSpec entityIconSpec,
      bool hideInNotificationPanel,
      bool hideInInspector,
      LocStrFormatted extraMessageForInspector,
      Duration timeToLive = default (Duration),
      bool suppressIconOnSuppress = false,
      Proto.ID? tutorial = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, type, style, iconAssetPath, hideInNotificationPanel, hideInInspector, new IconSpec?(entityIconSpec), timeToLive, suppressIconOnSuppress);
      this.ExtraMessageForInspector = extraMessageForInspector;
      this.Tutorial = tutorial;
    }

    public EntityNotificationProto.ID Id => new EntityNotificationProto.ID(base.Id.Value);

    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    [DebuggerDisplay("{Value,nq}")]
    public new readonly struct ID : 
      IEquatable<EntityNotificationProto.ID>,
      IComparable<EntityNotificationProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Notifications.NotificationProto.ID" />.
      /// </summary>
      public static implicit operator NotificationProto.ID(EntityNotificationProto.ID id)
      {
        return new NotificationProto.ID(id.Value);
      }

      public static bool operator ==(NotificationProto.ID lhs, EntityNotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(EntityNotificationProto.ID lhs, NotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(NotificationProto.ID lhs, EntityNotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityNotificationProto.ID lhs, NotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(EntityNotificationProto.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, EntityNotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(EntityNotificationProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, EntityNotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityNotificationProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(EntityNotificationProto.ID lhs, EntityNotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityNotificationProto.ID lhs, EntityNotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is EntityNotificationProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(EntityNotificationProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(EntityNotificationProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(EntityNotificationProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static EntityNotificationProto.ID Deserialize(BlobReader reader)
      {
        return new EntityNotificationProto.ID(reader.ReadString());
      }
    }
  }
}
