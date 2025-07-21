using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration;

public class AiConfiguration
{
    public string? ApiKey { get; set; }

    public string? ApiUrl { get; set; }

    public string? Model { get; set; }
}

