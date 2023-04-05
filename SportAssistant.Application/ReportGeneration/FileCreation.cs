using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SportAssistant.Domain.Interfaces.ReportGeneration;
using SportAssistant.Domain.Models.ReportGeneration;

namespace SportAssistant.Application.ReportGeneration;

public class FileCreation : IFileCreation
{
    private readonly int prefixCellCount = 2; // колонки для: 1) Ид 2) Название
    private readonly int liftCellCount = 4; // кол-во колонок в ячейке поднятий

    private int MaxCells => (liftCellCount * 10) + prefixCellCount;

    /// <inheritdoc />
    public byte[] Generate(ReportData report)
    {
        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet(report.PlanStartDate.ToString("dd.MM.yyyy"));

        SetSheetData(sheet, report);

        MemoryStream? ms;
        using (ms = new MemoryStream())
        {
            workbook.Write(ms, false);
        }

        return ms.ToArray();
    }

    private void SetSheetData(ISheet sheet, ReportData report)
    {
        var rowNum = 0;

        SetGlobalStyles(sheet);

        //дни
        foreach (var day in report.Days)
        {
            AddHeader(sheet, rowNum, day.DayDate);

            // упражнения в дне
            for (int i = 1; i <= day.Exercises.Count; i++)
            {
                IRow topRow = sheet.CreateRow(IncRow(ref rowNum));
                IRow bottomRow = sheet.CreateRow(IncRow(ref rowNum));

                var exercise = day.Exercises[i - 1];

                AddId(sheet, topRow, bottomRow, i);
                AddName(sheet, topRow, bottomRow, exercise.Name, i);

                if (string.IsNullOrWhiteSpace(exercise.ExtPlanData))
                {
                    CreateCommonExerciseGrid(sheet, topRow, bottomRow, i, exercise);
                }
                else
                {
                    CreateOfpExerciseGrid(sheet, topRow, bottomRow, i, exercise);
                }
            }

            IncRow(ref rowNum);
        }
    }


