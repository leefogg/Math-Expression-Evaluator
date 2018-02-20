using ExpressionEval.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionEvalTests {
	[TestClass]
	public class NodeTests {
		[TestMethod]
		public void ConstantNode_ReadWrite() {
			var node = new ConstantNode(3487);

			Assert.AreEqual(node.Value, 3487);
		}

		[TestMethod]
		public void AddNode() {
			int 
				val1 = 10,
				val2 = 20,
				result = val1 + val2;
			var node = new AddNode(new ConstantNode(val1), new ConstantNode(val2));

			Assert.AreEqual(node.Value, result);
		}

		[TestMethod]
		public void SubtractNode() {
			int
				val1 = 10,
				val2 = 20,
				result = val1 - val2;
			var node = new SubtractNode(new ConstantNode(val1), new ConstantNode(val2));

			Assert.AreEqual(node.Value, result);
		}

		[TestMethod]
		public void MultiplyNode() {
			int
				val1 = 10,
				val2 = 20,
				result = val1 * val2;
			var node = new MultiplyNode(new ConstantNode(val1), new ConstantNode(val2));

			Assert.AreEqual(node.Value, result);
		}

		[TestMethod]
		public void DivideNode() {
			int
				val1 = 50,
				val2 = 5,
				result = val1 / val2;
			var node = new DivideNode(new ConstantNode(val1), new ConstantNode(val2));

			Assert.AreEqual(node.Value, result);
		}

		[TestMethod]
		public void ConstantNode_Implicit_FromInt() {
			ConstantNode node = 1;

			Assert.AreEqual(node.Value, 1);
		}

		[TestMethod]
		public void Node_Implicit_ToInt() {
			Node node = new ConstantNode(10);

			Assert.AreEqual(node, 10);
		}
	}
}
