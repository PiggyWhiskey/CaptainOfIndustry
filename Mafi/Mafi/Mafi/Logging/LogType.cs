// Decompiled with JetBrains decompiler
// Type: Mafi.Logging.LogType
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Logging
{
  /// <summary>
  /// Log type flags. The lower the number the more severe (approximately).
  /// </summary>
  [Flags]
  public enum LogType
  {
    Exception = 2,
    Error = 4,
    Assert = 8,
    Warning = 16, // 0x00000010
    Info = 32, // 0x00000020
    GameProgress = 64, // 0x00000040
    Debug = 128, // 0x00000080
    All = 2147483647, // 0x7FFFFFFF
  }
}
