// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.TestObjForEditorInner
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  public class TestObjForEditorInner
  {
    [EditorClassName]
    public string ClassName => "Texture config";

    public string Name { get; set; }

    public string Hello { get; set; }

    public ColorRgba Color { get; set; }

    public TestObjForEditorInner2 Inside { get; set; }

    public TestObjForEditorInner()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = "change me";
      this.Hello = "352";
      this.Color = ColorRgba.Gold;
      this.Inside = new TestObjForEditorInner2();
    }
  }
}
