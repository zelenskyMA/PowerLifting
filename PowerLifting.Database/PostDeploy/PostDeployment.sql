/*
Post-Deployment Script Template              
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.    
 Use SQLCMD syntax to include a file in the post-deployment script.      
 Example:      :r .\myfile.sql                
 Use SQLCMD syntax to reference a variable in the post-deployment script.    
 Example:      :setvar TableName MyTable              
               SELECT * FROM [$(TableName)]          
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT Users ON

IF NOT EXISTS (SELECT * FROM Users WHERE [Id] = 1)
  INSERT INTO Users (Id, Email, Password, Salt) VALUES ( 1, 'admin@email.com', 'testPwd', 'testSalt');

SET IDENTITY_INSERT Users OFF 


SET IDENTITY_INSERT Percentages ON

IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 1)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 1, '< 50%', 'Меньше 50%', 0, 49);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 2)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 2, '50 - 60%', '', 50, 59);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 3)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 3, '60 - 70%', '', 60, 69);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 4)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 4, '70 - 80%', '', 70, 79);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 5)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 5, '80 - 90%', '', 80, 89);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 6)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 6, '90 - 100%', '', 90, 99);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 7)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 7, '100 - 110%', '', 100, 109);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 8)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 8, '110 - 120%', '', 110, 119);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 9)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 9, '> 120%', 'Больше 120%', 120, 999);

SET IDENTITY_INSERT Percentages OFF 


SET IDENTITY_INSERT ExerciseTypes ON

IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 1)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 1, 'Толчковые', 'Упражнения на толчок штанги');
IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 2)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 2, 'Рывковые', 'Упражнения на рывок штанги');
IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 3)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 3, 'Жимовые', 'Упражнения на жим штанги');

SET IDENTITY_INSERT ExerciseTypes OFF 


SET IDENTITY_INSERT Exercises ON

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 1)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 1, 1, 'Упражнение 1', 'Описание упражнения 134');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 2)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 2, 1, 'Упражнение 2', 'Описание упражнения 1ыва');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 3)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 3, 1, 'Упражнение 3 пп аа вв', 'Описание упражнения 1пр');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 4)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 4, 1, 'Упражнение 4', 'Описание упражнения 1смчит');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 5)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 5, 1, 'Упражнение 5 ппп', 'Описание упражнения 1ясчвм');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 6)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 6, 1, 'Упражнение 6 ааа', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 7)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 7, 1, 'Упражнение 7 ббб', 'Описание упражнения 1ывап');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 8)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 8, 2, 'Упражнение рр1', 'Описание упражнения 134');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 9)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 9, 2, 'Упражнение рр2', 'Описание упражнения 1ыва');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 10)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 10, 2, 'Упражнение рр3 пп аа вв', 'Описание упражнения 1пр');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 11)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 11, 2, 'Упражнение рр4', 'Описание упражнения 1смчит');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 12)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 12, 2, 'Упражнение рр5 ппп', 'Описание упражнения 1ясчвм');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 13)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 13, 2, 'Упражнение рр6 ааа', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 14)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 14, 2, 'Упражнение рр7 ббб', 'Описание упражнения 1ывап');

IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 15)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 15, 3, 'Упражнение жим рр6 ааа', '');
IF NOT EXISTS (SELECT * FROM Exercises WHERE [Id] = 16)
  INSERT INTO Exercises (Id, ExerciseTypeId, Name, Description) VALUES ( 16, 3, 'Упражнение жим рр7 ббб', 'Описание упражнения 1ывап');

SET IDENTITY_INSERT Exercises OFF 
