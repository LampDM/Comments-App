using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

public class EnableModelDetection : MonoBehaviour
{

    [SerializeField] string DataSetStandard = "";
    // Start is called before the first frame update

    ObjectTracker objectTracker;
    StateManager stateManager;
    string currentActiveDataSet = string.Empty;
    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    // Update is called once per frame

    void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);

        DeactivateActiveDataSets(true);
    }
    void OnVuforiaStarted()
    {
        this.stateManager = TrackerManager.Instance.GetStateManager();
        this.objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        LoadDataSet(DataSetStandard);

    }
    void LoadDataSet(string datasetName)
    {
        if (DataSet.Exists(datasetName))
        {
            DataSet dataset = this.objectTracker.CreateDataSet();

            if (dataset.Load(datasetName))
            {
                Debug.Log("Loaded DataSet: " + datasetName);
            }
            else
            {
                Debug.LogError("Failed to load DataSet: " + datasetName);
            }
        }
        else
        {
            Debug.Log("The following DataSet not found in 'StreamingAssets/Vuforia': " + datasetName);
        }
    }

    void DeactivateActiveDataSets(bool destroyDataSets = false)
    {
        List<DataSet> activeDataSets = this.objectTracker.GetActiveDataSets().ToList();

        foreach (DataSet ds in activeDataSets)
        {
            // The VuforiaEmulator.xml dataset (used by GroundPlane) is managed by Vuforia.
            if (!ds.Path.Contains("VuforiaEmulator.xml"))
            {
                this.objectTracker.DeactivateDataSet(ds);
                if (destroyDataSets)
                {
                    this.objectTracker.DestroyDataSet(ds, false);
                }
            }
        }
    }

    void ActivateDataSet(string datasetName)
    {
        if (this.currentActiveDataSet == datasetName)
        {
            Debug.Log("The selected dataset is already active.");
            // If the current dataset is already active, return.
            return;
        }

        // Stop the Object Tracker before activating/deactivating datasets.
        this.objectTracker.Stop();

        // Deactivate the currently active datasets.
        DeactivateActiveDataSets();

        var dataSets = this.objectTracker.GetDataSets();

        bool dataSetFoundAndActivated = false;

        foreach (DataSet ds in dataSets)
        {
            if (ds.Path.Contains(datasetName + ".xml"))
            {
                // Activate the selected dataset.
                if (this.objectTracker.ActivateDataSet(ds))
                {
                    this.currentActiveDataSet = datasetName;
                }

                dataSetFoundAndActivated = true;

                var trackables = ds.GetTrackables();

                foreach (Trackable t in trackables)
                {
                    ModelTarget modelTarget = t as ModelTarget;
                }

                Transform modelTargetTransform = null;

                // Once we find and process selected dataset, exit foreach loop.
                break;
            }
        }

        if (!dataSetFoundAndActivated)
        {
            Debug.LogError("DataSet Not Found: " + datasetName);
        }

        // Start the Object Tracker.
        this.objectTracker.Start();
    }


    public bool ActiveTrackablesExist()
    {
        foreach (TrackableBehaviour trackable in stateManager.GetActiveTrackableBehaviours())
        {
            return true;
        }

        return false;
    }

    public void LogActiveDataSets()
    {
        List<DataSet> activeDataSets = this.objectTracker.GetActiveDataSets().ToList();

        foreach (DataSet ds in activeDataSets)
        {
            Debug.Log("Active DS: " + ds.Path);
        }
    }

    public void LogAllDataSets()
    {
        List<DataSet> allDataSets = this.objectTracker.GetDataSets().ToList();

        foreach (DataSet ds in allDataSets)
        {
            Debug.Log("DS: " + ds.Path);
        }
    }
}