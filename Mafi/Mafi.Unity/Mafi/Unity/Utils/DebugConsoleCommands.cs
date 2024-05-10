// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.DebugConsoleCommands
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Console;
using Mafi.Unity.Audio;
using System.Reflection;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class DebugConsoleCommands
  {
    private readonly BackgroundMusicManager m_backgroundMusicManager;

    public DebugConsoleCommands(BackgroundMusicManager backgroundMusicManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_backgroundMusicManager = backgroundMusicManager;
    }

    [ConsoleCommand(true, false, null, null)]
    private string printSystemInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PropertyInfo property in typeof (SystemInfo).GetProperties(BindingFlags.Static | BindingFlags.Public))
        stringBuilder.AppendLine(string.Format("{0}: {1}", (object) property.Name, property.GetValue((object) null)));
      return stringBuilder.ToString();
    }

    [ConsoleCommand(true, false, null, null)]
    private void nextSong() => this.m_backgroundMusicManager.PlayNextTrack();
  }
}
