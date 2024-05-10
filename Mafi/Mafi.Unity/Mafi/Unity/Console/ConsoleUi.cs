// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Console.ConsoleUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Logging;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Console
{
  /// <summary>
  /// UI of in-game console, capable of invoking game console commands. Control and view are not divided for this part
  /// of UI. Show console through '`', hide through 'Esc'.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ConsoleUi : IUnityUi, IUnityInputController
  {
    private Panel m_consolePanel;
    private ScrollableContainer m_historyScrollContainer;
    private StackContainer m_historyContainer;
    private TxtField m_inputField;
    private UiBuilder m_builder;
    private readonly Lyst<string> m_commandsHistory;
    private readonly Lyst<string> m_commandsBuffer;
    private string m_currentCommand;
    private int m_historyPositionFromEnd;
    private readonly ConcurrentQueue<GameConsole.ConsoleMessage> m_toWriteQueue;
    private readonly GameConsoleCommandsExecutor m_consoleCommandsExecutor;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IUnityInputMgr m_unityInputMgr;
    private readonly INotificationsManager m_notifManager;
    private readonly string m_historyFilePath;
    private bool m_isLoggingToConsole;
    private bool m_notifyNewError;
    private bool m_wasGameOverlayVisible;

    public ControllerConfig Config { get; }

    public ConsoleUi(
      GameConsole gameConsole,
      GameConsoleCommandsExecutor consoleCommandsExecutor,
      ShortcutsManager shortcutsManager,
      IGameLoopEvents gameLoopEvents,
      IUnityInputMgr unityInputMgr,
      IFileSystemHelper fileSystemHelper,
      INotificationsManager notifManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_commandsHistory = new Lyst<string>();
      this.m_commandsBuffer = new Lyst<string>();
      this.m_currentCommand = "";
      this.Config = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = true
      };
      this.m_toWriteQueue = new ConcurrentQueue<GameConsole.ConsoleMessage>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_consoleCommandsExecutor = consoleCommandsExecutor;
      this.m_shortcutsManager = shortcutsManager;
      this.m_unityInputMgr = unityInputMgr;
      this.m_notifManager = notifManager;
      this.m_historyFilePath = fileSystemHelper.GetFilePath("commands_history", FileType.Console);
      this.tryLoadRecentCommandsHistory(this.m_historyFilePath);
      gameLoopEvents.InputUpdate.AddNonSaveable<ConsoleUi>(this, new Action<GameTime>(this.inputUpdate));
      gameLoopEvents.SyncUpdate.AddNonSaveable<ConsoleUi>(this, new Action<GameTime>(this.syncUpdate));
      gameConsole.OnMessage += (Action<GameConsole.ConsoleMessage>) (m => this.m_toWriteQueue.Enqueue(m));
    }

    [ConsoleCommand(true, false, null, null)]
    private string alsoLogToConsole(bool enabled = true)
    {
      if (this.m_isLoggingToConsole == enabled)
        enabled = !enabled;
      this.m_isLoggingToConsole = enabled;
      if (enabled)
        Log.LogReceived += new Action<LogEntry>(this.logReceived);
      else
        Log.LogReceived -= new Action<LogEntry>(this.logReceived);
      return "Logging to console is now " + (enabled ? nameof (enabled) : "disabled");
    }

    private void logReceived(LogEntry logEntry)
    {
      ColorRgba color;
      switch (logEntry.Type)
      {
        case Mafi.Logging.LogType.Exception:
        case Mafi.Logging.LogType.Error:
        case Mafi.Logging.LogType.Assert:
          color = new ColorRgba(16733491);
          this.m_notifyNewError = true;
          break;
        case Mafi.Logging.LogType.Warning:
          color = ColorRgba.Yellow;
          break;
        case Mafi.Logging.LogType.GameProgress:
          return;
        default:
          color = ColorRgba.White;
          break;
      }
      this.m_toWriteQueue.Enqueue(new GameConsole.ConsoleMessage(logEntry.ToString(5), color));
    }

    private void tryLoadRecentCommandsHistory(string filePath)
    {
      if (!File.Exists(filePath))
        return;
      Lyst<string> lyst = new Lyst<string>();
      try
      {
        using (StreamReader streamReader = new StreamReader(filePath))
        {
          long num1 = streamReader.BaseStream.Length < 1024L ? streamReader.BaseStream.Length : 1024L;
          long num2 = streamReader.BaseStream.Seek(-num1, SeekOrigin.End);
          streamReader.DiscardBufferedData();
          if (num2 > 0L)
            streamReader.ReadLine();
          string str;
          while ((str = streamReader.ReadLine()) != null)
            lyst.Add(str);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Unable to read command line history from file '" + filePath + "'.");
      }
      Set<string> set = new Set<string>();
      for (int index = lyst.Count - 1; index >= 0; --index)
      {
        string str = lyst[index];
        if (set.Add(str))
          this.m_commandsHistory.Add(str);
      }
      this.m_commandsHistory.Reverse();
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_builder = builder;
      this.m_consolePanel = this.m_builder.NewPanel("ConsolePanel").SetBackground(ColorRgba.DarkDarkGray.SetA((byte) 240)).SetBorderStyle(new BorderStyle(ColorRgba.Black, 2f)).PutToTopOf<Panel>((IUiElement) this.m_builder.GameOverlay, this.m_builder.GameOverlay.GetHeight() / 2f);
      this.m_consolePanel.Hide<Panel>();
      this.m_inputField = this.m_builder.NewTxtField("ConsoleInput", (IUiElement) this.m_consolePanel).SetPlaceholderText("Enter command... (help for print of all commands)").SetOnValidateInput(new TMP_InputField.OnValidateInput(this.validateInputFieldInput));
      TextStyle textStyle = new TextStyle(ColorRgba.White, 16);
      this.m_inputField.SetStyle(new TxtFieldStyle(textStyle, textStyle));
      this.m_inputField.SetFont(this.m_builder.FontMonoSpace.ValueOrNull);
      this.m_historyScrollContainer = new ScrollableContainer(this.m_builder, "History scroll").AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.m_consolePanel, Offset.Bottom(40f));
      this.m_historyContainer = this.m_builder.NewStackContainer("History").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f);
      this.m_historyScrollContainer.AddItemTop((IUiElement) this.m_historyContainer);
      this.m_inputField.PutToBottomOf<TxtField>((IUiElement) this.m_consolePanel, 35f);
    }

    private char validateInputFieldInput(string text, int charIndex, char addedChar)
    {
      return addedChar == '`' || addedChar == '\t' ? char.MinValue : addedChar;
    }

    private void syncUpdate(GameTime time)
    {
      GameConsole.ConsoleMessage result;
      while (this.m_toWriteQueue.TryDequeue(out result))
      {
        this.WriteLine(result.Message, result.Color);
        if (this.m_notifyNewError)
        {
          if (!this.m_consolePanel.IsVisible())
          {
            int num = 24;
            string str = result.Message.Substring(result.Message.Length.Min(num), result.Message.Length.Min(num + 30));
            this.m_notifManager.NotifyOnce<string>(IdsCore.Notifications.NewErrorOccurred, str);
          }
          this.m_notifyNewError = false;
        }
      }
    }

    private void writeLineInternal(string text, ColorRgba color)
    {
      Txt element = this.m_builder.NewTxt("HistRecord").SetText(text).SetFont(this.m_builder.FontMonoSpace.ValueOrNull).SetFontSize(14).SetColor(color).AllowVerticalOverflow();
      float num = this.m_historyContainer.GetWidth() - 10f;
      float preferedHeight = element.GetPreferedHeight(num);
      this.m_historyContainer.Append((IUiElement) element, new Vector2?(new Vector2(num, preferedHeight)), new ContainerPosition?(ContainerPosition.MiddleOrCenter));
      if (this.m_historyContainer.ItemsCount > 100)
        this.m_historyContainer.RemoveAt(0);
      this.m_historyScrollContainer.ResetToBottom();
    }

    public void WriteLine(string text, ColorRgba color) => this.writeLineInternal(text, color);

    public void WriteLine(string text) => this.WriteLine(text, ColorRgba.White);

    public void Activate()
    {
      this.m_consolePanel.Show<Panel>();
      this.m_wasGameOverlayVisible = this.m_builder.GameOverlay.IsVisible();
      this.m_builder.GameOverlay.Show<Panel>();
      this.m_inputField.Focus();
    }

    public void Deactivate()
    {
      this.m_consolePanel.Hide<Panel>();
      if (this.m_wasGameOverlayVisible)
        return;
      this.m_builder.GameOverlay.Hide<Panel>();
    }

    private string formatSuggestionLine(Lyst<string> suggestedCommands, int from, int count)
    {
      StringBuilder stringBuilder = new StringBuilder();
      count = count.Min(suggestedCommands.Count - from);
      for (int index = 0; index < count; ++index)
      {
        string suggestedCommand = suggestedCommands[from + index];
        stringBuilder.Append(suggestedCommand);
        stringBuilder.Append(' ', suggestedCommand.Length < 40 ? 40 - suggestedCommand.Length : 1);
      }
      return stringBuilder.ToString();
    }

    private void printCommandSuggestions(Lyst<string> suggestedCommands)
    {
      this.m_commandsBuffer.Sort();
      this.writeLineInternal("", ColorRgba.LightYellow);
      for (int from = 0; from < this.m_commandsBuffer.Count; from += 7)
        this.writeLineInternal(this.formatSuggestionLine(this.m_commandsBuffer, from, 7), ColorRgba.LightYellow);
    }

    /// <summary>
    /// Input update callback registered always, activates console on '`' key.
    /// </summary>
    private void inputUpdate(GameTime time)
    {
      if (this.m_inputField == null)
        return;
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.ToggleConsole))
      {
        if (this.m_consolePanel.IsVisible())
          this.m_unityInputMgr.DeactivateController((IUnityInputController) this);
        else
          this.m_unityInputMgr.ActivateNewController((IUnityInputController) this);
      }
      if (!this.m_inputField.IsFocused())
        return;
      if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow) && this.m_historyPositionFromEnd < this.m_commandsHistory.Count)
      {
        if (this.m_historyPositionFromEnd == 0)
          this.m_currentCommand = this.m_inputField.GetText();
        ++this.m_historyPositionFromEnd;
        this.setTextFieldFromHistory();
      }
      if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow) && this.m_historyPositionFromEnd > 0)
      {
        --this.m_historyPositionFromEnd;
        this.setTextFieldFromHistory();
      }
      if (!UnityEngine.Input.GetKeyDown(KeyCode.Tab))
        return;
      string newCommandStr;
      string helpFor = this.m_consoleCommandsExecutor.Executor.GetHelpFor(this.m_inputField.GetText(), out newCommandStr);
      this.m_inputField.SetText(newCommandStr);
      this.m_inputField.MoveCaretToEnd();
      if (string.IsNullOrEmpty(helpFor))
        return;
      this.writeLineInternal(helpFor, ColorRgba.LightYellow);
    }

    /// <summary>
    /// Input update invoked only when the console is active <see cref="T:Mafi.Unity.InputControl.IUnityInputController" />, needed so that the
    /// console cooperates well with other tools.
    /// </summary>
    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!UnityEngine.Input.GetKeyDown(KeyCode.Return) && !UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter))
        return false;
      string text = this.m_inputField.GetText();
      if (this.m_consoleCommandsExecutor.ExecuteOrSchedule(text))
      {
        this.addCommandToHistory(text);
        this.m_inputField.ClearInput();
        this.m_inputField.Activate();
      }
      return true;
    }

    private void addCommandToHistory(string command)
    {
      this.WriteLine(command, ColorRgba.Green);
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(this.m_historyFilePath, true))
        {
          streamWriter.WriteLine(command);
          streamWriter.Flush();
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to open history commands file. Already open?");
      }
      this.m_commandsHistory.Remove(command);
      this.m_commandsHistory.Add(command);
      this.m_currentCommand = "";
      this.m_historyPositionFromEnd = 0;
    }

    private void setTextFieldFromHistory()
    {
      if (this.m_historyPositionFromEnd == 0)
      {
        this.m_inputField.SetText(this.m_currentCommand);
        this.m_inputField.MoveCaretToEnd();
      }
      else
      {
        this.m_inputField.SetText(this.m_commandsHistory[this.m_commandsHistory.Count - this.m_historyPositionFromEnd]);
        this.m_inputField.MoveCaretToEnd();
      }
    }
  }
}
