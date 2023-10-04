using Microsoft.Extensions.Options;

namespace HyperSoa.Service.Host.Configuration
{
    public class HyperNodeHostConfigurationValidator : IValidateOptions<HyperNodeHostConfiguration>
    {
        public ValidateOptionsResult Validate(string? name, HyperNodeHostConfiguration options)
        {
            var errors = new List<string>();

            if (!options.HttpEndpoints.Any())
                errors.Add("At least one HTTP endpoint must be specified.");
            else
            {
                foreach (var httpEndpoint in options.HttpEndpoints)
                {
                    if (string.IsNullOrWhiteSpace(httpEndpoint.Name))
                        errors.Add($"The {nameof(HyperNodeHttpEndpoint)}.{nameof(HyperNodeHttpEndpoint.Name)} property is required and must not be blank.");
                    if (string.IsNullOrWhiteSpace(httpEndpoint.Uri))
                        errors.Add($"The {nameof(HyperNodeHttpEndpoint)}.{nameof(HyperNodeHttpEndpoint.Uri)} property is required and must be a valid HTTP or HTTPS URI.");
                }
            }

            return errors.Count > 0
                ? ValidateOptionsResult.Fail(errors)
                : ValidateOptionsResult.Success;
        }
    }
}
