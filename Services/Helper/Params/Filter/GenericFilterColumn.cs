﻿using System;
using System.Linq;

namespace Services.Helper.Params.Filter
{
    /// <summary>
    /// Filter column by condition on table
    /// </summary>
    /// <typeparam name="T">T is a class</typeparam>
    public class GenericFilterColumn<T> where T : class
    {
        public static IQueryable<T> Query(IQueryable<T> query, Func<T, object> selector, FilterColumn filterColumn, FilterValueType filterValueType)
        {
            filterColumn.ColValue = filterColumn.ColValue ?? string.Empty;

            switch (filterColumn.FilterCondition)
            {
                case FilterCondition.Chua:
                    query = query.Where(x => (selector(x) ?? string.Empty).ToString().Contains(filterColumn.ColValue));
                    break;
                case FilterCondition.KhongChua:
                    query = query.Where(x => !(selector(x) ?? string.Empty).ToString().Contains(filterColumn.ColValue));
                    break;
                case FilterCondition.Bang:
                    switch (filterValueType)
                    {
                        case FilterValueType.Decimal:
                            query = query.Where(x => selector(x) != null && selector(x).Equals(filterColumn.ColValue));
                            break;
                        case FilterValueType.String:
                            query = query.Where(x => (selector(x) ?? string.Empty).Equals(filterColumn.ColValue));
                            break;
                        default:
                            break;
                    }

                    break;
                case FilterCondition.Khac:
                    switch (filterValueType)
                    {
                        case FilterValueType.Decimal:
                            query = query.Where(x => !(selector(x) ?? string.Empty).Equals(filterColumn.ColValue));
                            break;
                        case FilterValueType.String:
                            query = query.Where(x => !(selector(x) ?? string.Empty).Equals(filterColumn.ColValue));
                            break;
                        default:
                            break;
                    }

                    break;
                case FilterCondition.BatDau:
                    query = query.Where(x => (selector(x) ?? string.Empty).ToString().StartsWith(filterColumn.ColValue));
                    break;
                case FilterCondition.KetThuc:
                    query = query.Where(x => (selector(x) ?? string.Empty).ToString().EndsWith(filterColumn.ColValue));
                    break;
                case FilterCondition.NhoHon:
                    switch (filterValueType)
                    {
                        case FilterValueType.Decimal:
                            query = query.Where(x => selector(x) != null && (((decimal)selector(x)) < decimal.Parse(filterColumn.ColValue)));
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterCondition.NhoHonHoacBang:
                    switch (filterValueType)
                    {
                        case FilterValueType.Decimal:
                            query = query.Where(x => selector(x) != null && (((decimal)selector(x)) <= decimal.Parse(filterColumn.ColValue)));
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterCondition.LonHon:
                    switch (filterValueType)
                    {
                        case FilterValueType.Decimal:
                            query = query.Where(x => selector(x) != null && (((decimal)selector(x)) > decimal.Parse(filterColumn.ColValue)));
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterCondition.LonHonHoacBang:
                    switch (filterValueType)
                    {
                        case FilterValueType.Decimal:
                            query = query.Where(x => selector(x) != null && (((decimal)selector(x)) >= decimal.Parse(filterColumn.ColValue)));
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterCondition.Trong:
                    query = query.Where(x => selector(x) == null || string.IsNullOrEmpty(selector(x).ToString()));
                    break;
                case FilterCondition.KhongTrong:
                    query = query.Where(x => selector(x) != null && !string.IsNullOrEmpty(selector(x).ToString()));
                    break;
                default:
                    break;
            }

            return query;
        }
    }

    public enum FilterValueType
    {
        String,
        DateTime,
        Decimal
    }
}