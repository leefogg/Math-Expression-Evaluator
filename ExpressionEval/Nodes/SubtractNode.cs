namespace ExpressionEval.Nodes {
	public class SubtractNode : ArithmeticNode {
		public SubtractNode() {}
		public SubtractNode(Node left, Node right) : base(left, right) {}

		public override int Value => Left - Right;
	}
}
