namespace ExpressionEval.Nodes {
	public class MultiplyNode : ArithmeticNode {
		public MultiplyNode() {}
		public MultiplyNode(Node left, Node right) : base(left, right) {}

		public override int Value => Left * Right;
	}
}
