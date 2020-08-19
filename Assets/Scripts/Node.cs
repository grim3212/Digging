using UnityEngine;

public class Node {

	public Node parent;
	public Vector3Int position;
	public int distanceFromStart = 0;
	public int estimateToEnd = 0;
	public int cost = 0;

	public Node (Node parent, Vector3Int position) {
		this.parent = parent;
		this.position = position;
	}

	public override bool Equals (object obj) {
		return this.Equals (obj as Node);
	}

	public bool Equals (Node node) {
		return this.position == node.position && this.parent == node.parent;
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public static bool operator == (Node left, Node right) {
		return left.position == right.position;
	}

	public static bool operator != (Node left, Node right) {
		return left.position != right.position;
	}

	public static bool operator < (Node left, Node right) {
		return left.cost < right.cost;
	}
	public static bool operator > (Node left, Node right) {
		return left.cost > right.cost;
	}
}