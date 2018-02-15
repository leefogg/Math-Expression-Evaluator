using ExpressionEval;
using ExpressionEval.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ExpressionEvalTests {
	[TestClass]
	public class EvaluatorTests {
		[TestMethod]
		public void Evaluator_SimpleConstant() {
			int result = Evaluator.evaluate("42");

			Assert.AreEqual(42, result);
		}

		[TestMethod]
		[ExpectedException(typeof(OverflowException))]
		public void Evaluator_LargeConstant() {
			int result = Evaluator.evaluate("44534534635734232");
		}

		[TestMethod]
		public void Evaluator_NegitiveConstant() {
			int result = Evaluator.evaluate("-42");

			Assert.AreEqual(-42, result);
		}

		[TestMethod]
		public void Evaluator_ConstantInBrackets() {
			int result = Evaluator.evaluate("(42)");

			Assert.AreEqual(42, result);
		}

		[TestMethod]
		public void Evaluator_NegitiveExpression() {
			int result = Evaluator.evaluate("-(42)");

			Assert.AreEqual(-42, result);
		}

		[TestMethod]
		public void Evaluator_RemoveWhitespace() {
			int result = Evaluator.evaluate(" - ( 42 ) ");

			Assert.AreEqual(-42, result);
		}

		[TestMethod]
		public void Evaluator_PlusPrefix() {
			int result = Evaluator.evaluate("+1");

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void Evaluator_ConstantInManyBrackets() {
			int result = Evaluator.evaluate("((((42))))");

			Assert.AreEqual(42, result);
		}

		[TestMethod]
		public void Evaluator_Add() {
			int result = Evaluator.evaluate("1+1");

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void Evaluator_Subtract() {
			int result = Evaluator.evaluate("1-1");

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Evaluator_Multiply() {
			int result = Evaluator.evaluate("5*3");

			Assert.AreEqual(15, result);
		}

		[TestMethod]
		public void Evaluator_Divide() {
			int result = Evaluator.evaluate("10/2");

			Assert.AreEqual(5, result);
		}

		[TestMethod]
		public void Evaluator_WholeExpressionInBrackets() {
			int result = Evaluator.evaluate("(1+1)");

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void Evaluator_BODMAS() {
			int result = Evaluator.evaluate("25-(3+2)*10/2");

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_SingleResolve() {
			List<Node> nodes = new List<Node>() {
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1)
			};

			var resolved = Evaluator.convergeOperators(nodes);

			Assert.IsTrue(resolved is AddNode);
			Assert.AreEqual(2, resolved);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_DoubleResolve() {
			List<Node> nodes = new List<Node>() {
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1)
			};

			var resolved = Evaluator.convergeOperators(nodes);

			Assert.IsTrue(resolved is AddNode);
			Assert.IsTrue((resolved as AddNode).left is AddNode);
			Assert.AreEqual(3, resolved);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_MultipleResolve() {
			List<Node> nodes = new List<Node>() {
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1)
			};

			var resolved = Evaluator.convergeOperators(nodes);

			Assert.IsTrue(resolved is AddNode);
			Assert.IsTrue((resolved as AddNode).left is AddNode);
			Assert.IsTrue(((resolved as AddNode).left as AddNode).left is AddNode);
			Assert.AreEqual(4, resolved);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_BODMAS() {
			List<Node> nodes = new List<Node>() {
				new ConstantNode(13),
				new SubtractNode(),
				new ConstantNode(3),
				new AddNode(),
				new ConstantNode(2),
				new MultiplyNode(),
				new ConstantNode(10),
				new DivideNode(),
				new ConstantNode(2)
			};

			var resolved = Evaluator.convergeOperators(nodes);

			Assert.AreEqual(0, resolved);
		}

		[TestMethod]
		public void Evaluator_Harsh() {
			int result = Evaluator.evaluate("88 / 44 * ((55 / 11 + 1) * 2) - 10 + 10");

			Assert.AreEqual(4, result);
		}
	}
}
