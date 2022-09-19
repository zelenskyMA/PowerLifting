

--SET IDENTITY_INSERT usr.Users ON

--IF NOT EXISTS (SELECT * FROM usr.Users WHERE [Id] = 1)
--  INSERT INTO usr.Users (Id, Email, Password, Salt) VALUES ( 1, 'admin@email.com', 'testPwd', 'testSalt');

--SET IDENTITY_INSERT usr.Users OFF 