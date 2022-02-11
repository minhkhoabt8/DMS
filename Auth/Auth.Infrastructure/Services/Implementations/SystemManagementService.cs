using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure.Services.Implementations;

public class SystemManagementService : ISystemManagementService
{
    private readonly HttpClient _client;

    public SystemManagementService(IConfiguration configuration, HttpClient client)
    {
        client.BaseAddress = new Uri(configuration["BaseAddresses:SystemManagement"]);
        _client = client;
    }

    public async Task<SMAuthDTO> LoginAsync(LoginInputDTO inputDTO)
    {
        var content = new StringContent(JsonSerializer.Serialize(inputDTO), Encoding.UTF8, "application/json");
        var result = await _client.PostAsync("api/users/login", content);

        if (!result.IsSuccessStatusCode)
        {
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new WrongCredentialsException();
            }

            // Unexpected error
            throw new Exception();
        }

        return JsonSerializer.Deserialize<SMAuthDTO>(await result.Content.ReadAsStringAsync(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException();
    }
}