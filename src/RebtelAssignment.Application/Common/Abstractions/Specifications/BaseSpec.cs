using Ardalis.Specification;

namespace RebtelAssignment.Application.Common.Abstractions.Specifications;

public class BaseSpec<TEntity, TResult> : Specification<TEntity, TResult>
{
}

public class BaseSpec<TEntity> : Specification<TEntity>
{
}