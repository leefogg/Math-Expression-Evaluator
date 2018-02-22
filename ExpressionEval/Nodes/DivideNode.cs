namespace ExpressionEval.Nodes {
	public class DivideNode : ArithmeticNode {
		public DivideNode() {}
		public DivideNode(Node left, Node right) : base(left, right) {}

		public override float Value => Left / Right;
	}
}
