namespace ConsumerExample.Application.Services
{
    public interface IFeatureToggleProvider
    {
        Task<bool> IsEnabledAsync(string featureName, CancellationToken ct = default);
    }
}
