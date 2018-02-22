using System.Collections.Generic;
using System.Linq;
using ExpressionEval.Nodes;

namespace ExpressionEval {
	public static class Evaluator {
		private static readonly char[] Operators = { '+', '-', '*', '/' };

		public static int Evaluate(string expression) {
			expression = NormalizeExpression(expression);

			var evaluationtree = BuildExpression(1, expression, out var explength);
			return evaluationtree.Value;
		}

		private static string NormalizeExpression(string expression) {
			expression = expression.Replace(" ", "");

			if (!expression.StartsWith("("))
				expression = '(' + expression;
			if (!expression.EndsWith(")"))
				expression += ')';

			// Simplifying here is much simpler than down there
			expression = expression.Replace("--", "+");
			expression = expression.Replace("++", "+");
			expression = expression.Replace("-+", "-");
			expression = expression.Replace("+-", "-");

			return expression;
		}

		private static Node BuildExpression(int startindex, string expression, out int length) {
			var index = startindex;
			var nodes = new List<Node>();

			while (index < expression.Length && expression[index] != ')') {
				if (expression[index].IsNumber()) {
					var numberend = FindEndOfNumber(index, expression);
					var number = expression.Substring(index, numberend - index);

					var value = int.Parse(number);
					ConstantNode constant = value;

					index = numberend;

					nodes.Add(constant);
				} else if (expression[index].IsOperator()) {
					var op = expression[index];
					nodes.Add(GetArithmaticNode(op));

					index++;
				} else if (expression[index] == '(') {
					nodes.Add(BuildExpression(index + 1, expression, out var subexplength));
					index += subexplength + 1;
				}
			}

			length = index - startindex + 1;

			var result = ConvergeOperators(nodes);
			return result;
		}

		public static Node ConvergeOperators(IList<Node> nodes) {
			if (nodes.Count == 1)
				return nodes[0];

			ConvergeNegitiveExpressions		(nodes);
			ConvergeOperator<DivideNode>	(nodes);
			ConvergeOperator<MultiplyNode>	(nodes);
			ConvergeOperator<AddNode>		(nodes);
			ConvergeOperator<SubtractNode>	(nodes);

			return nodes[0];
		}

		private static void ConvergeOperator<T>(IList<Node> nodes) where T : ArithmeticNode {
			for (var i = 0; i < nodes.Count - 1; i++) {
				if (!(nodes[i] is T))
					continue;

				var @operator = (ArithmeticNode)nodes[i];
				if (nodes.Count > i && @operator.Right == Node.ZERO) {
					@operator.Right = nodes[i + 1];
					nodes.RemoveAt(i + 1);
				}
				if (i != 0 && @operator.Left == Node.ZERO) {
					@operator.Left = nodes[i - 1];
					nodes.RemoveAt(i - 1);
					i--;
				}
			}
		}

		private static void ConvergeNegitiveExpressions(IList<Node> nodes) {
			for (var i = 0; i < nodes.Count - 1; i++) {
				if (!(nodes[i] is SubtractNode))
					continue;

				if (i != 0 && nodes[i - 1] is ConstantNode || i >= nodes.Count)
					continue;

				var @operator = (SubtractNode)nodes[i];
				@operator.Right = nodes[i + 1];
				nodes.RemoveAt(i+1);
			}
		}

		private static ArithmeticNode GetArithmaticNode(char op) {
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

		private static int FindEndOfNumber(int startindex, string expression) {
			while (startindex < expression.Length && expression[startindex].IsNumber())
				startindex++;

			return startindex;
		}

		private static bool IsNumber(this char character)
		{
			return character >= '0' && character <= '9';
		}

		private static bool IsOperator(this char character)
		{
			return Operators.Any(c => c == character);
		}
	}
}
