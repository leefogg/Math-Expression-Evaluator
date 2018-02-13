using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEval.Nodes {
	public abstract class ArithmeticNode : Node {
		private static readonly ConstantNode zero = new ConstantNode(0);

		public Node 
			left = zero, 
			right = zero;

		public ArithmeticNode() { }
		public ArithmeticNode(Node left, Node right) {
			this.left = left;
			this.right = right;
		}
	}
}
