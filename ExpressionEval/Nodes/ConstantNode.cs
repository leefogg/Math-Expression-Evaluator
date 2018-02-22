namespace ExpressionEval.Nodes {
	public class ConstantNode : Node {
		public override float Value { get; }

		public ConstantNode(int value) : this((float)value) { }
		public ConstantNode(float value) {
			Value = value;
		}

		public static implicit operator ConstantNode(int val) => new ConstantNode(val);
		public static implicit operator ConstantNode(float val) => new ConstantNode(val);
	}
}
