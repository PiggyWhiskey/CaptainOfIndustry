// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.TutorialProgressTracker
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Messages;
using Mafi.Core.Simulation;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class TutorialProgressTracker : ITutorialProgressTracker, ITutorialProgressCleaner
  {
    public const string TUTORIALS_KEY = "MessagesProgressKey";
    private Option<Dict<string, int>> m_tutorialsData;
    private readonly Lyst<MessageProto> m_messagesToMarkAsRead;

    public TutorialProgressTracker(ISimLoopEvents simLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_messagesToMarkAsRead = new Lyst<MessageProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.getCurrentData();
      simLoopEvents.Sync.AddNonSaveable<TutorialProgressTracker>(this, new Action(this.syncUpdate));
    }

    private void syncUpdate()
    {
      if (this.m_messagesToMarkAsRead.IsEmpty)
        return;
      Dict<string, int> currentData = this.getCurrentData();
      foreach (MessageProto messageProto in this.m_messagesToMarkAsRead)
      {
        int num;
        if (currentData.TryGetValue(messageProto.Id.Value, out num))
          currentData[messageProto.Id.Value] = num;
        else
          currentData.Add(messageProto.Id.Value, messageProto.CurrentVersion);
      }
      this.m_messagesToMarkAsRead.Clear();
      this.saveData(currentData);
    }

    /// <summary>WARNING: Can be only used from the main thread.</summary>
    private void markTutorialAsRead(MessageProto message)
    {
      if (message.AlwaysNotify)
        return;
      Dict<string, int> currentData = this.getCurrentData();
      int num;
      if (currentData.TryGetValue(message.Id.Value, out num))
        currentData[message.Id.Value] = num;
      else
        currentData.Add(message.Id.Value, message.CurrentVersion);
      this.saveData(currentData);
    }

    public void MarkTutorialAsReadFromSim(MessageProto message)
    {
      if (message.AlwaysNotify)
        return;
      this.m_messagesToMarkAsRead.Add(message);
    }

    public bool IsTutorialNew(MessageProto message)
    {
      int num;
      return message.AlwaysNotify || !this.getCurrentData().TryGetValue(message.Id.Value, out num) || message.CurrentVersion > num;
    }

    /// <summary>WARNING: Can be only used from the main thread.</summary>
    public void ResetTutorialProgress()
    {
      if (PlayerPrefs.HasKey("MessagesProgressKey"))
        PlayerPrefs.DeleteKey("MessagesProgressKey");
      this.m_tutorialsData = Option<Dict<string, int>>.None;
    }

    private Dict<string, int> getCurrentData()
    {
      if (this.m_tutorialsData.HasValue)
        return this.m_tutorialsData.Value;
      this.m_tutorialsData = (Option<Dict<string, int>>) new Dict<string, int>();
      try
      {
        if (!PlayerPrefs.HasKey("MessagesProgressKey"))
          return this.m_tutorialsData.Value;
        string str1 = PlayerPrefs.GetString("MessagesProgressKey");
        char[] separator = new char[1]{ ',' };
        foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
          string[] strArray = str2.Split(':');
          if (strArray.Length != 2)
          {
            Log.Error("Failed to parse tutorial entry " + str2);
            return this.m_tutorialsData.Value;
          }
          string key = strArray[0];
          int result;
          if (!int.TryParse(strArray[1], out result))
          {
            result = 0;
            Log.Error("Failed to parse tutorial version " + strArray[1]);
          }
          this.m_tutorialsData.Value.Add(key, result);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Reading preferences failed with exception");
      }
      return this.m_tutorialsData.Value;
    }

    private void saveData(Dict<string, int> data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, int> keyValuePair in data)
      {
        stringBuilder.Append(keyValuePair.Key);
        stringBuilder.Append(':');
        stringBuilder.Append(keyValuePair.Value);
        stringBuilder.Append(',');
      }
      string str = stringBuilder.ToString();
      try
      {
        PlayerPrefs.SetString("MessagesProgressKey", str);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Saving preferences failed with exception");
      }
    }
  }
}
