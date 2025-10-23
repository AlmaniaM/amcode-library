using System;
using System.Collections.Generic;
using AMCode.Sql.Where.Exceptions;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// A factory for constructing objects that implement the <see cref="IWhereClause"/> interface.
    /// </summary>
    public class WhereClauseTypeFactory : IWhereClauseTypeFactory
    {
        private readonly IDictionary<WhereClauseBuilderType, IWhereClauseFactory> whereClauseFactories;

        /// <summary>
        /// Creates an instance of the <see cref="WhereClauseTypeFactory"/> class.
        /// </summary>
        /// <param name="builderFactory">A <see cref="IWhereClauseBuilderFactory"/> to pass along to the <see cref="IWhereClause"/>.</param>
        /// <param name="filterConditionBuilderFactory">A <see cref="IFilterConditionOrganizerFactory"/> to pass along to the <see cref="IWhereClause"/>.</param>
        public WhereClauseTypeFactory(IWhereClauseBuilderFactory builderFactory, IFilterConditionOrganizerFactory filterConditionBuilderFactory)
        {
            whereClauseFactories = new Dictionary<WhereClauseBuilderType, IWhereClauseFactory>
            {
                [WhereClauseBuilderType.Data] = new WhereClauseFactory(() => new WhereClause(builderFactory, filterConditionBuilderFactory, WhereClauseBuilderType.Data)),
                [WhereClauseBuilderType.GlobalFilters] = new WhereClauseFactory(() => new WhereClause(builderFactory, filterConditionBuilderFactory, WhereClauseBuilderType.GlobalFilters))
            };
        }

        /// <summary>
        /// Creates an instance of an <see cref="IWhereClause"/>.
        /// </summary>
        /// <param name="whereClauseBuilderType">Provide a <see cref="WhereClauseBuilderType"/> of the type of
        /// <see cref="IWhereClause"/> you want to construct.</param>
        /// <returns>An object instance that implements <see cref="IWhereClause"/>.</returns>
        public IWhereClause Create(WhereClauseBuilderType whereClauseBuilderType)
        {
            if (whereClauseFactories.TryGetValue(whereClauseBuilderType, out var createWhereClause))
            {
                return createWhereClause.Create();
            }
            else
            {
                throw new NoSuchWhereClauseException(
                    $"[{nameof(WhereClauseBuilderFactory)}][{nameof(Create)}]({nameof(whereClauseBuilderType)})",
                    $"{whereClauseBuilderType}"
                );
            }
        }

        /// <summary>
        /// An internal factory class designed to create a <see cref="IWhereClause"/> object.
        /// </summary>
        public class WhereClauseFactory : IWhereClauseFactory
        {
            private readonly Func<IWhereClause> createWhereClause;

            /// <summary>
            /// Create an instance of the <see cref="WhereClauseFactory"/> class. This class is mainly used for the ability
            /// to lazily create an <see cref="IWhereClause"/>.
            /// </summary>
            /// <param name="createWhereClause">A <see cref="Func{TResult}"/> function that returns a <see cref="IWhereClause"/> object.</param>
            public WhereClauseFactory(Func<IWhereClause> createWhereClause)
            {
                this.createWhereClause = createWhereClause;
            }

            /// <summary>
            /// Create an <see cref="IWhereClause"/> object.
            /// </summary>
            /// <returns>A <see cref="IWhereClause"/> object.</returns>
            public IWhereClause Create() => createWhereClause();
        }
    }
}