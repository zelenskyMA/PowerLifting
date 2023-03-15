
SET IDENTITY_INSERT usr.Users ON

IF NOT EXISTS (SELECT * FROM usr.Users WHERE [Id] = 1)
  INSERT INTO usr.Users (Id, Email, Password, Salt, Blocked) VALUES 
  ( 1, 'caragus_niveus@mail.ru', 'BD-40-DC-2F-2B-E1-A6-EC-E0-27-63-65-BD-0C-E6-AB-57-54-8F-65', '1CCFD3C98A', 0); --password = asdf123

IF NOT EXISTS (SELECT * FROM usr.Users WHERE [Id] = 2)
  INSERT INTO usr.Users (Id, Email, Password, Salt, Blocked) VALUES 
  ( 2, 'coach@mail.ru', 'BD-40-DC-2F-2B-E1-A6-EC-E0-27-63-65-BD-0C-E6-AB-57-54-8F-65', '1CCFD3C98A', 0); --password = asdf123

SET IDENTITY_INSERT usr.Users OFF 

IF NOT EXISTS (SELECT * FROM usr.UserInfo WHERE [UserId] = 1) INSERT INTO usr.UserInfo (UserId, FirstName, Surname, Patronimic) VALUES ( 1, 'Администратор', '', '' );
IF NOT EXISTS (SELECT * FROM usr.UserInfo WHERE [UserId] = 2) INSERT INTO usr.UserInfo (UserId, FirstName, Surname, Patronimic) VALUES ( 2, 'Тренер', '', '' );

IF NOT EXISTS (SELECT * FROM usr.UserRoles WHERE [UserId] = 1 AND [RoleId] = 10) INSERT INTO usr.UserRoles (UserId, RoleId) VALUES ( 1, 10 ); -- admin
IF NOT EXISTS (SELECT * FROM usr.UserRoles WHERE [UserId] = 2 AND [RoleId] = 11) INSERT INTO usr.UserRoles (UserId, RoleId) VALUES ( 2, 11 ); -- coach
