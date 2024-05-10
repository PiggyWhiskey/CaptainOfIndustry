// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.TestObjForEditor2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  internal class TestObjForEditor2
  {
    public readonly int Number;
    public readonly int? NumberOpt;
    [EditorSection("New section", null, false, false)]
    [EditorLabel("Apply changes instantaneously!", null, false, false)]
    public readonly bool Apply;
    public Proto.ID? NullableProtoId;
    public readonly Dict<string, RelTile2i> TilesDictNull;
    [EditorCollapseObject]
    public readonly Dict<string, RelTile2i> TilesDict;
    public readonly Dict<string, TestObjForEditorInner> ObjectsDict;
    public Rect Rect;
    public TerrainMaterialSlimId MaterialSlimId;
    public AngleSlim Rotation;
    [EditorIgnore]
    private string m_inner;

    public string PostProcessedProperty
    {
      get => this.m_inner;
      set => this.m_inner = "[updated] " + value;
    }

    public TestObjForEditor2()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Number = 666;
      this.MaterialSlimId = new TerrainMaterialSlimId((byte) 1);
      this.Rotation = AngleSlim.FromAngleDegrees(AngleDegrees1f.Deg90);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TilesDict = new Dict<string, RelTile2i>();
      this.TilesDict["test"] = new RelTile2i(10, 20);
      this.ObjectsDict = new Dict<string, TestObjForEditorInner>();
      this.ObjectsDict["myKey"] = new TestObjForEditorInner();
    }
  }
}
