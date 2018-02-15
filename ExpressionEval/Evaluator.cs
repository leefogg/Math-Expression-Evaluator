using ExpressionEval.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;	 

namespace ExpressionEval {
	public static class Evaluator {
		private static readonly char[] operators = new char[] { '+', '-', '*', '/' };

		public static int evaluate(string expression) {
			expression = normalizeExpression(expression);

			int explength = 0;
			var evaluationtree = buildExpression(1, expression, out explength);
			return evaluationtree.value;
		}

		public static string normalizeExpression(string expression) {
			expression = expression.Replace(" ", "");

			if (!expression.StartsWith("("))
				expression = '(' + expression;
			if (!expression.EndsWith("("))
				expression += ')';

			expression = expression.Replace("--", "+");

			return expression;
		}

		private static Node buildExpression(int startindex, string expression, out int length) {
			int index = startindex;
			List<Node> nodes = new List<Node>();

			while (index < expression.Length && expression[index] != ')') {
				int subexplength = 0;
				if (expression[index].isNumber()) {
					var numberend = findEndOfNumber(index, expression);
					string number = expression.Substring(index, numberend - index);

					int value = Int32.Parse(number);
					ConstantNode constant = value;

					index = numberend;

					nodes.Add(constant);
				} else if (expression[index].isOperator()) {
					char op = expression[index];
					nodes.Add(getArithmaticNode(op));

					index++;
				} else if (expression[index] == '(') {
					nodes.Add(buildExpression(index + 1, expression, out subexplength));
					index += subexplength + 1;
				}
			}

			length = index - startindex + 1;

			var result = convergeOperators(nodes);
			return result;
		}

		public static Node convergeOperators(List<Node> nodes) {
			if (nodes.Count == 1)
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
				if (nodes.Count > i && @operator.right == Node.zero) {
					@operator.right = nodes[i + 1];
					nodes.RemoveAt(i + 1);
				}
				if (i != 0 && @operator.left == Node.zero) {
					@operator.left = nodes[i - 1];
					nodes.RemoveAt(i - 1);
					i--;
				}
			}
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

		private static int findEndOfNumber(int startindex, string expression) {
			while (startindex < expression.Length && expression[startindex].isNumber())
				startindex++;

			return startindex;
		}

		private static bool isNumber(this char character) => character >= '0' && character <= '9';

		private static bool isOperator(this char character) => operators.Any(c => c == character);
	}
}
