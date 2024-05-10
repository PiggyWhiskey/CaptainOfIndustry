// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.TimelapseCaptureManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.Utils;
using Mafi.Unity.Camera;
using Mafi.Unity.Utils;
using System;
using System.IO;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TimelapseCaptureManager
  {
    private readonly TimelapseManager m_timelapseManager;
    private readonly ScreenshotTaker m_screenshotTaker;
    private readonly CameraController m_cameraController;
    private readonly IFileSystemHelper m_fileSystemHelper;

    public TimelapseCaptureManager(
      TimelapseManager timelapseManager,
      ScreenshotTaker screenshotTaker,
      CameraController cameraController,
      IFileSystemHelper fileSystemHelper)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_timelapseManager = timelapseManager;
      this.m_screenshotTaker = screenshotTaker;
      this.m_cameraController = cameraController;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_timelapseManager.OnTimelapseCapture += new Action<TimelapseData>(this.onTimelapseCapture);
    }

    [ConsoleCommand(false, true, null, null)]
    private string timelapseList()
    {
      string str = this.m_timelapseManager.Data.Values.Select<TimelapseData, string>((Func<TimelapseData, string>) (x => string.Format("{0}: {1} images captured, ", (object) x.Name, (object) x.CapturedCount) + string.Format("capturing {0}x{1} every {2} sec, {3}saving as {4}", (object) x.Size.X, (object) x.Size.Y, (object) x.CaptureInterval.Seconds.ToIntRounded(), x.SuperSize ? (object) "super-sized, " : (object) "", x.SaveAsJpg ? (object) "JPG" : (object) "PNG"))).JoinStrings("\n");
      return this.m_timelapseManager.Data.Count <= 0 ? "No timelapses" : string.Format("Found {0} timelapse(s):\n{1}", (object) this.m_timelapseManager.Data.Count, (object) str);
    }

    [ConsoleCommand(false, true, "Starts capturing screenshots at regular intervals based on game-time. Captured images are saved to a separate directory in Screenshots folder. Timelapses are saved, after loading the capturing will continue.", null)]
    private GameCommandResult timelapseStartAtCurrentCameraPose(
      string name,
      int intervalInGameSeconds,
      int width = 1920,
      int height = 1080,
      bool superSize = false,
      bool saveAsJpg = false)
    {
      return GameCommandResult.Error("Timelapse feature is currently not available due to rendering optimizations. Please check on it later.");
    }

    [ConsoleCommand(false, true, null, null)]
    private GameCommandResult timelapseStop(string name)
    {
      return !this.m_timelapseManager.TryRemoveTimelapse(name) ? GameCommandResult.Error("Failed to remove timelapse '" + name + "'. Existing timelapses: " + this.m_timelapseManager.Data.Keys.JoinStrings(",")) : GameCommandResult.Success((object) ("Timelapse '" + name + "' removed."));
    }

    private void onTimelapseCapture(TimelapseData data)
    {
      string filePath = this.m_fileSystemHelper.GetFilePath(data.CapturedCount.ToString("00000"), FileType.Screenshot, true, "Timelapse_" + data.Name);
      if (data.CapturedCount != 0)
        return;
      string path = filePath + (data.SaveAsJpg ? ".jpg" : ".png");
      if (!File.Exists(path))
        return;
      data.NotifyCaptured((Option<string>) ("Some timelapse files already exists at '" + path + "'."));
    }
  }
}
