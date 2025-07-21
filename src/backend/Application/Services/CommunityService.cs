using Api.Models.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class CommunityService : ICommunityService
{
    private readonly ICommunityRepository _repository;
    private readonly IdentificationService _identificationService;
    public CommunityService(ICommunityRepository repository, IdentificationService identificationService)
    {
        _repository = repository;
        _identificationService = identificationService;
    }
    public async Task<List<CollectionModel>> GetCollections(string name, CancellationToken ct)
    {
        var user = await _identificationService.GetUser(ct);
        return await _repository.GetCollectionsAsync(name, user.Id, ct);
    }

    public async Task<bool> SaveCollection(int id, CancellationToken ct)
    {
        var user = await _identificationService.GetUser(ct);
        return await _repository.SaveCollection(id, user.Id, ct);
    }
}

