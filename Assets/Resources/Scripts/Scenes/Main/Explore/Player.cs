using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour {

    private SaveSystem saveSystem;

    private GameObject currentNode;
    private GameObject nodeSelected;
    private GameObject torchGameObject;

    private bool torchIsActive = false;

    private MapGenerator mapGen;

    [SerializeField]
    private float secondsToWaitBeforeMovingToNode;

    private string spriteFolderPath = "Sprites/CorePrototype/Explore/";

    // Use this for initialization
    void Awake ()
    {
        Initialise();
    }

    private void useTorch()
    {
        List<Node> nodesToReveale = new List<Node>();
        if (torchIsActive)
        {           
            for (int j = 0; j < currentNode.GetComponent<Node>().GetConnectedNodesList().Count; j++)
            {
                currentNode.GetComponent<Node>().GetConnectedNodesList()[j].SetNodeExploreState(NodeExploreState.Explored);

                if (currentNode.GetComponent<Node>().GetConnectedNodesList()[j].NodeCurrentEvent == NodeEvent.Encounter_Trap)
                    currentNode.GetComponent<Node>().GetConnectedNodesList()[j].DestroyTrap();               

                nodesToReveale.Add(currentNode.GetComponent<Node>().GetConnectedNodesList()[j]);
            }
            for (int i = 0; i < nodesToReveale.Count; i++)
            {
                for (int j = 0; j < nodesToReveale[i].GetConnectedNodesList().Count; j++)
                {
                    if (nodesToReveale[i].GetConnectedNodesList()[j].NodeCurrentExploreState != NodeExploreState.Explored)
                        nodesToReveale[i].GetConnectedNodesList()[j].SetNodeExploreState(NodeExploreState.Revealed);
                }
            }

            ConsumeTorch();

            if (saveSystem.get_sd_char().Torch_Quantity == 0)
                ToggleTorch();

        }
    }
    public void SetPlayerPosition(GameObject node)
    {
        currentNode = node;
        transform.position = currentNode.transform.position;

        currentNode.GetComponent<Node>().SetNodeExploreState(NodeExploreState.Explored);
        saveSystem.set_Node(currentNode);
        switch (currentNode.GetComponent<Node>().NodeCurrentEvent)
        {
            case NodeEvent.Battle:               
                saveSystem.get_Node().GetComponent<Node>().SetNodeEvent(NodeEvent.Empty_Path);
                saveSystem.GetComponent<SceneSwitcher>().battle_Scene();
                break;
            case NodeEvent.Encounter_Trap:
                //Show Tooltip
                FindObjectOfType<Trap_Tooltip>().Activate();
                currentNode.GetComponent<Node>().DestroyTrap();
                StopAllCoroutines();
                break;
        }


        for (int i = 0; i < currentNode.GetComponent<Node>().GetConnectedNodesList().Count; i++)
        {
            if (currentNode.GetComponent<Node>().GetConnectedNodesList()[i].NodeCurrentExploreState != NodeExploreState.Explored)
            {
                currentNode.GetComponent<Node>().GetConnectedNodesList()[i].SetNodeExploreState(NodeExploreState.Revealed);
            }
        }
    }

    public void ToggleTorch()
    {
        if(torchIsActive)
        {
            torchGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteFolderPath + "TorchOFF");
            torchIsActive = false;
        }
        else if(saveSystem.get_sd_char().Torch_Quantity > 0)
        {
            torchGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteFolderPath + "TorchOn");

            ConsumeTorch();

            List<Node> nodesToReveale = new List<Node>();

            for (int j = 0; j < currentNode.GetComponent<Node>().GetConnectedNodesList().Count; j++)
            {
                currentNode.GetComponent<Node>().GetConnectedNodesList()[j].SetNodeExploreState(NodeExploreState.Explored);

                if (currentNode.GetComponent<Node>().GetConnectedNodesList()[j].NodeCurrentEvent == NodeEvent.Encounter_Trap)
                    currentNode.GetComponent<Node>().GetConnectedNodesList()[j].DestroyTrap();

                nodesToReveale.Add(currentNode.GetComponent<Node>().GetConnectedNodesList()[j]);
            }
            for (int i = 0; i < nodesToReveale.Count; i++)
            {
                for (int j = 0; j < nodesToReveale[i].GetConnectedNodesList().Count; j++)
                {
                    if (nodesToReveale[i].GetConnectedNodesList()[j].NodeCurrentExploreState != NodeExploreState.Explored)
                        nodesToReveale[i].GetConnectedNodesList()[j].SetNodeExploreState(NodeExploreState.Revealed);
                }
            }
            torchIsActive = true;
        }
        
    }
    IEnumerator moveToNode(GameObject node, float seconds)
    {
        List<Node> pathToDestination = mapGen.GetGrid().GetShortestPath(currentNode.GetComponent<Node>(), node.GetComponent<Node>());
        if (pathToDestination != null)
        {
            for (int i = 0; i < pathToDestination.Count; i++)
            {
                SetPlayerPosition(pathToDestination[i].gameObject);

                if (i != 0)
                    useTorch();

                yield return new WaitForSeconds(seconds);
            }
        }
    }

    public void MoveToNode(GameObject node)
    {
        mapGen.ClickingNode = node.GetComponent<Node>();
        StopAllCoroutines();
        StartCoroutine(moveToNode(node, secondsToWaitBeforeMovingToNode));
    }

    public void Explore_Button_Pressed()
    {
        switch (currentNode.GetComponent<Node>().NodeCurrentEvent)
        {

            case NodeEvent.Exit:
                mapGen.Recreate_Grid();
                break;
            case NodeEvent.Encounter_Sack:
                saveSystem.GetComponent<SceneSwitcher>().sack_Scene();
                //currentNode.GetComponent<Node>().SetNodeEvent(NodeEvent.Empty_Path);
                break;
            case NodeEvent.Encounter_Chest:
                saveSystem.set_Node(currentNode);
                saveSystem.GetComponent<SceneSwitcher>().Load_Scene("Chest");
                break;
            case NodeEvent.Encounter_Camp:
                saveSystem.set_Node(currentNode);             
                SceneSwitcher.Instance.Load_Scene("Camp");
                break;
            case NodeEvent.Encounter_Fountain:
                saveSystem.set_Node(currentNode);
                SceneSwitcher.Instance.fountain_Scene();
                break;
            case NodeEvent.Encounter_Altar:
                saveSystem.set_Node(currentNode);
                saveSystem.GetComponent<SceneSwitcher>().Load_Scene("Altar");
                break;
            case NodeEvent.Encounter_Mushrooms:
                saveSystem.set_Node(currentNode);
                saveSystem.GetComponent<SceneSwitcher>().Load_Scene("Mushrooms");
                break;
            default:
                break;
        }
    }

    public GameObject GetCurrentNode()
    {
        return currentNode;
    }

    private void Initialise()
    {
        mapGen = FindObjectOfType<MapGenerator>();
        torchGameObject = GameObject.Find("Button_Torch");
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    public void ConsumeTorch()
    {
        saveSystem.get_sd_char().Torch_Quantity -= 1;    
        torchGameObject.transform.GetChild(0).GetComponent<Text>().text = saveSystem.get_sd_char().Torch_Quantity.ToString();
    }

}
