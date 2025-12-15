using Mafi;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Designation;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Terrain Designation Management
    /// </summary>
    public sealed partial class TerrainTowerEntity
    {
        /// <summary>
        /// Validate if a Designation is within the Tower Area
        /// </summary>
        /// <param name="designation"><see cref="IDesignation"/></param>
        /// <returns>TRUE if within <see cref="Area"/></returns>
        private bool isWithinArea(TerrainDesignation designation) => Area.ContainsTile(designation.OriginTileCoord);

        /// <summary>
        /// Update <see cref="m_managedDesignations"/> when designations are added within the Tower Area
        /// </summary>
        /// <param name="designation">TerrainDesignation provided</param>
        private void onDesignationAdded(TerrainDesignation designation)
        {
            if (!designation.Prototype.IsTerraforming || !isWithinArea(designation)) { return; }

            designation.AddManagingTower(this);
            if (m_managedDesignations.Add(designation))
            {
                if ((designation.ProtoId == IdsCore.TerrainDesignators.MiningDesignator
                        || designation.ProtoId == IdsCore.TerrainDesignators.LevelDesignator)
                    && designation.IsMiningNotFulfilled)
                {
                    m_unfulfilledMiningDesignations.AddAndAssertNew(designation);
                }
                if ((designation.ProtoId == IdsCore.TerrainDesignators.DumpingDesignator
                        || designation.ProtoId == IdsCore.TerrainDesignators.LevelDesignator)
                    && designation.IsDumpingNotFulfilled)
                {
                    m_unfulfilledDumpingDesignations.AddAndAssertNew(designation);
                }
            }

            updateTerrainNotifications();
        }

        /// <summary>
        /// If a Designation is fulfilled or unfulfilled (collapse), update the Tower Notifications, add/remove from temp variables
        /// </summary>
        /// <param name="designation">Designation that was changed</param>
        private void onDesignationFulfilledChanged(TerrainDesignation designation)
        {
            if (!designation.Prototype.IsTerraforming || !isWithinArea(designation) || !designation.Prototype.ShouldUpdateTowerNotificationOnFulfilledChanged)
            {
                return;
            }

            Assert.That(isWithinArea(designation)).IsTrue("designation not within Area");

            updateTerrainNotifications();

            Proto.ID protId = designation.ProtoId;

            if (protId == IdsCore.TerrainDesignators.MiningDesignator || protId == IdsCore.TerrainDesignators.LevelDesignator)
            {
                if (designation.IsMiningFulfilled)
                {
                    if (!m_unfulfilledMiningDesignations.Remove(designation))
                    {
                        //We can enter here if a Level Designation is used for Dumping. As Mining is already fulfilled, it was never added to unfulfilledMiningDesignations
                        //Logger.Warning("{0} Designation was not in unfulfilled mining designations", nameof(onDesignationFulfilledChanged));
                    }
                }
                else
                {
                    //Handle spill over causing designation to be unfulfilled
                    m_unfulfilledMiningDesignations.AddAndAssertNew(designation);
                }
            }

            if (protId == IdsCore.TerrainDesignators.DumpingDesignator || protId == IdsCore.TerrainDesignators.LevelDesignator)
            {
                if (designation.IsDumpingFulfilled)
                {
                    if (!m_unfulfilledDumpingDesignations.Remove(designation))
                    {
                        //We can enter here if a Level Designation is used for Mining. As Dumping is already fulfilled, it was never added to unfulfilledDumpingDesignations
                        //Logger.Warning("{0} Designation was not in unfulfilled dumping designations", nameof(onDesignationFulfilledChanged));
                    }
                }
                else
                {
                    //Handle terrain collapse over causing designation to be unfulfilled
                    m_unfulfilledDumpingDesignations.AddAndAssertNew(designation);
                }
            }
        }

        /// <summary>
        /// Update <see cref="m_managedDesignations"/> when designations are removed within the Tower Area
        /// </summary>
        /// <param name="designation">TerrainDesignation provided</param>
        private void onDesignationRemoved(TerrainDesignation designation)
        {
            if (!designation.Prototype.IsTerraforming || !isWithinArea(designation))
            {
                return;
            }

            Assert.That(isWithinArea(designation)).IsTrue("designation not within Area");

            designation.RemoveManagingTower(this);

            //TODO: Add logic for removing from unfulfilled lists, if needed
            _ = m_managedDesignations.Remove(designation);

            _ = m_unfulfilledMiningDesignations.Remove(designation);
            _ = m_unfulfilledDumpingDesignations.Remove(designation);

            if (designation.Equals(m_tmpMineDesignation)) { m_tmpMineDesignation = null; }
            if (designation.Equals(m_tmpDumpDesignation)) { m_tmpDumpDesignation = null; }

            updateTerrainNotifications();
        }

        /// <summary>
        /// Clear all designations from <see cref="m_managedDesignations"/> controlled via <see cref="onDesignationRemoved"/>
        /// </summary>
        private void removeAllManagedDesignations()
        {
            //Try to be more efficient by removing all at once
            //We don't need to check if if it's a Terraforming Designation, or that it's within the Area, as they're filtered when added
#if false
            //Have to loop to RemoveManagingTower(this)
            foreach (TerrainDesignation designation in m_managedDesignations.ToArray())
            {
                onDesignationRemoved(designation);
            }
#else
            m_managedDesignations.ForEach(x => x.RemoveManagingTower(this));

            //Fully clear remaining designations from unfulfilled cached list.
            m_managedDesignations.Clear();
            m_managedDesignations.TrimExcess();

            m_unfulfilledMiningDesignations.Clear();
            m_unfulfilledMiningDesignations.TrimExcess();

            m_unfulfilledDumpingDesignations.Clear();
            m_unfulfilledDumpingDesignations.TrimExcess();

            m_tmpMineDesignation = null;

            m_tmpDumpDesignation = null;
#endif
            Assert.That(m_managedDesignations).IsEmpty("ManagedDesignations not Empty");
            Assert.That(m_unfulfilledMiningDesignations).IsEmpty("UnfulfilledMiningDesignation not Empty");
            Assert.That(m_unfulfilledDumpingDesignations).IsEmpty("UnfulfilledDumpingDesignation not Empty");
            Assert.That(m_tmpMineDesignation).IsNull("tmpMineDesignation is not null");
            Assert.That(m_tmpDumpDesignation).IsNull("tmpDumpDesignation is not null");
        }
    }
}