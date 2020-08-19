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
		if (Object.ReferenceEquals (node, null)) {
			return false;
		}

		if (Object.ReferenceEquals (this, node)) {
			return true;
		}

		if (this.GetType () != node.GetType ()) {
			return false;
		}

		return this.position == node.position;
	}

	public override int GetHashCode () {
		return base.GetHashCode ();
	}

	public static bool operator == (Node left, Node right) {
		if (Object.ReferenceEquals (left, null)) {
			if (Object.ReferenceEquals (right, null)) {
				return true;
			}

			return false;
		}
		return left.Equals (right);
	}

	public static bool operator != (Node left, Node right) {
		return !(left == right);
	}
}