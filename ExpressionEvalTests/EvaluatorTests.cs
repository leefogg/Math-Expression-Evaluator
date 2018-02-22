using System;
using System.Collections.Generic;
using ExpressionEval;
using ExpressionEval.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionEvalTests {
	[TestClass]
	public class EvaluatorTests {
		[TestMethod]
		public void Evaluator_SimpleConstant() {
			int target = 42;
			var result = Evaluator.Evaluate("" + target);

			Assert.AreEqual(target, result);
		}

		[TestMethod]
		public void Evaluator_FloatConstant() {
			float target = 3.14159f;
			var result = Evaluator.Evaluate("" + target);

			Assert.AreEqual(target, result);
		}

		[TestMethod]
		public void Evaluator_FloatConstant_NoIntegerPart() {
			var result = Evaluator.Evaluate(".1");

			Assert.AreEqual(.1f, result);
		}

		[TestMethod]
		[ExpectedException(typeof(OverflowException))]
		public void Evaluator_LargeConstant() {
			Evaluator.Evaluate("445345346357342324453453463573423244534534635734232");
		}

		[TestMethod]
		public void Evaluator_NegitiveConstant() {
			var result = Evaluator.Evaluate("-42");

			Assert.AreEqual(-42, result);
		}

		[TestMethod]
		public void Evaluator_ConstantInBrackets() {
			var result = Evaluator.Evaluate("(42)");

			Assert.AreEqual(42, result);
		}

		[TestMethod]
		public void Evaluator_NegitiveExpression() {
			var result = Evaluator.Evaluate("-(42)");

			Assert.AreEqual(-42, result);
		}

		[TestMethod]
		public void Evaluator_RemoveWhitespace() {
			var result = Evaluator.Evaluate(" - ( 42 ) ");

			Assert.AreEqual(-42, result);
		}

		[TestMethod]
		public void Evaluator_PlusPrefix() {
			var result = Evaluator.Evaluate("+1");

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void Evaluator_ConstantInManyBrackets() {
			var result = Evaluator.Evaluate("((((42))))");

			Assert.AreEqual(42, result);
		}

		[TestMethod]
		public void Evaluator_NothingInBrackets() {
			var result = Evaluator.Evaluate("()");

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Evaluator_Add() {
			var result = Evaluator.Evaluate("1+1");

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void Evaluator_Add_Multiple() {
			var result = Evaluator.Evaluate("1+1+1");

			Assert.AreEqual(3, result);
		}

		[TestMethod]
		public void Evaluator_Subtract() {
			var result = Evaluator.Evaluate("1-1");

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Evaluator_Subtract_Multiple() {
			var result = Evaluator.Evaluate("2-1-1");

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Evaluator_Multiply() {
			var result = Evaluator.Evaluate("5*3");

			Assert.AreEqual(15, result);
		}

		[TestMethod]
		public void Evaluator_Multiply_Multiple() {
			var result = Evaluator.Evaluate("5*5*4");

			Assert.AreEqual(100, result);
		}

		[TestMethod]
		public void Evaluator_Divide() {
			var result = Evaluator.Evaluate("10/2");

			Assert.AreEqual(5, result);
		}

		[TestMethod]
		public void Evaluator_FloatMath() {
			var result = Evaluator.Evaluate("10/3*3");

			Assert.AreEqual(10, result);
		}

		[TestMethod]
		public void Evaluator_Divide_Multiple() { 
			var result = Evaluator.Evaluate("20/2/2");

			Assert.AreEqual(5, result);
		}

		[TestMethod]
		public void Evaluator_SubtractNegitiveShouldAdd() {
			var result = Evaluator.Evaluate("10 - -1");

			Assert.AreEqual(11, result);
		}

		[TestMethod]
		public void Evaluator_SubtractPossitiveShouldSubtract() {
			var result = Evaluator.Evaluate("10 - +1");

			Assert.AreEqual(9, result);
		}

		[TestMethod]
		public void Evaluator_AddNegitiveShouldSubtract() {
			var result = Evaluator.Evaluate("10 + -1");

			Assert.AreEqual(9, result);
		}

		[TestMethod]
		public void Evaluator_AddPositiveShouldAdd() {
			var result = Evaluator.Evaluate("10 + +1");

			Assert.AreEqual(11, result);
		}

		[TestMethod]
		public void Evaluator_WholeExpressionInBrackets() {
			var result = Evaluator.Evaluate("(1+1)");

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void Evaluator_BODMAS() {
			var result = Evaluator.Evaluate("25-(3+2)*10/2");

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Evaluator_MultiplyNegitive() {
			Assert.AreEqual(-10, Evaluator.Evaluate("10*-1"));
			Assert.AreEqual(-20, Evaluator.Evaluate("-10*2"));
		}

		[TestMethod]
		public void Evaluator_DivideNegitive() {
			Assert.AreEqual(-10, Evaluator.Evaluate("10/-1"));
			Assert.AreEqual(-5, Evaluator.Evaluate("-10/2"));
		}


		[TestMethod]
		public void Evaluator_convergeOperators_SingleResolve() {
			var nodes = new List<Node>
			{
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1)
			};

			var resolved = Evaluator.ConvergeOperators(nodes);

			Assert.IsTrue(resolved is AddNode);
			Assert.AreEqual(2f, resolved);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_DoubleResolve() {
			var nodes = new List<Node>
			{
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1)
			};

			var resolved = Evaluator.ConvergeOperators(nodes);

			Assert.IsTrue(resolved is AddNode);
			Assert.IsTrue((resolved as AddNode).Left is AddNode);
			Assert.AreEqual(3f, resolved);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_MultipleResolve() {
			var nodes = new List<Node>
			{
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1),
				new AddNode(),
				new ConstantNode(1)
			};

			var resolved = Evaluator.ConvergeOperators(nodes);

			Assert.IsTrue(resolved is AddNode);
			Assert.IsTrue((resolved as AddNode).Left is AddNode);
			Assert.IsTrue(((resolved as AddNode).Left as AddNode).Left is AddNode);
			Assert.AreEqual(4f, resolved);
		}

		[TestMethod]
		public void Evaluator_convergeOperators_BODMAS() {
			var nodes = new List<Node>
			{
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

			var resolved = Evaluator.ConvergeOperators(nodes);

			Assert.AreEqual(0f, resolved);
		}

		[TestMethod]
		public void Evaluator_Harsh() {
			var result = Evaluator.Evaluate("100 / 5 + ((88 / 44) * 1) - -(10 -10 + -10)");

			Assert.AreEqual(12, result);
		}
	}
}
