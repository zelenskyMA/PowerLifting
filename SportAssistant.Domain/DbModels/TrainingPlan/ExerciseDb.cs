﻿using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.TrainingPlan;

[Table("Exercises", Schema = "plan")]
public class ExerciseDb : NamedEntityDb
{
    public int? UserId { get; set; }

    public bool Closed { get; set; }

    public int ExerciseTypeId { get; set; }

    public int ExerciseSubTypeId { get; set; }
}
