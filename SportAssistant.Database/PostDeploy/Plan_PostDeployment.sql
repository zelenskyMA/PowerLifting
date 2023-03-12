

SET IDENTITY_INSERT [plan].Percentages ON

  MERGE Percentages AS Target
  USING (VALUES 
    ( 1, ' < 30%', 'Меньше 30%', 0, 29),
    ( 2, '30 - 40%', '', 30, 39),
    ( 3, '40 - 50%', '', 40, 49),
    ( 4, '50 - 60%', '', 50, 59),
    ( 5, '60 - 70%', '', 60, 69),
    ( 6, '70 - 80%', '', 70, 79),
    ( 7, '80 - 90%', '', 80, 89),
    ( 8, '90 - 100%', '', 90, 99),
    ( 9, '100 - 110%', '', 100, 109),
    ( 10, '110 - 120%', '', 110, 119),
    ( 11, '120 - 130%', '', 120, 129),
    ( 12, '130 - 140%', '', 130, 139),
    ( 13, '140 - 150%', '', 140, 149),
    ( 14, '> 150%', 'Больше 150%', 150, 999)
  )	AS Source (Id, Name, Description, MinValue, MaxValue)
    ON Source.[Id] = Target.[Id]
  WHEN NOT MATCHED BY Target THEN INSERT (Id, Name, Description, MinValue, MaxValue) VALUES (Source.Id, Source.Name, Source.Description, Source.MinValue, Source.MaxValue)
  WHEN MATCHED THEN UPDATE SET 
    Target.Name = Source.Name,
    Target.Description = Source.Description,
    Target.MinValue = Source.MinValue,
    Target.MaxValue = Source.MaxValue;

SET IDENTITY_INSERT [plan].Percentages OFF 


