﻿// Copyright (C) Arctium Software.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using System.Reflection;
using LappaORM.Misc;
using static LappaORM.Misc.Helper;

namespace LappaORM
{
    internal partial class QueryBuilder<T>
    {
        internal string BuildDelete(T entity, PropertyInfo[] primaryKeys)
        {
            sqlQuery.AppendFormat(numberFormat, querySettings.DeleteQuery, Pluralize<T>(), typeof(T).Name[0]);
            sqlQuery.AppendFormat(numberFormat, querySettings.Equal, primaryKeys[0].Name, primaryKeys[0].GetGetter<T>().GetValue(entity));

            for (var i = 1; i < primaryKeys.Length; i++)
                sqlQuery.AppendFormat(numberFormat, querySettings.AndEqual, primaryKeys[i].Name, primaryKeys[i].GetGetter<T>().GetValue(entity));

            return sqlQuery.ToString();
        }

        internal string BuildDelete(Expression expression)
        {
            sqlQuery.AppendFormat(numberFormat, querySettings.DeleteQuery, Pluralize<T>());

            Visit(expression);

            return sqlQuery.ToString();
        }
    }
}
