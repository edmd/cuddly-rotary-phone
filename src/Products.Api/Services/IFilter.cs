﻿namespace Products.Api.Services
{
    public interface IFilter
    {
        IFilter AdditionalFilter { get; set; }
        string AttributePath { get; }
        string ComparisonValue { get; }
        string ComparisonValueEncoded { get; }
        AttributeDataType? DataType { get; set; }
        ComparisonOperator FilterOperator { get; }

        string Serialize();
    }
}