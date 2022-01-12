using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Api;

public class ApiResponse<T>
{
    public bool Success { get; set; }
#nullable enable
    public T? Data { get; set; }
#nullable enable
    public DomainError[]? Errors { get; set; }
}
