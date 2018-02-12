using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEval.Nodes {
	public abstract class Node {
		public abstract int value { get; }

		public static implicit operator int(Node node) {
			return node.value;
		}
	}
}
