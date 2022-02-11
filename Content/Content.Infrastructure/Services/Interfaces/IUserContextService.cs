namespace Content.Infrastructure.Services.Interfaces
{
    public interface IUserContextService
    {
        public Guid ID { get; }
        public bool IsAuthenticated { get; }
    }
}