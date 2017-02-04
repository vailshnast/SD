using UnityEngine;
using System.Collections;

public class SceneSwitcher : MonoBehaviour {

    public static SceneSwitcher Instance { get; private set; }

    private Game_Scene[] scenes;
    private Game_Scene currentScene; 

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;
    }

    private Game_Scene find_scenes(string name)
    {
        scenes = Resources.FindObjectsOfTypeAll(typeof(Game_Scene)) as Game_Scene[];
        foreach (Game_Scene looked_scene in scenes)
        {
            if (looked_scene.name == name)
                return looked_scene;
                
        }
            return null;
    }
    private Game_Scene find_Active_Scene()
    {
        scenes = Resources.FindObjectsOfTypeAll(typeof(Game_Scene)) as Game_Scene[];
        foreach (Game_Scene looked_scene in scenes)
        {
            if (looked_scene.isActiveAndEnabled)
                return looked_scene;

        }
        return null;
    }
    private void switch_scene(string sceneName)
    {

        if (currentScene == null)
            currentScene = find_Active_Scene();
        
        currentScene.gameObject.SetActive(false);
        currentScene = find_scenes(sceneName);
        currentScene.gameObject.SetActive(true);
    }

    //Don't wanna make a separated class to switch scenes
    //Scene switchers

    public void main_menu()
    {
        switch_scene("Main Menu");
    }
    public void new_game()
    {
        switch_scene("New Game");
    }

    public void hero_preparation()
    {
        switch_scene("Hero Preparation");
    }
	
	public void explore_Scene()
	{
        switch_scene("Explore");
    }
    public void inventory_Scene()
    {
        switch_scene("Rune_Inventory");
    }
    public void reward_Scene()
    {
        switch_scene("Reward");
    }
    public void battle_Scene()
    {
        switch_scene("Battle");
    }
    public void sack_Scene()
    {
        switch_scene("Sack");
    }
    public void fountain_Scene()
    {
        switch_scene("Fountain");
    }
    public void Load_Scene(string name)
    {
        switch_scene(name);
    }
    public void close_application()
    {
        Application.Quit();
    }   
    
}
