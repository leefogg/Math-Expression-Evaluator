using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEval.Nodes {
	public abstract class ArithmeticNode : Node {
		protected Node left, right;

		public ArithmeticNode(Node left, Node right) {
			this.left = left;
			this.right = right;
		}
	}
}
