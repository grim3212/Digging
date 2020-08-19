using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour {

	public Tilemap bounds;
	public Tilemap paths;
	public Transform player;
	private int gridWidth;
	private int gridHeight;
	private Vector3 min;
	private Vector3 max;

	void Start () {
		this.gridWidth = bounds.size.x;
		// The extra one is to account for the top walkable area on the map
		this.gridHeight = bounds.size.y + 1;

		this.min = bounds.cellBounds.min;
		this.max = bounds.cellBounds.max;
	}

	// Update is called once per frame
	void Update () {

		// Use an is moving flag
		// If not moving run astar and calculate the next position to move to
		// If moving do nothing and let the coroutine finish
		AStar ();
	}

	private Vector3Int? AStar () {
		Queue<Node> openNodes = new Queue<Node> ();
		List<Node> closedNodes = new List<Node> ();

		Node startNode = new Node (null, bounds.WorldToCell (transform.position));
		Node endNode = new Node (null, bounds.WorldToCell (player.position));

		openNodes.Enqueue (startNode);

		int itr = 0;
		int maxItr = Mathf.FloorToInt ((gridWidth * gridHeight) / 2);

		Vector3Int[] adjacent = { Vector3Int.down, Vector3Int.up, Vector3Int.left, Vector3Int.right };

		while (openNodes.Count > 0) {
			itr++;

			if (itr >= maxItr) {
				Debug.Log ("Maximum iterations reached during pathfinding");
				break;
			}

			Node currentNode = openNodes.Dequeue ();
			closedNodes.Add (currentNode);

			if (currentNode == endNode) {
				return NextLocation (currentNode);
			}


			List<Node> children = new List<Node> ();
			foreach (Vector3Int newPos in adjacent) {
				Vector3Int nodePos = currentNode.position + newPos;

				// Need to adjust this check to make sure that we are only looking in the correct bounds
				if (nodePos.x > this.max.x || nodePos.x < this.min.x || nodePos.y > this.max.y || nodePos.y < this.min.y) {
					continue;
				}

				children.Add (new Node (currentNode, nodePos));
			}

			foreach (Node child in children) {
				if (closedNodes.Contains (child)) {
					continue;
				}

				child.distanceFromStart = currentNode.distanceFromStart + 1;
				child.estimateToEnd = Mathf.FloorToInt ((child.position.x - endNode.position.x) * 2) + Mathf.FloorToInt ((child.position.y - endNode.position.y) * 2);
				child.cost = child.distanceFromStart + child.estimateToEnd;

				if (openNodes.Contains (child)) {
					continue;
				}

				openNodes.Enqueue (child);
			}
		}

		Debug.Log ("No path found to get to player : " + itr);
		return null;
	}

	private Vector3Int NextLocation (Node node) {
		List<Vector3Int> path = new List<Vector3Int> ();
		Node current = node;

		while (current != null) {
			path.Add (current.position);
			current = current.parent;
		}

		// We only need the last position to move to
		return path[path.Count - 1];
	}
}
