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
  INSERT INTO Users (Id, Email, Password, Salt) VALUES ( 1, 'test@email.com', 'testPwd', 'testSalt');

SET IDENTITY_INSERT Users OFF 


SET IDENTITY_INSERT ExerciseTypes ON

IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 1)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 1, 'Толчковые', 'Упражнения, при выполнении задействующие толчок штанги');
IF NOT EXISTS (SELECT * FROM ExerciseTypes WHERE [Id] = 2)
  INSERT INTO ExerciseTypes (Id, Name, Description) VALUES ( 2, 'Рывковые', 'Упражнения, при выполнении задействующие рывок штанги');

SET IDENTITY_INSERT ExerciseTypes OFF 



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