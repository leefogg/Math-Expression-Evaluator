using ExpressionEval.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;	 

namespace ExpressionEval {
	public static class Evaluator {
		private static readonly char[] operators = new char[] { '+', '-', '*', '/' };

		public static int evaluate(string expression) {
			expression = expression.Replace(" ", "");
			expression = $"({expression})";
			int explength = 0;
			var evaluationtree = buildExpression(0, expression, out explength);
			return evaluationtree.value;
		}

		private static Node buildExpression(int startindex, string expression, out int length) {
			int index = startindex;
			List<Node> nodes = new List<Node>();

			while (expression[index] != ')') {
				int subexplength = 0;
				if (expression[index].isNumber()) {
					var numberend = findEndOfNumber(index, expression);
					string number = expression.Substring(index, numberend - index);

					int value = Int32.Parse(number);
					ConstantNode constant = value;

					index = numberend;

					nodes.Add(constant);
				} else if (expression[index].isOperator() && index < expression.Length) {
					char op = expression[index];
					var rightexpression = buildExpression(index + 1, expression, out subexplength);
					nodes.Add(getArithmaticNode(op));
					nodes.Add(rightexpression);

					index += subexplength + 1;
				} else if (expression[index] == '(') {
					nodes.Add(buildExpression(index + 1, expression, out subexplength));
					index += subexplength + 1;
				}
			}

			length = index - startindex;
			return convergeOperators(nodes);
		}

		public static Node convergeOperators(List<Node> nodes) {
			if (nodes.Count == 1 && nodes[0] is ConstantNode)
				return nodes[0];

			convergeOperator<DivideNode>	(nodes);
			convergeOperator<MultiplyNode>	(nodes);
			convergeOperator<AddNode>		(nodes);
			convergeOperator<SubtractNode>	(nodes);

			return nodes[0];
		}

		private static void convergeOperator<T>(List<Node> nodes) where T : ArithmeticNode {
			for (int i = 0; i < nodes.Count - 1; i++) {
				if (!(nodes[i] is T))
					continue;

				var @operator = nodes[i] as ArithmeticNode;
				if (nodes.Count > i) {
					@operator.right = nodes[i + 1];
					nodes.RemoveAt(i + 1);
				}
				if (i != 0) {
					@operator.left = nodes[i - 1];
					nodes.RemoveAt(i - 1);
				}

				i--;
			}
		}

		private static ArithmeticNode buildArithmeticNode(Node left, Node right, char op) {
			var @operator = getArithmaticNode(op);
			@operator.left = left;
			@operator.right = right;

			return @operator;
		}

		private static ArithmeticNode getArithmaticNode(char op) {
			switch (op) {
				case '+':
					return new AddNode();
				case '-':
					return new SubtractNode();
				case '*':
					return new MultiplyNode();
				case '/':
					return new DivideNode();
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
