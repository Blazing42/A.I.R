using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    //variable to store the file path to the save folder
    private static readonly string Save_Folder = Application.dataPath + "/Saves/";
    //variable to store the save extension
    private const string Save_Extension = "txt";
    //variable to determine if the save system has been initialised or not
    public static bool isInitialised = false;

    public static void InitialiseSaveSystem()
    {
        //makes sure that the save system is only initialised once so multiple files arent created
        if (isInitialised == false)
        {
            isInitialised = true;
            //test to see if the save folder exists
            if (!Directory.Exists(Save_Folder))
            {
                //if not create the save folder
                Directory.CreateDirectory(Save_Folder);
            }
        }
    }

    //save function that takes a string and saves it to a json file
    public static void Save(string savestring, string fileName, bool overwrite)
    {
        //if the save system doesnt already exist create one, otherwise do nothing
        InitialiseSaveSystem();
        //set the save file name to be the filename that was entered
        string saveFileName = fileName;
        //checks if the player wants to overwrite an existing file
        if (overwrite == false)
        {
            //if not
            //create a savenumber int
            int saveNumber = 1;
            //make sure the file save name is unique so that the file doesnt get overwritten
            while (File.Exists(Save_Folder + saveFileName + "." + Save_Extension))
            {
                //if a file exists with that name add a number to the end to ensure that it is unique
                saveNumber++;
                saveFileName = fileName + "_" + saveNumber;
            }
        }
        //write the save data to the savefile with the correct save name
        File.WriteAllText(Save_Folder + saveFileName + "." + Save_Extension, savestring);
    }

    public static string Load(string filename)
    {
        //if the save system doesnt already exist create one, otherwise do nothing
        InitialiseSaveSystem();
        //check is a file with the filename added exists
        if (File.Exists(Save_Folder + filename + "." + Save_Extension))
        {
            //if it exists return the info inside as a string
            string loadedString = File.ReadAllText(Save_Folder + filename + "." + Save_Extension);
            return loadedString;
        }
        else
        {
            //if file isnt found to be loaded a null value is returned along with a warning message
            Debug.LogWarning("Save File Not Found");
            return null;
        }

    }

    //method used to create an array of strings used to create a load menu 
    public static List<string> ListFilesToLoad()
    {
        //if the save system doesnt already exist create one, otherwise do nothing
        InitialiseSaveSystem();
        //get a list of all of the save files that we have in the save folder
        DirectoryInfo directoryInfo = new DirectoryInfo(Save_Folder);
        FileInfo[] savefiles = directoryInfo.GetFiles("*." + Save_Extension);
        //create a new list
        List<string> loadfilenames = new List<string>();
        //populate it with the load file names
        foreach(FileInfo savefile in savefiles)
        {
            loadfilenames.Add(savefile.Name.Remove(savefile.Name.Length - (Save_Extension.Length + 1)));
        }
        //return the list
        return loadfilenames;
    }

    public static string LoadMostRecentFile()
    {
        //if the save system doesnt already exist create one, otherwise do nothing
        InitialiseSaveSystem();
        //get a list of all of the save files that we have in the save folder
        DirectoryInfo directoryInfo = new DirectoryInfo(Save_Folder);
        FileInfo[] savefiles = directoryInfo.GetFiles("*." + Save_Extension);
        FileInfo mostRecentSave = null;
        //cycle through the list of save files, setting the most recent file to be loaded
        foreach (FileInfo savefile in savefiles)
        {
            Debug.Log(savefile.ToString());
            //if we have none set this one
            if(mostRecentSave == null)
            {
                mostRecentSave = savefile;
            }
            else
            {
                //if we have some savefiles already set it to the one that was last created/written to
                if(savefile.LastWriteTime > mostRecentSave.LastWriteTime)
                {
                    mostRecentSave = savefile;
                }
            }
        }
        //if we do have a most recent save file load up all of the info inside of it
        if(mostRecentSave != null)
        {
            //once the most recent savefile is found load all the contents as a string format
            string loadedstring = File.ReadAllText(mostRecentSave.FullName);
            return loadedstring;
        }
        else
        {
            //if file isnt found to be loaded a null value is returned along with a warning message
            Debug.LogWarning("Save File Not Found");
            return null;
        }
        return null;

    }

    //generics
    //methods that take any object converts them to json formatted string and saves them to file using the save function
    //if the player doesnt add a savename for the file
    public static void SaveObject(object saveobject)
    {
        //if the save system doesnt already exist create one, otherwise do nothing
        InitialiseSaveSystem();
        //set up the object for saving if no name was added
        SaveObject(saveobject, "save", false);
    }

    //if the player does input a name for the savefile and whether they want to overwrite it or not
    public static void SaveObject(object saveobject, string saveName, bool overwrite)
    {
        string stringforjson = JsonUtility.ToJson(saveobject);
        Save(stringforjson, saveName, overwrite);
    }

    //method to load any object as lond as it was the last saved item, e.g. a continue button function 
    public static TSaveObject LoadMostRecentObject<TSaveObject>()
    {
        //load the string from the appropriate file
        string loadedString = LoadMostRecentFile();
        if(loadedString != null)
        {
            //if the file was found load the object from the found string
            TSaveObject loadedObject = JsonUtility.FromJson<TSaveObject>(loadedString);
            return loadedObject;
        }
        else
        {
            //if not return a default object
            return default(TSaveObject);
        }
    }

    //method to load any object using the filename that it was saved under
    public static TSaveObject LoadObject<TSaveObject>(string filename)
    {
        //load the string from the appropriate file
        string loadedString = Load(filename);
        if (loadedString != null)
        {
            //if the file was found load the object from the found string
            TSaveObject loadedObject = JsonUtility.FromJson<TSaveObject>(loadedString);
            return loadedObject;
        }
        else
        {
            //if not return a default object
            return default(TSaveObject);
        }
    }
}
