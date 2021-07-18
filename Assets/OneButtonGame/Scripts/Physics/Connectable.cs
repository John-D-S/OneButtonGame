using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connection
{
    [Tooltip("")]
    public Connectable connectedConnectable;
    [HideInInspector] //the rigidbody of the other connectable
    public Rigidbody connectedRigidbody;

    public Connection(Connectable _connectedConnectable, Rigidbody _connectedRigidbody)
    {
        connectedConnectable = _connectedConnectable;
        connectedRigidbody = _connectedRigidbody;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class Connectable : MonoBehaviour
{
    public List<Connection> connections = new List<Connection>();
    List<Connectable> CurrentConnectedConnectables
    {
        get
        {
            List<Connectable> returnValue = new List<Connectable>();
            foreach (Connection connection in connections)
            {
                if (connection != null)
                {
                    returnValue.Add(connection.connectedConnectable);
                }
            }
            return returnValue;
        }
    }

    [SerializeField, HideInInspector]
    private List<Connection> oldConnections;
    List<Connectable> OldConnectedConnectables
    {
        get
        {
            List<Connectable> returnValue = new List<Connectable>();
            foreach (Connection connection in oldConnections)
            {
                returnValue.Add(connection.connectedConnectable);
            }
            return returnValue;
        }
    }

    [SerializeField, HideInInspector]
    public Rigidbody rigidBody;

    public List<Joint> Joints
    {
        get
        {
            return new List<Joint>(GetComponents<Joint>());
        }
    }

    public void AddJoint(Connection connection)
    {
        bool thisGOJointExists = false;
        List<Joint> thisGOJoints = Joints;
        foreach (Joint joint in thisGOJoints)
        {
            if (joint.connectedBody == connection.connectedConnectable.rigidBody)
            {
                thisGOJointExists = true;
            }
        }
        if (!thisGOJointExists)
        {
            Joint thisGOJoint = gameObject.AddComponent<FixedJoint>();
            thisGOJoint.connectedBody = connection.connectedConnectable.rigidBody;
            thisGOJoint.enablePreprocessing = false;
        }
    }

    public IEnumerator RemoveExcessJoints()
    {
        yield return new WaitForEndOfFrame();
        List<Joint> thisGOJoints = Joints;
        List<Rigidbody> connectedConnectableRigidbodies = new List<Rigidbody>();
        //if (connections != null)
        //{
            foreach (Connection connection in connections)
            {
                if (connection != null && connection.connectedConnectable != null)
                {
                    connectedConnectableRigidbodies.Add(connection.connectedConnectable.rigidBody);
                }
            }
            foreach (Joint joint in thisGOJoints)
            {
                if (!connectedConnectableRigidbodies.Contains(joint.connectedBody))
                {
                    DestroyImmediate(joint);
                }
            }
        //}
    }

    public Connection ConnectionToConnectable(Connectable connectable)
    {
        foreach (Connection connection in connections)
        {
            if (connection != null && connection.connectedConnectable == connectable)
            {
                return connection;
            }
        }
        return null;
    }

    private void OnValidate()
    {
        //add this rigidbody to the rigidbody variable.
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        if (connections != null)
        {
            if (oldConnections != null && connections.Count > oldConnections.Count)
            {
                connections[connections.Count - 1] = null;
            }
            foreach (Connection connection in connections)
            {
                if (connection != null && connection.connectedConnectable != null)
                {
                    AddJoint(connection);
                    if (connection.connectedConnectable.ConnectionToConnectable(this) == null)
                    {
                        connection.connectedConnectable.connections.Add(new Connection(this, rigidBody));
                    }
                    connection.connectedConnectable.AddJoint(connection.connectedConnectable.ConnectionToConnectable(this));
                }
            }
            foreach (Connection oldConnection in oldConnections)
            {
                if (oldConnection.connectedConnectable != null && !CurrentConnectedConnectables.Contains(oldConnection.connectedConnectable))
                {
                    Debug.Log("thingRemoved");
                    Connection connectionToThisConnectable = oldConnection.connectedConnectable.ConnectionToConnectable(this);
                    if (connectionToThisConnectable != null)
                    {
                        oldConnection.connectedConnectable.connections.Remove(connectionToThisConnectable);
                        oldConnection.connectedConnectable.StartCoroutine(RemoveExcessJoints());
                    }
                }
            }
        }
        foreach (Joint joint in Joints)
        {
            joint.enablePreprocessing = false;
        }
        StartCoroutine(RemoveExcessJoints());
        oldConnections = new List<Connection>(connections.ToArray());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (connections != null)
        {
            foreach (Connection connection in connections)
            {
                if (connection.connectedConnectable)
                {
                    Gizmos.DrawLine(transform.position, connection.connectedConnectable.gameObject.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (connections != null && UnityEditor.Selection.activeGameObject != gameObject)
        {
            foreach (Connection connection in connections)
            {
                if (connection != null && connection.connectedConnectable)
                {
                    Gizmos.DrawLine(transform.position, connection.connectedConnectable.gameObject.transform.position);
                }
            }
        }
    }
}
