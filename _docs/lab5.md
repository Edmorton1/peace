Лабораторная 5
Создание диаграмм классов проектирования
• Диаграмма классов не должна быть привязана к конкретному
языку реализации.
• Диаграмма классов должна быть согласована с диаграммами
коммуникаций.

# Создание диаграмм классов проектирования

```mermaid
classDiagram
    direction TB

    class Form1 {
        +Form1(controller: AppController)
        -RunAction(action: Action)
    }

    class FormView {
        -controller: AppController
        +FormView(controller: AppController, formData: Form)
        -Save()
    }

    class RoadmapForm {
        +RoadmapForm(roadmap: Roadmap)
    }

    class AppController {
        -repository: FormRepository
        +AppController(repository: FormRepository, service: AppService)
        +GetForm() Form
        +SaveForm(formData: Form)
        +OpenRoadmap() Roadmap
    }

    class AppService {
        +Validate(formData: Form)$
        +CreateRoadmap(formData: Form) Roadmap$
    }

    class FormRepository {
        -form: Form
        +Get() Form
        +Save(formData: Form)
    }

    class Form {
        +FullName: String
        +Patent: Patent
        +Contract: Contract
    }

    class Patent {
        +HasPatent: Boolean
        +IssueDate: Date
    }

    class Contract {
        +Type: ContractType
        +HasContract: Boolean
    }

    class ContractType {
        <<enumeration>>
        None
        Employment
        CivilLaw
    }

    class Roadmap {
        +Form: Form
        +Steps: RoadmapStep Collection
    }

    class RoadmapStep {
        +Description: String
        +Deadline: Date
    }

    Form1 --> AppController : вызывает сценарии
    Form1 ..> FormView : создаёт и показывает
    Form1 ..> RoadmapForm : создаёт и показывает
    FormView --> AppController : сохраняет анкету
    FormView ..> Form : создаёт
    FormView ..> Patent : создаёт
    FormView ..> Contract : создаёт
    RoadmapForm ..> Roadmap : отображает

    AppController --> FormRepository : получает и сохраняет анкету
    AppController ..> AppService : вызывает статические методы
    AppService ..> Form : проверяет и использует
    AppService ..> Roadmap : создаёт
    AppService ..> RoadmapStep : создаёт

    FormRepository "1" --> "0..1" Form : хранит в памяти
    Form "1" --> "1" Patent : Patent
    Form "1" --> "1" Contract : Contract
    Contract --> ContractType : Type
    Roadmap "0..*" --> "1" Form : Form
    Roadmap "1" *-- "0..*" RoadmapStep : Steps
```

Имена классов и операций соответствуют коду. Типы `String`, `Boolean`, `Date`
и `Collection` обозначены независимо от языка реализации. Элементы интерфейса,
вспомогательная раскладка окон и стандартные классы WinForms опущены.

- `Form` — модель анкеты. `FormView`, `Form1` и `RoadmapForm` — окна интерфейса.
- `FormRepository` хранит одну анкету в памяти. До сохранения `Get()` и `GetForm()`
  могут вернуть отсутствие анкеты; после закрытия приложения данные теряются.
- `IssueDate` и `Deadline` могут отсутствовать. `HasContract` вычисляется как
  `Type != None` и не изменяется отдельно.
- Методы `AppService` статические (обозначены `$` в исходнике Mermaid).
  Параметр `service` есть в конструкторе `AppController`, но в текущем коде
  не используется: методы вызываются через имя класса `AppService`.
- Связи `Form` с `Patent` и `Contract` — ссылки на объекты. `Roadmap` ссылается
  на сохранённую анкету и содержит коллекцию шагов. В модели коллекция может
  быть пустой; `CreateRoadmap()` создаёт один шаг без патента или три при его наличии.
