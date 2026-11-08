Лабораторная 4
Создание диаграмм последовательностей
С помощью диаграмм отобразить взаимодействие объектов в
системе.
Диаграмма создаётся для каждой системной функции всех
диаграмм последовательностей, из предыдущей лабораторной.

# Диаграмма последовательностей системных функций.

Окна интерфейса опущены. Обращения пользователя к контроллеру и ответы
показаны условно через границу системы; обработка нажатий и показ сообщений
остаются в интерфейсе и здесь не детализируются.

## Ввести данные

```mermaid
sequenceDiagram
    autonumber
    actor ИностранныйГражданин
    participant AppController
    participant AppService
    participant FormRepository
    ИностранныйГражданин->>AppController: SaveForm(formData)
    AppController->>AppService: Validate(formData)

    alt ФИО пустое, тип договора неверный или дата патента некорректна
        AppService-->>AppController: FormError
        AppController-->>ИностранныйГражданин: Ошибка проверки данных
    else Данные корректны
        AppService-->>AppController: Проверка завершена
        AppController->>FormRepository: Save(formData)
        FormRepository->>FormRepository: form = formData
        FormRepository-->>AppController: Сохранение завершено
        AppController-->>ИностранныйГражданин: Анкета сохранена
    end
```

## Открыть дорожную карту

```mermaid
sequenceDiagram
    autonumber
    actor ИностранныйГражданин
    participant AppController
    participant FormRepository
    participant AppService
    participant Roadmap
    ИностранныйГражданин->>AppController: OpenRoadmap()
    AppController->>FormRepository: Get()
    FormRepository-->>AppController: formData или null

    alt Анкета отсутствует
        AppController-->>ИностранныйГражданин: Ошибка: сначала заполните и сохраните анкету
    else Анкета сохранена в памяти
        AppController->>AppService: CreateRoadmap(formData)
        AppService->>Roadmap: new Roadmap(), Form = formData

        alt Патента нет
            AppService->>Roadmap: Steps.Add(new RoadmapStep)
            Note over AppService,Roadmap: «Получить патент». Срок не задан.
        else Патент есть
            AppService->>Roadmap: Steps.Add(new RoadmapStep)
            Note over AppService,Roadmap: Проверка данных патента: дата выдачи плюс 7 дней.
            alt Договор отсутствует
                AppService->>Roadmap: Steps.Add(new RoadmapStep)
                Note over AppService,Roadmap: Заключить трудовой договор или ГПХ:<br/>дата выдачи патента плюс 2 месяца.
            else Договор заключён
                AppService->>Roadmap: Steps.Add(new RoadmapStep)
                Note over AppService,Roadmap: Подготовить сведения о трудовом договоре или ГПХ:<br/>дата выдачи патента плюс 2 месяца.
            end
            AppService->>Roadmap: Steps.Add(new RoadmapStep)
            Note over AppService,Roadmap: Проверка следующего платежа:<br/>дата выдачи патента плюс 1 месяц.
            AppService->>Roadmap: Упорядочить Steps по Deadline
        end

        AppService-->>AppController: roadmap
        AppController-->>ИностранныйГражданин: roadmap
    end
```
