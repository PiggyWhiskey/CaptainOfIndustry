// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TextConfigurableNoise2dFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public class TextConfigurableNoise2dFactory : INoise2dFactory
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [EditorEnforceOrder(13)]
    public readonly Dict<string, object> Parameters;
    [DoNotSave(0, null)]
    private string m_parsingError;

    public static void Serialize(TextConfigurableNoise2dFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TextConfigurableNoise2dFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TextConfigurableNoise2dFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.Configuration);
      Dict<string, object>.Serialize(this.Parameters, writer);
    }

    public static TextConfigurableNoise2dFactory Deserialize(BlobReader reader)
    {
      TextConfigurableNoise2dFactory configurableNoise2dFactory;
      if (reader.TryStartClassDeserialization<TextConfigurableNoise2dFactory>(out configurableNoise2dFactory))
        reader.EnqueueDataDeserialization((object) configurableNoise2dFactory, TextConfigurableNoise2dFactory.s_deserializeDataDelayedAction);
      return configurableNoise2dFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Configuration = reader.ReadString();
      reader.SetField<TextConfigurableNoise2dFactory>(this, "Parameters", (object) Dict<string, object>.Deserialize(reader));
    }

    [EditorValidationSource("m_parsingError")]
    [EditorEnforceOrder(18)]
    [EditorTextArea(4, true)]
    public string Configuration { get; set; }

    [DoNotSave(0, null)]
    [EditorRebuildIfTrue]
    public bool RebuildUi { get; private set; }

    public bool TryCreateNoise(
      IResolver resolver,
      IReadOnlyDictionary<string, object> extraArgs,
      out INoise2D result,
      out string error)
    {
      ConfigurableNoise2dParser configurableNoise2dParser = resolver.Resolve<ConfigurableNoise2dParser>();
      result = (INoise2D) null;
      ConfigurableNoise2dFactorySpec factorySpec;
      if (!configurableNoise2dParser.TryParseNoiseFactorySpec((IEnumerable<string>) this.Configuration.SplitToLines(), out factorySpec, out error))
      {
        this.m_parsingError = error;
        return false;
      }
      foreach (KeyValuePair<string, Type> parameter in factorySpec.Parameters)
      {
        object obj;
        if (!this.Parameters.TryGetValue(parameter.Key, out obj) || !(obj.GetType() == parameter.Value))
        {
          try
          {
            this.Parameters[parameter.Key] = configurableNoise2dParser.GetDefaultParameter(parameter.Value);
            this.RebuildUi = true;
          }
          catch (Exception ex)
          {
            error = this.m_parsingError = string.Format("Failed to create new instance of type '{0}'.", (object) parameter.Value);
            return false;
          }
        }
      }
      if (this.Parameters.RemoveKeys((Predicate<string>) (x => !factorySpec.Parameters.ContainsKey(x))) > 0)
        this.RebuildUi = true;
      Dict<string, object> argsDict = this.Parameters.DeepClone();
      foreach (KeyValuePair<string, object> extraArg in (IEnumerable<KeyValuePair<string, object>>) extraArgs)
        argsDict.Add(extraArg.Key, extraArg.Value);
      if (!configurableNoise2dParser.TryBuildNoise(factorySpec, argsDict, out result, out error))
      {
        this.m_parsingError = error;
        return false;
      }
      this.m_parsingError = "";
      return true;
    }

    public TextConfigurableNoise2dFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Parameters = new Dict<string, object>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CConfiguration\u003Ek__BackingField = "";
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TextConfigurableNoise2dFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TextConfigurableNoise2dFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TextConfigurableNoise2dFactory) obj).SerializeData(writer));
      TextConfigurableNoise2dFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TextConfigurableNoise2dFactory) obj).DeserializeData(reader));
    }
  }
}
