namespace ExpressionEval.Nodes {
	public abstract class ArithmeticNode : Node {
		public Node
			Left = ZERO,
			Right = ZERO;

		public ArithmeticNode() {}

		public ArithmeticNode(Node left, Node right) {
			Left = left;
			Right = right;
		}
	}
}