SET IDENTITY_INSERT [plan].Exercises ON

  MERGE Exercises AS Target
  USING (VALUES 
-- Рывковые
  -- -- Рывок классический
    ( 1, 2, 50, 'Рывок: Рывок классический', ''),
    ( 2, 2, 50, 'Рывок без разброса ног', ''),
    ( 12, 2, 50, 'Рывок в стойку', ''),
    ( 13, 2, 50, 'Рывок в полуподсед', ''),
    ( 1015, 2, 50, 'Рывок с медленной тягой', ''),
    ( 1016, 2, 50, 'Рывок без захвата "замком"', ''),
  -- -- Рывок подсобные
    ( 3, 2, 54, 'Рывок с подставки', ''),
    ( 4, 2, 54, 'Рывок с плинтов ниже колен', ''),
    ( 5, 2, 54, 'Рывок с плинтов выше колен', ''),
    ( 6, 2, 54, 'Рывок с трех положений', ''),
    ( 1014, 2, 54, 'Рывок в три положения', ''),
    ( 7, 2, 54, 'Рывок с виса ниже колен', ''),
    ( 8, 2, 54, 'Рывок с виса выше колен', ''),
    ( 10, 2, 54, 'Рывок от паха', ''),
    ( 9, 2, 54, 'Рывок с паузой 3 секунды ниже колен', ''),
    ( 11, 2, 54, 'Рывок с паузой 3 секунды выше колен', ''),
    ( 1017, 2, 54, 'Рывок выше колен + рывок ниже колен', ''),
    ( 1018, 2, 54, 'Рывок ниже колен + рывок выше колен', ''),
    ( 1019, 2, 54, 'Рывок без разброса ног в полу подсед с виса выше колен', ''),
    ( 1100, 2, 54, 'Рывок без разброса ног + рывок классический', ''),
    ( 1101, 2, 54, 'Рывок в полуподсед + рывок классический', ''),
    ( 1102, 2, 54, 'Тяга рывковая до подрыва + рывок классический', ''),
    ( 1103, 2, 54, 'Тяга рывковая до колен + рывок классический', ''),
    ( 1104, 2, 54, 'Тяга рывковая+рывок с помоста', ''),
    ( 1105, 2, 54, 'Тяга рывковая+рывок с виса выше колен', ''),
    ( 1106, 2, 54, 'Тяга рывковая+рывок с виса ниже колен', ''),
  -- -- Рывоковые ОФП упражнения
    ( 84, 2, 55, 'Протяжка рывковая', ''),
    ( 1107, 2, 55, 'Протяжка рывковая без выхода на носки', ''),
    ( 1108, 2, 55, 'Протяжка рывковая в сед', ''),
    ( 1109, 2, 55, 'Протяжка рывковая + швунг жимовой', ''),
    ( 1110, 2, 55, 'Протяжка рывковая + швунг жимовой + приседание', ''),
    ( 1111, 2, 55, 'Протяжка рывковая + уходы в сед + приседание', ''),
    ( 1112, 2, 55, 'Протяжка рывковая + швунг жимовой + уходы в сед', ''),
    ( 1113, 2, 55, 'Протяжка рывковая + швунг + приседание', ''),
    ( 1114, 2, 55, 'Уходы рывковые', ''),
    ( 85, 2, 55, 'Швунг рывковым хватом из-за головы', ''),
    ( 86, 2, 55, 'Швунг рывковым хватом из-за головы в сед', ''),
    ( 1115, 2, 55, 'Швунг жимовой рывковым хватом из за головы', ''),
    ( 1116, 2, 55, 'Швунг жимовой рывковым хватом из за головы+приседания', ''),
    ( 1117, 2, 55, 'Жим рывковым хватом в седе', ''),

-- Толчковые
  -- -- Толчок классический
    ( 60, 1, 53, 'Толчок классический', ''),
    ( 67, 1, 53, 'Взятие на грудь + швунг толчковый', ''),
    ( 68, 1, 53, 'Взятие на грудь + швунг толчковый в сед', ''),
    ( 1069, 1, 53, 'Взятие на грудь в полуподсед + толчок с груди', ''),
    ( 1070, 1, 53, 'Взятие на грудь в полуподсед + швунг толчковый', ''),
    ( 1071, 1, 53, 'Взятие на грудь в стойку + толчок с груди', ''),
    ( 1072, 1, 53, 'Взятие на грудь в стойку + швунг толчковый', ''),
  -- -- Толчок классический подсобные
    ( 62, 1, 56, 'Взятие на грудь + швунг жимовой + толчок', ''),
    ( 1073, 1, 56, 'Взятие на грудь + швунг жимовой + швунг толчковый', ''),
    ( 64, 1, 56, 'Взятие на грудь + швунг толчковый + толчок', ''),
    ( 66, 1, 56, 'Взятие на грудь + толчок с паузой в ножницах', ''),
    ( 63, 1, 56, 'Взятие на грудь + приседания + толчок', ''),
    ( 1074, 1, 56, 'Взятие на грудь + приседания + швунг толчковый', ''),
    ( 1075, 1, 56, 'Взятие на грудь в полу подсед + швунг жимовой + швунг толчковый', ''),
  -- -- Толчковые ОФП упражнения
    ( 1200, 1, 57, 'Протяжка на грудь', ''),
    ( 1201, 1, 57, 'Протяжка на грудь без выхода на носки', ''),
    ( 1202, 1, 57, 'Протяжка на грудь в сед', ''),
    ( 1203, 1, 57, 'Протяжка на грудь + швунг жимовой', ''),
    ( 1204, 1, 57, 'Протяжка на грудь + швунг жимовой + приседание', ''),
    ( 1205, 1, 57, 'Протяжка на грудь + швунг толчковый + приседание', ''),
    ( 1206, 1, 57, 'Протяжка на грудь + швунг жимовой + швунг толчковый', ''),
    ( 1207, 1, 57, 'Швунг толчковым хватом из-за головы', ''),
    ( 1208, 1, 57, 'Швунг толчковым хватом из-за головы в сед', ''),
    ( 1209, 1, 57, 'Швунг толчковым хватом в сед', ''),
    ( 1210, 1, 57, 'Приседания на груди + швунг жимовой из седа (трастер)', ''),
    ( 1211, 1, 57, 'Взятие на грудь + швунг жим из седа (кластер)', ''),
  -- -- Взятие на грудь
    ( 1032, 1, 51, 'Взятие на грудь в сед', ''),
    ( 30, 1, 51, 'Взятие на грудь в полуподсед', ''),
    ( 29, 1, 51, 'Взятие на грудь в стойку', ''),
    ( 20, 1, 51, 'Взятие на грудь без разброса ног в сед', ''),
    ( 1033, 1, 51, 'Взятие на грудь без разброса ног в полуподсед', ''),
    ( 1034, 1, 51, 'Взятие на грудь в три положения', ''),
  -- -- Взятие на грудь подсобные
    ( 21, 1, 58, 'Взятие на грудь с подставки', ''),
    ( 1035, 1, 58, 'Взятие на грудь с подставки в полуподсед', ''),
    ( 1036, 1, 58, 'Взятие на грудь с подставки в стойку', ''),
    ( 22, 1, 58, 'Взятие на грудь с плинтов ниже колен', ''),
    ( 23, 1, 58, 'Взятие на грудь с плинтов выше колен', ''),
    ( 24, 1, 58, 'Взятие на грудь с трех положений', ''),
    ( 25, 1, 58, 'Взятие на грудь с виса ниже колен', ''),
    ( 1037, 1, 58, 'Взятие на грудь с виса выше колен', ''),
    ( 31, 1, 58, 'Взятие на грудь + приседания', ''),
    ( 28, 1, 58, 'Взятие на грудь с паузой ниже колен', ''),
  -- -- Толчок с груди
    ( 41, 1, 52, 'Толчок с груди со стоек', ''),
    ( 47, 1, 52, 'Швунг толчковый со стоек', ''),
    ( 48, 1, 52, 'Швунг толчковый в сед со стоек', ''),
    ( 1049, 1, 52, 'Толчок с груди с плинтов', ''),
    ( 40, 1, 52, 'Приседания на груди + толчок', ''),
  -- -- Толчок с груди подсобные
    ( 94, 1, 59, 'Швунг жимовой', ''),
    ( 44, 1, 59, 'Толчок с паузой в ножницах', ''),
    ( 43, 1, 59, 'Толчок с паузой в подседе', ''),
    ( 46, 1, 59, 'Швунг жимовой + толчок', ''),
    ( 1050, 1, 59, 'Швунг жимовой + швунг толчковой', ''),
    ( 1051, 1, 59, 'Швунг жимовой + швунг толчковой + толчок с груди', ''),
    ( 41, 1, 59, 'Толчок со стоек из за головы', ''),
    ( 1052, 1, 59, 'Толчок со стоек с дисками на краях', ''),

-- Базовые (толчковые и рывковые)
  -- -- Тяга Рывковая
    ( 80, 2, 60, 'Тяга рывковая', ''),
    ( 81, 2, 60, 'Тяга рывковая с подставки', ''),
    ( 82, 2, 60, 'Тяга рывковая с плинтов ниже колен', ''),
    ( 83, 2, 60, 'Тяга рывковая с плинтов выше колен', ''),
    ( 1088, 2, 60, 'Тяга рывковая с помоста + с виса выше колен', ''),
    ( 1089, 2, 60, 'Тяга рывковая с помоста + с виса ниже колен', ''),
    ( 1181, 2, 60, 'Тяга рывковая с остановкой ниже колен', ''),
    ( 1182, 2, 60, 'Тяга рывковая без подрыва', ''),
    ( 1183, 2, 60, 'Тяга рывковая с подрывом без выхода на носки', ''),
    ( 1184, 2, 60, 'Тяга рывковая высокая', ''),
    ( 1185, 2, 60, 'Тяга рывковая с остановкой в трех положениях', ''),
  -- -- Тяга Толчковая
    ( 90, 1, 61, 'Тяга толчковая', ''),
    ( 91, 1, 61, 'Тяга толчковая с подставки', ''),
    ( 92, 1, 61, 'Тяга толчковая с плинтов выше колена', ''),
    ( 93, 1, 61, 'Тяга толчковая с плинтов ниже колена', ''),
    ( 1098, 1, 61, 'Тяга толчковая без подрыва', ''),
    ( 1099, 1, 61, 'Тяга толчковая с подрывом без выхода на носки', ''),
    ( 1290, 1, 61, 'Тяга Толчковая с помоста + с виса выше колен', ''),
    ( 1291, 1, 61, 'ТягаТолчковая с помоста + с виса ниже колен', ''),
  -- -- Приседания на плечах
    ( 96, 1, 62, 'Приседания на плечах', ''),
    ( 1292, 1, 62, 'Приседания на плечах с остановкой в седе', ''),
    ( 1293, 1, 62, 'Приседание на плечах медленно', ''),
    ( 1294, 1, 62, 'Приседания на плечах ноги вместе', ''),
    ( 1295, 1, 62, 'Приседания на плечах полуподсед', ''),
  -- -- Приседания на груди
    ( 97, 1, 63, 'Приседания на груди', ''),
    ( 1296, 1, 63, 'Приседания на груди медленно', ''),
    ( 1297, 1, 63, 'Приседания на груди ноги вместе', ''),
    ( 1298, 1, 63, 'Приседания на груди с остановкой в полу подседе', ''),
    ( 1299, 1, 63, 'Приседания на груди с остановкой в седе', ''),

-- ОФП
  -- -- Силовые
    ( 1401, 3, 64, 'Бицепс', ''),
    ( 1402, 3, 64, 'Жим из-за головы рывковым хватом', ''),
    ( 1403, 3, 64, 'Жим лежа', ''),
    ( 1404, 3, 64, 'Жим с груди толчковым хватом сидя', ''),
    ( 1405, 3, 64, 'Жим с груди толчковым хватом стоя', ''),
    ( 1406, 3, 64, 'Наклоны со штангой на плечах', ''),
    ( 1407, 3, 64, 'Наклоны через козла', ''),
    ( 1408, 3, 64, 'Наклоны через козла + пресс', ''),
    ( 1409, 3, 64, 'Отведение плеча', ''),
    ( 1410, 3, 64, 'Приседания в ножницах', ''),
    ( 1411, 3, 64, 'Разгибание бедра в блоке', ''),
    ( 1412, 3, 64, 'Разгибание плеча', ''),
    ( 1413, 3, 64, 'Разгибание плеча + отведение плеча', ''),
    ( 1414, 3, 64, 'Сгибание бедра в блоке', ''),
    ( 1415, 3, 64, 'Трицепс', ''),
    ( 1416, 3, 64, 'Тяга средним хватом ноги прямые', ''),
    ( 1417, 3, 64, 'Прыжки со штангой из седа', ''),
    ( 1418, 3, 64, 'Прыжки со штангой из полу подседа', ''),
  -- -- Гимнастические
    ( 1451, 3, 65, 'Отжимание', ''),
    ( 1452, 3, 65, 'Отжимание на брусьях', ''),
    ( 1453, 3, 65, 'Подтягивание', ''),
    ( 1454, 3, 65, 'Прыжки на возвышенность', ''),
    ( 1455, 3, 65, 'Прыжки в длинну', ''),
    ( 1456, 3, 65, 'Прыжки через перекладину', ''),
    ( 1457, 3, 65, 'Бёрпи', ''),
  -- -- Кардио
    ( 1480, 3, 65, 'Велосипед', ''),
    ( 1481, 3, 65, 'Скакалка', ''),

-- Старые, не вошедшие в основную классификацию
    ( 42, 1, 52, 'Толчок с плинтов (доп)', ''),
    ( 45, 1, 52, 'Толчок из-за головы (доп)', ''),
    ( 61, 1, 53, 'Взятие на грудь + толчок (доп)', ''),
    ( 65, 1, 53, 'Взятие на грудь + толчок с паузой в подседе (доп)', ''),
    ( 26, 1, 51, 'Взятие на грудь от бедра (доп)', ''),
    ( 27, 1, 51, 'Взятие на грудь от паха (доп)', ''),
    ( 87, 2, 54, 'Жим из-за головы рывковым хватом (доп)', ''),
    ( 95, 1, 54, 'Швунг толчковый (доп)', '')
  )	AS Source (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description)
    ON Source.[Id] = Target.[Id]
  WHEN NOT MATCHED BY Target THEN INSERT (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description)
    VALUES (Source.Id, Source.ExerciseTypeId, Source.ExerciseSubTypeId, Source.Name, Source.Description)
  WHEN MATCHED THEN UPDATE SET 
    Target.ExerciseTypeId = Source.ExerciseTypeId,
    Target.ExerciseSubTypeId = Source.ExerciseSubTypeId,
    Target.Name = Source.Name,
    Target.Description = Source.Description;


