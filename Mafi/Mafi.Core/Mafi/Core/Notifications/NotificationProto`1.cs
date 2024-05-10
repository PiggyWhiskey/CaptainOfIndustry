// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationProto`1
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
  public abstract class NotificationProto<TParam> : NotificationProto
  {
    private readonly Option<NotificationProto<TParam>.NotificationMessageFormatter> m_messageFormatter;
    private readonly bool m_hasEntityInText;

    public NotificationProto(
      NotificationProto<TParam>.ID id,
      Proto.Str strings,
      Option<NotificationProto<TParam>.NotificationMessageFormatter> messageFormatter,
      NotificationType type,
      NotificationStyle style,
      Option<string> iconAssetPath,
      bool hideInNotificationPanel,
      bool hideInspector,
      IconSpec? entityIconSpec,
      Duration timeToLive = default (Duration))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((NotificationProto.ID) id, strings, type, style, iconAssetPath, hideInNotificationPanel, hideInspector, entityIconSpec, timeToLive);
      this.m_messageFormatter = messageFormatter;
      this.m_hasEntityInText = this.Strings.Name.TranslatedString.Contains("{entity}");
      if (!this.m_messageFormatter.IsNone)
        return;
      try
      {
        strings.Name.TranslatedString.FormatInvariant((object) "0");
      }
      catch (FormatException ex)
      {
        Mafi.Log.Error("Notification string '" + strings.Name.TranslatedString + "' is invalid, it has to contain at formatting string in form: '{0}'.");
      }
    }

    public LocStrFormatted GetMessage(TParam formattingParam, Option<IEntity> entity)
    {
      try
      {
        LocStrFormatted locStrFormatted = this.Strings.Name.AsFormatted;
        if (this.m_hasEntityInText && entity.HasValue)
          locStrFormatted = new LocStrFormatted(locStrFormatted.Value.Replace("{entity}", entity.Value.GetTitle()));
        LocStrFormatted message;
        ref LocStrFormatted local = ref message;
        string str;
        if (!this.m_messageFormatter.HasValue)
          str = locStrFormatted.Value.FormatInvariant((object) formattingParam);
        else
          str = this.m_messageFormatter.Value(locStrFormatted.Value, formattingParam);
        local = new LocStrFormatted(str);
        return message;
      }
      catch (FormatException ex)
      {
        Mafi.Log.Exception((Exception) ex, string.Format("Formatting message of notification '{0}' with parameter '{1}' ", (object) this, (object) formattingParam) + "in culture " + LocalizationManager.CurrentCultureInfo.TwoLetterISOLanguageName + " threw exception.");
        return this.Strings.Name.AsFormatted;
      }
    }

    public override LocStrFormatted GetMessage(Option<object> param, Option<IEntity> entity)
    {
      return param.ValueOrNull is TParam valueOrNull ? this.GetMessage(valueOrNull, entity) : base.GetMessage(param, entity);
    }

    public NotificationProto<TParam>.ID Id => new NotificationProto<TParam>.ID(base.Id.Value);

    [DebuggerStepThrough]
    [ManuallyWrittenSerialization]
    [DebuggerDisplay("{Value,nq}")]
    public new readonly struct ID : 
      IEquatable<NotificationProto<TParam>.ID>,
      IComparable<NotificationProto<TParam>.ID>
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
      public static implicit operator NotificationProto.ID(NotificationProto<TParam>.ID id)
      {
        return new NotificationProto.ID(id.Value);
      }

      public static bool operator ==(NotificationProto.ID lhs, NotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(NotificationProto<TParam>.ID lhs, NotificationProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(NotificationProto.ID lhs, NotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(NotificationProto<TParam>.ID lhs, NotificationProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(NotificationProto<TParam>.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, NotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(NotificationProto<TParam>.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, NotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(NotificationProto<TParam>.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(
        NotificationProto<TParam>.ID lhs,
        NotificationProto<TParam>.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(
        NotificationProto<TParam>.ID lhs,
        NotificationProto<TParam>.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is NotificationProto<TParam>.ID other1 && this.Equals(other1);
      }

      public bool Equals(NotificationProto<TParam>.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(NotificationProto<TParam>.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(NotificationProto<TParam>.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static NotificationProto<TParam>.ID Deserialize(BlobReader reader)
      {
        return new NotificationProto<TParam>.ID(reader.ReadString());
      }
    }

    public delegate string NotificationMessageFormatter(
      string messageFormatString,
      TParam notificationParameter);
  }
}
