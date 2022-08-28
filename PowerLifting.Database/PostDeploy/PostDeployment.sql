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

SET IDENTITY_INSERT ExerciseTypes ON

IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 1)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 1, 'Толчковые', 'Упражнения, при выполнении задействующие толчок штанги');
IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 2)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 2, 'Рывковые', 'Упражнения, при выполнении задействующие рывок штанги');

SET IDENTITY_INSERT ExerciseTypes OFF 



SET IDENTITY_INSERT Percentages ON

IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 1)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 1, '< 50%', 'Меньше 50%', 0, 50);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 2)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 2, '51 - 60%', '', 51, 60);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 3)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 3, '61 - 70%', '', 61, 70);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 4)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 4, '71 - 80%', '', 71, 80);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 5)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 5, '81 - 90%', '', 81, 90);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 6)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 6, '91 - 100%', '', 91, 100);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 7)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 7, '101 - 110%', '', 101, 110);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 8)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 8, '111 - 120%', '', 111, 120);
IF NOT EXISTS (SELECT * FROM Percentages WHERE [Id] = 9)
  INSERT INTO Percentages (Id, Name, Description, MinValue, MaxValue) VALUES ( 9, '> 120%', 'Больше 120%', 121, 999);

SET IDENTITY_INSERT Percentages OFF 