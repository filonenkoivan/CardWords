using Api.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository;

public interface ICommunityRepository
{
    Task<List<CollectionModel>> GetCollectionsAsync(string name, int userId, CancellationToken ct);
    Task<bool> SaveCollection(int id, int userId, CancellationToken ct);
}