    private void AddHeader(ISheet sheet, int rowNum, string header)
    {
        var font = sheet.Workbook.CreateFont();
        font.IsBold = true;

        var style = sheet.Workbook.CreateCellStyle();
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.SetFont(font);

        sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 0, MaxCells - 1));
        for (int i = 0; i < MaxCells; i++)
        {
            sheet.CreateRow(rowNum).CreateCell(i);
        }

        var headerCell = sheet.CreateRow(rowNum).CreateCell(0);
        headerCell.SetCellValue(header);
        headerCell.CellStyle = style;

        sheet.GetRow(rowNum).Height = 2 * 256;
    }

    private void AddId(ISheet sheet, IRow topRow, IRow bottomRow, int exerciseNumber)
    {
        int cellId = 0;
        var style = sheet.Workbook.CreateCellStyle();
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.FillPattern = FillPattern.SolidForeground;
        style.FillForegroundColor = RowColorIndex(exerciseNumber);

        sheet.AddMergedRegion(new CellRangeAddress(topRow.RowNum, bottomRow.RowNum, cellId, cellId));
        topRow.CreateCell(cellId).SetCellValue(exerciseNumber);
        bottomRow.CreateCell(cellId);

        topRow.GetCell(cellId).CellStyle = style;
        bottomRow.GetCell(cellId).CellStyle = style;
    }

    private void AddName(ISheet sheet, IRow topRow, IRow bottomRow, string name, int exerciseNumber)
    {
        int cellId = 1;
        var style = sheet.Workbook.CreateCellStyle();
        style.WrapText = true;
        style.Alignment = HorizontalAlignment.Left;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Medium;
        style.BorderTop = BorderStyle.Thin;
        style.FillPattern = FillPattern.SolidForeground;
        style.FillForegroundColor = RowColorIndex(exerciseNumber);

        sheet.AddMergedRegion(new CellRangeAddress(topRow.RowNum, bottomRow.RowNum, cellId, cellId));
        topRow.CreateCell(cellId).SetCellValue(name);
        bottomRow.CreateCell(cellId);

        topRow.GetCell(cellId).CellStyle = style;
        bottomRow.GetCell(cellId).CellStyle = style;
    }

    private void CreateCommonExerciseGrid(ISheet sheet, IRow topRow, IRow bottomRow, int exerciseNumber, ReportExercise exercise)
    {
        // создание шаблона на всю длину строк
        var cellId = prefixCellCount;
        for (int i = 1; i <= ((MaxCells - prefixCellCount) / liftCellCount); i++)
        {
            var rowsColumns = Enumerable.Range(0, liftCellCount).Select(t => topRow.CreateCell(cellId + t)).ToList(); // 4 колонки в строку 1
            rowsColumns.AddRange(Enumerable.Range(0, liftCellCount).Select(t => bottomRow.CreateCell(cellId + t))); // 4 колонки в строку 2
            sheet.AddMergedRegion(new CellRangeAddress(topRow.RowNum, topRow.RowNum, cellId, cellId + 2)); // вес - weight
            sheet.AddMergedRegion(new CellRangeAddress(topRow.RowNum, bottomRow.RowNum, cellId + 3, cellId + 3)); // подходы - iterations

            foreach (var col in rowsColumns)
            {
                var style = sheet.Workbook.CreateCellStyle();
                style.Alignment = col.ColumnIndex == cellId + 3 ? HorizontalAlignment.Left : HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                style.FillPattern = FillPattern.SolidForeground;
                style.FillForegroundColor = RowColorIndex(exerciseNumber);

                col.CellStyle = style;
            }

            cellId = (i * liftCellCount) + prefixCellCount; // следующее поднятие
        }

        // заполнение шаблона данными из упражнения
        for (int j = 0; j < exercise.ExerciseSettings.Count; j++)
        {
            var exSettings = exercise.ExerciseSettings[j];
            cellId = (j * liftCellCount) + prefixCellCount;

            if (exSettings.Weight > 0) { topRow.GetCell(cellId).SetCellValue(exSettings.Weight); }
            if (exSettings.Iterations > 0) { topRow.GetCell(cellId + 3).SetCellValue(exSettings.Iterations); }

            if (exSettings.ExercisePart1 > 0) { bottomRow.GetCell(cellId).SetCellValue(exSettings.ExercisePart1); }
            if (exSettings.ExercisePart2 > 0) { bottomRow.GetCell(cellId + 1).SetCellValue(exSettings.ExercisePart2); }
            if (exSettings.ExercisePart3 > 0) { bottomRow.GetCell(cellId + 2).SetCellValue(exSettings.ExercisePart3); }
        }
    }

    private void CreateOfpExerciseGrid(ISheet sheet, IRow topRow, IRow bottomRow, int exerciseNumber, ReportExercise exercise)
    {
        // создание шаблона на всю длину строк
        var firstCellId = prefixCellCount;
        var lastCellId = MaxCells - prefixCellCount;

        var rowsColumns = Enumerable.Range(0, lastCellId).Select(t => topRow.CreateCell(firstCellId + t)).ToList();
        rowsColumns.AddRange(Enumerable.Range(0, lastCellId).Select(t => bottomRow.CreateCell(firstCellId + t)));
        sheet.AddMergedRegion(new CellRangeAddress(topRow.RowNum, bottomRow.RowNum, firstCellId, firstCellId + lastCellId - 1));

        foreach (var col in rowsColumns)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = RowColorIndex(exerciseNumber);
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.WrapText = true;

            col.CellStyle = style;
        }

        // заполнение шаблона данными из упражнения
        topRow.GetCell(firstCellId).SetCellValue(exercise.ExtPlanData);
    }

    private void SetGlobalStyles(ISheet sheet)
    {
        sheet.SetColumnWidth(0, (int)(2.8 * 256)); // ид
        sheet.SetColumnWidth(1, 31 * 256); // название

        var columnId = prefixCellCount;
        for (int i = 1; i <= ((MaxCells - prefixCellCount) / liftCellCount); i++) // поднятия
        {
            sheet.SetColumnWidth(columnId, 2 * 256);
            sheet.SetColumnWidth(columnId + 1, 2 * 256);
            sheet.SetColumnWidth(columnId + 2, 2 * 256);
            sheet.SetColumnWidth(columnId + 3, 4 * 256);

            columnId = (i * liftCellCount) + prefixCellCount; // следующее поднятие
        }
    }

    private short RowColorIndex(int rowId) => rowId % 2 == 0 ? IndexedColors.White.Index : IndexedColors.Grey25Percent.Index;

    private int IncRow(ref int baseValue)
    {
        baseValue++;
        return baseValue;
    }
}
