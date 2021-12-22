using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Vuforia;

namespace VuforiaManager
{
    public class DisableVuforia : MonoBehaviour
    {
        public void StopVuforia()
        {
            if (VuforiaBehaviour.Instance.enabled)
            {
                DeactivateActiveDataSets(true);
                VuforiaBehaviour.Instance.enabled = false;
            }
        }
        public void ViewModelOnTrackingFound()
        {
            var stateManager = TrackerManager.Instance.GetStateManager();
            var objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        }

        void OnDestroy()
        {
            // Below process normally happens when VuforiaBehaviour is destroyed/created.
            // For the MRTK sample, VuforiaBehaviour exists in the Base scene throughout the application.
            var vuforiaBehaviourGameObject = VuforiaBehaviour.Instance.gameObject;
            DestroyImmediate(VuforiaBehaviour.Instance);
            vuforiaBehaviourGameObject.AddComponent<VuforiaBehaviour>();
        }

        public void DeactivateActiveDataSets(bool destroyDataSets)
        {
            var objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            var stateManager = TrackerManager.Instance.GetStateManager();

            if (objectTracker != null && objectTracker.IsActive)
            {
                List<DataSet> allDataSets = objectTracker.GetDataSets().ToList();
                List<DataSet> activeDataSets = objectTracker.GetActiveDataSets().ToList();

                objectTracker.Stop();

                // Deactivate all the DataSets
                foreach (DataSet ds in activeDataSets)
                {
                    // When in PlayMode, the VuforiaEmulator.xml dataset (used by GroundPlane) is managed by Vuforia.
                    var dsFileName = Path.GetFileName(ds.Path);

                    if (dsFileName != "VuforiaEmulator.xml")
                    {
                        objectTracker.DeactivateDataSet(ds);
                    }
                }

                // Destroy all the DataSets
                foreach (DataSet ds in allDataSets)
                {
                    var dsFileName = Path.GetFileName(ds.Path);

                    if (dsFileName != "VuforiaEmulator.xml")
                    {
                        DestroyTrackableBehavioursForDataSet(ds);

                        if (destroyDataSets)
                        {
                            objectTracker.DestroyDataSet(ds, false);
                        }
                    }
                }

                objectTracker.Start();
            }
        }

        private void DestroyTrackableBehavioursForDataSet(DataSet dataSet)
        {
            var stateManager = TrackerManager.Instance.GetStateManager();

            if (stateManager != null)
            {
                var trackables = dataSet.GetTrackables();
                foreach (Trackable trackable in trackables)
                {
                    stateManager.DestroyTrackableBehavioursForTrackable(trackable, true);
                }

                var vumarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();

                if (vumarkManager != null)
                {
                    var vumarkTrackableBehaviours = vumarkManager.GetAllBehaviours();
                    foreach (VuMarkBehaviour vb in vumarkTrackableBehaviours)
                    {
                        stateManager.DestroyTrackableBehavioursForTrackable(vb.Trackable, true);
                        if (vb && vb.gameObject)
                        {
                            var obj = vb.gameObject;
                            Destroy(vb);
                            Destroy(obj);
                        }
                    }

                    var allRemainingVuMarkBehaviours = FindObjectsOfType<VuMarkBehaviour>();
                    foreach (VuMarkBehaviour vb in allRemainingVuMarkBehaviours)
                    {
                        Debug.Log("Manually Destroying VuMark GameObject.");
                        Destroy(vb);

                    }
                }
            }
        }
    }

}
