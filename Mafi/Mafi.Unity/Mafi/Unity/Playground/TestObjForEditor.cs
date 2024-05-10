// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.TestObjForEditor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Random.Noise;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  internal class TestObjForEditor
  {
    [EditorReadonly]
    public int ReadonlyNumber;
    public int IncreaseToRebuild;
    public int CustomRebuildsDone;
    [EditorRange(0.0, 100.0)]
    public Quantity Quantity;
    public SimplexNoise2dSeed NoiseSeed;
    public readonly Lyst<SimplexNoise2dSeed> NoiseSeeds;
    public readonly Dict<string, SimplexNoise2dSeed> NoiseSeedsDict;
    public Direction90 Direction;
    public ulong Ulong;
    public uint Uint;
    public ushort Ushort;
    public byte AnotherOneBitesTheDust;
    [EditorRange(100.0, 10000.0)]
    public Fix64 Fix64Validated;
    public HeightTilesF HeightTiles;
    [EditorRange(-100.0, 777.0)]
    public readonly int Number;
    public readonly int? NumberOpt;
    [EditorSection("New section", "This is collapsed by default", true, true)]
    public readonly float NumberFloat;
    [EditorRangePercent(10, 90)]
    [EditorLabel(null, "This tooltip will explain you all the things you need to know.", false, false)]
    public readonly Percent Fraction;
    public Fix32 NumberFix;
    [EditorLabel("This is button", "Tooltip for label", false, false)]
    [EditorButton("Click me!", "Tooltip on button", true, ObjEditorIcon.None)]
    public readonly Action m_randomize;
    [EditorButton(null, "Just tooltip", false, ObjEditorIcon.None)]
    public readonly Action Triple;
    [EditorSection("New section 2", "This is new section", true, false)]
    public Fix64 NumberFix64;
    public readonly RelTile2i Rel2;
    public readonly RelTile3i Rel3;
    [EditorLabel("Depth", "I'm nested and you have no idea!", false, false)]
    public readonly RelTile1i Rel1;
    public readonly Machine.State MachineState;
    public readonly RelTile2i? OptRelTile;
    [EditorLabel(null, null, true, false)]
    [EditorCollection(true, false)]
    public readonly Lyst<RelTile2i> TilesList;
    [EditorCollapseObject]
    [EditorCollection(false, true)]
    public readonly Lyst<RelTile1i> Tiles1List;
    public readonly Lyst<int> IntsList;
    public readonly Lyst<int> IntsListNull;
    public readonly Lyst<TestObjForEditorInner> ObjectsList;
    [EditorDropdown("m_listOfStrings")]
    public string DropdownBackedByListOfStrings;
    [EditorDropdown("m_arrayOfStrings")]
    public string DropdownBackedByImmArrOfStrings;
    private readonly Lyst<string> m_listOfStrings;
    private readonly ImmutableArray<string> m_arrayOfStrings;
    public TestObjForEditor.StructToInline DistanceTransform;
    public Action TypeText;
    public Action TypeTextYes2;
    public Action TypeTextYes3;
    public TestObjForEditor.StructToInline2 BaseNoise;
    [EditorButton(null, null, false, ObjEditorIcon.View)]
    public Action ViewBtn;
    [EditorButton(null, null, false, ObjEditorIcon.Edit)]
    public Action EditBtn;
    [EditorButton(null, null, false, ObjEditorIcon.Delete)]
    public Action DeleteBtn;
    [EditorButton(null, null, false, ObjEditorIcon.Clone)]
    public Action CloneBtn;
    public TestObjForEditor.TestStruct? OptStr;

    [EditorMaxLength(8)]
    [EditorLabel(null, "This tooltip will explain you all the things you need to know.", false, false)]
    public string Name { get; set; }

    [EditorMaxLength(245)]
    [EditorTextArea(4, true)]
    public string Desc { get; set; }

    [EditorLabel(null, "Readonly display tooltip", false, false)]
    public string Status { get; }

    [EditorLabel("", null, false, true)]
    public string Status2 { get; private set; }

    [EditorLabel("", null, false, false)]
    public string Status3 { get; private set; }

    [EditorRebuildIfTrue]
    public bool RebuildIfTrue { get; set; }

    [EditorValidationSource("ValidationResult")]
    [EditorLabel(null, "This field needs to contain a letter b!", false, false)]
    public string WithValidationSource { get; set; }

    public string ValidationResult
    {
      get => !this.WithValidationSource.Contains("b") ? "This must contain b!" : (string) null;
    }

    [EditorTextArea(2, false)]
    public string Desc2 { get; set; }

    public bool CheckMe { get; set; }

    [EditorLabel("Polygon feature (PolygonTerrainFeatureGenerator)", "Tooltip for sub-editor title", true, false)]
    public TestObjForEditorInner Dummy2 { get; private set; }

    public TestObjForEditor()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Status = "Readonly status";
      // ISSUE: reference to a compiler-generated field
      this.\u003CStatus2\u003Ek__BackingField = "Readonly status 2";
      // ISSUE: reference to a compiler-generated field
      this.\u003CStatus3\u003Ek__BackingField = "";
      this.ReadonlyNumber = 150;
      this.NoiseSeed = new SimplexNoise2dSeed(0.25.ToFix32(), 0.5.ToFix32());
      this.NoiseSeeds = new Lyst<SimplexNoise2dSeed>()
      {
        new SimplexNoise2dSeed(0.5.ToFix32(), 0.5.ToFix32())
      };
      this.NoiseSeedsDict = new Dict<string, SimplexNoise2dSeed>()
      {
        {
          "test",
          new SimplexNoise2dSeed((Fix32) 1, (Fix32) 1)
        },
        {
          "test2",
          new SimplexNoise2dSeed((Fix32) 2, (Fix32) 2)
        }
      };
      this.Direction = Direction90.PlusX;
      this.Ulong = 150UL;
      this.Uint = 200U;
      this.Ushort = (ushort) 250;
      this.AnotherOneBitesTheDust = (byte) 127;
      this.Fix64Validated = (Fix64) 150L;
      this.HeightTiles = HeightTilesF.One;
      // ISSUE: reference to a compiler-generated field
      this.\u003CWithValidationSource\u003Ek__BackingField = "b";
      this.Number = 666;
      this.NumberFloat = 0.2187f;
      this.Fraction = 20.Percent();
      this.NumberFix = 0.2187f.ToFix32();
      this.NumberFix64 = 0.2187f.ToFix64();
      this.Rel2 = new RelTile2i(2, 5);
      this.Rel3 = new RelTile3i(2, 5, 7);
      this.Rel1 = new RelTile1i(2);
      this.MachineState = Machine.State.Broken;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = "change me";
      this.Desc = "I grow as you type \n\ntest\n\ntest\n\ntest";
      this.Desc2 = "I have fixed height";
      this.CheckMe = true;
      this.Dummy2 = new TestObjForEditorInner();
      this.TilesList = new Lyst<RelTile2i>()
      {
        new RelTile2i(10, 20),
        new RelTile2i(20, 10),
        new RelTile2i(5, 5),
        new RelTile2i(7, 14)
      };
      this.Tiles1List = new Lyst<RelTile1i>()
      {
        new RelTile1i(10),
        new RelTile1i(20)
      };
      this.IntsList = new Lyst<int>();
      this.ObjectsList = new Lyst<TestObjForEditorInner>()
      {
        new TestObjForEditorInner()
      };
      this.TypeText = this.TypeTextYes2 = this.TypeTextYes3 = (Action) (() => this.Desc = "Text added via button!");
      this.ViewBtn = this.EditBtn = this.DeleteBtn = this.CloneBtn = (Action) (() => { });
      this.m_randomize = (Action) (() => this.NumberFix *= 2);
      this.Triple = (Action) (() => this.NumberFix *= 3);
      this.m_listOfStrings = new Lyst<string>()
      {
        "One",
        "Two",
        "Three"
      };
      this.m_arrayOfStrings = this.m_listOfStrings.ToImmutableArray();
      this.DropdownBackedByListOfStrings = "One";
      this.DropdownBackedByImmArrOfStrings = "Two";
    }

    internal struct StructToInline
    {
      public readonly int MeanValue;
      public readonly int Amplitude;
      public readonly int Period;
    }

    internal struct StructToInline2
    {
      public readonly int Multiplier;
      public readonly int Addend;
      public readonly int FrequencyMultiplier;
    }

    internal struct TestStruct
    {
      public Option<string> StrMaybe;
      public TestObjForEditor.TestStruct.TestStruct2? OptInside;

      internal struct TestStruct2
      {
        public readonly Machine.State MachineState;
        public readonly RelTile2i? Rel2;
        public Option<string> StrMaybe;
      }
    }
  }
}
