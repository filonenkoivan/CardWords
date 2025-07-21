using Api.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ICommunityService
{
    Task<List<CollectionModel>> GetCollections(string name, CancellationToken ct);
    Task<bool> SaveCollection(int id, CancellationToken ct);
}