-- Рывковые
-- -- Рывок классический
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1) INSERT INTO [plan].Exercises VALUES  ( 1, 2, 50, 'Рывок: Рывок классический', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 2) INSERT INTO [plan].Exercises VALUES  ( 2, 2, 50, 'Рывок без разброса ног', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 12) INSERT INTO [plan].Exercises VALUES ( 12, 2, 50, 'Рывок в стойку', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 13) INSERT INTO [plan].Exercises VALUES ( 13, 2, 50, 'Рывок в полуподсед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1015) INSERT INTO [plan].Exercises VALUES ( 1015, 2, 50, 'Рывок с медленной тягой', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1016) INSERT INTO [plan].Exercises VALUES ( 1016, 2, 50, 'Рывок без захвата "замком"', '');
-- -- Рывок подсобные
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 3) INSERT INTO [plan].Exercises VALUES   ( 3, 2, 54, 'Рывок с подставки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 4) INSERT INTO [plan].Exercises VALUES   ( 4, 2, 54, 'Рывок с плинтов ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 5) INSERT INTO [plan].Exercises VALUES   ( 5, 2, 54, 'Рывок с плинтов выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 6) INSERT INTO [plan].Exercises VALUES   ( 6, 2, 54, 'Рывок с трех положений', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1014) INSERT INTO [plan].Exercises VALUES  ( 1014, 2, 54, 'Рывок в три положения', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 7) INSERT INTO [plan].Exercises VALUES   ( 7, 2, 54, 'Рывок с виса ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 8) INSERT INTO [plan].Exercises VALUES   ( 8, 2, 54, 'Рывок с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 10) INSERT INTO [plan].Exercises VALUES  ( 10, 2, 54, 'Рывок от паха', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 9) INSERT INTO [plan].Exercises VALUES   ( 9, 2, 54, 'Рывок с паузой 3 секунды ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 11) INSERT INTO [plan].Exercises VALUES  ( 11, 2, 54, 'Рывок с паузой 3 секунды выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1017) INSERT INTO [plan].Exercises VALUES  ( 1017, 2, 54, 'Рывок выше колен + рывок ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1018) INSERT INTO [plan].Exercises VALUES  ( 1018, 2, 54, 'Рывок ниже колен + рывок выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1019) INSERT INTO [plan].Exercises VALUES  ( 1019, 2, 54, 'Рывок без разброса ног в полу подсед с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1100) INSERT INTO [plan].Exercises VALUES ( 1100, 2, 54, 'Рывок без разброса ног + рывок классический', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1101) INSERT INTO [plan].Exercises VALUES ( 1101, 2, 54, 'Рывок в полуподсед + рывок классический', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1102) INSERT INTO [plan].Exercises VALUES ( 1102, 2, 54, 'Тяга рывковая до подрыва + рывок классический', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1103) INSERT INTO [plan].Exercises VALUES ( 1103, 2, 54, 'Тяга рывковая до колен + рывок классический', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1104) INSERT INTO [plan].Exercises VALUES ( 1104, 2, 54, 'Тяга рывковая+рывок с помоста', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1105) INSERT INTO [plan].Exercises VALUES ( 1105, 2, 54, 'Тяга рывковая+рывок с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1106) INSERT INTO [plan].Exercises VALUES ( 1106, 2, 54, 'Тяга рывковая+рывок с виса ниже колен', '');
-- -- Рывоковые ОФП упражнения
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 84) INSERT INTO [plan].Exercises VALUES  ( 84, 2, 55, 'Протяжка рывковая', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1107) INSERT INTO [plan].Exercises VALUES ( 1107, 2, 55, 'Протяжка рывковая без выхода на носки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1108) INSERT INTO [plan].Exercises VALUES ( 1108, 2, 55, 'Протяжка рывковая в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1109) INSERT INTO [plan].Exercises VALUES ( 1109, 2, 55, 'Протяжка рывковая + швунг жимовой', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1110) INSERT INTO [plan].Exercises VALUES ( 1110, 2, 55, 'Протяжка рывковая + швунг жимовой + приседание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1111) INSERT INTO [plan].Exercises VALUES ( 1111, 2, 55, 'Протяжка рывковая + уходы в сед + приседание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1112) INSERT INTO [plan].Exercises VALUES ( 1112, 2, 55, 'Протяжка рывковая + швунг жимовой + уходы в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1113) INSERT INTO [plan].Exercises VALUES ( 1113, 2, 55, 'Протяжка рывковая + швунг + приседание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1114) INSERT INTO [plan].Exercises VALUES ( 1114, 2, 55, 'Уходы рывковые', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 85) INSERT INTO [plan].Exercises VALUES  ( 85, 2, 55, 'Швунг рывковым хватом из-за головы', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 86) INSERT INTO [plan].Exercises VALUES  ( 86, 2, 55, 'Швунг рывковым хватом из-за головы в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1115) INSERT INTO [plan].Exercises VALUES ( 1115, 2, 55, 'Швунг жимовой рывковым хватом из за головы', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1116) INSERT INTO [plan].Exercises VALUES ( 1116, 2, 55, 'Швунг жимовой рывковым хватом из за головы+приседания', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1117) INSERT INTO [plan].Exercises VALUES ( 1117, 2, 55, 'Жим рывковым хватом в седе', '');


-- Толчковые
-- -- Толчок классический
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 60) INSERT INTO [plan].Exercises VALUES ( 60, 1, 53, 'Толчок классический', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 67) INSERT INTO [plan].Exercises VALUES ( 67, 1, 53, 'Взятие на грудь + швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 68) INSERT INTO [plan].Exercises VALUES ( 68, 1, 53, 'Взятие на грудь + швунг толчковый в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1069) INSERT INTO [plan].Exercises VALUES ( 1069, 1, 53, 'Взятие на грудь в полуподсед + толчок с груди', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1070) INSERT INTO [plan].Exercises VALUES ( 1070, 1, 53, 'Взятие на грудь в полуподсед + швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1071) INSERT INTO [plan].Exercises VALUES ( 1071, 1, 53, 'Взятие на грудь в стойку + толчок с груди', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1072) INSERT INTO [plan].Exercises VALUES ( 1072, 1, 53, 'Взятие на грудь в стойку + швунг толчковый', '');
-- -- Толчок классический подсобные
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 62) INSERT INTO [plan].Exercises VALUES ( 62, 1, 56, 'Взятие на грудь + швунг жимовой + толчок', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1073) INSERT INTO [plan].Exercises VALUES ( 1073, 1, 56, 'Взятие на грудь + швунг жимовой + швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 64) INSERT INTO [plan].Exercises VALUES ( 64, 1, 56, 'Взятие на грудь + швунг толчковый + толчок', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 66) INSERT INTO [plan].Exercises VALUES ( 66, 1, 56, 'Взятие на грудь + толчок с паузой в ножницах', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 63) INSERT INTO [plan].Exercises VALUES ( 63, 1, 56, 'Взятие на грудь + приседания + толчок', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1074) INSERT INTO [plan].Exercises VALUES ( 1074, 1, 56, 'Взятие на грудь + приседания + швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1075) INSERT INTO [plan].Exercises VALUES ( 1075, 1, 56, 'Взятие на грудь в полу подсед + швунг жимовой + швунг толчковый', '');
-- -- Толчковые ОФП упражнения
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1200) INSERT INTO [plan].Exercises VALUES ( 1200, 1, 57, 'Протяжка на грудь', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1201) INSERT INTO [plan].Exercises VALUES ( 1201, 1, 57, 'Протяжка на грудь без выхода на носки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1202) INSERT INTO [plan].Exercises VALUES ( 1202, 1, 57, 'Протяжка на грудь в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1203) INSERT INTO [plan].Exercises VALUES ( 1203, 1, 57, 'Протяжка на грудь + швунг жимовой', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1204) INSERT INTO [plan].Exercises VALUES ( 1204, 1, 57, 'Протяжка на грудь + швунг жимовой + приседание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1205) INSERT INTO [plan].Exercises VALUES ( 1205, 1, 57, 'Протяжка на грудь + швунг толчковый + приседание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1206) INSERT INTO [plan].Exercises VALUES ( 1206, 1, 57, 'Протяжка на грудь + швунг жимовой + швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1207) INSERT INTO [plan].Exercises VALUES ( 1207, 1, 57, 'Швунг толчковым хватом из-за головы', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1208) INSERT INTO [plan].Exercises VALUES ( 1208, 1, 57, 'Швунг толчковым хватом из-за головы в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1209) INSERT INTO [plan].Exercises VALUES ( 1209, 1, 57, 'Швунг толчковым хватом в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1210) INSERT INTO [plan].Exercises VALUES ( 1210, 1, 57, 'Приседания на груди + швунг жимовой из седа (трастер)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1211) INSERT INTO [plan].Exercises VALUES ( 1211, 1, 57, 'Взятие на грудь + швунг жим из седа (кластер)', '');
-- -- Взятие на грудь
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1032) INSERT INTO [plan].Exercises VALUES ( 1032, 1, 51, 'Взятие на грудь в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 30) INSERT INTO [plan].Exercises VALUES ( 30, 1, 51, 'Взятие на грудь в полуподсед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 29) INSERT INTO [plan].Exercises VALUES ( 29, 1, 51, 'Взятие на грудь в стойку', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 20) INSERT INTO [plan].Exercises VALUES ( 20, 1, 51, 'Взятие на грудь без разброса ног в сед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1033) INSERT INTO [plan].Exercises VALUES ( 1033, 1, 51, 'Взятие на грудь без разброса ног в полуподсед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1034) INSERT INTO [plan].Exercises VALUES ( 1034, 1, 51, 'Взятие на грудь в три положения', '');
-- -- Взятие на грудь подсобные
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 21) INSERT INTO [plan].Exercises VALUES ( 21, 1, 58, 'Взятие на грудь с подставки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1035) INSERT INTO [plan].Exercises VALUES ( 1035, 1, 58, 'Взятие на грудь с подставки в полуподсед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1036) INSERT INTO [plan].Exercises VALUES ( 1036, 1, 58, 'Взятие на грудь с подставки в стойку', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 22) INSERT INTO [plan].Exercises VALUES ( 22, 1, 58, 'Взятие на грудь с плинтов ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 23) INSERT INTO [plan].Exercises VALUES ( 23, 1, 58, 'Взятие на грудь с плинтов выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 24) INSERT INTO [plan].Exercises VALUES ( 24, 1, 58, 'Взятие на грудь с трех положений', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 25) INSERT INTO [plan].Exercises VALUES ( 25, 1, 58, 'Взятие на грудь с виса ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1037) INSERT INTO [plan].Exercises VALUES ( 1037, 1, 58, 'Взятие на грудь с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 31) INSERT INTO [plan].Exercises VALUES ( 31, 1, 58, 'Взятие на грудь + приседания', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 28) INSERT INTO [plan].Exercises VALUES ( 28, 1, 58, 'Взятие на грудь с паузой ниже колен', '');
-- -- Толчок с груди
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 41) INSERT INTO [plan].Exercises VALUES ( 41, 1, 52, 'Толчок с груди со стоек', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 47) INSERT INTO [plan].Exercises VALUES ( 47, 1, 52, 'Швунг толчковый со стоек', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 48) INSERT INTO [plan].Exercises VALUES ( 48, 1, 52, 'Швунг толчковый в сед со стоек', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1049) INSERT INTO [plan].Exercises VALUES ( 1049, 1, 52, 'Толчок с груди с плинтов', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 40) INSERT INTO [plan].Exercises VALUES ( 40, 1, 52, 'Приседания на груди + толчок', '');
-- -- Толчок с груди подсобные
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 94) INSERT INTO [plan].Exercises VALUES ( 94, 1, 59, 'Швунг жимовой', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 44) INSERT INTO [plan].Exercises VALUES ( 44, 1, 59, 'Толчок с паузой в ножницах', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 43) INSERT INTO [plan].Exercises VALUES ( 43, 1, 59, 'Толчок с паузой в подседе', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 46) INSERT INTO [plan].Exercises VALUES ( 46, 1, 59, 'Швунг жимовой + толчок', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1050) INSERT INTO [plan].Exercises VALUES ( 1050, 1, 59, 'Швунг жимовой + швунг толчковой', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1051) INSERT INTO [plan].Exercises VALUES ( 1051, 1, 59, 'Швунг жимовой + швунг толчковой + толчок с груди', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 41) INSERT INTO [plan].Exercises VALUES ( 41, 1, 59, 'Толчок со стоек из за головы', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1052) INSERT INTO [plan].Exercises VALUES ( 1052, 1, 59, 'Толчок со стоек с дисками на краях', '');


