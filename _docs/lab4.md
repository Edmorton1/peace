Лабораторная 4
Создание диаграмм последовательностей
С помощью диаграмм отобразить взаимодействие объектов в
системе.
Диаграмма создаётся для каждой системной функции всех
диаграмм последовательностей, из предыдущей лабораторной.

# Диаграмма последовательностей системных функций.

## Ввести данные

```mermaid
sequenceDiagram
    autonumber
    actor User as Иностранный гражданин
    participant View as formView: FormView
    participant Controller as controller: AppController
    participant Service as AppService
    participant Repository as repository: FormRepository

    Note over User,View: Анкета открыта. Пользователь заполняет поля.
    User->>View: Нажать «Сохранить»
    View->>View: Save()

    alt Не выбран статус патента или договора
        View-->>User: Сообщение об обязательных полях
        Note over User,View: Форма остаётся открытой для исправления.
    else Статусы выбраны
        View->>View: Создать Form, Patent и Contract из полей
        View->>Controller: SaveForm(formData)
        Controller->>Service: Validate(formData)
        Note over Service: Статический метод.<br/>Проверяет ФИО и тип договора.<br/>Если патент есть, проверяет наличие даты<br/>и диапазон от сегодня минус 2 месяца до сегодня включительно.

        alt ФИО пустое, тип договора неверный или дата патента некорректна
            Service-->>Controller: ArgumentException
            Controller-->>View: Передать исключение
            View-->>User: Показать сообщение об ошибке
            Note over User,Repository: Анкета не заменяется. Форма остаётся открытой.
        else Данные корректны
            Service-->>Controller: Проверка завершена
            Controller->>Controller: Удалить пробелы по краям ФИО
            opt Патента нет
                Controller->>Controller: formData.Patent.IssueDate = null
            end
            Controller->>Repository: Save(formData)
            Repository->>Repository: form = formData
            Note over Repository: Хранится ссылка на одну анкету в памяти.<br/>Файлы и база данных не используются.
            Repository-->>Controller: Сохранение завершено
            Controller-->>View: Сохранение завершено
            View-->>User: «Анкета сохранена»
            View->>View: DialogResult = OK, закрыть окно
        end
    end
```

## Открыть дорожную карту

```mermaid
sequenceDiagram
    autonumber
    actor User as Иностранный гражданин
    participant Main as mainForm: Form1
    participant Controller as controller: AppController
    participant Repository as repository: FormRepository
    participant Service as AppService
    participant Map as roadmap: Roadmap
    participant View as roadmapForm: RoadmapForm

    User->>Main: Нажать «Открыть дорожную карту»
    Main->>Controller: OpenRoadmap()
    Controller->>Repository: Get()
    Repository-->>Controller: formData или null

    alt Анкета отсутствует
        Controller-->>Main: InvalidOperationException
        Main-->>User: «Сначала заполните и сохраните анкету»
    else Анкета сохранена в памяти
        Controller->>Service: CreateRoadmap(formData)
        Note over Service: Статический метод. Использует сохранённую анкету.<br/>Повторная проверка даты здесь не выполняется.
        Service->>Map: new Roadmap(), Form = formData

        alt Патента нет
            Service->>Map: Steps.Add(new RoadmapStep)
            Note over Service,Map: «Получить патент». Срок не задан.
        else Патент есть
            Service->>Map: Steps.Add(new RoadmapStep)
            Note over Service,Map: Проверка данных патента: дата выдачи плюс 7 дней.
            alt Договор отсутствует
                Service->>Map: Steps.Add(new RoadmapStep)
                Note over Service,Map: Заключить трудовой договор или ГПХ:<br/>дата выдачи патента плюс 2 месяца.
            else Договор заключён
                Service->>Map: Steps.Add(new RoadmapStep)
                Note over Service,Map: Подготовить сведения о трудовом договоре или ГПХ:<br/>дата выдачи патента плюс 2 месяца.
            end
            Service->>Map: Steps.Add(new RoadmapStep)
            Note over Service,Map: Проверка следующего платежа:<br/>дата выдачи патента плюс 1 месяц.
            Service->>Map: Упорядочить Steps по Deadline
        end

        Note over Service,Map: Описания действий и сроки — заглушки из кода.
        Service-->>Controller: roadmap
        Controller-->>Main: roadmap
        Main->>View: new RoadmapForm(roadmap)
        Note over View: Заполнить таблицу описаниями и сроками шагов.
        Main->>View: ShowDialog(mainForm)
        View-->>User: Показать дорожную карту
    end
```
