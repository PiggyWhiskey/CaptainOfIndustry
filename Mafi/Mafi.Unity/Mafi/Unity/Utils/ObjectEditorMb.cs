// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ObjectEditorMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Map;
using Mafi.Core.Utils;
using Mafi.Random.Noise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  [ExecuteInEditMode]
  public class ObjectEditorMb : MonoBehaviour
  {
    private static readonly char[] EQ_SEPARATOR;
    public string EditedObjectType;
    public bool ApplyChanges;
    public bool RefreshData;
    [TextArea(10, 30)]
    public string Data;
    private ImmutableArray<IObjectEditorCustomParser> m_customSerializers;
    private IRandom m_random;

    public object EditedObject { get; private set; }

    public event Action<object> OnObjectEdited;

    public void Initialize(
      object editedObject,
      ImmutableArray<IObjectEditorCustomParser> customSerializers)
    {
      this.EditedObject = editedObject.CheckNotNull<object>();
      this.EditedObjectType = editedObject.GetType().Name;
      this.m_customSerializers = customSerializers;
      this.m_random = (IRandom) new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, (ulong) Environment.TickCount, 18446744073709551573UL);
      this.m_random.Jump();
      this.populateData();
    }

    public void Clear()
    {
      this.EditedObject = (object) null;
      this.EditedObjectType = (string) null;
      this.ApplyChanges = false;
      this.RefreshData = false;
      this.populateData();
    }

    public void Update()
    {
      if (this.EditedObject == null)
        return;
      if (this.ApplyChanges)
      {
        this.ApplyChanges = false;
        this.parseAndSetFields();
        Action<object> onObjectEdited = this.OnObjectEdited;
        if (onObjectEdited != null)
          onObjectEdited(this.EditedObject);
      }
      if (!this.RefreshData)
        return;
      this.RefreshData = false;
      this.populateData();
    }

    private void populateData()
    {
      if (this.EditedObject == null)
      {
        this.Data = "";
      }
      else
      {
        System.Type type = this.EditedObject.GetType();
        StringBuilder stringBuilder = new StringBuilder(100);
        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
          object obj = field.GetValue(this.EditedObject);
          Option<string> str = this.valueToStr(field.FieldType, obj);
          stringBuilder.AppendLine(str.HasValue ? field.Name + " = " + str.Value : string.Format("// {0} = no serializer for {1}", (object) field.Name, (object) field.FieldType));
        }
        this.Data = stringBuilder.ToString();
      }
    }

    private void parseAndSetFields()
    {
      for (int index = 0; index < this.Data.Split('\n', StringSplitOptions.None).Length; ++index)
      {
        string str = this.Data.Split('\n', StringSplitOptions.None)[index];
        int length = str.IndexOf("//", StringComparison.Ordinal);
        if (length >= 0)
          str = str.Substring(0, length);
        if (!string.IsNullOrWhiteSpace(str))
        {
          string[] strArray = str.Split(ObjectEditorMb.EQ_SEPARATOR, 2);
          if (strArray.Length != 2)
            Log.Error("Invalid assignment line '" + str + "'.");
          else
            this.trySetField(strArray[0].Trim(), strArray[1].Trim());
        }
      }
    }

    private void trySetField(string name, string value)
    {
      System.Type type = this.EditedObject.GetType();
      FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public);
      if (field == (FieldInfo) null)
      {
        Log.Error("Failed to set field `" + name + "` on type `" + type.Name + "`.");
      }
      else
      {
        try
        {
          field.SetValue(this.EditedObject, this.parseStrValueOrThrow(value, field.FieldType));
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to set `" + name + "` on type `" + type.Name + "`.");
        }
      }
    }

    private Option<string> valueToStr(System.Type type, object value)
    {
      foreach (IObjectEditorCustomParser customSerializer in this.m_customSerializers)
      {
        if (customSerializer.CanParse(type))
          return customSerializer.SerializeToStr(type, value);
      }
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Option<>))
      {
        IOptionNonGeneric optionNonGeneric = (IOptionNonGeneric) value;
        if (!optionNonGeneric.HasValue)
          return (Option<string>) string.Format("none  // Option<{0}>", (object) type.GetGenericArguments()[0]);
        Option<string> str = this.valueToStr(type.GetGenericArguments()[0], optionNonGeneric.Value);
        return !str.HasValue ? Option<string>.None : (Option<string>) (str.Value + "  // can be 'none'");
      }
      if (type.IsEnum)
        return (Option<string>) Enum.GetName(type, value);
      if (type == typeof (string))
        return (Option<string>) (string) value;
      if (type == typeof (int))
        return (Option<string>) value.ToString();
      if (type == typeof (bool))
        return (Option<string>) ((bool) value ? "true" : "false");
      if (type == typeof (Fix32))
        return (Option<string>) ((Fix32) value).ToString();
      if (type == typeof (RelTile1i))
        return (Option<string>) ((RelTile1i) value).Value.ToString();
      if (type == typeof (ThicknessTilesI))
        return (Option<string>) ((ThicknessTilesI) value).Value.ToString();
      if (type == typeof (ThicknessTilesF))
        return (Option<string>) ((ThicknessTilesF) value).Value.ToString();
      if (type == typeof (Tile2i))
      {
        Tile2i tile2i = (Tile2i) value;
        return (Option<string>) string.Format("x: {0}; y: {1}", (object) tile2i.X, (object) tile2i.Y);
      }
      if (type == typeof (Percent))
        return (Option<string>) ((Percent) value).ToStringRounded(1);
      if (type == typeof (MapCellId))
        return (Option<string>) ((MapCellId) value).Value.ToString();
      if (type == typeof (SimplexNoise2dParams))
      {
        SimplexNoise2dParams simplexNoise2dParams = (SimplexNoise2dParams) value;
        return (Option<string>) ("mean: " + simplexNoise2dParams.MeanValue.ToStringRounded(1) + "; ampl: " + simplexNoise2dParams.Amplitude.ToStringRounded(1) + "; period: " + simplexNoise2dParams.Period.ToStringRounded());
      }
      if (type == typeof (NoiseTurbulenceParams))
      {
        NoiseTurbulenceParams turbulenceParams = (NoiseTurbulenceParams) value;
        return (Option<string>) (string.Format("cnt: {0}; lac: {1}; ", (object) turbulenceParams.OctavesCount, (object) turbulenceParams.Lacunarity.ToStringRounded()) + "prs: " + turbulenceParams.Persistence.ToStringRounded());
      }
      if (type == typeof (SteppedNoiseParams))
      {
        SteppedNoiseParams steppedNoiseParams = (SteppedNoiseParams) value;
        return (Option<string>) ("size(>0): " + steppedNoiseParams.StepSize.ToStringRounded() + "; steepness(>1): " + steppedNoiseParams.StepSteepness.ToStringRounded(1));
      }
      if (!(type == typeof (SimplexNoise2dSeed)))
        return Option<string>.None;
      SimplexNoise2dSeed simplexNoise2dSeed = (SimplexNoise2dSeed) value;
      return (Option<string>) string.Format("x: {0}; y: {1}  // empty to randomize", (object) simplexNoise2dSeed.SeedX.RawValue, (object) simplexNoise2dSeed.SeedY.RawValue);
    }

    private object parseStrValueOrThrow(string value, System.Type type)
    {
      foreach (IObjectEditorCustomParser customSerializer in this.m_customSerializers)
      {
        if (customSerializer.CanParse(type))
        {
          object strValueOrThrow;
          if (customSerializer.TryParseFromStrAs(value, type, out strValueOrThrow))
            return strValueOrThrow;
          throw new Exception("Failed to parse '" + value + "' as '" + type.Name + "'.");
        }
      }
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Option<>))
        return typeof (Option).GetMethod("Create", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(type.GetGenericArguments()[0]).Invoke((object) null, new object[1]
        {
          value == "none" ? (object) null : this.parseStrValueOrThrow(value, type.GetGenericArguments()[0])
        });
      if (type.IsEnum)
        return Enum.Parse(type, value);
      if (type == typeof (string))
        return (object) value;
      if (type == typeof (int))
        return (object) int.Parse(value);
      if (type == typeof (bool))
        return (object) bool.Parse(value);
      if (type == typeof (Fix32))
        return (object) Fix32.FromFloat(float.Parse(value));
      if (type == typeof (RelTile1i))
        return (object) new RelTile1i(int.Parse(value));
      if (type == typeof (ThicknessTilesI))
        return (object) new ThicknessTilesI(int.Parse(value));
      if (type == typeof (ThicknessTilesF))
        return (object) new ThicknessTilesF(parseFix32(value));
      if (type == typeof (Tile2i))
      {
        string[] separatedNamedValues = getCommaSeparatedNamedValues(value);
        Assert.That<string[]>(separatedNamedValues).HasLength<string>(2);
        return (object) new Tile2i(int.Parse(separatedNamedValues[0]), int.Parse(separatedNamedValues[1]));
      }
      if (type == typeof (Percent))
        return (object) parsePercent(value);
      if (type == typeof (MapCellId))
        return (object) new MapCellId(int.Parse(value));
      if (type == typeof (SimplexNoise2dParams))
      {
        string[] separatedNamedValues = getCommaSeparatedNamedValues(value);
        Assert.That<string[]>(separatedNamedValues).HasLength<string>(3);
        return (object) new SimplexNoise2dParams(parseFix32(separatedNamedValues[0]), parseFix32(separatedNamedValues[1]), parseFix32(separatedNamedValues[2]));
      }
      if (type == typeof (NoiseTurbulenceParams))
      {
        string[] separatedNamedValues = getCommaSeparatedNamedValues(value);
        Assert.That<string[]>(separatedNamedValues).HasLength<string>(3);
        return (object) new NoiseTurbulenceParams(int.Parse(separatedNamedValues[0]), parsePercent(separatedNamedValues[1]), parsePercent(separatedNamedValues[2]));
      }
      if (type == typeof (SteppedNoiseParams))
      {
        string[] separatedNamedValues = getCommaSeparatedNamedValues(value);
        Assert.That<string[]>(separatedNamedValues).HasLength<string>(2);
        return (object) new SteppedNoiseParams(parseFix32(separatedNamedValues[0]), parseFix32(separatedNamedValues[1]));
      }
      if (!(type == typeof (SimplexNoise2dSeed)))
        throw new Exception(string.Format("No deserialization defined for type '{0}'.", (object) type));
      SimplexNoise2dSeed strValueOrThrow1;
      if (string.IsNullOrWhiteSpace(value))
      {
        strValueOrThrow1 = this.m_random.NoiseSeed2d();
      }
      else
      {
        string[] separatedNamedValues = getCommaSeparatedNamedValues(value);
        Assert.That<string[]>(separatedNamedValues).HasLength<string>(2);
        strValueOrThrow1 = new SimplexNoise2dSeed(Fix32.FromRaw(int.Parse(separatedNamedValues[0])), Fix32.FromRaw(int.Parse(separatedNamedValues[1])));
      }
      return (object) strValueOrThrow1;

      static string[] getCommaSeparatedNamedValues(string str)
      {
        return ((IEnumerable<string>) str.Split(';', StringSplitOptions.None)).Select<string, string>((Func<string, string>) (x => x.Split(new char[1]
        {
          ':'
        }, 2)[1])).ToArray<string>();
      }

      static Percent parsePercent(string str)
      {
        return Percent.FromDouble(double.Parse(str.TrimEnd('%')) / 100.0);
      }

      static Fix32 parseFix32(string str) => Fix32.FromDouble(double.Parse(str));
    }

    public ObjectEditorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ObjectEditorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjectEditorMb.EQ_SEPARATOR = new char[1]{ '=' };
    }
  }
}
