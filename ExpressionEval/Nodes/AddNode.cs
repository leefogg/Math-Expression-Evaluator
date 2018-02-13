using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEval.Nodes {
	public class AddNode : ArithmeticNode {
		public AddNode() : base() { }
		public AddNode(Node left, Node right) : base(left, right) { }

		public override int value => left + right;
	}
}
