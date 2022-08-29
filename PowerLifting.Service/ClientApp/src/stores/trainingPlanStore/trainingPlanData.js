export class TrainingPlanModel {
  constructor() {
    this.id = 0;
    this.userId = 0;
    this.startDate = null;
    this.comments = '';
    this.TrainingDays = [];
  }
}

export class TrainingDayModel {
  constructor() {
    this.id = 0;
    this.activityDate = null;
    this.liftCounterSum = 0;
    this.weightLoadSum = 0;
    this.intensitySum = 0;
    this.exercises = [];
  }
}

export class ExerciseModel {
  constructor() {
    this.id = 0;
    this.exerciseTypeId = 0;
    this.name = '';
    this.order = 0;
    this.liftCounter = 0;
    this.weightLoad = 0;
    this.intensity = 0;
    this.exerciseData = []; /* ExercisePercentage */
  }
}

export class ExercisePercentageModel {
  constructor() {
    this.id = 0;
    this.percentage = null;
    this.values = null;
  }
}

export class PercentageModel {
  constructor() {
    this.id = 0;
    this.name = '';
    this.description = '';
    this.minValue = 0;
    this.maxValue = 0;
  }
}

export class ExerciseValueModel {
  constructor() {
    this.id = 0;
    this.weight = 0;
    this.iterations = 0;
    this.exercisePart1 = 0;
    this.exercisePart2 = 0;
    this.exercisePart3 = 0;
    this.comments = '';
  }
}
