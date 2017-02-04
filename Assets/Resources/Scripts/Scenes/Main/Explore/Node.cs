using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class Node : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    private Player player;

    public Event_Info event_Info { get; private set; }

    private Transform eventGamobject;
    private Quaternion eventGameobjectRotation;

    private Vector2 nodePos;

    public Vector2 NodePos
    {
        get
        {
            return nodePos;
        }
        set
        {
            nodePos = value;
        }
    }

    private NodeState nodeCurrentState = NodeState.Null;
    private NodeEvent nodeCurrentEvent = NodeEvent.Empty_Path;
    private NodeExploreState nodeCurrentExploreState = NodeExploreState.Hidden;

    public NodeState NodeCurrentState
    {
        get
        {
            return nodeCurrentState;
        }
    }
    public NodeEvent NodeCurrentEvent
    {
        get
        {
            return nodeCurrentEvent;
        }
    }
    public NodeExploreState NodeCurrentExploreState
    {
        get
        {
            return nodeCurrentExploreState;
        }
        set
        {
            nodeCurrentExploreState = value;
        }
    }
  
    private Dictionary<NeighbourDirection, GameObject> NeighboursDictionary = new Dictionary<NeighbourDirection, GameObject>();
    public List<GameObject> connectedNeighbours = new List<GameObject>();

    private Image ImageComponent;
    private Image eventImageComponent;

    private string spriteFolderPath = "Sprites/CorePrototype/Explore/Events/";
    #endregion

    void Start()
    {
        Initialise();
    }
    
    void Update()
    {
        nodeEventControl();
        nodeStateControl();
        nodeExploreStateControl();
    }

    private void Initialise()
    {
        player = FindObjectOfType<Player>();
        
        eventGamobject = transform.GetChild(0);
        eventGameobjectRotation = eventGamobject.rotation;

        event_Info = new Event_Info();

        eventImageComponent = eventGamobject.GetComponent<Image>();
        ImageComponent = GetComponent<Image>();
    }

    private void nodeStateControl()
    {
        switch (nodeCurrentState)
        {
            case NodeState.Null:
                break;

            #region NodeState.Constructed
            case NodeState.Constructed:
                switch (connectedNeighbours.Count)
                {
                    case 1:

                        ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Dead_End");
                        if (connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Top])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        if (connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Bottom])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, -90);
                        }
                        if (connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Left])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                        if (connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Right])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        break;

                    case 2:

                        if ((connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Top] || connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Bottom]) && (connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Top] || connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Bottom]))
                        {
                            ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Straight_Line");
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        if ((connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Left] || connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Right]) && (connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Left] || connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Right]))
                        {
                            ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Straight_Line");
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        if ((connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Left] || connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Top]) && (connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Left] || connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Top]))
                        {
                            ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Corner");
                            transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                        if ((connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Top] || connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Right]) && (connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Top] || connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Right]))
                        {
                            ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Corner");
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        if ((connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Left] || connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Bottom]) && (connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Left] || connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Bottom]))
                        {
                            ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Corner");
                            transform.rotation = Quaternion.Euler(0, 0, -90);
                        }
                        if ((connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Right] || connectedNeighbours[0] == NeighboursDictionary[NeighbourDirection.Bottom]) && (connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Right] || connectedNeighbours[1] == NeighboursDictionary[NeighbourDirection.Bottom]))
                        {
                            ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Corner");
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        break;

                    case 3:

                        ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "TripleWay");
                        GameObject notConnectedNode = GetNotConnectedNeighbourNode();
                        if (notConnectedNode == NeighboursDictionary[NeighbourDirection.Top])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, -90);
                        }
                        if (notConnectedNode == NeighboursDictionary[NeighbourDirection.Bottom])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        if (notConnectedNode == NeighboursDictionary[NeighbourDirection.Left])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        if (notConnectedNode == NeighboursDictionary[NeighbourDirection.Right])
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                        break;

                    case 4:
                        ImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "CrossWay");
                        break;
                }
                break;
            #endregion
            case NodeState.Disabled:
                break;
            default:
                break;
        }
        eventGamobject.rotation = eventGameobjectRotation;
    }

    private void nodeEventControl()
    {
        switch (nodeCurrentEvent)
        {
            case NodeEvent.Battle:
                event_Info.Set_Info(FindObjectOfType<Database_Monsters>().getMonster(0));
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Battle");
                break;
            case NodeEvent.Exit:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Exit");
                break;
            case NodeEvent.Empty_Path:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Empty");
                break;


            case NodeEvent.Encounter_Sack:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "SackIcon");
                break;
            case NodeEvent.Encounter_Altar:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "AltarIcon");
                break;
            case NodeEvent.Encounter_Camp:
                event_Info.Set_Info( FindObjectOfType<Database_Monsters>().getMonster(0));
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "CampIcon");
                break;
            case NodeEvent.Encounter_Chest:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "ChestIcon");
                break;                
            case NodeEvent.Encounter_Fountain:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "FountainIcon");
                break;
            case NodeEvent.Encounter_Mushrooms:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "MushroomsIcon");
                break;
            case NodeEvent.Encounter_Trap:
                eventImageComponent.sprite = Resources.Load<Sprite>(spriteFolderPath + "Empty");              
                break;
        }
    }

    private void nodeExploreStateControl()
    {
        switch (nodeCurrentExploreState)
        {
            case NodeExploreState.Hidden:
                ImageComponent.enabled = false;
                eventImageComponent.enabled = false;
                break;
            case NodeExploreState.Revealed:
                ImageComponent.enabled = true;
                ImageComponent.color = new Color(ImageComponent.color.r, ImageComponent.color.g, ImageComponent.color.b, 0.7f);
                eventImageComponent.enabled = false;
                break;
            case NodeExploreState.Explored:
                ImageComponent.color = new Color(ImageComponent.color.r, ImageComponent.color.g, ImageComponent.color.b, 1f);
                ImageComponent.enabled = true;
                eventImageComponent.enabled = true;                             
                break;
            default:
                break;
        }
    }
    public void DestroyTrap()
    {
        StartCoroutine(FadeTrap(1));
    }

    private IEnumerator FadeTrap(float timeToFade)
    {
        SetNodeEvent(NodeEvent.Empty_Path);

        GameObject eventTrap = Instantiate(Resources.Load("Prefabs/Explore/Event_Trap"), transform, false) as GameObject;
        eventTrap.transform.rotation = eventGamobject.rotation;

        eventTrap.GetComponent<Image>().enabled = true;
        eventTrap.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteFolderPath + "TrapIcon");

        Color original_Color = eventTrap.GetComponent<Image>().color;
        Color target_Color = new Color(original_Color.r, original_Color.g, original_Color.b, 0);
        float t = 0f;

        while (t <= 1)
        {
            t += Time.deltaTime / timeToFade;
            eventTrap.GetComponent<Image>().color = Color.Lerp(original_Color, target_Color, t);
            yield return null;
        }
        Destroy(eventTrap);

    }
    public Monster GetNodeMonster()
    {
        return event_Info.monster;
    }
    public void SetNodeState(NodeState nodeState)
    {
        nodeCurrentState = nodeState;
    }
    public void SetNodeEvent(NodeEvent nodeEvent)
    {
        nodeCurrentEvent = nodeEvent;

        if(eventImageComponent)
        nodeEventControl();
    }
    public void SetNodeExploreState(NodeExploreState nodeExploreState)
    {
        nodeCurrentExploreState = nodeExploreState;
    }
    public void SetNode(NodeEvent nodeEvent, NodeState nodeState)
    {
        nodeCurrentEvent = nodeEvent;
        nodeCurrentState = nodeState;
    }

    public void AddNeighbour(NeighbourDirection neighbourDirection, GameObject neighbour)
    {
        NeighboursDictionary.Add(neighbourDirection, neighbour);
    }
    public void AddConnectedNeighbourToList(GameObject neighbour)
    {
        connectedNeighbours.Add(neighbour);
    }

    public List<GameObject> GetNeighboursList()
    {
        List<GameObject> NeighbourList = new List<GameObject>();
        foreach (GameObject neighbour in NeighboursDictionary.Values)
        {
            if (neighbour)
                NeighbourList.Add(neighbour);
        }
        return NeighbourList;
    }

    private GameObject GetNotConnectedNeighbourNode()
    {
        GameObject notConnectedNode = null;

        for (int i = 0; i < GetNeighboursList().Count; i++)
        {
            if (connectedNeighbours.Contains(GetNeighboursList()[i]))
            {

            }
            else
                notConnectedNode = GetNeighboursList()[i];
        }
        return notConnectedNode;
    }

    public GameObject GetRandomNotConnectedNeighbourNode()
    {
        List<GameObject> notConnectedNeighbourNodes = new List<GameObject>();
        GameObject randomNotConnectedNeighbourNode;
        for (int i = 0; i < GetNeighboursList().Count; i++)
        {
            if (!connectedNeighbours.Contains(GetNeighboursList()[i]))
                notConnectedNeighbourNodes.Add(GetNeighboursList()[i]);
        }
        if (notConnectedNeighbourNodes.Count > 0)
        {
            randomNotConnectedNeighbourNode = notConnectedNeighbourNodes[Random.Range(0, notConnectedNeighbourNodes.Count)];
            return randomNotConnectedNeighbourNode;
        }


        return null;
    }

    public GameObject GetRandomNeighbour()
    {
        List<GameObject> NeighbourList = new List<GameObject>();
        GameObject randomNeighbour;
        foreach (GameObject neighbour in NeighboursDictionary.Values)
        {
            if (neighbour)
                NeighbourList.Add(neighbour);
        }
        randomNeighbour = NeighbourList[Random.Range(0, NeighbourList.Count)];
        return randomNeighbour;
    }
    public GameObject GetRandomNeighbourByState(NodeState state)
    {
        List<GameObject> NeighbourList = new List<GameObject>();
        GameObject randomNeighbour;
        foreach (GameObject neighbour in NeighboursDictionary.Values)
        {
            if (neighbour)
            {
                if (neighbour.GetComponent<Node>().nodeCurrentState == state)
                    NeighbourList.Add(neighbour);
            }
        }
        randomNeighbour = NeighbourList[Random.Range(0, NeighbourList.Count)];
        return randomNeighbour;
    }

    public List<Node> GetConnectedNodesList()
    {
        List<Node> connectedNodeList = new List<Node>();
        for (int i = 0; i < connectedNeighbours.Count; i++)
        {
            connectedNodeList.Add(connectedNeighbours[i].GetComponent<Node>());
        }
        return connectedNodeList;
    }

    public bool ContainsAvailableNeigboursToConnect()
    {
        if (GetNeighboursList().Count > connectedNeighbours.Count)
        {
            return true;
        }
        return false;
    }

    public float DistanceToNode(Node n)
    {
        return Vector2.Distance(new Vector2(NodePos.x, NodePos.y), new Vector2(n.NodePos.x, n.NodePos.y));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        player.MoveToNode(gameObject);
    }
}
