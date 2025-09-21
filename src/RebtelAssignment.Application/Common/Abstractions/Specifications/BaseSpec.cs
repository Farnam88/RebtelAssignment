using Ardalis.Specification;

namespace RebtelAssignment.Application.Common.Abstractions.Specifications;

public class BaseSpec<T, TResult> : Specification<T, TResult>
{
}

public class BaseSpec<T> : Specification<T>
{
}