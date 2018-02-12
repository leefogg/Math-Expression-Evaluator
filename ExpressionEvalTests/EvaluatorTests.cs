using ExpressionEval;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
	}
}
