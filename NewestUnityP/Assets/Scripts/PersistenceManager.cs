using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    [SerializeField] private string fileName = "persistentObjects.bin";

    public static PersistenceManager Instance;

    private static string filePath;
    private List<PersistentObject> persistentObjects = new List<PersistentObject>();
    private Dictionary<string, PersistentObjectData> loadedPersistentObjects = new Dictionary<string, PersistentObjectData>();

    private static List<PersistentObject> pObjects => Instance.persistentObjects;
    private static Dictionary<string, PersistentObjectData> loadedPObjects => Instance.loadedPersistentObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"{nameof(PersistenceManager)}: exists in more than one instance");
            Destroy(this);
        }

        filePath = Application.persistentDataPath + Instance.fileName;
        ReadPObjects();
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
        var wrapper = new PersistentObjectSerializarionWrapper();

        foreach(var pObject in pObjects)
        {
            pObject.SaveState();
            wrapper.Data.Add(pObject.Data);
        }

        if (!File.Exists(filePath))
            File.Create(filePath);

        File.WriteAllText(filePath, JsonUtility.ToJson(wrapper));
    }

    private static void ReadPObjects()
    {
        string filePath = Application.persistentDataPath + Instance.fileName;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<PersistentObjectSerializarionWrapper>(json);
            Instance.loadedPersistentObjects = data.Data.ToDictionary(x => x.id, x => x);
        }
    }
    #endregion
}