-- Базовые (толчковые и рывковые)
-- -- Тяга Рывковая
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 80) INSERT INTO [plan].Exercises VALUES ( 80, 2, 60, 'Тяга рывковая', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 81) INSERT INTO [plan].Exercises VALUES ( 81, 2, 60, 'Тяга рывковая с подставки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 82) INSERT INTO [plan].Exercises VALUES ( 82, 2, 60, 'Тяга рывковая с плинтов ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 83) INSERT INTO [plan].Exercises VALUES ( 83, 2, 60, 'Тяга рывковая с плинтов выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1088) INSERT INTO [plan].Exercises VALUES ( 1088, 2, 60, 'Тяга рывковая с помоста + с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1089) INSERT INTO [plan].Exercises VALUES ( 1089, 2, 60, 'Тяга рывковая с помоста + с виса ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1181) INSERT INTO [plan].Exercises VALUES ( 1181, 2, 60, 'Тяга рывковая с остановкой ниже колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1182) INSERT INTO [plan].Exercises VALUES ( 1182, 2, 60, 'Тяга рывковая без подрыва', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1183) INSERT INTO [plan].Exercises VALUES ( 1183, 2, 60, 'Тяга рывковая с подрывом без выхода на носки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1184) INSERT INTO [plan].Exercises VALUES ( 1184, 2, 60, 'Тяга рывковая высокая', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1185) INSERT INTO [plan].Exercises VALUES ( 1185, 2, 60, 'Тяга рывковая с остановкой в трех положениях', '');
-- -- Тяга Толчковая
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 90) INSERT INTO [plan].Exercises VALUES ( 90, 1, 61, 'Тяга толчковая', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 91) INSERT INTO [plan].Exercises VALUES ( 91, 1, 61, 'Тяга толчковая с подставки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 92) INSERT INTO [plan].Exercises VALUES ( 92, 1, 61, 'Тяга толчковая с плинтов выше колена', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 93) INSERT INTO [plan].Exercises VALUES ( 93, 1, 61, 'Тяга толчковая с плинтов ниже колена', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1098) INSERT INTO [plan].Exercises VALUES ( 1098, 1, 61, 'Тяга толчковая без подрыва', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1099) INSERT INTO [plan].Exercises VALUES ( 1099, 1, 61, 'Тяга толчковая с подрывом без выхода на носки', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1290) INSERT INTO [plan].Exercises VALUES ( 1290, 1, 61, 'Тяга Толчковая с помоста + с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1291) INSERT INTO [plan].Exercises VALUES ( 1291, 1, 61, 'ТягаТолчковая с помоста + с виса ниже колен', '');
-- -- Приседания на плечах
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 96) INSERT INTO [plan].Exercises VALUES ( 96, 1, 62, 'Приседания на плечах', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1292) INSERT INTO [plan].Exercises VALUES ( 1292, 1, 62, 'Приседания на плечах с остановкой в седе', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1293) INSERT INTO [plan].Exercises VALUES ( 1293, 1, 62, 'Приседание на плечах медленно', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1294) INSERT INTO [plan].Exercises VALUES ( 1294, 1, 62, 'Приседания на плечах ноги вместе', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1295) INSERT INTO [plan].Exercises VALUES ( 1295, 1, 62, 'Приседания на плечах полуподсед', '');
-- -- Приседания на груди
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 97) INSERT INTO [plan].Exercises VALUES ( 97, 1, 63, 'Приседания на груди', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1296) INSERT INTO [plan].Exercises VALUES ( 1296, 1, 63, 'Приседания на груди медленно', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1297) INSERT INTO [plan].Exercises VALUES ( 1297, 1, 63, 'Приседания на груди ноги вместе', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1298) INSERT INTO [plan].Exercises VALUES ( 1298, 1, 63, 'Приседания на груди с остановкой в полу подседе', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1299) INSERT INTO [plan].Exercises VALUES ( 1299, 1, 63, 'Приседания на груди с остановкой в седе', '');


