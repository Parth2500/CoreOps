using CoreOps.Domain.Common;

namespace CoreOps.Domain.Operations;

public class RequestStateHistory : Entity
{
    public Guid RequestId { get; private set; }
    public string? FromStateCode { get; private set; }
    public string ToStateCode { get; private set; } = default!;
    public Guid TransitionedBy { get; private set; }
    public DateTime TransitionedAt { get; private set; }
    public string? Comment { get; private set; }

    private RequestStateHistory() { }

    public RequestStateHistory(
        Guid tenantId,
        Guid requestId,
        string? fromStateCode,
        string toStateCode,
        Guid transitionedBy,
        string? comment)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        RequestId = requestId;
        FromStateCode = fromStateCode;
        ToStateCode = toStateCode;
        TransitionedBy = transitionedBy;
        TransitionedAt = DateTime.UtcNow;
        Comment = comment;
    }
}
