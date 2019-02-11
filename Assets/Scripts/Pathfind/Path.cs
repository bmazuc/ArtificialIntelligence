using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Path
    {
        List<Connection> connections = null;
        public List<Connection> Connections { get { return connections; } }

        public void SetSegments(List<Connection> newConnections)
        {
            this.connections = newConnections;
        }

        public void AddSegments(List<Connection> newConnections)
        {
            foreach (Connection connection in newConnections)
                this.connections.Add(connection);
        }

        public float GetParam(Vector3 position, float lastParam)
        {
            float param = 0f;
            float tempParam = 0f;

            Connection currentConnection = null;

            foreach (Connection connection in connections)
            {
                tempParam += Vector3.Distance(connection.FromNode.Position, connection.ToNode.Position);
                if (lastParam <= tempParam)
                {
                    currentConnection = connection;
                    break;
                }
            }
            if (currentConnection == null)
                return 0f;

            Vector3 begin = currentConnection.FromNode.Position;
            Vector3 end = currentConnection.ToNode.Position;

            Vector3 currentPosition = position - begin;
            Vector3 segmentDirection = Vector3.Normalize(end - begin);

            Vector3 pointInSegment = Vector3.Project(currentPosition, segmentDirection);
            param = tempParam - Vector3.Distance(begin, end);
            param += pointInSegment.magnitude;

            return param;
        }

        public Vector3 GetPosition(float param)
        {
            Vector3 position = Vector3.zero;
            float tempParam = 0f;
            Connection currentConnection = null;

            foreach (Connection connection in connections)
            {
                tempParam += Vector3.Distance(connection.FromNode.Position, connection.ToNode.Position);
                if (param <= tempParam)
                {
                    currentConnection = connection;
                    break;
                }
            }
            if (currentConnection == null)
                return position;

            Vector3 begin = currentConnection.FromNode.Position;
            Vector3 end = currentConnection.ToNode.Position;

            Vector3 currentPosition = position - begin;
            Vector3 segmentDirection = Vector3.Normalize(end - begin);

            tempParam = param - tempParam;
            position = begin + segmentDirection * tempParam;

            return position;
        }
    }
}