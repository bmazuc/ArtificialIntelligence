using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Navigation
{
    class NodeRecord
    {
        public NodeRecord(Node record)
        {
            node = record;
            costSoFar = estimatedTotalCost = 0f;
        }
        
        public Node node;
        public Connection connection;

        public float costSoFar;
        public float estimatedTotalCost;
    }

    public class AStarPathfinder : MonoBehaviour
    {
        private Node targetNode = null;
        private Node startNode = null;

        List<NodeRecord> openList = new List<NodeRecord>();
        List<NodeRecord> closedList = new List<NodeRecord>();

        List<Connection> path = null;
        public List<Connection> Path { get { return path; } }

        public List<Connection> FindPath(Vector3 pos)
        {
            if (pos == transform.position)
                return null;

            TileNavGraph graph = TileNavGraph.Instance;

            if (graph == null)
                return null;

            targetNode = graph.GetNode(pos);

            if (targetNode == null || targetNode.Weight == graph.UnreachableCost)
                return null;

            startNode = graph.GetNode(transform.position);

            ComputePathfinding(targetNode);
            return path;
        }

        private float EstimateDistance(Vector3 posA, Vector3 posB)
        {
            return Mathf.Abs(posB.x - posA.x) + Mathf.Abs(posB.z - posA.z);
        }

        private void ComputePathfinding(Node target)
        {
            NodeRecord goal = new NodeRecord(target);
            TileNavGraph graph = TileNavGraph.Instance;

            NodeRecord start = new NodeRecord(startNode);

            openList.Add(start);

            NodeRecord currentNode = null;

            while (openList.Count > 0)
            {
                if (openList.Count > 1)
                    openList = openList.OrderBy(o => o.estimatedTotalCost).ToList();
                currentNode = openList[0];

                closedList.Add(currentNode);
                openList.Remove(currentNode);

                if (currentNode.node == goal.node)
                    break;

                foreach (Connection connection in graph.ConnectionsGraph[currentNode.node])
                {
                    Node nextNode = connection.ToNode;
                    NodeRecord nextNodeRecord = closedList.Find(o => o.node == nextNode);
                    if (nextNodeRecord == null)
                    {
                        float nextNodeCost = currentNode.costSoFar + connection.Cost;
                        nextNodeRecord = openList.Find(o => o.node == nextNode);

                        if (nextNodeRecord == null)
                        {
                            NodeRecord record = new NodeRecord(nextNode);
                            record.costSoFar = nextNodeCost;
                            record.estimatedTotalCost = record.costSoFar + EstimateDistance(nextNode.Position, goal.node.Position);
                            record.connection = connection;
                            openList.Add(record);
                        }
                        else if (nextNodeRecord.costSoFar > nextNodeCost)
                        {
                            nextNodeRecord.costSoFar = nextNodeCost;
                            nextNodeRecord.estimatedTotalCost = nextNodeRecord.costSoFar + EstimateDistance(nextNode.Position, goal.node.Position);
                            nextNodeRecord.connection = connection;
                        }
                    }
                }
            }

            openList.Clear();
            RetracePath(start, currentNode);
        }

        private void RetracePath(NodeRecord start, NodeRecord end)
        {
            path = new List<Connection>();

            NodeRecord current = end;

            while (current != start)
            {
                path.Add(current.connection);
                current = closedList.Find(o => o.node == current.connection.FromNode);
            }

            path.Reverse();
            closedList.Clear();
        }
    }
}
