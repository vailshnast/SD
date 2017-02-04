using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour {

	private GameObject[,] nodes;
	[SerializeField]
	private GameObject NodePrefab;

	[System.Serializable]
	public class GridProperties
	{
		public int height;
		public int width;
		public Vector2 startPosition;
		public Vector2 offset;
	}

	[SerializeField]
	public GridProperties gridProperties = new GridProperties();

	public void constructGrid(int height,int width,Vector2 startPosition, Vector2 Offset)
	{
		nodes = new GameObject[height, width];
		int id = 0;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{				
				nodes[i, j] =  Instantiate(NodePrefab, new Vector2(startPosition.x + j * Offset.x, startPosition.y + Offset.y * i), Quaternion.identity) as GameObject;
				nodes[i, j].transform.SetParent(transform,false);
                nodes[i, j].GetComponent<Node>().NodePos = new Vector2(j, i);
				id++;
			}
		}
        SetNeighbours(nodes);
	}
	

    public GameObject GetRandomDeadEndNode()
    {
        List<GameObject> deadEndNodesList = new List<GameObject>();
        GameObject randomDeadEnd;
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i, j].GetComponent<Node>().GetConnectedNodesList().Count == 1)
                {
                    deadEndNodesList.Add(nodes[i, j]);
                }
            }
        }
        randomDeadEnd = deadEndNodesList[Random.Range(0, deadEndNodesList.Count)];
        return randomDeadEnd;
    }
    public Node GetRandomNodeByState(NodeState state)
    {
        List<Node> nodesByState = new List<Node>();
        Node randomNodeByState;
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if(nodes[i,j].GetComponent<Node>().NodeCurrentState == state)
                {
                    nodesByState.Add(nodes[i, j].GetComponent<Node>());
                }
           }            
        }
        randomNodeByState = nodesByState[Random.Range(0, nodesByState.Count)];
        return randomNodeByState;
    }
    public List<GameObject> GetListOfNodesByState(NodeState state)
    {
        List<GameObject> nodesByState = new List<GameObject>();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i, j].GetComponent<Node>().NodeCurrentState == state)
                {
                    nodesByState.Add(nodes[i, j]);
                }
            }
        }
        return nodesByState;
    }
    private Node[,] getGraphOfAvailableNodes()
    {
        Node[,] nodeGraph = new Node[gridProperties.height, gridProperties.width];
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                nodeGraph[i, j] = nodes[i, j].GetComponent<Node>();
            }
        }
        return nodeGraph;
    }
    private void  SetNeighbours(GameObject[,] nodesArray)
    {
        for (int i = 0; i < nodesArray.GetLength(0); i++)
        {
            for (int j = 0; j < nodesArray.GetLength(1); j++)
            {
                if (i>0)
                    nodesArray[i , j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Top, nodesArray[i - 1, j]);
                else nodesArray[i, j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Top, null);
                if ((i< nodesArray.GetLength(0)-1))
                    nodesArray[i , j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Bottom, nodesArray[i + 1, j]);
                else nodesArray[i, j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Bottom, null);
                if (j>0)
                    nodesArray[i , j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Left, nodesArray[i , j - 1]);
                else nodesArray[i, j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Left, null);
                if ((j < nodesArray.GetLength(1)-1))
                    nodesArray[i, j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Right, nodesArray[i , j + 1]);
                else nodesArray[i, j].GetComponent<Node>().AddNeighbour(NeighbourDirection.Right, null);
            }
        }
    }

    public void ClearGrid()
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                Destroy(nodes[i, j]);
            }
        }
        nodes = new GameObject[0, 0];
    }

    public GameObject[,] GetNodesArray()
    {
        return nodes;
    }

    
    public List<Node> GetShortestPath(Node source,Node targetNode)
    {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        List<Node> currentPath = new List<Node>();

        Node[,] graph = getGraphOfAvailableNodes();

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if(v!=source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if(u==null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if(u == targetNode)
            {
                break;
            }

            unvisited.Remove(u);

            foreach (Node v in u.GetConnectedNodesList())
            {
                float alt = dist[u] + u.DistanceToNode(v);
                if(alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
       if(prev[targetNode] == null)
        {
            return null;
        }

        Node currentNode = targetNode;

        while(currentNode!=null)
        {
            currentPath.Add(currentNode);
            currentNode = prev[currentNode];
        }
        currentPath.Reverse();
        return currentPath;
    }
}
