using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectScroller : MonoBehaviour
{

    //um
    //list of like circular buttons or whatever
    //scroll through them and choose one and then set the index and auto load it in the play scene
    //cool

    public LevelButton buttonFab;
    public List<LevelButton> levelList = new List<LevelButton>();
    public int visibleDistance;
    public float spacing;
    public float indent;
    public float transitionSpeed;
    public float buttonScale;

    int currentIndex;

    private void Start()
    {
        LoadSavesFromPersistent();
        levelList.Reverse();
        StartUp();
        Swotch(0);
    }

    void StartUp()
    {
        currentIndex = levelList.Count - 1;
        for (int i = 0; i < levelList.Count; ++i)
        {

            int distance = Mathf.Abs(i - currentIndex) + 1;

            //levelList[i].levelTitle.text = distance.ToString();

            levelList[i].gameObject.SetActive(true);
            levelList[i].SetImmediate(GetComponent<RectTransform>().anchoredPosition + new Vector2(distance * indent, (i - currentIndex) * spacing), (Vector3.one * buttonScale) / distance);
        }
    }


    void LoadSavesFromPersistent()
    {
     

        string path = Application.persistentDataPath + "/SongSaves/";

        if (Utilities.DirectoryStuff(path))
        {
            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                Debug.Log(path);
                string filePath = file.Replace(path, "");
                filePath = filePath.Replace(".json", "");
                LevelButton b = Instantiate(buttonFab, transform);
                b.levelTitle.text = filePath;
                levelList.Add(b);
               
            }
        }
        //else
        //{
        //    //Called if the directory has not been created yet to move all existing saves
        //    //from resources into the persistent data path 

        //    TransportFilesFromResources();
        //    LoadSavesFromPersistent();
        //}
    }

    void TransportFilesFromResources()
    {
        List<TextAsset> songSaves = new List<TextAsset>();
        songSaves = Resources.LoadAll<TextAsset>("SongSaves").ToList();
        foreach (TextAsset file in songSaves)
        {
            string jsonData = file.text;
            string path = Application.persistentDataPath + "/SongSaves/" + file.name + ".json";
            System.IO.File.WriteAllText(path, jsonData);
        }
    }

    public void Swotch(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, levelList.Count-1);

        for (int i = 0; i < levelList.Count; ++i)
        {

            int distance = Mathf.Abs(i - currentIndex) +1;
            //levelList[i].levelTitle.text = distance.ToString();
            if (distance > visibleDistance)
                levelList[i].gameObject.SetActive(false);
            else
            {
                levelList[i].gameObject.SetActive(true);
                levelList[i].Move(GetComponent<RectTransform>().anchoredPosition + new Vector2(distance * indent,(i-currentIndex)  * spacing), (Vector3.one * buttonScale) / distance, transitionSpeed);
                
            }
        }

    }
}
