using System;
using System.Linq;

using Mafi;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Props;

using TerrainTower.Extras;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Terrain Manipulation
    /// </summary>
    public sealed partial class TerrainTowerEntity
    {
        /// <summary>
        /// Clear any Custom Surfaces from a tileAndIndex if it is above the targetHeight
        /// </summary>
        /// <param name="tileAndIndex">Tile to search</param>
        /// <param name="targetHeight">Height to check for</param>
        private void clearCustomSurface(Tile2iAndIndex tileAndIndex, HeightTilesF targetHeight)
        {
            if (ContextTerrainManager.TryGetTileSurface(tileAndIndex.Index, out TileSurfaceData tileSurfaceData))
            {
                if (!tileSurfaceData.IsAutoPlaced && tileSurfaceData.Height > targetHeight)
                {
                    ContextTerrainManager.ClearCustomSurface(tileAndIndex);
                }
            }
        }

        /// <summary>
        /// Remove props from a tile (Bushes etc)
        /// - Taken from MiningJob.handleMining()
        /// </summary>
        /// <param name="tile">Tile2i of location</param>
        private void clearProps(Tile2i tile)
        {
            TerrainPropsManager tpm = m_designationManager.TerrainPropsManager;

            m_designationManager.TreesManager.RemoveStumpAtTile(tile);
            if (tpm.TerrainTileToProp.TryGetValue(tile.AsSlim, out TerrainPropId key))
            {
                if (!tpm.TerrainProps.TryGetValue(key, out TerrainPropData propData))
                {
                    Logger.Warning("clearProps: PropData not found for {0}", key);
                }
                else if (!tpm.TryRemovePropAtTile(tile, false))
                {
                    Logger.Warning("clearProps: Failed to remove prop {0}", key);
                }
                else
                {
                    if (propData.Proto.ProductWhenHarvested.IsNotEmpty)
                    {
                        //Send Prop product to shipyard
                        ProductQuantity pq = propData.Proto.ProductWhenHarvested.ScaledBy(propData.Scale);
                        m_productsManager.ProductCreated(pq.Product, pq.Quantity, CreateReason.MinedFromTerrain);
                        Context.AssetTransactionManager.StoreClearedProduct(pq);
                    }
                }
            }
        }

        /// <summary>
        /// Called from <see cref="IEntityWithSimUpdate.SimUpdate"/>
        /// - Tries to Mine and (if Unable) Dump nearest Designation
        /// - Handled Differently than Sorting, only conducted per Timer, instead of holding a 'Finished' timer to start immediately
        /// </summary>
        /// <returns>TRUE if any Terrain Manipulation occured</returns>
        private bool simStepTerrainProcessing()
        {
            //FUTURE: Possible Changes - Allow 1x Mine & 1x Dump per round
            //FUTURE: Possible CHanges - Increase MAX_MINE or MAX_DUMP if Boosted
            if (!m_terrainTimer.IsFinished)
            {
                return false;
            }

            bool ActionThisRound = false;

            //TODO: Check if this has too much overhead, and if so we'll need to re-do the logic to find/process a small subset of Designations
            if (Functions.TryFindUnfulfilledDesignation(m_unfulfilledDumpingDesignations, Position2f.Tile2i, Random, out TerrainDesignation bestDumpDesig))
            {
                m_tmpDumpDesignation = bestDumpDesig;
            }
            else
            {
                m_missingDesignationNotif.Activate(this);
            }

            //Conduct Mining if we have Mixed Capacity Left, and a Mining Designation
            if (MixedCapacityLeft > Quantity.Zero) { ActionThisRound |= tryMineDesignation(m_tmpMineDesignation, TerrainTowerProto.MAX_MINE_QUANTITY); }
            //Conduct Dumping if we have Dump Total, and a Dumping Designation
            if (DumpTotal.IsPositive) { ActionThisRound |= tryDumpDesignation(m_tmpDumpDesignation, TerrainTowerProto.MAX_DUMP_QUANTITY); }

            //Unity = 5x faster & Idle = 2x slower.
            /*
            * Boost + Active = 1 second
            * Boost + Not Active = 2.5 seconds = If Unity is applied, but no action for a while (Idle) = re-check every 2.5 seconds
            * NoBoost + Active = 5 seconds
            * NoBoost + Not Active = 10 Seconds
            */

            Duration duration = Prototype.TerrainDuration / (IsBoostRequested ? 5 : 1) * (ActionThisRound ? 1 : 2);
            m_terrainTimer.Start(duration);
            return ActionThisRound;
        }

        /// <summary>
        /// Tries to dump to the designation. Pulls directly from <see cref="m_dumpBuffers"/>
        /// </summary>
        /// <param name="designation">Designation to try to dump on</param>
        /// <param name="maxDumpQuantity">Max quantity to dump. Used as local variale to subtract from</param>
        /// <returns>TRUE if any dumping occured</returns>
        private bool tryDumpDesignation(TerrainDesignation designation, Quantity maxDumpQuantity)
        {
            if (designation == null || designation.IsDumpingFulfilled || maxDumpQuantity.IsNotPositive || DumpTotal.IsNotPositive)
            {
                //Logger.InfoDebug("tryDumpDesignation: IsDumpingFulfilled {0} - maxDumpQuantity {1} - DumpTotal {2}", designation.IsDumpingFulfilled, maxDumpQuantity, DumpTotal);
                //Invalid Designation, already fulfilled, or no quantity to dump or DumpTotal is empty
                m_missingDumpItemNotif.NotifyIff(DumpTotal.IsNotPositive, this);
                return false;
            }

            bool Action = false;
            /*
            Changed from Dumping based on Quantity to Thickness to allow smaller Partial Quantities to be dumped without rounding issues.
            1. Get Buffer Quanity to Thickness
            2. Dump based on Thickness (This allows partial/small values without rounding issues of Quantity vs PartialQuantity)
            3. Sum all dumped Thickness
            4. Convert Thickness Dumped to Quantity, and remove from Buffer
            */

            //First Buffer with value
            IProductBuffer buff = DumpBuffers.First(x => x.Quantity.IsPositive);

            // Min (Dump Buffer | maxDumpQuantity)
            LooseProductQuantity looseProductQuantity = new LooseProductQuantity(buff.Product.DumpableProduct.Value, maxDumpQuantity.Min(buff.Quantity));

            //Convert to Thickness (Total thickness dumpable during this process)
            ThicknessTilesF dumpTotalThickness = looseProductQuantity.ToTerrainThickness().Thickness;

            //FUTURE: Check if we can change to ForEachNonFulfilledTile - Test with Mining
            //Loop through 5x5 area around Designation (1 Designation = 4x4 = 25 index points) - This includes the borders
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (dumpTotalThickness.IsNotPositive)
                    {
                        //No thickness left to Dump
                        break;
                    }

                    //Get Tile and Index for TerrainManager
                    Tile2i tile = designation.Data.OriginTile + new RelTile2i(i, j);
                    Tile2iAndIndex tileAndIndex = tile.ExtendIndex(ContextTerrainManager);

                    HeightTilesF targetHeight = designation.GetTargetHeightAt(tile);
                    HeightTilesF terrainHeight = ContextTerrainManager.GetHeight(tile);

                    if (designation.IsDumpingFulfilledAt(tile) || terrainHeight >= targetHeight)
                    {
                        // No Dumping, or no quantity to dump, or terrain is already at or above target terrainHeight
                        continue;
                    }

                    //Terrain Difference & Max Dumpable Thickness & Max Terrain Modification Thickness
                    ThicknessTilesF dumpTileThickness = (targetHeight - terrainHeight).Min(dumpTotalThickness).Min(TerrainTowerProto.MAX_TERRAIN_MOD_THICKNESS);

                    //Dump Product to Terrain
                    ContextTerrainManager.DumpMaterial(tileAndIndex, new TerrainMaterialThicknessSlim(looseProductQuantity.Product.TerrainMaterial.Value.SlimId, dumpTileThickness));

                    dumpTotalThickness -= dumpTileThickness;

                    Action = true;
                } // J Loop
            } // I Loop

            ProductQuantity totalDumped = new TerrainMaterialThickness(looseProductQuantity.Product.TerrainMaterial.Value, dumpTotalThickness).ToProductQuantityRounded();

            if ((buff.Quantity - totalDumped.Quantity) <= Quantity.One)
            {
                //Insignificant quantity, unable to convert to TerrainMaterialThickness for processing. Add to totalDumped and remove from buffer
                //Don't directly reference buff.ProductQuantity, it just wipes the entire DumpTotal
                totalDumped = new ProductQuantity(buff.Product, buff.Quantity);
            }

            buff.RemoveExactly(totalDumped.Quantity);
            DumpTotal -= totalDumped.Quantity;
            Context.ProductsManager.ProductDestroyed(totalDumped, DestroyReason.DumpedOnTerrain);

            return Action;
        }

        /// <summary>
        /// Tries to mine a designation. Adds directly to <see cref="m_productsData"/>
        /// </summary>
        /// <param name="designation">Designation to try to mine on</param>
        /// <param name="maxDumpQuantity">Max quantity to mine. Used as local variale to subtract from to prevent over work</param>
        /// <returns>TRUE if any mmining occured</returns>
        private bool tryMineDesignation(TerrainDesignation designation, Quantity maxMineQuantity)
        {
            //Local function to allow re-checking, and reduce duplicate checks.
            bool ValidateDesignation()
            {
                //TODO:  MixedTotal >= Capacity OR during tile Loop, MixedCapacityLeft <= maxMineQuantity
                bool result = true;
                if (designation == null)
                {
                    Log.Warning("tryMineDesignation: Designation is null, unable to mine");
                    result = false;
                }
                if (designation.IsMiningFulfilled)
                {
                    Log.Warning("tryMineDesignation: Designation is already fulfilled, unable to mine");
                    result = false;
                }
                if (!Area.ContainsTile(designation.OriginTileCoord))
                {
                    Log.Warning("tryMineDesignation: Designation is outside of Tower Area, unable to mine");
                    result = false;
                }
                if (maxMineQuantity.IsNotPositive)
                {
                    Log.Warning("tryMineDesignation: maxMineQuantity is not positive, unable to mine");
                    result = false;
                }
                if (MixedTotal > Capacity)
                {
                    Log.Warning("tryMineDesignation: MixedTotal is greater than Capacity, unable to mine");
                    result = false;
                }
                return result;
            }

            if (!ValidateDesignation()) return false;


            bool hasMined = false;

            for (int i = 0; i <= designation.SizeTiles; i++)
            {
                for (int j = 0; j <= designation.SizeTiles; j++)
                {
                    Tile2i tile = designation.Data.OriginTile + new RelTile2i(j, i);

                    //Not allowed to mine, exit function
                    if (maxMineQuantity.IsNotPositive) { return hasMined; }

                    //Nothing to mine at tile, skip loop
                    if (designation.IsMiningFulfilledAt(tile)) { continue; }

                    HeightTilesF targetHeight = designation.GetTargetHeightAt(tile);
                    HeightTilesF terrainHeight = ContextTerrainManager.GetHeight(tile);
                    Tile2iAndIndex tileAndIndex = tile.ExtendIndex(ContextTerrainManager);

                    //Clear Custom Surface if it exists and is above target terrainHeight
                    //Process before terrain Height Check to allow clearing of Props
                    //Uncleared Props flag designation as Unfulfilled
                    clearCustomSurface(tileAndIndex, targetHeight);
                    clearProps(tile);

                    // Terrain is already at or below target terrainHeight, skip loop
                    if (terrainHeight <= targetHeight) { continue; }

                    hasMined |= tryMineDesignation_MineLayer(tileAndIndex, targetHeight, ref maxMineQuantity, true);
                    //hasMined |= tryMineDesignation_MineLayer(tileAndIndex, targetHeight, ref maxMineQuantity, false);
                }
            }
            return hasMined;
        }

        /// <summary>
        /// Mine Layer at a specific TileAndIndex - Called from <see cref="tryMineDesignation"/>
        /// </summary>
        /// <param name="tileAndIndex">Location to Mine</param>
        /// <param name="targetHeight">Height to Mine to</param>
        /// <param name="maxMineQuantity">Ref value of Max Mined Quantity per SimUpdate</param>
        /// <param name="layer">TRUE if FIRST Layer else SECOND Layer</param>
        /// <returns>TRUE if Mining has occured</returns>
        private bool tryMineDesignation_MineLayer(Tile2iAndIndex tileAndIndex, HeightTilesF targetHeight, ref Quantity maxMineQuantity, bool firstLayer)
        {
            //CHANGES: Removed Ability to mine from Second Layer
            //Not enough layers to mine
            //if (ContextTerrainManager.GetLayersCountNoBedrock(tileAndIndex.Index) < (firstLayer ? 1 : 2))
            //{
            //    Logger.InfoDebug("tryMineDesignation_MineLayer: Not enough layers to mine: FirstLayer? {0}: LayersCount {1}: Index {2}", firstLayer, ContextTerrainManager.GetLayersCountNoBedrock(tileAndIndex.Index), tileAndIndex.Index);
            //    return false;
            //}

            //TerrainMaterialThickness terrainLayer = firstLayer ? ContextTerrainManager.GetFirstLayer(tileAndIndex.Index) : ContextTerrainManager.GetSecondLayer(tileAndIndex.Index);
            TerrainMaterialThickness terrainLayer = ContextTerrainManager.GetFirstLayer(tileAndIndex.Index);

            //Not allowed to mine this product
            //HACK: Allow ALL products to be mined without consideration to type/count etc
            //!(AllSupportedProducts.Contains(terrainLayer.Material.MinedProduct) || !AllowedProducts.Contains(terrainLayer.Material.MinedProduct))
            //AllowedProducts = m_productsData.Keys
            if (!(AllSupportedProducts.Contains(terrainLayer.Material.MinedProduct) || AllowedProducts.Contains(terrainLayer.Material.MinedProduct)))
            {
                return false;
            }

            ThicknessTilesF maxThickness = Functions.MinThickness(
                TerrainTowerProto.MAX_TERRAIN_MOD_THICKNESS,
                ContextTerrainManager.GetHeight(tileAndIndex.Index) - targetHeight,
                terrainLayer.Material.QuantityToThickness(maxMineQuantity));

            //Not thick enough to mine
            //TODO: Possible issue with rounding down to 0 thickness leaving tiny clumps and designation being considered 'Complete'
            if (maxThickness.IsNotPositive)
            {
                return false;
            }

            //TerrainMaterialThicknessSlim miningOutput = firstLayer ? ContextTerrainManager.MineMaterial(tileAndIndex, maxThickness) : ContextTerrainManager.MineMaterialFromSecondLayer(tileAndIndex, maxThickness);
            TerrainMaterialThicknessSlim miningOutput = ContextTerrainManager.MineMaterial(tileAndIndex, maxThickness);

            //FUTURE: Check if we need maxThickness or MAX_TERRAIN_MOD_THICKNESS
            //TEST if this removes the weird spikes on Terrain
            //ContextTerrainManager.DisruptExactly(tileAndIndex, maxThickness);
            ContextTerrainManager.Disrupt(tileAndIndex, maxThickness);

            //No Mining Output
            if (miningOutput.Thickness.IsZero)
            {
                return false;
            }

            PartialProductQuantity ppq = miningOutput.ToPartialProductQuantity(ContextTerrainManager);
            //TODO: Some rounding loss, Change TerrainTowerProductData to PartialQuantity???
            Quantity quantityMined = ppq.Quantity.IntegerPart;

            if (!m_productsData.ContainsKey(ppq.Product))
            {
                addProductDataFor(ppq.Product);
            }
            m_productsData[ppq.Product].UnsortedQuantity += quantityMined;
            MixedTotal += quantityMined;
            updateMixedBufferNotifications();
            maxMineQuantity -= quantityMined;

            return true;
        }
    }
}