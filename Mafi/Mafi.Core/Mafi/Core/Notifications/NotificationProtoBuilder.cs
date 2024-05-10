// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Notifications.NotificationProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Gfx;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable enable
namespace Mafi.Core.Notifications
{
  public sealed class NotificationProtoBuilder : IProtoBuilder
  {
    public 
    #nullable disable
    ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public NotificationProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public NotificationProtoBuilder.StateGeneral Start(
      string name,
      GeneralNotificationProto.ID protoId,
      string translationComment = "notification")
    {
      return new NotificationProtoBuilder.StateGeneral(this, (NotificationProto.ID) protoId, name, translationComment);
    }

    public NotificationProtoBuilder.StateEntity Start(
      string name,
      EntityNotificationProto.ID protoId,
      string translationComment = "notification")
    {
      return new NotificationProtoBuilder.StateEntity(this, (NotificationProto.ID) protoId, name, translationComment);
    }

    public NotificationProtoBuilder.StateFormattedGeneral<T> StartFormatted<T>(
      string name,
      GeneralNotificationProto<T>.ID protoId,
      string translationComment = "notification, any strings in curly braces { } must be preserved and non-translated")
    {
      return new NotificationProtoBuilder.StateFormattedGeneral<T>(this, protoId, name, translationComment);
    }

    public NotificationProtoBuilder.StateFormattedEntity<T> StartFormatted<T>(
      string name,
      EntityNotificationProto<T>.ID protoId,
      string translationComment = "notification, any strings in curly braces { } must be preserved and non-translated")
    {
      return new NotificationProtoBuilder.StateFormattedEntity<T>(this, protoId, name, translationComment);
    }

    public class StateBaseGeneral<TState> : ProtoBuilderState<TState> where TState : NotificationProtoBuilder.StateBaseGeneral<TState>
    {
      protected static readonly Duration DEFAULT_TIME_TO_LIVE;
      protected Duration TimeToLive;
      protected NotificationType? Type;
      protected NotificationStyle? Style;
      protected Option<string> IconAssetPath;
      protected bool HideInNotificationUiPanel;
      protected bool ShouldHideInInspector;
      protected bool SuppressEntityIconOnSuppress;

      protected StateBaseGeneral(
        NotificationProtoBuilder builder,
        NotificationProto.ID protoId,
        string name,
        string translationComment = "notification")
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.TimeToLive = Duration.Zero;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, (Proto.ID) protoId, name, translationComment);
      }

      public TState SetStyle(NotificationStyle style)
      {
        this.Style = new NotificationStyle?(style);
        return (TState) this;
      }

      public TState SetType(NotificationType type)
      {
        this.Type = new NotificationType?(type);
        return (TState) this;
      }

      public TState SetTimeToLive(Duration timeToLive)
      {
        Assert.That<Duration>(timeToLive).IsNotNegative();
        this.TimeToLive = timeToLive;
        return (TState) this;
      }

      public TState SetSuppressEntityIconOnSuppress()
      {
        this.SuppressEntityIconOnSuppress = true;
        return (TState) this;
      }

      public TState AddIcon(string assetPath)
      {
        this.IconAssetPath = (Option<string>) assetPath.CheckNotNull<string>();
        return (TState) this;
      }

      public TState HideInNotificationPanel()
      {
        this.HideInNotificationUiPanel = true;
        return (TState) this;
      }

      public TState HideInInspector()
      {
        this.ShouldHideInInspector = true;
        return (TState) this;
      }

