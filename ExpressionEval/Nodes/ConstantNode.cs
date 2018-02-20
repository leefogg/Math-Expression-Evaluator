namespace ExpressionEval.Nodes {
	public class ConstantNode : Node {
		public override int Value { get; }

		public ConstantNode(int value) {
			Value = value;
		}

		public static implicit operator ConstantNode(int val) => new ConstantNode(val);
	}
}
