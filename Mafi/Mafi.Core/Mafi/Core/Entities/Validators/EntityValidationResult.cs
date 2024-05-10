// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Validators.EntityValidationResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Entities.Validators
{
  public readonly struct EntityValidationResult
  {
    public static readonly EntityValidationResult Success;
    /// <summary>Whether entity addition/removal was successful.</summary>
    public readonly EntityValidationResultStatus ValidationStatus;
    /// <summary>
    /// Error message if <see cref="P:Mafi.Core.Entities.Validators.EntityValidationResult.IsSuccess" /> == false, empty string otherwise.
    /// This message is shown to the player. For debug use <see cref="F:Mafi.Core.Entities.Validators.EntityValidationResult.ErrorMessage" />
    /// </summary>
    public readonly string ErrorMessageForPlayer;
    /// <summary>
    /// Error message if <see cref="P:Mafi.Core.Entities.Validators.EntityValidationResult.IsSuccess" /> == false, empty string otherwise.
    /// This message is used in logs.
    /// </summary>
    public readonly string ErrorMessage;

    public bool IsSuccess => this.ValidationStatus == EntityValidationResultStatus.Valid;

    public bool IsError => !this.IsSuccess;

    private EntityValidationResult(
      EntityValidationResultStatus status,
      string playerMessage,
      string debugMessage)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ValidationStatus = status;
      this.ErrorMessageForPlayer = playerMessage;
      this.ErrorMessage = debugMessage;
    }

    public static EntityValidationResult CreateError(
      LocStrFormatted playerMessage,
      string debugMessage = null)
    {
      return new EntityValidationResult(EntityValidationResultStatus.Error, playerMessage.Value, debugMessage ?? playerMessage.Value);
    }

    public static EntityValidationResult CreateError(string playerMessage, string debugMessage = null)
    {
      return new EntityValidationResult(EntityValidationResultStatus.Error, playerMessage, debugMessage ?? playerMessage);
    }

    public static EntityValidationResult CreateErrorFatal(
      LocStrFormatted playerMessage,
      string debugMessage = null)
    {
      return new EntityValidationResult(EntityValidationResultStatus.FatalError, playerMessage.Value, debugMessage ?? playerMessage.Value);
    }

    static EntityValidationResult()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityValidationResult.Success = new EntityValidationResult(EntityValidationResultStatus.Valid, string.Empty, string.Empty);
    }
  }
}
