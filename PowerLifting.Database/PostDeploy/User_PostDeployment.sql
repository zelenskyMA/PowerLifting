

SET IDENTITY_INSERT usr.Roles ON

IF NOT EXISTS (SELECT * FROM usr.Roles WHERE [Id] = 1)
  INSERT INTO usr.Roles (Id, Name, Description) VALUES ( 1, 'Администратор', 'Администратор сайта.');
IF NOT EXISTS (SELECT * FROM usr.Roles WHERE [Id] = 2)
  INSERT INTO usr.Roles (Id, Name, Description) VALUES ( 2, 'Тренер', 'Продвинутый пользователь сайта. Управляет группами спортсменов.');
IF NOT EXISTS (SELECT * FROM usr.Roles WHERE [Id] = 3)
  INSERT INTO usr.Roles (Id, Name, Description) VALUES ( 3, 'Спортсмен', 'Базовый пользователь сайта.');

SET IDENTITY_INSERT usr.Roles OFF 


--SET IDENTITY_INSERT usr.Users ON

--IF NOT EXISTS (SELECT * FROM usr.Users WHERE [Id] = 1)
--  INSERT INTO usr.Users (Id, Email, Password, Salt) VALUES ( 1, 'admin@email.com', 'testPwd', 'testSalt');

--SET IDENTITY_INSERT usr.Users OFF 