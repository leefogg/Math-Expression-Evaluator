namespace ExpressionEval.Nodes {
	public abstract class Node {
		public static readonly ConstantNode ZERO = new ConstantNode(0);

		public abstract float Value { get; }

		public static implicit operator float(Node node) => node.Value;
	}
}
