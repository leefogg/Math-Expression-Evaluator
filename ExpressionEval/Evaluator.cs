using ExpressionEval.Nodes;
using System;
using System.Linq;

namespace ExpressionEval {
	public static class Evaluator {
		private static readonly char[] operators = new char[] { '+', '-', '*', '/' };

		public static int evaluate(string expression) {
			expression = expression.Replace(" ", "");
			var evaluationtree = buildExpression(0, expression);
			return evaluationtree.value;
		}

		private static Node buildExpression(int startindex, string expression) {
			int index = startindex;

			if (expression[index].isNumber()) {
				var numberend = findEndOfNumber(index, expression);
				string number = expression.Substring(index, numberend - index);

				int value = Int32.Parse(number);
				ConstantNode constant = value;

				index = numberend;
				if (index >= expression.Length || !expression[index].isOperator())
					return constant;

				char op = expression[index];
				var rightexpression = buildExpression(index + 1, expression);
				return buildArithmeticNode(constant, rightexpression, op);
			} else if (expression[index] == '-') {
				return new MultiplyNode(buildExpression(index+1, expression), new ConstantNode(-1));
			} else if (expression[index] == '(') {
				return buildExpression(index + 1, expression);
			}

			return null;
		}

		private static ArithmeticNode buildArithmeticNode(Node left, Node right, char op) {
			switch (op) {
				case '+':
					return new AddNode(left, right);
				case '-':
					return new SubtractNode(left, right);
				case '*':
					return new MultiplyNode(left, right);
				case '/':
					return new DivideNode(left, right);
			}

			return null;
		}

		private static int findCloseBracket(int index, string expression) {
			int depth = 0;
			do {
				if (expression[index] == '(')
					depth++;
				else if (expression[index] == ')')
					depth--;
			} while (index < expression.Length && depth > 0);

			return index;
		}

		private static int findEndOfNumber(int startindex, string expression) {
			while (startindex < expression.Length && expression[startindex].isNumber())
				startindex++;

			return startindex;
		}

		private static bool isNumber(this char character) {
			return character >= '0' && character <= '9';
		}

		private static bool isOperator(this char character) {
			return operators.Any(c => c == character);
		}
	}
}
