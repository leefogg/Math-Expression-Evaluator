using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEval.Nodes {
	public class ConstantNode : Node {
		private int val;

		public ConstantNode(int value) {
			val = value;
		}

		public override int value => val;

		public static implicit operator ConstantNode(int val) {
			return new ConstantNode(val);
		}
	}
}
