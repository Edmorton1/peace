# Диаграмма последовательностей системных функций.

## Ввести данные

```mermaid
%%{init: {'sequence': {'mirrorActors': false}}}%%
sequenceDiagram
    actor ИностранныйГражданин
    participant AppController
    participant AppService
    participant FormRepository
    ИностранныйГражданин->>AppController: SaveForm(formData)
    AppController->>AppService: Validate(formData)

    alt ФИО пустое, тип договора неверный или дата патента некорректна
        AppService-->>AppController: ArgumentException
        AppController-->>ИностранныйГражданин: Ошибка проверки данных, завершить сохранение
    end

    AppService-->>AppController: Проверка завершена
    AppController->>AppController: formData.FullName =<br/>formData.FullName.Trim()
    opt Патента нет
        AppController->>AppController: formData.Patent.IssueDate = null
    end
    AppController->>FormRepository: Save(formData)
    FormRepository->>FormRepository: form = formData
    FormRepository-->>AppController: Сохранение завершено
    AppController-->>ИностранныйГражданин: Анкета сохранена
```

## Открыть дорожную карту

```mermaid
%%{init: {'sequence': {'mirrorActors': false}}}%%
sequenceDiagram
    actor ИностранныйГражданин
    participant AppController
    participant FormRepository
    participant AppService
    participant Roadmap
    participant RoadmapSteps
    participant RoadmapStep
    ИностранныйГражданин->>AppController: OpenRoadmap()
    AppController->>FormRepository: Get()
    FormRepository-->>AppController: formData или null

    alt Анкета отсутствует
        AppController-->>ИностранныйГражданин: InvalidOperationException: сначала сохраните анкету
    end

    AppController->>AppService: CreateRoadmap(formData)
    AppService->>Roadmap: new Roadmap(), Form = formData
    AppService->>RoadmapSteps: CreateSteps(formData)
    Note over RoadmapSteps: Правила хранятся в Steps
    loop Для каждого подходящего правила
        RoadmapSteps->>RoadmapStep: new RoadmapStep()<br/>Задать Description и Deadline
        RoadmapStep-->>RoadmapSteps: step
        RoadmapSteps->>RoadmapSteps: steps.Add(step)
    end
    RoadmapSteps->>RoadmapSteps: Упорядочить steps по Deadline
    RoadmapSteps-->>AppService: steps
    AppService->>Roadmap: Steps = steps
    AppService-->>AppController: roadmap
    AppController-->>ИностранныйГражданин: roadmap
```
