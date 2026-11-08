# Диаграмма последовательностей системных функций.

## Ввести данные

```mermaid
%%{init: {'sequence': {'mirrorActors': false}}}%%
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
    end

    AppService-->>AppController: Проверка завершена
    AppController->>FormRepository: Save(formData)
    FormRepository->>FormRepository: form = formData
    FormRepository-->>AppController: Сохранение завершено
    AppController-->>ИностранныйГражданин: Анкета сохранена
```

## Открыть дорожную карту

```mermaid
%%{init: {'sequence': {'mirrorActors': false}}}%%
sequenceDiagram
    autonumber
    actor ИностранныйГражданин
    participant AppController
    participant FormRepository
    participant AppService
    participant Roadmap
    participant Steps
    ИностранныйГражданин->>AppController: OpenRoadmap()
    AppController->>FormRepository: Get()
    FormRepository-->>AppController: formData или null

    alt Анкета отсутствует
        AppController-->>ИностранныйГражданин: Ошибка: сначала заполните и сохраните анкету
    end

    AppController->>AppService: CreateRoadmap(formData)
    AppService->>Roadmap: new Roadmap(), Form = formData
    Note over AppService,Roadmap: Нужные шаги в зависимости от Form
    AppService->>Steps: new RoadmapStep()
    Steps-->>AppService: step
    AppService->>Roadmap: Steps.Add(step)
    AppService-->>AppController: roadmap
    AppController-->>ИностранныйГражданин: roadmap
```
