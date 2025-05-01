using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public record QuizCardModel(string? Word, List<string>? Answers, string? CorrectAnswer);
}
