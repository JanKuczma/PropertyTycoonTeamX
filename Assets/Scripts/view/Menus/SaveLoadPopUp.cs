using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveLoadPopUp : MonoBehaviour
{
    public ToggleGroup toggles;
    public Button LoadSave;
    public Button Delete;
    public Button Return;

    public static SaveLoadPopUp Create(Transform parent, bool saveMode)
    {
        SaveLoadPopUp popUp = Instantiate(Asset.LoadSavePopUpPreFab,parent).GetComponent<SaveLoadPopUp>();
        if(saveMode) { 
            popUp.LoadSave.GetComponentInChildren<Text>().text = "Save";
            popUp.LoadSave.onClick.AddListener(popUp.save);
        } else {
            popUp.LoadSave.GetComponentInChildren<Text>().text = "Load"; 
            popUp.LoadSave.onClick.AddListener(popUp.load);
        }
        return popUp;
    }

    public void closePopup()
    {
        Destroy(gameObject);
    }

    public void load()
    {
        List<Toggle> slot = toggles.ActiveToggles().ToList<Toggle>();
        if(slot.Count == 0) { return; }
        if(slot[0].GetComponent<SaveSlot>().data == null) { return; }
        GameObject obj = Instantiate(new GameObject());
        obj.tag = "GameData";
        obj.AddComponent<GameData>();
        obj.GetComponent<GameData>().loadData(slot[0].GetComponent<SaveSlot>().data);
        SceneManager.LoadScene(3);
    }
    public void save()
    {
        List<Toggle> slot = toggles.ActiveToggles().ToList<Toggle>();
        if(slot.Count == 0) { return; }
        GameData data = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        GameData.GameDataWrapper.saveGame(slot[0].gameObject.name,data);
        slot[0].GetComponent<SaveSlot>().data = GameData.GameDataWrapper.loadGame(slot[0].gameObject.name);
        slot[0].GetComponent<SaveSlot>().ShowInfo();
    }

    public void remove()
    {
        List<Toggle> slot = toggles.ActiveToggles().ToList<Toggle>();
        if(slot.Count == 0) { return; }
        if(slot[0].GetComponent<SaveSlot>().data == null) { return; }
        GameData.GameDataWrapper.deleteGame(slot[0].gameObject.name);
        slot[0].GetComponent<SaveSlot>().MakeEmpty();
    }

}