-- ОФП
-- -- Силовые
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1401) INSERT INTO [plan].Exercises VALUES ( 1401, 3, 64, 'Бицепс', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1402) INSERT INTO [plan].Exercises VALUES ( 1402, 3, 64, 'Жим из-за головы рывковым хватом', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1403) INSERT INTO [plan].Exercises VALUES ( 1403, 3, 64, 'Жим лежа', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1404) INSERT INTO [plan].Exercises VALUES ( 1404, 3, 64, 'Жим с груди толчковым хватом сидя', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1405) INSERT INTO [plan].Exercises VALUES ( 1405, 3, 64, 'Жим с груди толчковым хватом стоя', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1406) INSERT INTO [plan].Exercises VALUES ( 1406, 3, 64, 'Наклоны со штангой на плечах', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1407) INSERT INTO [plan].Exercises VALUES ( 1407, 3, 64, 'Наклоны через козла', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1408) INSERT INTO [plan].Exercises VALUES ( 1408, 3, 64, 'Наклоны через козла + пресс', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1409) INSERT INTO [plan].Exercises VALUES ( 1409, 3, 64, 'Отведение плеча', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1410) INSERT INTO [plan].Exercises VALUES ( 1410, 3, 64, 'Приседания в ножницах', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1411) INSERT INTO [plan].Exercises VALUES ( 1411, 3, 64, 'Разгибание бедра в блоке', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1412) INSERT INTO [plan].Exercises VALUES ( 1412, 3, 64, 'Разгибание плеча', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1413) INSERT INTO [plan].Exercises VALUES ( 1413, 3, 64, 'Разгибание плеча + отведение плеча', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1414) INSERT INTO [plan].Exercises VALUES ( 1414, 3, 64, 'Сгибание бедра в блоке', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1415) INSERT INTO [plan].Exercises VALUES ( 1415, 3, 64, 'Трицепс', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1416) INSERT INTO [plan].Exercises VALUES ( 1416, 3, 64, 'Тяга средним хватом ноги прямые', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1417) INSERT INTO [plan].Exercises VALUES ( 1417, 3, 64, 'Прыжки со штангой из седа', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1418) INSERT INTO [plan].Exercises VALUES ( 1418, 3, 64, 'Прыжки со штангой из полу подседа', '');
-- -- Гимнастические
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1451) INSERT INTO [plan].Exercises VALUES ( 1451, 3, 65, 'Отжимание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1452) INSERT INTO [plan].Exercises VALUES ( 1452, 3, 65, 'Отжимание на брусьях', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1453) INSERT INTO [plan].Exercises VALUES ( 1453, 3, 65, 'Подтягивание', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1454) INSERT INTO [plan].Exercises VALUES ( 1454, 3, 65, 'Прыжки на возвышенность', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1455) INSERT INTO [plan].Exercises VALUES ( 1455, 3, 65, 'Прыжки в длинну', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1456) INSERT INTO [plan].Exercises VALUES ( 1456, 3, 65, 'Прыжки через перекладину', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1457) INSERT INTO [plan].Exercises VALUES ( 1457, 3, 65, 'Бёрпи', '');
-- -- Кардио
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1480) INSERT INTO [plan].Exercises VALUES ( 1480, 3, 65, 'Велосипед', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 1481) INSERT INTO [plan].Exercises VALUES ( 1481, 3, 65, 'Скакалка', '');



-- Старые, не вошедшие в основную классификацию
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 42) INSERT INTO [plan].Exercises VALUES ( 42, 1, 52, 'Толчок с плинтов (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 45) INSERT INTO [plan].Exercises VALUES ( 45, 1, 52, 'Толчок из-за головы (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 61) INSERT INTO [plan].Exercises VALUES ( 61, 1, 53, 'Взятие на грудь + толчок (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 65) INSERT INTO [plan].Exercises VALUES ( 65, 1, 53, 'Взятие на грудь + толчок с паузой в подседе (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 26) INSERT INTO [plan].Exercises VALUES ( 26, 1, 51, 'Взятие на грудь от бедра (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 27) INSERT INTO [plan].Exercises VALUES ( 27, 1, 51, 'Взятие на грудь от паха (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 87) INSERT INTO [plan].Exercises VALUES ( 87, 2, 54, 'Жим из-за головы рывковым хватом (доп)', '');
IF NOT EXISTS (SELECT * FROM [plan].Exercises WHERE [Id] = 95) INSERT INTO [plan].Exercises VALUES ( 95, 1, 54, 'Швунг толчковый (доп)', '');


SET IDENTITY_INSERT [plan].Exercises OFF 
