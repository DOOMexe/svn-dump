using System;
using GodLesZ.Library.Amf.Util;

namespace GodLesZ.Library.Amf.Expression {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class OpMODULUS : BinaryOperator {
		public OpMODULUS() {
		}

		protected override object Evaluate(object context, EvaluationContext evalContext) {
			object left = Left.EvaluateInternal(context, evalContext);
			object right = Right.EvaluateInternal(context, evalContext);

			if (NumberUtils.IsNumber(left) && NumberUtils.IsNumber(right)) {
				return NumberUtils.Modulus(left, right);
			} else {
				throw new ArgumentException("Cannot calculate modulus for instances of '"
					+ left.GetType().FullName
					+ "' and '"
					+ right.GetType().FullName
					+ "'.");
			}
		}

	}
}
