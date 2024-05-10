// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Gfx;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Notifications
{
  [DebuggerDisplay("Notification: {Id}")]
  public abstract class NotificationProto : Proto
  {
    public readonly bool HideInNotificationPanel;
    public readonly bool HideInInspector;
    /// <summary>Determines only its look in the UI.</summary>
    public readonly NotificationStyle Style;
    /// <summary>
    /// Determines the way how the notification is reported and removed. Read more at <see cref="T:Mafi.Core.Notifications.NotificationType" />.
    /// </summary>
    public readonly NotificationType Type;
    /// <summary>
    /// Icon asset path to be used next to the alert in the UI. Leave None to use a generic one.
    /// </summary>
    public readonly Option<string> IconAssetPath;
    /// <summary>Optional icon that will be shown above the entity.</summary>
    public readonly IconSpec? EntityIconSpec;
    /// <summary>
    /// Whether we hide the icon when the notification is suppressed
    /// </summary>
    public readonly bool SuppressEntityIconOnSuppress;
    /// <summary>
    /// Duration until which the notification will be removed if not done by the player (0 is undefined). This is
    /// mandatory if you use <see cref="F:Mafi.Core.Notifications.NotificationType.OneTimeOnly" />. For instance we don't want to have
    /// notification like "New research unlocked" hanging there for 15 minutes. Don't use this for critical alerts.
    /// They should be removed immediately once the thing is fixed.
    /// </summary>
    public readonly Duration TimeToLive;
    private readonly bool m_hasEntityInText;

    public bool IsTimeLimited => this.TimeToLive.Ticks > 0;

    public NotificationProto(
      NotificationProto.ID id,
      Proto.Str strings,
      NotificationType type,
      NotificationStyle style,
      Option<string> iconAssetPath,
      bool hideInNotificationPanel = false,
      bool hideInInspector = false,
      IconSpec? entityIconSpec = null,
      Duration timeToLive = default (Duration),
      bool suppressIconOnSuppress = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings);
      this.Type = type;
      this.Style = style;
      this.IconAssetPath = iconAssetPath;
      this.EntityIconSpec = entityIconSpec;
      this.TimeToLive = timeToLive.CheckNotNegative();
      this.SuppressEntityIconOnSuppress = suppressIconOnSuppress;
      this.HideInNotificationPanel = hideInNotificationPanel;
      this.HideInInspector = hideInInspector;
      if (this.HideInNotificationPanel)
        Mafi.Assert.That<Option<string>>(this.IconAssetPath).HasValue<string>();
      this.m_hasEntityInText = this.Strings.Name.TranslatedString.Contains("{entity}");
      if (this.Type == NotificationType.OneTimeOnly && this.TimeToLive.Ticks <= 0)
        throw new ProtoBuilderException(string.Format("One time only notification '{0}' needs to have TTL > 0!", (object) id));
    }

    public virtual LocStrFormatted GetMessage(Option<object> param, Option<IEntity> entity)
    {
      return this.m_hasEntityInText && entity.HasValue ? new LocStrFormatted(this.Strings.Name.TranslatedString.Replace("{entity}", entity.Value.GetTitle())) : this.Strings.Name.AsFormatted;
    }

    public NotificationProto.ID Id => new NotificationProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : 
      IEquatable<NotificationProto.ID>,
      IComparable<NotificationProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(NotificationProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, NotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(NotificationProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, NotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(NotificationProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(NotificationProto.ID lhs, NotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(NotificationProto.ID lhs, NotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is NotificationProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(NotificationProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(NotificationProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(NotificationProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static NotificationProto.ID Deserialize(BlobReader reader)
      {
        return new NotificationProto.ID(reader.ReadString());
      }
    }
  }
}
