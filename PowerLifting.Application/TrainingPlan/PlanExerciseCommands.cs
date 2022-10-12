using AutoMapper;
using PowerLifting.Application.UserData;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application.Process;
using PowerLifting.Domain.Interfaces.UserData.Application;
using PowerLifting.Domain.Models.TrainingPlan;
using System.Collections.Generic;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanExerciseCommands : IPlanExerciseCommands
    {
        private readonly IPlanExerciseSettingsCommands _planExerciseSettingsCommands;
        private readonly IProcessUserAchivements _userAchivementCommands;
        private readonly IExerciseCommands _exerciseCommands;
        private readonly IPlanCountersSetup _planCountersSetup;

        private readonly ICrudRepo<PlanExerciseDb> _planExerciseRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public PlanExerciseCommands(
         IPlanExerciseSettingsCommands planExerciseSettingsCommands,
         IProcessUserAchivements userAchivementCommands,
         IExerciseCommands exerciseCommands,
         IPlanCountersSetup planCountersSetup,
         ICrudRepo<PlanExerciseDb> plannedExerciseRepository,
         IUserProvider user,
         IMapper mapper)
        {
            _planExerciseSettingsCommands = planExerciseSettingsCommands;
            _userAchivementCommands = userAchivementCommands;
            _exerciseCommands = exerciseCommands;
            _planCountersSetup = planCountersSetup;

            _planExerciseRepository = plannedExerciseRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetAsync(int dayId) => await GetAsync(new List<int>() { dayId });

        /// <inheritdoc />
        public async Task<List<PlanExercise>> GetAsync(List<int> dayIds)
        {
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => dayIds.Contains(t.PlanDayId));
            return await PrepareExerciseDataAsync(planExercisesDb);
        }

        /// <inheritdoc />
        public async Task<PlanExercise> GetByIdAsync(int id)
        {
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => t.Id == id);
            return (await PrepareExerciseDataAsync(planExercisesDb)).FirstOrDefault() ?? new PlanExercise();
        }

        /// <inheritdoc />
        public async Task CreateAsync(int dayId, List<Exercise> exercises)
        {
            if (exercises.Count == 0)
            {
                return;
            }

            //удаляем лишние записи вместе со связями
            var planExercisesDb = await _planExerciseRepository.FindAsync(t => t.PlanDayId == dayId);
            if (planExercisesDb.Count > 0)
            {
                var itemsToDelete = planExercisesDb.Where(t => !exercises.Select(t => t.PlannedExerciseId).Contains(t.Id)).ToList();
                if (itemsToDelete.Count > 0)
                {
                    await _planExerciseSettingsCommands.DeleteByPlanExerciseIdAsync(itemsToDelete.Select(t => t.Id).ToList());
                    _planExerciseRepository.DeleteList(itemsToDelete);

                    itemsToDelete.Select(t => planExercisesDb.Remove(t));
                }
            }

            for (int i = 1; i <= exercises.Count; i++)
            {
                // обновление существующего упражнения
                var planExercise = planExercisesDb.FirstOrDefault(t => t.Id == exercises[i - 1].PlannedExerciseId);
                if (planExercise != null)
                {
                    planExercise.Order = i;
                    _planExerciseRepository.Update(planExercise);
                    continue;
                }

                // добавление нового упражнения
                planExercise = new PlanExerciseDb()
                {
                    PlanDayId = dayId,
                    ExerciseId = exercises[i - 1].Id,
                    Order = i
                };

                await _planExerciseRepository.CreateAsync(planExercise);
            }
        }

        /// <inheritdoc />
        public async Task UpdateAsync(int userId, PlanExercise planExercise)
        {
            var planExerciseDb = (await _planExerciseRepository.FindAsync(t => t.Id == planExercise.Id)).FirstOrDefault();
            if (planExerciseDb == null)
            {
                throw new BusinessException("Не найдено упражнение для обновления");
            }

            if (planExerciseDb.Comments != planExercise.Comments)
            {
                planExerciseDb.Comments = planExercise.Comments;
                _planExerciseRepository.Update(planExerciseDb);
            }

            userId = userId == 0 ? _user.Id : userId;
            var achivement = await _userAchivementCommands.GetByExerciseTypeAsync(userId, planExercise.Exercise.ExerciseTypeId);
            if (achivement == null || achivement.Result == 0)
            {
                throw new BusinessException("Рекорд спортсмена не указан. Нельзя запланировать тренировку.");
            }

            await _planExerciseSettingsCommands.UpdateAsync(planExercise.Id, achivement.Result, planExercise.Settings);
        }

        private async Task<List<PlanExercise>> PrepareExerciseDataAsync(List<PlanExerciseDb> planExercisesDb)
        {
            if (planExercisesDb.Count() == 0)
            {
                return new List<PlanExercise>();
            }

            var exerciseIds = planExercisesDb.Select(t => t.ExerciseId).Distinct().ToList();
            var exercises = await _exerciseCommands.GetAsync(exerciseIds);

            var settings = await _planExerciseSettingsCommands.GetAsync(planExercisesDb.Select(t => t.Id).ToList());

            var planExercises = planExercisesDb.Select(t => _mapper.Map<PlanExercise>(t)).ToList();
            foreach (var item in planExercises)
            {
                item.Exercise = exercises.First(t => t.Id == item.Exercise.Id).Clone();
                item.Exercise.PlannedExerciseId = item.Id;

                item.Settings = settings.Where(t => t.PlanExerciseId == item.Id).OrderBy(t => t.Percentage.MinValue).ToList();

                _planCountersSetup.SetPlanExerciseCounters(item);
            }

            return planExercises;
        }
    }
}
