using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    /// <summary>
    /// Saves the player's data to a file in binary by serialization.
    /// </summary>
    /// <param name="player"></param>
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        SavePlayerData data = new SavePlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Loads the player's data after deserializing it and returns it.
    /// </summary>
    /// <returns></returns>
    public static SavePlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/PlayerData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavePlayerData data = formatter.Deserialize(stream) as SavePlayerData;
            stream.Close();

            return data;
        }
        else
        {
            SavePlayerData data = new SavePlayerData();
            return data;
        }
    }

    public static void DeletePlayer()
    {
        string path = Application.persistentDataPath + "/PlayerData.dat";
        File.Delete(path);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
