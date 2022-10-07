

SET IDENTITY_INSERT Percentages ON

IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 1)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 1, ' < 30%', 'Меньше 30%', 0, 29);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 2)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 2, '30 - 40%', '', 30, 39);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 3)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 3, '40 - 50%', '', 40, 49);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 4)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 4, '50 - 60%', '', 50, 59);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 5)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 5, '60 - 70%', '', 60, 69);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 6)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 6, '70 - 80%', '', 70, 79);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 7)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 7, '80 - 90%', '', 80, 89);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 8)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 8, '90 - 100%', '', 90, 99);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 9)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 9, '100 - 110%', '', 100, 109);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 10)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 10, '110 - 120%', '', 110, 119);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 11)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 11, '120 - 130%', '', 120, 129);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 12)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 12, '130 - 140%', '', 130, 139);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 13)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 13, '140 - 150%', '', 140, 149);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 14)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 14, '> 150%', 'Больше 150%', 150, 999);

SET IDENTITY_INSERT Percentages OFF 


SET IDENTITY_INSERT Exercises ON

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 1)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 1, 2, 50, 'Рывок: Рывок классический', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 2)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 2, 2, 50, 'Рывок без разброса ног', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 3)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 3, 2, 50, 'Рывок с подставки', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 4)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 4, 2, 50, 'Рывок с плинтов ниже колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 5)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 5, 2, 50, 'Рывок с плинтов выше колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 6)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 6, 2, 50, 'Рывок с трех положений', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 7)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 7, 2, 50, 'Рывок с виса ниже колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 8)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 8, 2, 50, 'Рывок с виса выше колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 9)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 9, 2, 50, 'Рывок от бедра', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 10)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 10, 2, 50, 'Рывок от паха', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 11)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 11, 2, 50, 'Рывок с паузой', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 12)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 12, 2, 50, 'Рывок в стойку', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 13)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 13, 2, 50, 'Рывок в полуподсед', '');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 20)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 20, 1, 51, 'Взятие на грудь без разброса ног', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 21)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 21, 1, 51, 'Взятие на грудь с подставки', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 22)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 22, 1, 51, 'Взятие на грудь с плинтов ниже колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 23)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 23, 1, 51, 'Взятие на грудь с плинтов выше колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 24)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 24, 1, 51, 'Взятие на грудь с трех положений', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 25)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 25, 1, 51, 'Взятие на грудь с виса ниже колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 26)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 26, 1, 51, 'Взятие на грудь от бедра', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 27)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 27, 1, 51, 'Взятие на грудь от паха', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 28)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 28, 1, 51, 'Взятие на грудь с паузой', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 29)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 29, 1, 51, 'Взятие на грудь в стойку', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 30)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 30, 1, 51, 'Взятие на грудь в полуподсед', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 31)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 31, 1, 51, 'Взятие на грудь с приседаниями', '');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 40)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 40, 1, 52, 'Приседания на груди + толчок', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 41)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 41, 1, 52, 'Толчок со стоек', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 42)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 42, 1, 52, 'Толчок с плинтов', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 43)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 43, 1, 52, 'Толчок с паузой в подседе', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 44)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 44, 1, 52, 'Толчок с паузой в ножницах', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 45)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 45, 1, 52, 'Толчок из-за головы', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 46)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 46, 1, 52, 'Швунг жимовой + толчок', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 47)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 47, 1, 52, 'Швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 48)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 48, 1, 52, 'Швунг толчковый в сед', '');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 60)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 60, 1, 53, 'Толчок: Толчок классический', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 61)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 61, 1, 53, 'Взятие на грудь + толчок', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 62)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 62, 1, 53, 'Взятие на грудь + швунг жимовой + толчок', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 63)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 63, 1, 53, 'Взятие на грудь + приседания + толчок', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 64)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 64, 1, 53, 'Взятие на грудь + швунг толчковый + толчок', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 65)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 65, 1, 53, 'Взятие на грудь + толчок с паузой в подседе', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 66)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 66, 1, 53, 'Взятие на грудь + толчок с паузой в ножницах', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 67)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 67, 1, 53, 'Взятие на грудь + швунг толчковый', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 68)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 68, 1, 53, 'Взятие на грудь + швунг толчковый в сед', '');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 80)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 80, 2, 54, 'Тяга рывковая', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 81)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 81, 2, 54, 'Тяга рывковая с подставки', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 82)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 82, 2, 54, 'Тяга рывковая с плинтов ниже колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 83)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 83, 2, 54, 'Тяга рывковая с плинтов выше колен', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 84)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 84, 2, 54, 'Протяжка рывковая', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 85)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 85, 2, 54, 'Швунг рывковым хватом из-за головы', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 86)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 86, 2, 54, 'Швунг рывковым хватом из-за головы в сед', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 87)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 87, 2, 54, 'Жим из-за головы рывковым хватом', '');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 90)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 90, 1, 54, 'Тяга толчковая', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 91)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 91, 1, 54, 'Тяга толчковая с подставки', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 92)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 92, 1, 54, 'Тяга толчковая с плинтов выше колена', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 93)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 93, 1, 54, 'Тяга толчковая с плинтов ниже колена', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 94)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 94, 1, 54, 'Швунг жимовой', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 95)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 95, 1, 54, 'Швунг толчковый', '');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 96)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 96, 1, 54, 'Приседания на плечах', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 97)
  INSERT INTO Exercises (Id, ExerciseTypeId, ExerciseSubTypeId, Name, Description) VALUES ( 97, 1, 54, 'Приседания на груди', '');

SET IDENTITY_INSERT Exercises OFF 
