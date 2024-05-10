// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.NotificationsUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  /// <summary>Notifications panel styles.</summary>
  public class NotificationsUiStyle : BaseUiStyle
  {
    public NotificationsUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    /// <summary>
    /// Right offset from the screen of the whole notification panel.
    /// </summary>
    public virtual float RightOffset => 16f;

    /// <summary>
    /// Buttons that is displayed when there are NOT any notifications.
    /// </summary>
    public virtual BtnStyle MainButtonNoNotifications
    {
      get => new BtnStyle(new TextStyle?(new TextStyle((ColorRgba) 10066329)));
    }

    /// <summary>
    /// Buttons that is displayed when there are notifications and are visible.
    /// </summary>
    public virtual BtnStyle MainButtonActive
    {
      get => new BtnStyle(new TextStyle?(new TextStyle((ColorRgba) 14386209)));
    }

    /// <summary>
    /// Button that is displayed when there are notifications but are hidden (minimized).
    /// </summary>
    public virtual BtnStyle MainButtonCollapsed
    {
      get => new BtnStyle(new TextStyle?(new TextStyle((ColorRgba) 16619815)));
    }

    /// <summary>
    /// Buttons that is displayed when there are notifications and are visible.
    /// </summary>
    public virtual BtnStyle MutedButton
    {
      get => new BtnStyle(new TextStyle?(new TextStyle((ColorRgba) 16777215)));
    }

    /// <summary>
    /// Button that is displayed when there are notifications but are hidden (minimized).
    /// </summary>
    public virtual BtnStyle UnmutedButton
    {
      get => new BtnStyle(new TextStyle?(new TextStyle((ColorRgba) 10066329)));
    }

    /// <summary>
    /// Button that is displayed when there is zero notifications.
    /// </summary>
    public virtual TextStyle NotificationsCountTextStyle
    {
      get => this.Global.Title.Extend(new ColorRgba?((ColorRgba) 2565927));
    }

    /// <summary>
    /// Text style of message that informs the player that there are no new notifications.
    /// </summary>
    public virtual TextStyle NoItemsInfoTextStyle
    {
      get => this.Global.Text.Extend(new ColorRgba?((ColorRgba) 15724527));
    }

    /// <summary>
    /// Dimensions of a single notification box. (Righ offset must be included to the width).
    /// </summary>
    public virtual float ItemHeight => 32f;

    /// <summary>Notification box backgroung.</summary>
    public virtual ColorRgba ItemBackground => new ColorRgba(2105376);

    /// ===============================================================
    ///             * Style for
    ///             <see cref="F:Mafi.Core.Notifications.NotificationStyle.Success" />
    /// 
    ///             ===============================================================
    public virtual IconStyle SuccessIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?((ColorRgba) 6998565));
      }
    }

    public virtual TextStyle SuccessTextStyle
    {
      get
      {
        return new TextStyle()
        {
          FontSize = 14,
          FontStyle = FontStyle.Bold,
          Color = (ColorRgba) 6998565
        };
      }
    }

    /// ===============================================================
    ///             * Style for
    ///             <see cref="F:Mafi.Core.Notifications.NotificationStyle.Warning" />
    /// 
    ///             ===============================================================
    public virtual IconStyle WarningIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?((ColorRgba) 16619815));
      }
    }

    public virtual TextStyle WarningTextStyle
    {
      get
      {
        return new TextStyle()
        {
          FontSize = 14,
          FontStyle = FontStyle.Bold,
          Color = (ColorRgba) 16619815
        };
      }
    }

    /// ===============================================================
    ///             * Style for
    ///             <see cref="F:Mafi.Core.Notifications.NotificationStyle.Critical" />
    /// 
    ///             ===============================================================
    public virtual IconStyle CriticalIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?((ColorRgba) 16528955));
      }
    }

    public virtual TextStyle CriticalTextStyle
    {
      get
      {
        return new TextStyle()
        {
          FontSize = 14,
          FontStyle = FontStyle.Bold,
          Color = (ColorRgba) 16528955
        };
      }
    }
  }
}
