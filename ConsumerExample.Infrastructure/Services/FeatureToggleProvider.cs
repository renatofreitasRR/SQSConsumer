using ConsumerExample.Application.Services;

namespace ConsumerExample.Infrastructure.Services
{
    public class FeatureToggleProvider : IFeatureToggleProvider
    {
        public async Task<bool> IsEnabledAsync(string featureName, CancellationToken ct = default)
        {
            return true;
        }
    }
}
