// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.UiPlayground
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  /// <summary>
  /// To run demos add them to <see cref="F:Mafi.Unity.Playground.UiPlayground.FILES_TO_WATCH" />. Content will automatically
  /// updated on save.
  /// 
  /// The tool will fin the first class in the file. If you need to run other class
  /// add the following comment into your file:
  /// // ClassToRun = LoadWindow;
  /// 
  /// Class either has to extend UiComponent or needs to have "Content" field set:
  /// public readonly UiComponent Content;
  /// 
  /// You can also paste content from your clipboard.
  /// </summary>
  public class UiPlayground
  {
    private static readonly string[] FILES_TO_WATCH;
    private readonly DependencyResolver m_resolver;
    public readonly Mafi.Unity.UiToolkit.Library.Window Window;
    private TabContainer m_tabContainer;
    private Label m_status;
    private Option<UiPlayground.DemoTab> m_pasteTab;
    private Option<UiPlayground.DemoTab> m_tabToUpdate;
    private int m_demosDone;

    public UiPlayground(DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
      this.Window = new Mafi.Unity.UiToolkit.Library.Window((LocStrFormatted) LocStr.Empty);
      this.Window.Fullscreen();
      this.Window.Schedule.Execute(new Action(this.checkFile)).Every(2000L);
      this.Window.Body.Add((UiComponent) (this.m_tabContainer = new TabContainer(true)));
      this.Window.OnHide(new Action(this.handleHide));
      this.m_tabContainer.ControlsBar.Add((UiComponent) (this.m_status = new Label().FontBold<Label>().MarginLeftRight<Label>(4.pt())), (UiComponent) new ButtonText("Paste".AsLoc(), new Action(this.pasteCode)));
      string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".."));
      foreach (string path2 in UiPlayground.FILES_TO_WATCH)
      {
        string str = Path.Combine(fullPath, path2);
        UiPlayground.DemoTab content = new UiPlayground.DemoTab(this, (Option<string>) str);
        this.m_tabContainer.AddTab(Path.GetFileName(str).AsLoc(), (UiComponent) content, switchTo: true, scroll: false);
      }
    }

    private void checkFile()
    {
      if (!(this.m_tabContainer.ActiveTab.ValueOrNull is UiPlayground.DemoTab valueOrNull))
        return;
      valueOrNull.CheckFileChange();
    }

    public UiPlayground Show()
    {
      this.Window.Show<Mafi.Unity.UiToolkit.Library.Window>();
      return this;
    }

    private void handleHide() => this.Window.RemoveFromHierarchy();

    public bool IsVisible() => this.Window.IsVisible();

    public bool InputUpdate() => this.Window.InputUpdate();

    private void pasteCode()
    {
      if (this.m_pasteTab.IsNone)
      {
        this.m_pasteTab = (Option<UiPlayground.DemoTab>) new UiPlayground.DemoTab(this, (Option<string>) Option.None);
        this.m_tabContainer.AddTab("Pasted".AsLoc(), (UiComponent) this.m_pasteTab.Value);
      }
      this.runForTab(GUIUtility.systemCopyBuffer, this.m_pasteTab.Value);
    }

    private void runForTab(string codeToCompile, UiPlayground.DemoTab tabToUpdate)
    {
      this.m_tabToUpdate = (Option<UiPlayground.DemoTab>) tabToUpdate;
      this.m_status.Text<Label>((this.runCode(codeToCompile) ? "Last update " + DateTime.Now.ToString("HH:mm:ss tt") : "Failed").AsLoc());
    }

    private bool runCode(string codeToCompile)
    {
      ++this.m_demosDone;
      Regex regex = new Regex("namespace\\s+([A-Za-z0-9_.]+);");
      Match match1 = regex.Match(codeToCompile);
      if (!match1.Success)
      {
        Log.Error("Failed to parse the namespace");
        return false;
      }
      string str1 = match1.Groups[1].Value;
      string str2 = string.Format("{0}{1}", (object) str1, (object) this.m_demosDone);
      string replacement = "namespace " + str2 + " {\n";
      codeToCompile = regex.Replace(codeToCompile, replacement);
      Match match2 = new Regex("//\\s*ClassToRun\\s*=\\s*([A-Za-z0-9_]+);").Match(codeToCompile);
      string str3;
      if (match2.Success)
      {
        str3 = match2.Groups[1].Value;
      }
      else
      {
        string pattern = "^\\s*(public|private|internal)?\\s*class\\s+(\\w+)";
        Match match3 = Regex.Match(codeToCompile, pattern, RegexOptions.Multiline);
        if (!match3.Success)
        {
          Log.Error("Failed to find a class to run, consider adding ClassToRun token.");
          return false;
        }
        str3 = match3.Groups[2].Value;
      }
      codeToCompile = "using " + str1 + ";\n" + codeToCompile;
      codeToCompile = codeToCompile + "\n public static class Runner { public static void Run(UiPlayground playground) { playground.RunDemo<" + str3 + ">(); }  }";
      codeToCompile += "\n}";
      string[] strArray = new string[23]
      {
        "netstandard.dll",
        "System.dll",
        "System.Core.dll",
        "System.Numerics.dll",
        "System.Data.dll",
        "System.Xml",
        "System.Xml.Linq.dll",
        "Mafi.dll",
        "Mafi.Core.dll",
        "Mafi.Base.dll",
        "Mafi.Unity.dll",
        "Assembly-CSharp.dll",
        "UnityEngine.dll",
        "UnityEngine.CoreModule.dll",
        "UnityEngine.AssetBundleModule.dll",
        "UnityEngine.IMGUIModule.dll",
        "UnityEngine.TextCore.dll",
        "UnityEngine.TextRenderingModule.dll",
        "UnityEngine.UI.dll",
        "UnityEngine.UIElementsModule.dll",
        "UnityEngine.UIModule.dll",
        "UnityEngine.AudioModule.dll",
        "UnityEngine.VideoModule.dll"
      };
      CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
      CompilerParameters options = new CompilerParameters()
      {
        GenerateInMemory = true
      };
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (!assembly.IsDynamic)
        {
          foreach (string str4 in strArray)
          {
            if (assembly.Location.EndsWith(str4))
              options.ReferencedAssemblies.Add(assembly.Location);
          }
        }
      }
      CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(options, codeToCompile);
      if (compilerResults.Errors.HasErrors)
      {
        StringBuilder stringBuilder = new StringBuilder("Compilation error for class '" + str3 + "':");
        stringBuilder.AppendLine();
        foreach (CompilerError error in (CollectionBase) compilerResults.Errors)
          stringBuilder.AppendLine(string.Format("[{0}]: {1}: {2}", (object) error.Line, error.IsWarning ? (object) "W" : (object) "E", (object) error.ErrorText));
        Log.Error(stringBuilder.ToString());
        return false;
      }
      System.Type type = compilerResults.CompiledAssembly.GetType(str2 + ".Runner");
      if (type == (System.Type) null)
      {
        Log.Error("Couldn't find " + str2 + ".Runner!");
        Log.Error(codeToCompile);
        return false;
      }
      MethodInfo method = type.GetMethod("Run");
      if (method == (MethodInfo) null)
      {
        Log.Error("Couldn't find Run() method!");
        return false;
      }
      try
      {
        method.Invoke((object) null, new object[1]
        {
          (object) this
        });
      }
      catch (Exception ex)
      {
        Log.Exception(ex);
        return false;
      }
      return true;
    }

    public void RunDemo<T>()
    {
      if (this.m_tabToUpdate.IsNone)
      {
        Log.Error("No tab to update");
      }
      else
      {
        T obj = this.m_resolver.Instantiate<T>();
        if (obj is IDemoSetup demoSetup)
          demoSetup.Run();
        UiPlayground.DemoTab tab = this.m_tabToUpdate.Value;
        if (!(obj is UiComponent child))
          child = (UiComponent) obj.GetType().GetField("Content").GetValue((object) obj);
        tab.Clear();
        tab.Add(child);
        this.m_tabContainer.SwitchToTab((UiComponent) tab);
      }
    }

    static UiPlayground()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      UiPlayground.FILES_TO_WATCH = new string[4]
      {
        "Mafi.Unity/Playground/Demo.cs",
        "Mafi.Unity/Playground/TooltipsDemo.cs",
        "Mafi.Unity/Playground/ControlsDemo.cs",
        "Mafi.Unity/Playground/ObjectEditorDemo.cs"
      };
    }

    public class DemoTab : Column
    {
      private readonly UiPlayground m_playground;
      private readonly Option<string> m_fileToMonitor;
      private DateTime m_lastWritten;

      public DemoTab(UiPlayground playground, Option<string> pathToFile)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.AlignItemsStretch<UiPlayground.DemoTab>().Fill<UiPlayground.DemoTab>();
        this.m_playground = playground;
        this.m_fileToMonitor = pathToFile;
      }

      internal void CheckFileChange()
      {
        if (this.m_fileToMonitor.IsNone)
          return;
        string path = this.m_fileToMonitor.Value;
        if (!File.Exists(path))
        {
          Log.Error("File " + path + " does not exist!");
        }
        else
        {
          DateTime lastWriteTime = File.GetLastWriteTime(path);
          if (lastWriteTime <= this.m_lastWritten)
            return;
          this.m_lastWritten = lastWriteTime;
          this.m_playground.runForTab(File.ReadAllText(path), this);
        }
      }
    }
  }
}
