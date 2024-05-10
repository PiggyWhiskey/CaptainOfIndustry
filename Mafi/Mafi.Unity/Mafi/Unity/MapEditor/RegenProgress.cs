// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.RegenProgress
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public readonly struct RegenProgress
  {
    public readonly string Message;
    public readonly Percent Progress;
    public readonly Option<string> ErrorTip;

    public RegenProgress(string message, Percent progress = default (Percent), Option<string> errorTip = default (Option<string>))
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Message = message;
      this.Progress = progress;
      this.ErrorTip = errorTip;
    }

    public bool IsError => this.ErrorTip.HasValue;
  }
}
