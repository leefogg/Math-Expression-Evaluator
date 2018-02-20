namespace ExpressionEval.Nodes {
	public class AddNode : ArithmeticNode {
		public AddNode() {}
		public AddNode(Node left, Node right) : base(left, right) {}

		public override int Value => Left + Right;
	}
}
