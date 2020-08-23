using UnityEngine;

public class Node : System.IComparable<Node> {

	public Node parent;
	public Vector3Int position;
	public Vector3Int direction;
	public int distanceFromStart = 0;
	public int estimateToEnd = 0;
	public int cost = 0;

	public Node (Node parent, Vector3Int position, Vector3Int direction) {
		this.parent = parent;
		this.position = position;
		this.direction = direction;
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

	public int CompareTo (Node node) {
		return this.cost.CompareTo (node.cost);
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