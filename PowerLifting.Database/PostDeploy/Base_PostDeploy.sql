

SET IDENTITY_INSERT DictionaryTypes ON

IF NOT EXISTS (SELECT * FROM DictionaryTypes WHERE [Id] = 1)
  INSERT INTO DictionaryTypes (Id, Name, Description) VALUES ( 1, 'Тип упражнений', 'Базовые типы упражнений');
IF NOT EXISTS (SELECT * FROM DictionaryTypes WHERE [Id] = 2)
  INSERT INTO DictionaryTypes (Id, Name, Description) VALUES ( 2, 'Категория упражнений', 'Подраздел в рамках базового типа упражнений');
IF NOT EXISTS (SELECT * FROM DictionaryTypes WHERE [Id] = 3)
  INSERT INTO DictionaryTypes (Id, Name, Description) VALUES ( 3, 'Рекорды', 'Учетные рекорды в выполнении упражнений');

SET IDENTITY_INSERT DictionaryTypes OFF 


SET IDENTITY_INSERT Dictionaries ON

IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 1)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 1, 1, 'Толчковые', 'Упражнения на толчок штанги');
IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 2)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 2, 1, 'Рывковые', 'Упражнения на рывок штанги');
IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 3)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 3, 1, 'Жимовые', 'Упражнения на жим штанги');

IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 50)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 50, 2, 'Рывок классический', '');
IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 51)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 51, 2, 'Толчок. Взятие на грудь', '');
IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 52)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 52, 2, 'Толчок с груди', '');
IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 53)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 53, 2, 'Толчок классический', '');
IF NOT EXISTS (SELECT * FROM Dictionaries WHERE [Id] = 54)
  INSERT INTO Dictionaries (Id, TypeId, Name, Description) VALUES ( 54, 2, 'ОФП', '');

SET IDENTITY_INSERT Dictionaries OFF 