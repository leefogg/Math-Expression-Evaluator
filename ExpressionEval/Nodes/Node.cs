namespace ExpressionEval.Nodes {
	public abstract class Node {
		public static readonly ConstantNode ZERO = new ConstantNode(0);

		public abstract int Value { get; }

		public static implicit operator int(Node node) {
			return node.Value;
		}
	}
}
