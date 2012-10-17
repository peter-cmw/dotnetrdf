/*

Copyright dotNetRDF Project 2009-12
dotnetrdf-develop@lists.sf.net

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF.Query.Expressions;
using VDS.RDF.Nodes;
using VDS.RDF.Query.Expressions.Primary;

namespace VDS.RDF.Query.Aggregates.Sparql
{
    /// <summary>
    /// Class representing MAX Aggregate Functions
    /// </summary>
    public class MaxAggregate
        : BaseAggregate
    {
        private String _varname;

        /// <summary>
        /// Creates a new MAX Aggregate
        /// </summary>
        /// <param name="expr">Variable Expression</param>
        /// <param name="distinct">Whether a DISTINCT modifier applies</param>
        public MaxAggregate(VariableTerm expr, bool distinct)
            : base(expr, distinct)
        {
            this._varname = expr.ToString().Substring(1);
        }

        /// <summary>
        /// Creates a new MAX Aggregate
        /// </summary>
        /// <param name="expr">Expression</param>
        /// <param name="distinct">Whether a DISTINCT modifier applies</param>
        public MaxAggregate(ISparqlExpression expr, bool distinct)
            : base(expr, distinct) { }

        /// <summary>
        /// Creates a new MAX Aggregate
        /// </summary>
        /// <param name="expr">Variable Expression</param>
        public MaxAggregate(VariableTerm expr)
            : this(expr, false) { }

        /// <summary>
        /// Creates a new MAX Aggregate
        /// </summary>
        /// <param name="expr">Expression</param>
        public MaxAggregate(ISparqlExpression expr)
            : this(expr, false) { }

        /// <summary>
        /// Creates a new MAX Aggregate
        /// </summary>
        /// <param name="distinct">Distinct Modifier</param>
        /// <param name="expr">Expression</param>
        public MaxAggregate(ISparqlExpression distinct, ISparqlExpression expr)
            : base(expr)
        {
            if (distinct is DistinctModifier)
            {
                this._distinct = true;
            }
            else
            {
                throw new RdfQueryException("The first argument to the MaxAggregate constructor must be of type DistinctModifierExpression");
            }
        }

        /// <summary>
        /// Applies the Max Aggregate function to the results
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingIDs">Binding IDs over which the Aggregate applies</param>
        /// <returns></returns>
        public override IValuedNode Apply(SparqlEvaluationContext context, IEnumerable<int> bindingIDs)
        {
            List<IValuedNode> values = new List<IValuedNode>();

            if (this._varname != null)
            {
                //Ensured the MAXed variable is in the Variables of the Results
                if (!context.Binder.Variables.Contains(this._varname))
                {
                    throw new RdfQueryException("Cannot use the Variable " + this._expr.ToString() + " in a MAX Aggregate since the Variable does not occur in a Graph Pattern");
                }
            }

            foreach (int id in bindingIDs)
            {
                try
                {
                    values.Add(this._expr.Evaluate(context, id));
                }
                catch
                {
                    //Ignore errors
                }
            }

            values.Sort(new SparqlOrderingComparer());
            values.Reverse();
            return values.FirstOrDefault();
        }

        /// <summary>
        /// Gets the String representation of the Aggregate
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("MAX(");
            if (this._distinct) output.Append("DISTINCT ");
            output.Append(this._expr.ToString() + ")");
            return output.ToString();
        }

        /// <summary>
        /// Gets the Functor of the Aggregate
        /// </summary>
        public override string Functor
        {
            get
            {
                return SparqlSpecsHelper.SparqlKeywordMax;
            }
        }
    }
}