      static StateBaseGeneral()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        NotificationProtoBuilder.StateBaseGeneral<TState>.DEFAULT_TIME_TO_LIVE = 2.Minutes();
      }
    }

    public class StateBaseEntity<TState> : NotificationProtoBuilder.StateBaseGeneral<TState> where TState : NotificationProtoBuilder.StateBaseEntity<TState>
    {
      private 
      #nullable enable
      string? m_iconPath;
      private ColorRgba? m_iconColor;
      protected Proto.ID? Tutorial;

      protected IconSpec? EntityIconSpec
      {
        get
        {
          if (this.m_iconPath == null)
            return new IconSpec?();
          if (!this.m_iconColor.HasValue)
          {
            NotificationStyle? style1 = this.Style;
            NotificationStyle notificationStyle1 = NotificationStyle.Critical;
            if (style1.GetValueOrDefault() == notificationStyle1 & style1.HasValue)
            {
              this.m_iconColor = new ColorRgba?(ColorRgba.Red);
            }
            else
            {
              NotificationStyle? style2 = this.Style;
              NotificationStyle notificationStyle2 = NotificationStyle.Success;
              this.m_iconColor = !(style2.GetValueOrDefault() == notificationStyle2 & style2.HasValue) ? new ColorRgba?(ColorRgba.Orange) : new ColorRgba?(ColorRgba.Green);
            }
          }
          return new IconSpec?(new IconSpec(this.m_iconPath, this.m_iconColor.Value));
        }
      }

      protected StateBaseEntity(
        #nullable disable
        NotificationProtoBuilder builder,
        NotificationProto.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, protoId, name, translationComment);
      }

      public TState AddEntityIcon(string assetPath, ColorRgba? color = null)
      {
        this.m_iconPath = assetPath;
        this.m_iconColor = color;
        return (TState) this;
      }

      public TState SetTutorial(Proto.ID tutorial)
      {
        this.Tutorial = new Proto.ID?(tutorial);
        return (TState) this;
      }
    }

    public class StateGeneral : 
      NotificationProtoBuilder.StateBaseGeneral<NotificationProtoBuilder.StateGeneral>
    {
      private readonly NotificationProto.ID m_protoId;

      public StateGeneral(
        NotificationProtoBuilder builder,
        NotificationProto.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, protoId, name, translationComment);
        this.m_protoId = protoId;
      }

      public NotificationProto BuildAndAdd()
      {
        if (this.TimeToLive.IsZero)
        {
          NotificationType? type = this.Type;
          NotificationType notificationType = NotificationType.OneTimeOnly;
          if (type.GetValueOrDefault() == notificationType & type.HasValue)
            this.TimeToLive = NotificationProtoBuilder.StateBaseGeneral<NotificationProtoBuilder.StateGeneral>.DEFAULT_TIME_TO_LIVE;
        }
        NotificationProto.ID protoId = this.m_protoId;
        Proto.Str strings = new Proto.Str(this.Name);
        int type1 = (int) this.ValueOrThrow<NotificationType>(this.Type, "Type");
        int style = (int) this.ValueOrThrow<NotificationStyle>(this.Style, "Style");
        bool notificationUiPanel = this.HideInNotificationUiPanel;
        bool shouldHideInInspector = this.ShouldHideInInspector;
        Option<string> iconAssetPath = this.IconAssetPath;
        int num1 = notificationUiPanel ? 1 : 0;
        int num2 = shouldHideInInspector ? 1 : 0;
        Duration timeToLive = this.TimeToLive;
        int num3 = this.SuppressEntityIconOnSuppress ? 1 : 0;
        return (NotificationProto) this.AddToDb<GeneralNotificationProto>(new GeneralNotificationProto(protoId, strings, (NotificationType) type1, (NotificationStyle) style, iconAssetPath, num1 != 0, num2 != 0, timeToLive, num3 != 0));
      }
    }

    public class StateEntity : 
      NotificationProtoBuilder.StateBaseEntity<NotificationProtoBuilder.StateEntity>
    {
      private readonly NotificationProto.ID m_protoId;
      private LocStrFormatted m_extraMessageForInspector;

      public StateEntity(
        NotificationProtoBuilder builder,
        NotificationProto.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_extraMessageForInspector = LocStrFormatted.Empty;
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, protoId, name, translationComment);
        this.m_protoId = protoId;
      }

      public NotificationProtoBuilder.StateEntity SetExtraMessageForInspector(
        LocStrFormatted message)
      {
        this.m_extraMessageForInspector = message;
        return this;
      }

      public NotificationProto BuildAndAdd()
      {
        if (this.TimeToLive.IsZero)
        {
          NotificationType? type = this.Type;
          NotificationType notificationType = NotificationType.OneTimeOnly;
          if (type.GetValueOrDefault() == notificationType & type.HasValue)
            this.TimeToLive = NotificationProtoBuilder.StateBaseGeneral<NotificationProtoBuilder.StateEntity>.DEFAULT_TIME_TO_LIVE;
        }
        NotificationProto.ID protoId = this.m_protoId;
        Proto.Str strings = new Proto.Str(this.Name);
        int type1 = (int) this.ValueOrThrow<NotificationType>(this.Type, "Type");
        int style = (int) this.ValueOrThrow<NotificationStyle>(this.Style, "Style");
        bool notificationUiPanel = this.HideInNotificationUiPanel;
        bool shouldHideInInspector = this.ShouldHideInInspector;
        Option<string> iconAssetPath = this.IconAssetPath;
        IconSpec entityIconSpec = this.ValueOrThrow<IconSpec>(this.EntityIconSpec, "EntityIconSpec");
        int num1 = notificationUiPanel ? 1 : 0;
        int num2 = shouldHideInInspector ? 1 : 0;
        LocStrFormatted messageForInspector = this.m_extraMessageForInspector;
        Duration timeToLive = this.TimeToLive;
        int num3 = this.SuppressEntityIconOnSuppress ? 1 : 0;
        Proto.ID? tutorial = this.Tutorial;
        return (NotificationProto) this.AddToDb<EntityNotificationProto>(new EntityNotificationProto(protoId, strings, (NotificationType) type1, (NotificationStyle) style, iconAssetPath, entityIconSpec, num1 != 0, num2 != 0, messageForInspector, timeToLive, num3 != 0, tutorial));
      }
    }

    public class StateFormattedGeneral<T> : 
      NotificationProtoBuilder.StateBaseGeneral<NotificationProtoBuilder.StateFormattedGeneral<T>>
    {
      private readonly GeneralNotificationProto<T>.ID m_protoId;
      private Option<NotificationProto<T>.NotificationMessageFormatter> m_messageFormatter;

      public StateFormattedGeneral(
        NotificationProtoBuilder builder,
        GeneralNotificationProto<T>.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_messageFormatter = (Option<NotificationProto<T>.NotificationMessageFormatter>) Option.None;
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, (NotificationProto.ID) protoId, name, translationComment);
        this.m_protoId = protoId;
      }

      public NotificationProtoBuilder.StateFormattedGeneral<T> SetMessageFormatter(
        NotificationProto<T>.NotificationMessageFormatter messageFormatter)
      {
        this.m_messageFormatter = (Option<NotificationProto<T>.NotificationMessageFormatter>) messageFormatter;
        return this;
      }

      public NotificationProto<T> BuildAndAdd()
      {
        if (this.TimeToLive.IsZero)
        {
          NotificationType? type = this.Type;
          NotificationType notificationType = NotificationType.OneTimeOnly;
          if (type.GetValueOrDefault() == notificationType & type.HasValue)
            this.TimeToLive = NotificationProtoBuilder.StateBaseGeneral<NotificationProtoBuilder.StateFormattedGeneral<T>>.DEFAULT_TIME_TO_LIVE;
        }
        GeneralNotificationProto<T>.ID protoId = this.m_protoId;
        Proto.Str strings = new Proto.Str(this.Name);
        Option<NotificationProto<T>.NotificationMessageFormatter> messageFormatter = this.m_messageFormatter;
        int type1 = (int) this.ValueOrThrow<NotificationType>(this.Type, "Type");
        int style = (int) this.ValueOrThrow<NotificationStyle>(this.Style, "Style");
        bool notificationUiPanel = this.HideInNotificationUiPanel;
        Option<string> iconAssetPath = this.IconAssetPath;
        int num = notificationUiPanel ? 1 : 0;
        Duration timeToLive = this.TimeToLive;
        return (NotificationProto<T>) this.AddToDb<GeneralNotificationProto<T>>(new GeneralNotificationProto<T>(protoId, strings, messageFormatter, (NotificationType) type1, (NotificationStyle) style, iconAssetPath, num != 0, timeToLive: timeToLive));
      }
    }

    public class StateFormattedEntity<T> : 
      NotificationProtoBuilder.StateBaseEntity<NotificationProtoBuilder.StateFormattedEntity<T>>
    {
      private readonly EntityNotificationProto<T>.ID m_protoId;
      private Option<NotificationProto<T>.NotificationMessageFormatter> m_messageFormatter;

      public StateFormattedEntity(
        NotificationProtoBuilder builder,
        EntityNotificationProto<T>.ID protoId,
        string name,
        string translationComment)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_messageFormatter = (Option<NotificationProto<T>.NotificationMessageFormatter>) Option.None;
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, (NotificationProto.ID) protoId, name, translationComment);
        this.m_protoId = protoId;
      }

      public NotificationProtoBuilder.StateFormattedEntity<T> SetMessageFormatter(
        NotificationProto<T>.NotificationMessageFormatter messageFormatter)
      {
        this.m_messageFormatter = (Option<NotificationProto<T>.NotificationMessageFormatter>) messageFormatter;
        return this;
      }

      public NotificationProto<T> BuildAndAdd()
      {
        if (this.TimeToLive.IsZero)
        {
          NotificationType? type = this.Type;
          NotificationType notificationType = NotificationType.OneTimeOnly;
          if (type.GetValueOrDefault() == notificationType & type.HasValue)
            this.TimeToLive = NotificationProtoBuilder.StateBaseGeneral<NotificationProtoBuilder.StateFormattedEntity<T>>.DEFAULT_TIME_TO_LIVE;
        }
        EntityNotificationProto<T>.ID protoId = this.m_protoId;
        Proto.Str strings = new Proto.Str(this.Name);
        Option<NotificationProto<T>.NotificationMessageFormatter> messageFormatter = this.m_messageFormatter;
        int type1 = (int) this.ValueOrThrow<NotificationType>(this.Type, "Type");
        int style = (int) this.ValueOrThrow<NotificationStyle>(this.Style, "Style");
        bool notificationUiPanel = this.HideInNotificationUiPanel;
        bool shouldHideInInspector = this.ShouldHideInInspector;
        Option<string> iconAssetPath = this.IconAssetPath;
        IconSpec entityIconSpec = this.ValueOrThrow<IconSpec>(this.EntityIconSpec, "EntityIconSpec");
        int num1 = notificationUiPanel ? 1 : 0;
        int num2 = shouldHideInInspector ? 1 : 0;
        Duration timeToLive = this.TimeToLive;
        return (NotificationProto<T>) this.AddToDb<EntityNotificationProto<T>>(new EntityNotificationProto<T>(protoId, strings, messageFormatter, (NotificationType) type1, (NotificationStyle) style, iconAssetPath, entityIconSpec, num1 != 0, num2 != 0, timeToLive));
      }
    }
  }
}
