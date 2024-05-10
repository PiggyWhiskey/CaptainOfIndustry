// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.EntityLayoutParser
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  /// <summary>Implements parsing of layout entity ASCII layout.</summary>
  /// <remarks>
  /// This class is not registered as dependency because it is being used before dependency resolver is created.
  /// </remarks>
  public class EntityLayoutParser
  {
    public const int MAX_TERRAIN_SURFACE_DIFF_TILES = 4;
    private static readonly char[] PORT_DIRECTIONS;
    public static readonly ThicknessTilesF VEHICLE_SURFACE_EXTRA_THICKNESS;
    public static readonly Proto.ID DEFAULT_HARDENED_SURFACE;
    public static readonly Proto.ID DEFAULT_HARDENED_MATERIAL;
    private readonly ProtosDb m_protosDb;
    private readonly Dict<char, IoPortShapeProto> m_portShapesCache;
    private static readonly CustomLayoutToken[] DEFAULT_TOKENS;

    public EntityLayoutParser(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_portShapesCache = new Dict<char, IoPortShapeProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb.CheckNotNull<ProtosDb>();
    }

    /// <summary>
    /// Parses given layout and normalizes it by cropping spaces around.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Entities.Static.Layout.InvalidEntityLayoutException">When layout is not valid.</exception>
    public EntityLayout ParseLayoutOrThrow(params string[] layout)
    {
      return this.ParseLayoutOrThrow(EntityLayoutParams.DEFAULT, layout);
    }

    /// <summary>
    /// Parses given layout and normalizes it by cropping spaces around.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Entities.Static.Layout.InvalidEntityLayoutException">When layout is not valid.</exception>
    public EntityLayout ParseLayoutOrThrow(EntityLayoutParams layoutParams, params string[] layout)
    {
      int length1 = layout.Length != 0 ? layout.Length : throw new InvalidEntityLayoutException("Layout is empty.");
      for (int index = 0; index < length1; ++index)
      {
        string token = layout[index];
        int num = token.IndexOf('\t');
        if (num >= 0)
        {
          int? line = new int?(index + 1);
          int? col = new int?(num + 1);
          throw new InvalidEntityLayoutException("Line contains a tab.", token, line, col);
        }
        if (token.Length % 3 != 0)
        {
          int? line = new int?(index + 1);
          throw new InvalidEntityLayoutException("Line length is not multiple of 3.", token, line);
        }
      }
      int length2 = layout[0].Length / 3;
      for (int index = 1; index < length1; ++index)
      {
        if (layout[index].Length != length2 * 3)
        {
          string message = string.Format("Length {0} of line {1} does not match ", (object) layout[index].Length, (object) (index + 1)) + string.Format("layout line length {0}.", (object) (length2 * 3));
          int? nullable = new int?(index);
          string token = layout[index];
          int? line = nullable;
          int? col = new int?();
          Vector2i? tokenPos = new Vector2i?();
          throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
        }
      }
      if (length2 == 0)
        throw new InvalidEntityLayoutException("Layout is empty.");
      foreach (CustomLayoutToken customToken in layoutParams.CustomTokens)
      {
        if (customToken.Token.Length != 3)
          throw new InvalidEntityLayoutException("Custom layout token '" + customToken.Token + "' does not have length of 3.", customToken.Token);
      }
      Dict<char, CustomLayoutToken[]> customTokensDict = ((IEnumerable<CustomLayoutToken>) layoutParams.CustomTokens.ConcatToArray(EntityLayoutParser.DEFAULT_TOKENS)).Distinct<CustomLayoutToken, string>((Func<CustomLayoutToken, string>) (x => x.Token)).GroupBy<CustomLayoutToken, char>((Func<CustomLayoutToken, char>) (x => x.Token[0])).ToDict<IGrouping<char, CustomLayoutToken>, char, CustomLayoutToken[]>((Func<IGrouping<char, CustomLayoutToken>, char>) (x => x.Key), (Func<IGrouping<char, CustomLayoutToken>, CustomLayoutToken[]>) (x => x.ToArray<CustomLayoutToken>()));
      string[,] strArray = new string[length1, length2];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        string str1 = layout[index1];
        for (int index2 = 0; index2 < length2; ++index2)
        {
          int startIndex = index2 * 3;
          string str2 = str1.Substring(startIndex, 3);
          strArray[index1, index2] = str2 == "   " ? "" : str2;
        }
      }
      Dict<Vector2i, ThicknessIRange> dict1 = new Dict<Vector2i, ThicknessIRange>();
      Dict<Vector2i, ThicknessTilesF> vehicleHeights = new Dict<Vector2i, ThicknessTilesF>();
      Dict<Vector2i, ThicknessTilesI> terrainHeights = new Dict<Vector2i, ThicknessTilesI>();
      Dict<Vector2i, ThicknessTilesI> minTerrainHeights = new Dict<Vector2i, ThicknessTilesI>();
      Dict<Vector2i, ThicknessTilesI> maxTerrainHeights = new Dict<Vector2i, ThicknessTilesI>();
      Set<Vector2i> tileHasVehicleSurface = new Set<Vector2i>();
      List<EntityLayoutParser.PortLayout> source = new List<EntityLayoutParser.PortLayout>();
      List<Vector2i> vector2iList = new List<Vector2i>();
      Dict<Vector2i, EntityLayoutParser.PortExtraInfo> dict2 = new Dict<Vector2i, EntityLayoutParser.PortExtraInfo>();
      Dict<Vector2i, LayoutTileConstraint> constraints = new Dict<Vector2i, LayoutTileConstraint>();
      Dict<Vector2i, Proto.ID> materials = new Dict<Vector2i, Proto.ID>();
      Dict<Vector2i, Proto.ID> surfaces = new Dict<Vector2i, Proto.ID>();
      Set<Vector2i> set = new Set<Vector2i>();
      Vector2i minXy = Vector2i.MaxValue;
      int maxY = 0;
      int? nullable1;
      ThicknessTilesF thicknessTilesF1;
      for (int y = 0; y < length1; ++y)
      {
        for (int x = 0; x < length2; ++x)
        {
          string str = strArray[y, x];
          if (str.Length == 3)
          {
            Vector2i pos = new Vector2i(x, y);
            if (str == " * ")
            {
              vector2iList.Add(pos);
              if (x + 1 < length2 && strArray[y, x + 1] != " * ")
                str = strArray[y, x + 1];
              else if (y + 1 < length1 && strArray[y + 1, x] != " * ")
                str = strArray[y + 1, x];
              else if (x > 0 && strArray[y, x - 1] != " * ")
                str = strArray[y, x - 1];
              else if (y > 0 && strArray[y - 1, x] != " * ")
              {
                str = strArray[y - 1, x];
              }
              else
              {
                string token = str;
                Vector2i? nullable2 = new Vector2i?(pos);
                nullable1 = new int?();
                int? line = nullable1;
                nullable1 = new int?();
                int? col = nullable1;
                Vector2i? tokenPos = nullable2;
                throw new InvalidEntityLayoutException("Origin token has no non-origins around it.", token, line, col, tokenPos);
              }
            }
            int? nullable3 = new int?();
            int? nullable4 = new int?();
            LayoutTileConstraint layoutTileConstraint = LayoutTileConstraint.None;
            ThicknessTilesI? nullable5 = new ThicknessTilesI?();
            ThicknessTilesI? nullable6 = new ThicknessTilesI?();
            ThicknessTilesI? nullable7 = new ThicknessTilesI?();
            Proto.ID? nullable8 = new Proto.ID?();
            Proto.ID? nullable9 = new Proto.ID?();
            LayoutTokenSpec spec;
            ThicknessTilesI valueOrDefault1;
            if (tryMatchCustomToken(str, out spec))
            {
              if (spec.IsPort)
              {
                EntityLayoutParser.PortLayout portToken = EntityLayoutParser.parsePortToken(str, y, x);
                portToken.MachineHeight = spec.PortHeight;
                source.Add(portToken);
              }
              else
              {
                nullable3 = new int?(spec.HeightFrom.Value);
                nullable4 = new int?(spec.HeightToExcl.Value);
                nullable5 = spec.TerrainSurfaceHeight;
                nullable6 = spec.MinTerrainHeight;
                nullable7 = spec.MaxTerrainHeight;
                nullable8 = spec.TerrainMaterialId;
                nullable9 = spec.SurfaceId;
                layoutTileConstraint = spec.Constraint;
                if (spec.IsRamp)
                {
                  set.Add(pos);
                  if (spec.VehicleHeight.HasValue)
                  {
                    string token = str;
                    Vector2i? nullable10 = new Vector2i?(pos);
                    nullable1 = new int?();
                    int? line = nullable1;
                    nullable1 = new int?();
                    int? col = nullable1;
                    Vector2i? tokenPos = nullable10;
                    throw new InvalidEntityLayoutException("Vehicle height for ramps should not be set.", token, line, col, tokenPos);
                  }
                  nullable1 = nullable3;
                  int num1 = 0;
                  if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
                  {
                    nullable1 = nullable4;
                    int num2 = 0;
                    if (nullable1.GetValueOrDefault() == num2 & nullable1.HasValue)
                    {
                      string token = str;
                      Vector2i? nullable11 = new Vector2i?(pos);
                      nullable1 = new int?();
                      int? line = nullable1;
                      nullable1 = new int?();
                      int? col = nullable1;
                      Vector2i? tokenPos = nullable11;
                      throw new InvalidEntityLayoutException("Entity height was not set for ramp token.", token, line, col, tokenPos);
                    }
                  }
                }
                else if (spec.VehicleHeight.HasValue)
                {
                  if (nullable5.HasValue && spec.VehicleHeight.Value < nullable5.Value)
                  {
                    string message = string.Format("Terrain surface {0} ", (object) nullable5.Value) + string.Format("is above vehicle height {0}.", (object) spec.VehicleHeight);
                    string token = str;
                    Vector2i? nullable12 = new Vector2i?(pos);
                    nullable1 = new int?();
                    int? line = nullable1;
                    nullable1 = new int?();
                    int? col = nullable1;
                    Vector2i? tokenPos = nullable12;
                    throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
                  }
                  addVehicleHeight(pos, spec.VehicleHeight.Value);
                  thicknessTilesF1 = spec.VehicleHeight.Value;
                  if (thicknessTilesF1.IsZero)
                  {
                    valueOrDefault1 = nullable5.GetValueOrDefault();
                    if (!nullable5.HasValue)
                      nullable5 = new ThicknessTilesI?(ThicknessTilesI.Zero);
                    if (!nullable6.HasValue)
                      nullable6 = nullable5;
                  }
                  valueOrDefault1 = nullable7.GetValueOrDefault();
                  if (!nullable7.HasValue)
                  {
                    thicknessTilesF1 = spec.VehicleHeight.Value;
                    nullable7 = new ThicknessTilesI?(thicknessTilesF1.FlooredThicknessTilesI);
                  }
                }
              }
            }
            else if (str.Any<char>((Func<char, bool>) (c => ((IEnumerable<char>) EntityLayoutParser.PORT_DIRECTIONS).Contains<char>(c))))
              source.Add(EntityLayoutParser.parsePortToken(str, y, x));
            else if (str[0] == ' ' || str[2] == ' ')
            {
              EntityLayoutParser.PortExtraInfo portInfoToken = EntityLayoutParser.parsePortInfoToken(str, y, x);
              dict2.Add(pos, portInfoToken);
            }
            else
            {
              string message = "Unrecognized token '" + str + "'.";
              string token = str;
              Vector2i? nullable13 = new Vector2i?(pos);
              nullable1 = new int?();
              int? line = nullable1;
              nullable1 = new int?();
              int? col = nullable1;
              Vector2i? tokenPos = nullable13;
              throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
            }
            if (nullable3.HasValue || nullable4.HasValue)
            {
              int valueOrDefault2 = nullable3.GetValueOrDefault();
              if (!nullable3.HasValue)
                nullable3 = new int?(0);
              valueOrDefault2 = nullable4.GetValueOrDefault();
              if (!nullable4.HasValue)
                nullable4 = new int?(0);
              if (nullable3.Value >= nullable4.Value)
              {
                string message = string.Format("Invalid entity height range from {0} to ", (object) nullable3.Value) + string.Format("{0}.", (object) nullable4.Value);
                string token = str;
                Vector2i? nullable14 = new Vector2i?(pos);
                nullable1 = new int?();
                int? line = nullable1;
                nullable1 = new int?();
                int? col = nullable1;
                Vector2i? tokenPos = nullable14;
                throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
              }
              dict1.Add(pos, new ThicknessIRange(new ThicknessTilesI(nullable3.Value), new ThicknessTilesI(nullable4.Value)));
              if (nullable8.HasValue)
                materials.Add(pos, nullable8.Value);
              if (nullable9.HasValue)
              {
                if (!nullable5.HasValue)
                  nullable5 = new ThicknessTilesI?(ThicknessTilesI.Zero);
                else if (nullable5.Value != ThicknessTilesI.Zero)
                {
                  string message = string.Format("Terrain height on tiles with surface must be zero but it is {0}.", (object) nullable5.Value);
                  string token = str;
                  Vector2i? nullable15 = new Vector2i?(pos);
                  nullable1 = new int?();
                  int? line = nullable1;
                  nullable1 = new int?();
                  int? col = nullable1;
                  Vector2i? tokenPos = nullable15;
                  throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
                }
                surfaces.Add(pos, nullable9.Value);
              }
              if (nullable5.HasValue)
              {
                terrainHeights.Add(pos, nullable5.Value);
                valueOrDefault1 = nullable6.GetValueOrDefault();
                if (!nullable6.HasValue)
                  nullable6 = new ThicknessTilesI?(nullable5.Value.Min(ThicknessTilesI.Zero));
                valueOrDefault1 = nullable7.GetValueOrDefault();
                if (!nullable7.HasValue)
                  nullable7 = new ThicknessTilesI?(nullable5.Value.Max(ThicknessTilesI.Zero));
                if (nullable3.Value <= nullable5.Value.Value)
                  layoutTileConstraint |= LayoutTileConstraint.Ground;
              }
              if (nullable6.HasValue)
                minTerrainHeights.Add(pos, nullable6.Value);
              if (nullable7.HasValue)
                maxTerrainHeights.Add(pos, nullable7.Value);
              constraints.Add(pos, layoutTileConstraint);
              if (nullable5.HasValue)
              {
                if (nullable6.HasValue)
                {
                  ThicknessTilesI? nullable16 = nullable6;
                  ThicknessTilesI thicknessTilesI = nullable5.Value;
                  if ((nullable16.HasValue ? (nullable16.GetValueOrDefault() > thicknessTilesI ? 1 : 0) : 0) != 0)
                  {
                    string message = string.Format("Min terrain height {0} ", (object) nullable6.Value) + string.Format("is greater than terrain height {0}", (object) nullable5.Value);
                    string token = str;
                    Vector2i? nullable17 = new Vector2i?(pos);
                    nullable1 = new int?();
                    int? line = nullable1;
                    nullable1 = new int?();
                    int? col = nullable1;
                    Vector2i? tokenPos = nullable17;
                    throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
                  }
                }
                if (nullable7.HasValue)
                {
                  ThicknessTilesI? nullable18 = nullable7;
                  ThicknessTilesI thicknessTilesI = nullable5.Value;
                  if ((nullable18.HasValue ? (nullable18.GetValueOrDefault() < thicknessTilesI ? 1 : 0) : 0) != 0)
                  {
                    string message = string.Format("Max terrain height {0} ", (object) nullable7.Value) + string.Format("is lower than terrain height {0}", (object) nullable5.Value);
                    string token = str;
                    Vector2i? nullable19 = new Vector2i?(pos);
                    nullable1 = new int?();
                    int? line = nullable1;
                    nullable1 = new int?();
                    int? col = nullable1;
                    Vector2i? tokenPos = nullable19;
                    throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
                  }
                }
              }
              if (nullable6.HasValue && nullable7.HasValue && nullable6.Value > nullable7.Value)
              {
                string message = string.Format("Min terrain height {0} ", (object) nullable6.Value) + string.Format("is greater than max terrain height {0}", (object) nullable7.Value);
                string token = str;
                Vector2i? nullable20 = new Vector2i?(pos);
                nullable1 = new int?();
                int? line = nullable1;
                nullable1 = new int?();
                int? col = nullable1;
                Vector2i? tokenPos = nullable20;
                throw new InvalidEntityLayoutException(message, token, line, col, tokenPos);
              }
              minXy = minXy.Min(pos);
              maxY = Math.Max(maxY, pos.Y);
            }

            bool tryMatchCustomToken(string token, out LayoutTokenSpec spec)
            {
              CustomLayoutToken[] customLayoutTokenArray;
              if (!customTokensDict.TryGetValue(token[0], out customLayoutTokenArray))
              {
                spec = new LayoutTokenSpec();
                return false;
              }
              foreach (CustomLayoutToken customLayoutToken in customLayoutTokenArray)
              {
                string token1 = customLayoutToken.Token;
                if (token1 == token)
                {
                  spec = customLayoutToken.CreateTokenSpecFn(layoutParams, 0);
                  return true;
                }
                if (token1[1] == '0' && char.IsDigit(token[1]) && (int) token1[0] == (int) token[0] && (int) token1[2] == (int) token[2])
                {
                  int height = parseHeight(token[1], token, pos);
                  spec = customLayoutToken.CreateTokenSpecFn(layoutParams, height);
                  return true;
                }
              }
              spec = new LayoutTokenSpec();
              return false;
            }
          }
        }
      }
      foreach (Vector2i vector2i in set)
      {
        int x1 = vector2i.X;
        int y = vector2i.Y;
        int x2 = -1;
        for (int newX = x1 - 1; newX >= 0; --newX)
        {
          if (!set.Contains(vector2i.SetX(newX)))
          {
            x2 = newX;
            break;
          }
        }
        int x3 = -1;
        for (int newX = x1 + 1; newX < length2; ++newX)
        {
          if (!set.Contains(vector2i.SetX(newX)))
          {
            x3 = newX;
            break;
          }
        }
        if (x2 < 0 || x3 < 0)
        {
          Vector2i? nullable21 = new Vector2i?(vector2i);
          nullable1 = new int?();
          int? line = nullable1;
          nullable1 = new int?();
          int? col = nullable1;
          Vector2i? tokenPos = nullable21;
          throw new InvalidEntityLayoutException("Failed to find start or end of a ramp", line: line, col: col, tokenPos: tokenPos);
        }
        ThicknessTilesF thicknessTilesF2;
        if (!vehicleHeights.TryGetValue(new Vector2i(x2, y), out thicknessTilesF2) || thicknessTilesF2 == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL)
          throw new InvalidEntityLayoutException("Start ramp token '" + strArray[y, x2] + "' does not have vehicle access", line: new int?(y + 1), col: new int?(x2 * 3 + 1));
        ThicknessTilesF other;
        if (!vehicleHeights.TryGetValue(new Vector2i(x3, y), out other) || thicknessTilesF2 == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL)
          throw new InvalidEntityLayoutException("End ramp token '" + strArray[y, x3] + "' does not have vehicle access", line: new int?(y + 1), col: new int?(x3 * 3 + 1));
        int scale = x3 - x2 - 1;
        ThicknessTilesF thickness = thicknessTilesF2.Lerp(other, (Fix32) (x1 - x2 - 1), (Fix32) scale);
        ThicknessTilesF thicknessTilesF3 = thicknessTilesF2.Lerp(other, (Fix32) (x1 - x2), (Fix32) scale);
        if (thickness == thicknessTilesF3)
          throw new InvalidEntityLayoutException("Ramp between tokens of the same heights '" + strArray[y, x2] + "' and '" + strArray[y, x3] + "'", line: new int?(y + 1), col: new int?(x3 * 3 + 1));
        thicknessTilesF1 = thickness.Min(thicknessTilesF3) + ThicknessTilesF.Epsilon;
        ThicknessTilesI flooredThicknessTilesI = thicknessTilesF1.FlooredThicknessTilesI;
        thicknessTilesF1 = thickness.Max(thicknessTilesF3) - ThicknessTilesF.Epsilon;
        ThicknessTilesI ceiledThicknessTilesI = thicknessTilesF1.CeiledThicknessTilesI;
        Assert.That<ThicknessTilesI>(ceiledThicknessTilesI).IsPositive("Invalid ramp top height.");
        ThicknessIRange thicknessIrange;
        if (dict1.TryGetValue(vector2i, out thicknessIrange))
        {
          if (flooredThicknessTilesI < thicknessIrange.From || ceiledThicknessTilesI > thicknessIrange.To)
            throw new InvalidEntityLayoutException(string.Format("Ramp is from {0} to {1} is outside of entity height range from {2} ", (object) flooredThicknessTilesI, (object) ceiledThicknessTilesI, (object) thicknessIrange.From) + string.Format("to {0}, token '{1}' ", (object) thicknessIrange.To, (object) strArray[y, x1]), line: new int?(y + 1), col: new int?(x3 * 3 + 1));
          EntityLayoutParser.safeAddVehicleHeight(vector2i, thickness, vehicleHeights);
          EntityLayoutParser.safeAddVehicleHeight(vector2i.DecrementY, thickness, vehicleHeights);
          EntityLayoutParser.safeAddVehicleHeight(vector2i.IncrementX, thicknessTilesF3, vehicleHeights);
          EntityLayoutParser.safeAddVehicleHeight(vector2i.IncrementX.DecrementY, thicknessTilesF3, vehicleHeights);
        }
        else
        {
          Vector2i? nullable22 = new Vector2i?(vector2i);
          nullable1 = new int?();
          int? line = nullable1;
          nullable1 = new int?();
          int? col = nullable1;
          Vector2i? tokenPos = nullable22;
          throw new InvalidEntityLayoutException("No entity height found at ramp position.", line: line, col: col, tokenPos: tokenPos);
        }
      }
      Assert.That<Vector2i>(minXy).IsNotEqualTo<Vector2i>(Vector2i.MaxValue);
      if (minXy == Vector2i.MaxValue)
      {
        minXy = Vector2i.Zero;
      }
      else
      {
        maxY -= minXy.Y;
        Assert.That<int>(maxY).IsNotNegative();
      }
      List<IoPortTemplate> items = new List<IoPortTemplate>();
      foreach (EntityLayoutParser.PortLayout portLayout in source)
      {
        EntityLayoutParser.PortLayout port = portLayout;
        int num3 = source.Count<EntityLayoutParser.PortLayout>((Func<EntityLayoutParser.PortLayout, bool>) (x => (int) x.Name == (int) port.Name));
        if (num3 > 1)
          throw new InvalidEntityLayoutException(string.Format("Multiple ports ({0}) with the name '{1}' in layout.", (object) num3, (object) port.Name), port.Token, new int?(port.Line), new int?(port.Column));
        IoPortType portType;
        Vector2i portDir;
        string error;
        if (!EntityLayoutParser.tryGetPortType(port, dict1, out portType, out portDir, out error))
          throw new InvalidEntityLayoutException("Invalid port layout token: " + error, port.Token, new int?(port.Line), new int?(port.Column));
        Vector2i key = port.Position - portDir;
        int z;
        if (layoutParams.CustomPortHeights.IsNotEmpty)
        {
          int num4;
          if (layoutParams.CustomPortHeights.TryGetValue<char, int>(port.Name, out num4))
          {
            z = num4 - 1;
            if (z < 0)
              throw new InvalidEntityLayoutException(string.Format("Invalid port height '{0}' in token '{1}'.", (object) (z + 1), (object) port.Token));
            ThicknessIRange thicknessIrange = dict1[key];
            if (z >= thicknessIrange.To.Value)
              throw new InvalidEntityLayoutException(string.Format("Too large port height '{0}'.", (object) (z + 1)), port.Token, new int?(port.Line), new int?(port.Column));
          }
          else
            z = port.MachineHeight;
        }
        else
          z = port.MachineHeight;
        IoPortShapeProto portShape = this.getPortShape(port, port.ShapeChar);
        if (portDir.Y != 0)
          portDir = portDir.SetY(-portDir.Y);
        Vector3i vector3i = new Vector3i(key.X - minXy.X, maxY - (key.Y - minXy.Y), z);
        items.Add(new IoPortTemplate(new PortSpec(port.Name, portType, portShape, layoutParams.PortsCanOnlyConnectToTransports), new RelTile3i(vector3i), new Direction90(portDir)));
      }
      Assert.That<int>(dict1.Count).IsEqualTo(constraints.Count);
      ImmutableArray<LayoutTile> immutableArray = dict1.Select<KeyValuePair<Vector2i, ThicknessIRange>, LayoutTile>((Func<KeyValuePair<Vector2i, ThicknessIRange>, LayoutTile>) (kvp =>
      {
        RelTile2i coord = transformCoord(kvp.Key);
        ThicknessTilesI? valueOrNull1 = terrainHeights.GetValueOrNull<Vector2i, ThicknessTilesI>(kvp.Key);
        ThicknessTilesI? valueOrNull2 = minTerrainHeights.GetValueOrNull<Vector2i, ThicknessTilesI>(kvp.Key);
        ThicknessTilesI? valueOrNull3 = maxTerrainHeights.GetValueOrNull<Vector2i, ThicknessTilesI>(kvp.Key);
        ThicknessIRange occupiedThickness = kvp.Value;
        ThicknessTilesI? terrainHeight = valueOrNull1;
        ThicknessTilesI? minTerrainHeight = valueOrNull2;
        ThicknessTilesI? maxTerrainHeight = valueOrNull3;
        int constraint = (int) constraints[kvp.Key];
        Proto.ID id1;
        Option<TerrainMaterialProto> terrainMaterialProto = materials.TryGetValue(kvp.Key, out id1) ? getMaterialProto(id1) : Option<TerrainMaterialProto>.None;
        Proto.ID id2;
        Option<TerrainTileSurfaceProto> tileSurfaceProto = surfaces.TryGetValue(kvp.Key, out id2) ? getTileSurface(id2) : Option<TerrainTileSurfaceProto>.None;
        int num = tileHasVehicleSurface.Contains(kvp.Key) ? 1 : 0;
        return new LayoutTile(coord, occupiedThickness, terrainHeight, minTerrainHeight, maxTerrainHeight, (LayoutTileConstraint) constraint, terrainMaterialProto, tileSurfaceProto, num != 0);
      })).OrderBy<LayoutTile, int>((Func<LayoutTile, int>) (x => x.Coord.Y)).ThenBy<LayoutTile, int>((Func<LayoutTile, int>) (x => x.Coord.X)).ToImmutableArray<LayoutTile>();
      if (layoutParams.CustomVertexDataLayout.HasValue && layoutParams.CustomVertexDataLayout.Value.Length != maxY - minXy.Y + 2)
        throw new InvalidEntityLayoutException(string.Format("Custom vertex data expected to have {0} lines, ", (object) (maxY - minXy.Y)) + string.Format("but have {0}.", (object) layoutParams.CustomVertexDataLayout.Value.Length));
      Dict<RelTile2i, ThicknessTilesF> dict3 = vehicleHeights.ToDict<KeyValuePair<Vector2i, ThicknessTilesF>, RelTile2i, ThicknessTilesF>((Func<KeyValuePair<Vector2i, ThicknessTilesF>, RelTile2i>) (x => transformCoord(x.Key)), (Func<KeyValuePair<Vector2i, ThicknessTilesF>, ThicknessTilesF>) (x => x.Value));
      ImmutableArray<TerrainVertexRel> verticesFromTiles = EntityLayoutParser.ComputeVerticesFromTiles(immutableArray, this.m_protosDb, dict3, layoutParams.CustomVertexDataLayout.ValueOrNull, layoutParams.CustomVertexTransformFn.ValueOrNull);
      RelTile2f? originTile = new RelTile2f?();
      if (vector2iList.IsNotEmpty<Vector2i>())
        originTile = new RelTile2f?(vector2iList.Select<Vector2i, RelTile2i>(new Func<Vector2i, RelTile2i>(transformCoord)).Aggregate<RelTile2i, RelTile2i>(RelTile2i.Zero, (Func<RelTile2i, RelTile2i, RelTile2i>) ((a, b) => a + b)).RelTile2f / (Fix32) vector2iList.Count + Fix32.Half);
      int collapseVerticesThreshold = layoutParams.CustomCollapseVerticesThreshold ?? verticesFromTiles.Count((Func<TerrainVertexRel, bool>) (x => x.MinTerrainHeight.HasValue || x.MaxTerrainHeight.HasValue)).Sqrt().ToIntFloored();
      return new EntityLayout(((IEnumerable<string>) layout).JoinStrings("\n"), immutableArray, verticesFromTiles, items.ToImmutableArray<IoPortTemplate>(), layoutParams, collapseVerticesThreshold, originTile);

      static int parseHeight(char c, string token, Vector2i pos)
      {
        int height = (int) c - 48;
        if (height <= 0 || height >= 10)
        {
          string message = string.Format("Invalid entity height '{0}'.", (object) c);
          string token1 = token;
          Vector2i? nullable = new Vector2i?(pos);
          int? line = new int?();
          int? col = new int?();
          Vector2i? tokenPos = nullable;
          throw new InvalidEntityLayoutException(message, token1, line, col, tokenPos);
        }
        return height;
      }

      void addVehicleHeight(Vector2i pos, ThicknessTilesF vehicleHeight)
      {
        tileHasVehicleSurface.Add(pos);
        EntityLayoutParser.safeAddVehicleHeight(pos, vehicleHeight, vehicleHeights);
        EntityLayoutParser.safeAddVehicleHeight(pos.IncrementX, vehicleHeight, vehicleHeights);
        EntityLayoutParser.safeAddVehicleHeight(pos.DecrementY, vehicleHeight, vehicleHeights);
        EntityLayoutParser.safeAddVehicleHeight(pos.IncrementX.DecrementY, vehicleHeight, vehicleHeights);
      }

      RelTile2i transformCoord(Vector2i v) => new RelTile2i(v.X - minXy.X, maxY - (v.Y - minXy.Y));

      Option<TerrainTileSurfaceProto> getTileSurface(Proto.ID id)
      {
        if (id == TerrainTileSurfaceProto.PHANTOM_PRODUCT_ID)
          return Option<TerrainTileSurfaceProto>.None;
        TerrainTileSurfaceProto proto;
        if (this.m_protosDb.TryGetProto<TerrainTileSurfaceProto>(id, out proto))
        {
          Assert.That<TerrainTileSurfaceProto>(proto).IsNotNullOrPhantom<TerrainTileSurfaceProto>();
          return (Option<TerrainTileSurfaceProto>) proto;
        }
        Log.Error(string.Format("Tile surface proto '{0}' was not found.", (object) id));
        return Option<TerrainTileSurfaceProto>.None;
      }

      Option<TerrainMaterialProto> getMaterialProto(Proto.ID id)
      {
        if (id == TerrainTileSurfaceProto.PHANTOM_PRODUCT_ID)
          return Option<TerrainMaterialProto>.None;
        TerrainMaterialProto proto;
        if (this.m_protosDb.TryGetProto<TerrainMaterialProto>(id, out proto))
          return (Option<TerrainMaterialProto>) proto;
        Log.Error(string.Format("Terrain material proto '{0}' was not found.", (object) id));
        return Option<TerrainMaterialProto>.None;
      }
    }

    internal static ImmutableArray<TerrainVertexRel> ComputeVerticesFromTiles(
      ImmutableArray<LayoutTile> layoutTiles,
      ProtosDb protosDb,
      Dict<RelTile2i, ThicknessTilesF> vehicleHeights = null,
      string[] customVertexDataLayout = null,
      Func<TerrainVertexRel, char, TerrainVertexRel> customVertexTransformFn = null)
    {
      Assert.That<bool>(customVertexDataLayout == null).IsEqualTo<bool>(customVertexTransformFn == null, "Both or neither `customVertexDataLayout` and `customVertexTransformFn` should be provided.");
      Dict<RelTile2i, TerrainVertexRel> terrainVertices = new Dict<RelTile2i, TerrainVertexRel>(layoutTiles.Length * 2);
      RelTile2i relTile2i;
      for (int index = 0; index < layoutTiles.Length; ++index)
      {
        LayoutTile layoutTile = layoutTiles[index];
        mergeVertex(layoutTile.Coord, layoutTile, index);
        relTile2i = layoutTile.Coord;
        mergeVertex(relTile2i.IncrementX, layoutTile, index);
        relTile2i = layoutTile.Coord;
        mergeVertex(relTile2i.IncrementY, layoutTile, index);
        relTile2i = layoutTile.Coord;
        mergeVertex(relTile2i.AddXy(1), layoutTile, index);
      }
      if (customVertexDataLayout != null && customVertexTransformFn != null)
      {
        for (int index1 = 0; index1 < customVertexDataLayout.Length; ++index1)
        {
          string str = customVertexDataLayout[index1];
          for (int index2 = 0; index2 < str.Length; ++index2)
          {
            RelTile2i key = new RelTile2i(index2, customVertexDataLayout.Length - index1 - 1);
            TerrainVertexRel terrainVertexRel;
            if (!terrainVertices.TryGetValue(key, out terrainVertexRel))
              throw new InvalidEntityLayoutException(string.Format("Failed to set custom vertex data, no vertex at coordinate ({0}, {1}) was found.", (object) index2, (object) index1), line: new int?(index1 + 1), col: new int?(index2 + 1));
            terrainVertices[key] = customVertexTransformFn(terrainVertexRel, str[index2]);
          }
        }
      }
      Set<RelTile2i> verticesUnevenTerrainAround = new Set<RelTile2i>();
      foreach (KeyValuePair<RelTile2i, TerrainVertexRel> keyValuePair in terrainVertices)
      {
        KeyValuePair<RelTile2i, TerrainVertexRel> pair = keyValuePair;
        if (pair.Value.TerrainHeight.HasValue && !pair.Value.TerrainHeight.Value.IsZero)
        {
          verticesUnevenTerrainAround.Add(pair.Key);
          relTile2i = pair.Key;
          checkHeightDiffAndAdd(relTile2i.IncrementX);
          relTile2i = pair.Key;
          checkHeightDiffAndAdd(relTile2i.IncrementY);
          relTile2i = pair.Key;
          checkHeightDiffAndAdd(relTile2i.DecrementX);
          relTile2i = pair.Key;
          checkHeightDiffAndAdd(relTile2i.DecrementY);
        }

        void checkHeightDiffAndAdd(RelTile2i nbrPos)
        {
          verticesUnevenTerrainAround.Add(nbrPos);
          TerrainVertexRel terrainVertexRel1;
          if (!terrainVertices.TryGetValue(nbrPos, out terrainVertexRel1))
            throw new InvalidEntityLayoutException("Vertex with non-default terrain height is at layout edge.");
          ThicknessTilesI thicknessTilesI1 = terrainVertexRel1.TerrainHeight ?? ThicknessTilesI.Zero;
          TerrainVertexRel terrainVertexRel2 = pair.Value;
          ThicknessTilesI thicknessTilesI2 = terrainVertexRel2.TerrainHeight.Value;
          int num = (thicknessTilesI1 - thicknessTilesI2).Abs.Value;
          if (num > 4)
          {
            terrainVertexRel2 = pair.Value;
            throw new InvalidEntityLayoutException(string.Format("Vertex with terrain height {0} has a neighbor on terrain ", (object) terrainVertexRel2.TerrainHeight.Value) + string.Format("height {0} that has ", terrainVertexRel1.TerrainHeight.HasValue ? (object) terrainVertexRel1.TerrainHeight.Value : (object) "0 (None)") + string.Format("larger delta {0} than max allowed {1} tiles.", (object) num, (object) 4));
          }
        }
      }
      if (verticesUnevenTerrainAround.IsNotEmpty)
      {
        TerrainMaterialProto orThrow = protosDb.GetOrThrow<TerrainMaterialProto>(EntityLayoutParser.DEFAULT_HARDENED_MATERIAL);
        foreach (RelTile2i key in verticesUnevenTerrainAround)
        {
          bool exists;
          ref TerrainVertexRel local = ref terrainVertices.GetRefValue(key, out exists);
          Assert.That<bool>(exists).IsTrue();
          local = local.WithTerrainMaterial(orThrow);
        }
      }
      ImmutableArray<TerrainVertexRel> verticesFromTiles = terrainVertices.Values.OrderBy<TerrainVertexRel, int>((Func<TerrainVertexRel, int>) (x => x.Coord.Y)).ThenBy<TerrainVertexRel, int>((Func<TerrainVertexRel, int>) (x => x.Coord.X)).ToImmutableArray<TerrainVertexRel>();
      if (verticesFromTiles.Count((Func<TerrainVertexRel, bool>) (x => x.ContributingTiles >= 4)) >= 4)
        verticesFromTiles = verticesFromTiles.Map<TerrainVertexRel>((Func<TerrainVertexRel, TerrainVertexRel>) (x => x.ContributingTiles < 4 ? x.WithExtraConstraint(LayoutTileConstraint.NoRubbleAfterCollapse) : x));
      return verticesFromTiles;

      ThicknessTilesF? getVehicleHeight(RelTile2i coord)
      {
        if (vehicleHeights == null)
          return new ThicknessTilesF?();
        ThicknessTilesF thicknessTilesF;
        return vehicleHeights.TryGetValue(coord, out thicknessTilesF) ? new ThicknessTilesF?(thicknessTilesF == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL ? EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL : thicknessTilesF + EntityLayoutParser.VEHICLE_SURFACE_EXTRA_THICKNESS) : new ThicknessTilesF?();
      }

      void mergeVertex(RelTile2i newCoord, LayoutTile t, int index)
      {
        bool exists;
        ref TerrainVertexRel local = ref terrainVertices.GetRefValue(newCoord, out exists);
        if (!exists)
        {
          local = new TerrainVertexRel(newCoord, t.OccupiedThickness, t.Constraint, t.TerrainMaterialProto, t.TerrainHeight, t.MinTerrainHeight, t.MaxTerrainHeight, getVehicleHeight(newCoord), 1, index);
        }
        else
        {
          if (local.TerrainMaterial.HasValue && t.TerrainMaterialProto.HasValue)
            Assert.That<TerrainMaterialProto>(local.TerrainMaterial.Value).IsEqualTo<TerrainMaterialProto>(t.TerrainMaterialProto.Value, "Two different materials contributing to a single terrain vertex. We need a way to resolve this.");
          local = new TerrainVertexRel(newCoord, new ThicknessIRange(local.OccupiedThickness.From.Min(t.OccupiedThickness.From), local.OccupiedThickness.To.Max(t.OccupiedThickness.To)), local.Constraint | t.Constraint, local.TerrainMaterial | t.TerrainMaterialProto, local.TerrainHeight.HasValue ? (t.TerrainHeight.HasValue ? new ThicknessTilesI?(local.TerrainHeight.Value.Max(t.TerrainHeight.Value)) : local.TerrainHeight) : t.TerrainHeight, local.MinTerrainHeight.HasValue ? (t.MinTerrainHeight.HasValue ? new ThicknessTilesI?(local.MinTerrainHeight.Value.Min(t.MinTerrainHeight.Value)) : local.MinTerrainHeight) : t.MinTerrainHeight, local.MaxTerrainHeight.HasValue ? (t.MaxTerrainHeight.HasValue ? new ThicknessTilesI?(local.MaxTerrainHeight.Value.Max(t.MaxTerrainHeight.Value)) : local.MaxTerrainHeight) : t.MaxTerrainHeight, local.VehicleSurfaceRelHeight, local.ContributingTiles + 1, local.LowestTileIndex.Min(index));
        }
      }
    }

    private static void safeAddVehicleHeight(
      Vector2i pos,
      ThicknessTilesF thickness,
      Dict<Vector2i, ThicknessTilesF> vehicleHeights)
    {
      if (thickness == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL)
      {
        vehicleHeights.AddIfNotPresent(pos, thickness);
      }
      else
      {
        Assert.That<ThicknessTilesF>(thickness).IsNotNegative();
        ThicknessTilesF other;
        if (vehicleHeights.TryGetValue(pos, out other))
        {
          if (other == EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL)
            vehicleHeights[pos] = thickness;
          else
            Assert.That<ThicknessTilesF>(thickness).IsEqualTo(other, "Vehicle surface heights miss-match.");
        }
        else
          vehicleHeights.Add(pos, thickness);
      }
    }

    private static EntityLayoutParser.PortLayout parsePortToken(string token, int y, int x)
    {
      char? nullable1 = new char?();
      char? nullable2 = new char?();
      char? nullable3 = new char?();
      for (int index = 0; index < 3; ++index)
      {
        if (token[index] >= 'A' && token[index] <= 'Z')
          nullable2 = new char?(token[index]);
        else if (((IEnumerable<char>) EntityLayoutParser.PORT_DIRECTIONS).Contains<char>(token[index]))
          nullable1 = new char?(token[index]);
        else
          nullable3 = new char?(token[index]);
      }
      if (!nullable1.HasValue || !nullable2.HasValue || !nullable3.HasValue)
        throw new InvalidEntityLayoutException("Layout token '" + token + "' failed to parse as a port.", line: new int?(y + 1), col: new int?(x * 3 + 1));
      return new EntityLayoutParser.PortLayout()
      {
        Token = token,
        DirectionChar = nullable1.Value,
        ShapeChar = nullable3.Value,
        Position = new Vector2i(x, y),
        MachineHeight = 0,
        Line = y + 1,
        Column = x * 3 + 1,
        Name = nullable2.Value
      };
    }

    private static EntityLayoutParser.PortExtraInfo parsePortInfoToken(string token, int y, int x)
    {
      char ch = char.MinValue;
      int num1 = 0;
      for (int index = 0; index < 3; ++index)
      {
        if (token[index] != ' ')
        {
          if (char.IsNumber(token[index]))
          {
            int num2 = (int) token[index] - 48;
            if (num2 <= 0 || num2 >= 10)
              throw new InvalidEntityLayoutException(string.Format("Invalid port height '{0}' in token '{1}'.", (object) token[index], (object) token), line: new int?(y + 1), col: new int?(x * 3 + 1));
            num1 = num2 - 1;
          }
          else
            ch = ch == char.MinValue ? token[index] : throw new InvalidEntityLayoutException(string.Format("Invalid port spec char '{0}' in token '{1}'.", (object) token[index], (object) token), line: new int?(y + 1), col: new int?(x * 3 + 1));
        }
      }
      if (ch == char.MinValue)
        throw new InvalidEntityLayoutException("Port shape char was not specified in token '" + token + "'.", line: new int?(y + 1), col: new int?(x * 3 + 1));
      return new EntityLayoutParser.PortExtraInfo()
      {
        HeightZ = num1,
        Shape = ch,
        Token = token
      };
    }

    private IoPortShapeProto getPortShape(EntityLayoutParser.PortLayout port, char shapeChar)
    {
      IoPortShapeProto portShape;
      if (this.m_portShapesCache.TryGetValue(shapeChar, out portShape))
        return portShape;
      this.m_portShapesCache.Clear();
      foreach (IoPortShapeProto ioPortShapeProto1 in this.m_protosDb.All<IoPortShapeProto>())
      {
        IoPortShapeProto ioPortShapeProto2;
        if (this.m_portShapesCache.TryGetValue(ioPortShapeProto1.LayoutChar, out ioPortShapeProto2))
          throw new InvalidEntityLayoutException(string.Format("Duplicate layout char '{0}' used in ", (object) ioPortShapeProto1.LayoutChar) + string.Format("protos {0} and {1}", (object) ioPortShapeProto1, (object) ioPortShapeProto2), port.Token, new int?(port.Line), new int?(port.Column));
        this.m_portShapesCache.Add(ioPortShapeProto1.LayoutChar, ioPortShapeProto1);
      }
      Assert.That<Dict<char, IoPortShapeProto>>(this.m_portShapesCache).IsNotEmpty<char, IoPortShapeProto>("No port shapes defined!");
      if (this.m_portShapesCache.TryGetValue(shapeChar, out portShape))
        return portShape;
      throw new InvalidEntityLayoutException(string.Format("Undefined shape '{0}' of port '{1}'.", (object) shapeChar, (object) port.Name), port.Token, new int?(port.Line), new int?(port.Column));
    }

    /// <summary>
    /// Computes port type based on direction char and presence of entity.
    /// </summary>
    private static bool tryGetPortType(
      EntityLayoutParser.PortLayout port,
      Dict<Vector2i, ThicknessIRange> entityHeights,
      out IoPortType portType,
      out Vector2i portDir,
      out string error)
    {
      int x = port.Position.X;
      int y = port.Position.Y;
      error = "";
      switch (port.DirectionChar)
      {
        case '+':
          Direction90? nullable = new Direction90?();
          Vector2i vector2i = Vector2i.Zero;
          foreach (Direction90 allFourDirection in Direction90.AllFourDirections)
          {
            Vector2i directionVector = allFourDirection.DirectionVector;
            if (isEntityAt(x + directionVector.X, y + directionVector.Y))
            {
              if (!nullable.HasValue)
              {
                nullable = new Direction90?(allFourDirection);
                vector2i = -directionVector;
              }
              else
              {
                error = "More than one entity tile found around the port.";
                goto label_21;
              }
            }
          }
          if (nullable.HasValue)
          {
            portDir = vector2i;
            portType = IoPortType.Any;
            return true;
          }
          error = "Entity not found around the port.";
          break;
        case '<':
        case '>':
          if (isEntityAt(x - 1, y))
          {
            portDir = new Vector2i(1, 0);
            portType = port.DirectionChar == '>' ? IoPortType.Output : IoPortType.Input;
            return true;
          }
          if (isEntityAt(x + 1, y))
          {
            portDir = new Vector2i(-1, 0);
            portType = port.DirectionChar == '>' ? IoPortType.Input : IoPortType.Output;
            return true;
          }
          error = "Entity not found on either +x or -x neighbors of the port.";
          break;
        case '^':
        case 'v':
          if (isEntityAt(x, y - 1))
          {
            portDir = new Vector2i(0, 1);
            portType = port.DirectionChar == 'v' ? IoPortType.Output : IoPortType.Input;
            return true;
          }
          if (isEntityAt(x, y + 1))
          {
            portDir = new Vector2i(0, -1);
            portType = port.DirectionChar == 'v' ? IoPortType.Input : IoPortType.Output;
            return true;
          }
          error = "Entity not found on either +y or -y neighbors of the port.";
          break;
      }
label_21:
      portType = IoPortType.Any;
      portDir = new Vector2i();
      return false;

      bool isEntityAt(int xCoord, int yCoord)
      {
        return entityHeights.TryGetValue(new Vector2i(xCoord, yCoord), out ThicknessIRange _);
      }
    }

    static EntityLayoutParser()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityLayoutParser.PORT_DIRECTIONS = new char[5]
      {
        '^',
        '>',
        'v',
        '<',
        '+'
      };
      EntityLayoutParser.VEHICLE_SURFACE_EXTRA_THICKNESS = 0.05.TilesThick();
      EntityLayoutParser.DEFAULT_HARDENED_SURFACE = IdsCore.TerrainTileSurfaces.DefaultConcrete;
      EntityLayoutParser.DEFAULT_HARDENED_MATERIAL = IdsCore.TerrainMaterials.HardenedRock;
      EntityLayoutParser.DEFAULT_TOKENS = new CustomLayoutToken[9]
      {
        new CustomLayoutToken("{0}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: h))),
        new CustomLayoutToken("10}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: 10 + h))),
        new CustomLayoutToken("(0)", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: h, terrainSurfaceHeight: new int?(0)))),
        new CustomLayoutToken("10)", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: 10 + h, terrainSurfaceHeight: new int?(0)))),
        new CustomLayoutToken("[0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("10]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 10 + h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("~0~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: h, constraint: LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("10~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: 10 + h, constraint: LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("_0_", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? terrainSurfaceHeight = new int?(0);
          Fix32? nullable1 = new Fix32?((Fix32) (h - 1));
          Proto.ID? nullable2 = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = nullable1;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable2;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      };
    }

    private struct PortExtraInfo
    {
      public int HeightZ;
      public char Shape;
      public string Token;
    }

    private class PortLayout
    {
      public char Name;
      public char DirectionChar;
      public char ShapeChar;
      public Vector2i Position;
      public int MachineHeight;
      public string Token;
      public int Line;
      public int Column;

      public PortLayout()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
