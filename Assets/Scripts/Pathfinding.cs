using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour {

	public Grid grid;
	public Tilemap bounds;
	public Tilemap paths;
	public Tile clearTile;
	public Transform player;
	private int gridWidth;
	private int gridHeight;
	private Vector3 min;
	private Vector3 max;
	private bool isMoving = false;

	void Start () {
		this.gridWidth = bounds.size.x;
		// The extra one is to account for the top walkable area on the map
		this.gridHeight = bounds.size.y + 1;

		this.min = bounds.cellBounds.min;
		this.max = bounds.cellBounds.max;
	}

	// Update is called once per frame
	void Update () {

		if (!isMoving) {
			Node nextLoc = AStar ();

			if (nextLoc != null) {
				input = nextLoc.direction;
				StartCoroutine (move (transform));
			}
		}

		// Use an is moving flag
		// If not moving run astar and calculate the next position to move to
		// If moving do nothing and let the coroutine finish

	}

	public float moveSpeed = 1f;
	public float gridSize = 0.32f;
	private Vector3 input;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;

	public IEnumerator move (Transform transform) {
		isMoving = true;
		startPosition = transform.position;
		t = 0;
		endPosition = new Vector3 (startPosition.x + System.Math.Sign (input.x) * gridSize,
			startPosition.y + System.Math.Sign (input.y) * gridSize, startPosition.z);

		factor = 1f;

		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed / gridSize) * factor;
			transform.position = Vector3.Lerp (startPosition, endPosition, t);
			yield return null;
		}

		isMoving = false;
		yield return 0;
	}

	private Node AStar () {
		Queue<Node> openNodes = new Queue<Node> ();
		List<Node> closedNodes = new List<Node> ();

		Node startNode = new Node (null, bounds.WorldToCell (transform.position), Vector3Int.zero);
		Node endNode = new Node (null, bounds.WorldToCell (player.position), Vector3Int.zero);

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

				if (ValidTile (paths.GetTile (nodePos))) {
					children.Add (new Node (currentNode, nodePos, newPos));
				}
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

		return null;
	}

	private Node NextLocation (Node node) {
		List<Node> path = new List<Node> ();
		Node current = node;

		while (current != null) {
			path.Add (current);
			current = current.parent;
		}


		if (path.Count >= 2) {
			// We don't need the child pos and need the first actual next node
			return path[path.Count - 2];
		}
		return null;
	}

	private bool ValidTile (TileBase tile) {
		Debug.Log(tile);
		return tile == clearTile;
	}
}
