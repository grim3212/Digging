using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour {

	public Grid grid;
	public Tilemap map;
	public Tile clearTile;
	public Transform player;
	private Vector3 max;
	private bool isMoving = false;

	void Start () {
		this.max = map.cellBounds.max + Vector3.up;
	}

	// Update is called once per frame
	void Update () {

		if (!isMoving) {
			Node nextLoc = AStar ();

			if (nextLoc != null) {
				input = nextLoc.direction;
				StartCoroutine (move (transform));
			} else {
				NonTrackingMovement ();
			}

		}
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

		Node startNode = new Node (null, map.WorldToCell (transform.position), Vector3Int.zero);
		Node endNode = new Node (null, map.WorldToCell (player.position), Vector3Int.zero);

		openNodes.Enqueue (startNode);

		int itr = 0;

		Vector3Int[] adjacent = { Vector3Int.down, Vector3Int.up, Vector3Int.left, Vector3Int.right };

		while (openNodes.Count > 0) {
			itr++;

			if (itr >= 1000) {
				Debug.Log ("Maximum iterations of [1000] reached during pathfinding");
				break;
			}

			Node currentNode = openNodes.Dequeue ();
			closedNodes.Add (currentNode);

			if (currentNode == endNode) {
				return NextLocation (currentNode);
			}


			List<Node> children = new List<Node> ();
			foreach (Vector3Int direction in adjacent) {
				Vector3Int nodePos = currentNode.position + direction;

				if (nodePos.x > this.max.x || nodePos.y > this.max.y) {
					continue;
				}

				if (ValidTile (map.GetTile (nodePos))) {
					// Is it possible to raycast here as well to check if there is a collision
					// What happens if a tilemap is setup wrong or for some other reason we need
					// to make sure that it is not a collision
					children.Add (new Node (currentNode, nodePos, direction));
				}
				else {
					if (nodePos.y + 1 == this.max.y) {
						children.Add (new Node (currentNode, nodePos, direction));
					}
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
		return tile == clearTile;
	}

	private Vector3Int previousDirection;

	private void NonTrackingMovement () {
		Vector3Int coordinate = grid.WorldToCell (transform.position);
		if (previousDirection != Vector3Int.zero && ValidTile (map.GetTile (coordinate + previousDirection))) {
			input = new Vector2 (previousDirection.x, previousDirection.y);
		}
		else if (previousDirection == Vector3Int.up) {
			if (ValidTile (map.GetTile (coordinate + Vector3Int.right))) {
				input = Vector2.right;
				previousDirection = Vector3Int.right;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.left))) {
				input = Vector2.left;
				previousDirection = Vector3Int.left;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.down))) {
				input = Vector2.down;
				previousDirection = Vector3Int.down;
			}
			else {
				input = Vector2.zero;
				previousDirection = Vector3Int.zero;
			}
		}
		else if (previousDirection == Vector3Int.right) {
			if (ValidTile (map.GetTile (coordinate + Vector3Int.up))) {
				input = Vector2.up;
				previousDirection = Vector3Int.up;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.down))) {
				input = Vector2.down;
				previousDirection = Vector3Int.down;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.left))) {
				input = Vector2.left;
				previousDirection = Vector3Int.left;
			}
			else {
				input = Vector2.zero;
				previousDirection = Vector3Int.zero;
			}
		}
		else if (previousDirection == Vector3Int.left) {
			if (ValidTile (map.GetTile (coordinate + Vector3Int.up))) {
				input = Vector2.up;
				previousDirection = Vector3Int.up;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.down))) {
				input = Vector2.down;
				previousDirection = Vector3Int.down;
			}

			else if (ValidTile (map.GetTile (coordinate + Vector3Int.right))) {
				input = Vector2.right;
				previousDirection = Vector3Int.right;
			}
			else {
				input = Vector2.zero;
				previousDirection = Vector3Int.zero;
			}
		}
		else if (previousDirection == Vector3Int.down) {
			if (ValidTile (map.GetTile (coordinate + Vector3Int.left))) {
				input = Vector2.left;
				previousDirection = Vector3Int.left;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.right))) {
				input = Vector2.right;
				previousDirection = Vector3Int.right;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.up))) {
				input = Vector2.up;
				previousDirection = Vector3Int.up;
			}
			else {
				input = Vector2.zero;
				previousDirection = Vector3Int.zero;
			}
		}
		else if (ValidTile (map.GetTile (coordinate + Vector3Int.up))) {
			input = Vector2.up;
			previousDirection = Vector3Int.up;
		}
		else if (ValidTile (map.GetTile (coordinate + Vector3Int.left))) {
			input = Vector2.left;
			previousDirection = Vector3Int.left;
		}
		else if (ValidTile (map.GetTile (coordinate + Vector3Int.down))) {
			input = Vector2.down;
			previousDirection = Vector3Int.down;
		}
		else if (ValidTile (map.GetTile (coordinate + Vector3Int.right))) {
			input = Vector2.right;
			previousDirection = Vector3Int.right;
		}
		else {
			input = Vector2.zero;
			previousDirection = Vector3Int.zero;
		}

		if (input != Vector3.zero) {
			if (!(input == Vector3.up && (transform.position.y + 0.32f) >= this.max.y)) {
				RaycastHit2D hit = Physics2D.Raycast (transform.position, input, 0.32f);
				if (hit.collider == null) {
					StartCoroutine (move (transform));
				}
				else {
					previousDirection = Vector3Int.zero;
				}
			}
		}
	}
}
