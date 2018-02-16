using System;
using ExpressionEval.Nodes;
using System.Collections.Generic;
using System.Linq;	 

namespace ExpressionEval {
	public static class Evaluator {
		private static readonly char[] Operators = { '+', '-', '*', '/' };

		public static int evaluate(string expression) {
			expression = normalizeExpression(expression);

			var evaluationtree = buildExpression(1, expression, out var explength);
			return evaluationtree.value;
		}

		public static string normalizeExpression(string expression) {
			expression = expression.Replace(" ", "");

			if (!expression.StartsWith("("))
				expression = '(' + expression;
			if (!expression.EndsWith("("))
				expression += ')';

			// Simplifying here is much simpler than down there
			expression = expression.Replace("--", "+");
			expression = expression.Replace("++", "+");
			expression = expression.Replace("-+", "-");
			expression = expression.Replace("+-", "-");

			return expression;
		}

		private static Node buildExpression(int startindex, string expression, out int length) {
			var index = startindex;
			var nodes = new List<Node>();

			while (index < expression.Length && expression[index] != ')') {
				if (expression[index].isNumber()) {
					var numberend = findEndOfNumber(index, expression);
					var number = expression.Substring(index, numberend - index);

					var value = int.Parse(number);
					ConstantNode constant = value;

					index = numberend;

					nodes.Add(constant);
				} else if (expression[index].isOperator()) {
					var op = expression[index];
					nodes.Add(getArithmaticNode(op));

					index++;
				} else if (expression[index] == '(') {
					nodes.Add(buildExpression(index + 1, expression, out var subexplength));
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

		private static void convergeOperator<T>(IList<Node> nodes) where T : ArithmeticNode {
			for (var i = 0; i < nodes.Count - 1; i++) {
				if (!(nodes[i] is T))
					continue;

				var @operator = (ArithmeticNode)nodes[i];
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

		private static bool isOperator(this char character) => Operators.Any(c => c == character);
	}
}
