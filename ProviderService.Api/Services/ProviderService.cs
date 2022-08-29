using ProviderService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderService.Api.Services
{
    public interface IProviderService
    {
        Task<List<Provider>> GetProviders();
        Task<Provider> GetProviderById(int id);
        Task<bool> AddProvider(Provider provider);
        Task<bool> UpdateProvider(Provider provider);
        Task<bool> Remove(Provider provider);
    }

    public class ProviderService : IProviderService
    {
        private readonly Random _random;
        private static readonly List<Provider> _providers = new()
        {
            new Provider
            {
                Id = 1,
                IsActive = true,
                CompanyName = "Advantasure 1",
                AlternateIdentifier = Guid.NewGuid()
            },
            new Provider
            {
                Id = 2,
                IsActive = true,
                CompanyName = "Advantasure 2",
                AlternateIdentifier = Guid.NewGuid()
            },
            new Provider
            {
                Id = 3,
                IsActive = true,
                CompanyName = "Advantasure 3",
                AlternateIdentifier = Guid.NewGuid()
            }
        };

        public ProviderService()
        {
            _random = new Random();
        }

        public async Task<List<Provider>> GetProviders()
        {
            return await Task.Run(() =>
            {
                return _providers;
            });
        }

        public async Task<Provider> GetProviderById(int id)
        {
            return await Task.Run(() =>
            {
                if (id <= 0)
                    return null;

                var provider = _providers.FirstOrDefault(x => x.Id == id);

                return provider;
            });
        }

        public async Task<bool> AddProvider(Provider provider)
        {
            return await Task.Run(() =>
            {
                if (provider == null)
                    return false;

                _providers.Add(provider);

                return true;
            });
        }

        public async Task<bool> UpdateProvider(Provider provider)
        {
            return await Task.Run(() =>
            {
                if (provider == null)
                    return false;

                if (provider.Id <= 0)
                    provider.Id = _random.Next(4, 100);

                if (string.IsNullOrEmpty(provider.AlternateIdentifier.ToString()))
                    provider.AlternateIdentifier = Guid.NewGuid();

                var providerToUpdate = _providers.FirstOrDefault(x => x.Id == provider.Id);

                providerToUpdate.IsActive = provider.IsActive;
                providerToUpdate.CompanyName = provider.CompanyName;
                providerToUpdate.AlternateIdentifier = provider.AlternateIdentifier;

                return true;
            });
        }

        public async Task<bool> Remove(Provider provider)
        {
            return await Task.Run(() =>
            {
                if (provider == null)
                    return false;

                if (provider.Id <= 0)
                    return false;

                var providerToRemove = _providers.FirstOrDefault(x => x.Id == provider.Id);

                if (providerToRemove == null)
                    return false;

                return _providers.Remove(providerToRemove);
            });
        }
    }
}
