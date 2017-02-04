using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class MapGenerator:MonoBehaviour {

    #region Variables

    [System.Serializable]
    public class EventCount
    {
        public int min_Count;
        public int max_Count;
        public List<Event_Chances> event_chances = new List<Event_Chances>();


        [System.Serializable]
        public class Event_Chances
        {
            public NodeEvent Event;
            public int chance;
        }


        public int GetRandom()
        {
            return Random.Range(min_Count, max_Count + 1);
        }
    }
    private Grid grid;

    private SaveSystem save_System;
    
    private GameObject firstNode;
    private GameObject doorNode;
    private GameObject currentNode;
    private GameObject nextNode;
    private GameObject battleNode;


    [SerializeField]
    private int mapMaxLength;
    [SerializeField]
    private int NodesToConnect;
    [SerializeField]
    private EventCount battle_info;
    [SerializeField]
    private EventCount encounter_info;

    //test Variables
    private List<Node> path;
    public Node ClickingNode;
    private Node StartingPlayerNode;
    private Player player;
    private bool enableNodeVision = false;

    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;
    #endregion

    // Use this for initialization
    void Start ()
    {
        Initialise();        
    }
    private void Initialise()
    {
        save_System = FindObjectOfType<SaveSystem>();
        grid = GetComponent<Grid>();
        player = FindObjectOfType<Player>();
        grid.constructGrid(grid.gridProperties.height, grid.gridProperties.width, grid.gridProperties.startPosition, grid.gridProperties.offset);       
        generateMap();
        initialised = true;
    }
	// Update is called once per frame
	void Update ()
    {
        DebugControl();

        if (path != null)
            path = grid.GetShortestPath(player.GetCurrentNode().GetComponent<Node>(), ClickingNode);
    }

    private void generateMap()
    {
        //Starting Node
        firstNode = grid.GetRandomNodeByState(NodeState.Null).gameObject;
        firstNode.GetComponent<Node>().SetNodeState(NodeState.Constructed);

        //make a map
        for (int i = 0; i < mapMaxLength; i++)
        {
            currentNode = GetRandomAvailableNode();
            nextNode = currentNode.GetComponent<Node>().GetRandomNeighbourByState(NodeState.Null);
            nextNode.GetComponent<Node>().SetNodeState(NodeState.Constructed);
            ConnectNodes(currentNode, nextNode);
        }

        //Не работает
        //AddNonNaturalConnections(NodesToConnect);

        //Create a door
        doorNode = grid.GetRandomDeadEndNode();
        doorNode.GetComponent<Node>().SetNode(NodeEvent.Exit, NodeState.Constructed);

        //place Random Battles
        for (int i = 0; i < battle_info.GetRandom(); i++)
        {
            battleNode = GetRandomNode(NodeState.Constructed,NodeEvent.Empty_Path);
            battleNode.GetComponent<Node>().SetNodeEvent(NodeEvent.Battle);
        }

        //place Random Events
        AddRandomEvents();

        //place a player
        StartingPlayerNode = GetRandomNode(NodeState.Constructed, NodeEvent.Empty_Path).GetComponent<Node>();
        player.SetPlayerPosition(StartingPlayerNode.gameObject);
       
    }
    public void Recreate_Grid()
    {
        grid.ClearGrid();
        grid.constructGrid(grid.gridProperties.height, grid.gridProperties.width, grid.gridProperties.startPosition, grid.gridProperties.offset);
        generateMap();
        enableNodeVision = false;
    }

    void OnDrawGizmos()
    {
        if (path != null && path.Count > 1)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(path[i - 1].transform.position, path[i].transform.position);
            }
        }
    }

    private GameObject GetRandomAvailableNode()
    {
        GameObject[,] nodeArray = grid.GetNodesArray();
        List<GameObject> availableNodeList = new List<GameObject>();
        GameObject randomAvailableNode;

        for (int i = 0; i < nodeArray.GetLength(0); i++)
        {
            for (int j = 0; j < nodeArray.GetLength(1); j++)
            {
                if (nodeArray[i, j].GetComponent<Node>().NodeCurrentState == NodeState.Constructed)
                {
                    for (int f = 0; f < nodeArray[i, j].GetComponent<Node>().GetNeighboursList().Count; f++)
                    {
                        if (nodeArray[i, j].GetComponent<Node>().GetNeighboursList()[f].GetComponent<Node>().NodeCurrentState == NodeState.Null)
                            availableNodeList.Add(nodeArray[i, j]);
                    }
                }
            }
        }
        randomAvailableNode = availableNodeList[Random.Range(0, availableNodeList.Count)];
        return randomAvailableNode;
    }
    
    private GameObject GetRandomNode(NodeState node_State, NodeEvent node_Event)
    {
        List<GameObject> constructedNodeList = grid.GetListOfNodesByState(node_State);
        List<GameObject> emptyNodeList = new List<GameObject>();

        for (int i = 0; i < constructedNodeList.Count; i++)
        {
            if (constructedNodeList[i].GetComponent<Node>().NodeCurrentEvent == node_Event)
                emptyNodeList.Add(constructedNodeList[i]);
        }
        return emptyNodeList[Random.Range(0, emptyNodeList.Count)];
    }

    private void ConnectNodes(GameObject firstNode,GameObject secondNode)
    {
        firstNode.GetComponent<Node>().AddConnectedNeighbourToList(secondNode);
        secondNode.GetComponent<Node>().AddConnectedNeighbourToList(firstNode);
    }
    private void AddNonNaturalConnections(int connectionCount)
    {
        List<GameObject> constructedNodes = grid.GetListOfNodesByState(NodeState.Constructed);
        List<GameObject> nodesWithConnections = new List<GameObject>();
        for (int i = 0; i < constructedNodes.Count; i++)
        {
            if (constructedNodes[i].GetComponent<Node>().ContainsAvailableNeigboursToConnect())
                nodesWithConnections.Add(constructedNodes[i]);
        }

        for (int i = 0; i < NodesToConnect; i++)
        {
            int randomIndex = Random.Range(0, nodesWithConnections.Count);
            ConnectNodes(nodesWithConnections[randomIndex], nodesWithConnections[randomIndex].GetComponent<Node>().GetRandomNotConnectedNeighbourNode());
        }
    }

    private void AddRandomEvents()
    {
        int spawnedEvents = 0;
        float randomChance = 0;
        for (int i = 0; i < encounter_info.event_chances.Count; i++)
        {
            randomChance = Random.value * 100;

            if(randomChance < encounter_info.event_chances[i].chance)
            {
                battleNode = GetRandomNode(NodeState.Constructed,NodeEvent.Empty_Path);
                battleNode.GetComponent<Node>().SetNodeEvent(encounter_info.event_chances[i].Event);
                spawnedEvents++;
            }

            if (spawnedEvents == encounter_info.max_Count)
                return;

            if (i == encounter_info.event_chances.Count - 1 && spawnedEvents < encounter_info.min_Count)
                i = -1;
        }
    }

    private void DebugControl()
    {       
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Recreate_Grid();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            List<GameObject> constructedNodes = grid.GetListOfNodesByState(NodeState.Constructed);
            if (enableNodeVision)
            {
                for (int i = 0; i < constructedNodes.Count; i++)
                {
                    constructedNodes[i].GetComponent<Node>().SetNodeExploreState(NodeExploreState.Hidden);
                    enableNodeVision = false;
                }
            }  
            else
            {
                for (int i = 0; i < constructedNodes.Count; i++)
                {
                    constructedNodes[i].GetComponent<Node>().SetNodeExploreState(NodeExploreState.Explored);
                    enableNodeVision = true;
                }
            }      
            
        }
    }
    public Grid GetGrid()
    {
        return grid;
    }
}