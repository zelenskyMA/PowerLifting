
SET IDENTITY_INSERT DictionaryTypes ON

  MERGE DictionaryTypes AS Target
  USING (VALUES 
    ( 1, 'Тип упражнений', 'Базовые типы упражнений'),
    ( 2, 'Категория упражнений', 'Подраздел в рамках базового типа упражнений'),
    ( 3, 'Роль пользователя', 'Роли пользователей, определяющие набор их прав')
  )	AS Source (Id, Name, Description)
    ON Source.[Id] = Target.[Id]
  WHEN NOT MATCHED BY Target THEN INSERT (Id, Name, Description) VALUES (Source.Id, Source.Name, Source.Description)
  WHEN MATCHED THEN UPDATE SET 
    Target.Name = Source.Name, 
    Target.Description = Source.Description;

SET IDENTITY_INSERT DictionaryTypes OFF 


SET IDENTITY_INSERT Dictionaries ON

  MERGE Dictionaries AS Target
  USING (VALUES 
    ( 1, 1, 'Толчковые', 'Упражнения на толчок штанги'),
    ( 2, 1, 'Рывковые', 'Упражнения на рывок штанги'),
    ( 3, 1, 'ОФП', 'Упражнения общей физической подготовки'),

    ( 10, 3, 'Администратор', 'Администратор сайта.'),
    ( 11, 3, 'Тренер', 'Продвинутый пользователь сайта. Управляет группами спортсменов.'),

    ( 50, 2, 'Рывок классический', ''),
    ( 51, 2, 'Взятие на грудь', ''),
    ( 52, 2, 'Толчок с груди', ''),
    ( 54, 2, 'Рывок подсобные', ''),
    ( 55, 2, 'Рывоковые ОФП упражнения', ''),

    ( 53, 2, 'Толчок классический', ''),
    ( 56, 2, 'Толчок классический подсобные', ''),
    ( 57, 2, 'Толчковые ОФП упражнения', ''),
    ( 58, 2, 'Взятие на грудь подсобные', ''),
    ( 59, 2, 'Толчок с груди подсобные', ''),

    ( 60, 2, 'Тяга Рывковая', ''),
    ( 61, 2, 'Тяга Толчковая', ''),
    ( 62, 2, 'Приседания на плечах', ''),
    ( 63, 2, 'Приседания на груди', ''),

    ( 64, 2, 'Силовые', ''),
    ( 65, 2, 'Гимнастические', ''),
    ( 66, 2, 'Кардио', '')
  )	AS Source (Id, TypeId, Name, Description)
    ON Source.[Id] = Target.[Id]
  WHEN NOT MATCHED BY Target THEN INSERT (Id, TypeId, Name, Description) VALUES (Source.Id, Source.TypeId, Source.Name, Source.Description)
  WHEN MATCHED THEN UPDATE SET 
    Target.TypeId	= Source.TypeId,
    Target.Name	= Source.Name,
    Target.Description = Source.Description;

SET IDENTITY_INSERT Dictionaries OFF


SET IDENTITY_INSERT Settings ON

  MERGE Settings AS Target
  USING (VALUES 
    ( 1, 'Максимум активных планов', 30, 'Предел активных планов, которые можно назначить'),
    ( 2, 'Максимум упражнений в день', 10, 'Предел упражнений, которые можно назначить на один день'),
    ( 3, 'Максимум поднятий в упражнении', 10, 'Предел поднятий, которые можно назначить в одно упражнение')
  )	AS Source (Id, Name, Value, Description)
    ON Source.[Id] = Target.[Id]
  WHEN NOT MATCHED BY Target THEN INSERT (Id, Name, Value, Description) VALUES (Source.Id, Source.Name, Source.Value, Source.Description)
  WHEN MATCHED THEN UPDATE SET 
    Target.Name = Source.Name,
    Target.Value = Source.Value,
    Target.Description = Source.Description;

SET IDENTITY_INSERT Settings OFF 


TRUNCATE TABLE EmailMessages
SET IDENTITY_INSERT EmailMessages ON

IF NOT EXISTS (SELECT * FROM EmailMessages WHERE [Id] = 1)
  INSERT INTO EmailMessages (Id, Subject, Body) VALUES ( 1, 'Подтверждение регистрации',
  '<h4>Поздравляем вас с регистрацией в спортивном ассистенте</h4>
<p>Прежде чем перейти к планированию тренировок, подтвердите регистрацию перейдя по ссылке {0}</p>');

IF NOT EXISTS (SELECT * FROM EmailMessages WHERE [Id] = 2)
  INSERT INTO EmailMessages (Id, Subject, Body) VALUES ( 2, 'Сброс пароля',
  '<h4>Для вашей учетной записи был запрошен сброс пароля</h4>
<p>По запросу ваш старый пароль был стерт и заменен новым: <strong>{0}</strong></p>');

SET IDENTITY_INSERT EmailMessages OFF 