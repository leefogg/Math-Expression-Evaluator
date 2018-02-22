namespace ExpressionEval.Nodes {
	public class SubtractNode : ArithmeticNode {
		public SubtractNode() {}
		public SubtractNode(Node left, Node right) : base(left, right) {}

		public override float Value => Left - Right;
	}
}
