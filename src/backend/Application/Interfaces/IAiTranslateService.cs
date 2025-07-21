using Application.Models.Responsel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAiTranslateService
{
    public Task<TranslationDTO> GetTranslationArray(string word);
}

