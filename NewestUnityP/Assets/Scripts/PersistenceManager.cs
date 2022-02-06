using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    [SerializeField] private string fileName = "persistentObjects.json";

    public static PersistenceManager Instance;
    private static List<PersistentObject> pObjects => Instance.persistentObjects;
    private static Dictionary<string, PersistentObjectData> loadedPObjects => Instance.loadedPersistentObjects;

    private static string filePath;
    private List<PersistentObject> persistentObjects = new List<PersistentObject>();
    private Dictionary<string, PersistentObjectData> loadedPersistentObjects = new Dictionary<string, PersistentObjectData>();    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            filePath = $"{Application.persistentDataPath}/{Instance.fileName}";
            ReadPObjects();
        }
        else
        {
            Debug.LogError($"{nameof(PersistenceManager)}: exists in more than one instance");
            Destroy(this);
        }        
    }

    public static void RegisterPersistentObject(PersistentObject pObject)
    {
        Instance.persistentObjects.Add(pObject);

        if (loadedPObjects != null && loadedPObjects.ContainsKey(pObject.ID))
        {
            pObject.RestoreState(loadedPObjects[pObject.ID]);
        }
    }

    private void OnApplicationQuit()
    {
        SavePObjects();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SavePObjects();
    }

    #region IO Operations
    private static void SavePObjects()
    {
        var wrapper = new PersistentObjectSerializationWrapper();

        foreach(var pObject in pObjects)
        {
            pObject.SaveState();
            wrapper.Data.Add(pObject.Data);
        }

        File.WriteAllText(filePath, JsonUtility.ToJson(wrapper));
    }

    private static void ReadPObjects()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var dataWrapper = JsonUtility.FromJson<PersistentObjectSerializationWrapper>(json);
            Instance.loadedPersistentObjects = dataWrapper.Data.ToDictionary(data => data.id, data => data);
        }
    }

    [ContextMenu("ResetPositions")]
    private void RestoreInitialPositions()
    {
        var filePath = $"{Application.persistentDataPath}/{fileName}";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    #endregion
}
