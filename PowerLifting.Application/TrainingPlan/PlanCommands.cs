using AutoMapper;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application;
using PowerLifting.Domain.Interfaces.TrainingPlan.Application.Process;
using PowerLifting.Domain.Models.TrainingPlan;

namespace PowerLifting.Application.TrainingPlan
{
    public class PlanCommands : IPlanCommands
    {
        private readonly IPlanExerciseCommands _plannedExerciseCommands;
        private readonly IPlanCountersSetup _planCountersSetup;

        private readonly ICrudRepo<PlanDb> _trainingPlanRepository;
        private readonly ICrudRepo<PlanDayDb> _trainingDayRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public PlanCommands(
            IPlanExerciseCommands plannedExerciseCommands,
            IPlanCountersSetup planCountersSetup,
            ICrudRepo<PlanDb> trainingPlanRepository,
            ICrudRepo<PlanDayDb> trainingDayRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _plannedExerciseCommands = plannedExerciseCommands;
            _planCountersSetup = planCountersSetup;
            _trainingPlanRepository = trainingPlanRepository;
            _trainingDayRepository = trainingDayRepository;
            _user = user;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Plans> GetPlansAsync(int userId)
        {
            userId = userId == 0 ? _user.Id : userId;

            var plansDb = await _trainingPlanRepository.FindAsync(t => t.UserId == userId);
            var plansList = plansDb.Select(t => _mapper.Map<Plan>(t)).ToList();

            var plans = new Plans()
            {
                ActivePlans = plansList.Where(t => t.StartDate.AddDays(7) >= DateTime.Now.Date)
                    .OrderByDescending(t => t.StartDate).ToList(),

                ExpiredPlans = plansList.Where(t => t.StartDate.AddDays(7) < DateTime.Now.Date)
                    .OrderByDescending(t => t.StartDate).ToList(),
            };

            return plans;
        }

        /// <inheritdoc />
        public async Task<Plan> GetPlanAsync(int Id)
        {
            var dbPlan = (await _trainingPlanRepository.FindAsync(t => t.Id == Id)).FirstOrDefault();
            if (dbPlan == null)
            {
                return null;
            }

            var plan = _mapper.Map<Plan>(dbPlan);

            var planDays = (await _trainingDayRepository.FindAsync(t => t.PlanId == dbPlan.Id)).Select(t => _mapper.Map<PlanDay>(t)).ToList();
            var planExercises = await _plannedExerciseCommands.GetAsync(planDays.Select(t => t.Id).ToList());
            foreach (var planDay in planDays)
            {
                planDay.Exercises = planExercises.Where(t => t.PlanDayId == planDay.Id).OrderBy(t => t.Order).ToList();
                _planCountersSetup.SetPlanDayCounters(planDay);
            }

            plan.TrainingDays = planDays;
            _planCountersSetup.SetPlanCounters(plan);

            return plan;
        }

        /// <inheritdoc />
        public async Task<PlanDay> GetPlanDayAsync(int dayId)
        {
            var planDayDb = (await _trainingDayRepository.FindAsync(t => t.Id == dayId)).FirstOrDefault();
            if (planDayDb == null)
            {
                return null;
            }

            var planExercises = await _plannedExerciseCommands.GetAsync(dayId);
            var percentages = planExercises.Where(t => t.Settings != null)
                .SelectMany(t => t.Settings.Select(z => z.Percentage))
                .DistinctBy(t => t.Id);

            var planDay = _mapper.Map<PlanDay>(planDayDb);
            planDay.Exercises = planExercises.Where(t => t.PlanDayId == dayId).OrderBy(t => t.Order).ToList();
            planDay.Percentages = planExercises.Where(t => t.Settings != null)
                .SelectMany(t => t.Settings.Select(z => z.Percentage))
                .DistinctBy(t => t.Id).OrderBy(t => t.MinValue).ToList();

            _planCountersSetup.SetPlanDayCounters(planDay);

            return planDay;
        }

        /// <inheritdoc />
        public async Task<PlanDay> GetCurrentDayAsync()
        {
            var now = DateTime.Now.Date;
            var emptyDay = new PlanDay();

            var dbPlans = await _trainingPlanRepository.FindAsync(t =>
                t.UserId == _user.Id &&
                t.StartDate <= now && t.StartDate >= now.AddDays(-6));
            if (!dbPlans.Any())
            {
                return emptyDay;
            }

            var planId = dbPlans.First().Id;
            var planDayDb = (await _trainingDayRepository.FindAsync(t => t.PlanId == planId && t.ActivityDate.Date == now)).FirstOrDefault();
            if (planDayDb == null)
            {
                return emptyDay;
            }

            return await GetPlanDayAsync(planDayDb.Id);
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(RequestPlanCreation request)
        {
            var userId = _user.Id;
            if (request.UserId != 0)
            {
                // проверяем что _user.Id - тренер указанного UserId
                userId = request.UserId;
            }

            var prevPlanDate = request.CreationDate.AddDays(-6);
            var nextPlanDate = request.CreationDate.AddDays(6);
            var preventingPlans = await _trainingPlanRepository.FindAsync(t =>
                t.UserId == userId &&
                t.StartDate >= prevPlanDate &&
                t.StartDate <= nextPlanDate);

            if (preventingPlans.Any())
            {
                string errorDates = string.Join(", ", preventingPlans.Select(t => t.StartDate.ToString("dd/MM/yyyy")));
                throw new BusinessException($"Найдены пересекающийся по датам планы. Даты начала: {errorDates}");
            }

            var plan = new PlanDb() { StartDate = request.CreationDate, UserId = userId };
            await _trainingPlanRepository.CreateAsync(plan);

            for (int i = 0; i < 7; i++) // 7 days standard plan
            {
                var trainingDay = new PlanDayDb() { PlanId = plan.Id, ActivityDate = request.CreationDate.AddDays(i) };
                await _trainingDayRepository.CreateAsync(trainingDay);
            }

            return plan.Id;
        }
    }
}
