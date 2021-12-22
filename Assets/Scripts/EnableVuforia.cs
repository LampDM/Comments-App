using UnityEngine;
using Vuforia;
using System.IO;

namespace VuforiaManager
{
    
    
    public class EnableVuforia : MonoBehaviour
    {
        //Enter the target Data Base name which you want to use
        [SerializeField]
        private string targetDataBase = null;

        public void OnEnable()
        {
            InitVuforia();
        }

        public void InitVuforia()
        {
            Debug.Log("Enable Vuforia...");
            if (VuforiaBehaviour.Instance && !VuforiaBehaviour.Instance.isActiveAndEnabled)
            {
                VuforiaBehaviour.Instance.enabled = true;
            }

            if (!VuforiaARController.Instance.HasStarted)
            {
                VuforiaARController.Instance.RegisterVuforiaStartedCallback(VuforiaStart);
            }
            else
            {
                VuforiaStart();
            }
        }

        private void VuforiaStart()
        {
            var objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            var stateManager = TrackerManager.Instance.GetStateManager();

            // We need to call this for cleaning out VuMarks
            stateManager.ReassociateTrackables();

            bool isDataSetAlreadyActive = false;

            var allDataSets = objectTracker.GetDataSets();
            foreach (DataSet ds in allDataSets)
            {
                var dsName = Path.GetFileNameWithoutExtension(ds.Path);
            }

            var activeDataSets = objectTracker.GetActiveDataSets();
            foreach (DataSet ds in activeDataSets)
            {
                var dsName = Path.GetFileNameWithoutExtension(ds.Path);
                if (dsName == (this.targetDataBase))
                {
                    isDataSetAlreadyActive = true;
                }
            }

            // We need to reassociate the trackables in case the needed DataSet already exists in the scene
            if (DataSet.Exists(this.targetDataBase))
            {
                if (!isDataSetAlreadyActive)
                {
                    LoadTargetDatabase(this.targetDataBase);
                }
                else
                {
                    stateManager.ReassociateTrackables();
                }
            }

            if (objectTracker != null && objectTracker.IsActive == false)
            {
                objectTracker.Start();
            }
        }

        private void LoadTargetDatabase(string targetDatabaseName)
        {
            ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

            if (DataSet.Exists(targetDatabaseName))
            {
                DataSet dataset = objectTracker.CreateDataSet();

                if (dataset.Load(targetDatabaseName))
                {
                    Debug.Log("DATASET LOADED!");
                    objectTracker.ActivateDataSet(dataset);
                    Debug.Log(dataset.GetTrackables());
                    StateManager stateManager = TrackerManager.Instance.GetStateManager();
                }
                else
                {
                    Debug.LogError("Failed to load DataSet: " + targetDatabaseName);
                }
            }
            else
            {
                Debug.Log("The following DataSet not found in 'StreamingAssets/Vuforia': " + targetDatabaseName);
            }
        }
    }
}

