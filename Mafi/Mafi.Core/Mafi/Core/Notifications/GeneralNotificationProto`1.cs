// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.GeneralNotificationProto`1
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
  public class GeneralNotificationProto<TParam> : NotificationProto<TParam>
  {
    public GeneralNotificationProto(
      GeneralNotificationProto<TParam>.ID id,
      Proto.Str strings,
      Option<NotificationProto<TParam>.NotificationMessageFormatter> messageFormatter,
      NotificationType type,
      NotificationStyle style,
      Option<string> iconAssetPath,
      bool hideInNotificationPanel = false,
      bool hideInInspector = false,
      Duration timeToLive = default (Duration))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((NotificationProto<TParam>.ID) id, strings, messageFormatter, type, style, iconAssetPath, hideInNotificationPanel, hideInInspector, new IconSpec?(), timeToLive);
      if (!strings.Name.TranslatedString.Contains("{entity}"))
        return;
      Mafi.Log.Error(string.Format("Cannot use {{entity}} tokens in general notifications, notification '{0}', ", (object) id) + "string id '" + strings.Name.Id + "': " + strings.Name.TranslatedString);
    }

    public GeneralNotificationProto<TParam>.ID Id
    {
      get => new GeneralNotificationProto<TParam>.ID(base.Id.Value);
    }

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : 
      IEquatable<GeneralNotificationProto<TParam>.ID>,
      IComparable<GeneralNotificationProto<TParam>.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      public static implicit operator NotificationProto<TParam>.ID(
        GeneralNotificationProto<TParam>.ID id)
      {
        return new NotificationProto<TParam>.ID(id.Value);
      }

      public static bool operator ==(
        NotificationProto<TParam>.ID lhs,
        GeneralNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        GeneralNotificationProto<TParam>.ID lhs,
        NotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        NotificationProto<TParam>.ID lhs,
        GeneralNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        GeneralNotificationProto<TParam>.ID lhs,
        NotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Notifications.NotificationProto.ID" />.
      /// </summary>
      public static implicit operator NotificationProto.ID(GeneralNotificationProto<TParam>.ID id)
      {
        return new NotificationProto.ID(id.Value);
      }

      public static bool operator ==(
        NotificationProto.ID lhs,
        GeneralNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        GeneralNotificationProto<TParam>.ID lhs,
        NotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        NotificationProto.ID lhs,
        GeneralNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        GeneralNotificationProto<TParam>.ID lhs,
        NotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(GeneralNotificationProto<TParam>.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, GeneralNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(GeneralNotificationProto<TParam>.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, GeneralNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(GeneralNotificationProto<TParam>.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        GeneralNotificationProto<TParam>.ID lhs,
        GeneralNotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        GeneralNotificationProto<TParam>.ID lhs,
        GeneralNotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is GeneralNotificationProto<TParam>.ID other1 && this.Equals(other1);
      }

      public bool Equals(GeneralNotificationProto<TParam>.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(GeneralNotificationProto<TParam>.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(GeneralNotificationProto<TParam>.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static GeneralNotificationProto<TParam>.ID Deserialize(BlobReader reader)
      {
        return new GeneralNotificationProto<TParam>.ID(reader.ReadString());
      }
    }
  }
}
