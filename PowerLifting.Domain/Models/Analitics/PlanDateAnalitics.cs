using PowerLifting.Domain.Models.Common;
using System.Collections.Generic;

namespace PowerLifting.Domain.Models.Analitics
{
    public class PlanDateAnalitics
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public int LiftCounterSum { get; set; }

        public int WeightLoadSum { get; set; }

        public int IntensitySum { get; set; }

        //Значения из справочника подтипов упражнений. Изменять при изменении справочника в БД
        // DictionaryTypes = 2

        /// <summary> Рывок классический, 50 </summary>
        public int ClassicJerk { get; set; }

        /// <summary> Толчок. Взятие на грудь, 51 </summary>
        public int PushOnChest { get; set; }

        /// <summary> Толчок с груди, 52 </summary>
        public int PushFromChest { get; set; }

        /// <summary> Толчок классический, 53 </summary>
        public int ClassicPush { get; set; }

        /// <summary> ОФП, 54 </summary>
        public int Ofp { get; set; }
    }
}