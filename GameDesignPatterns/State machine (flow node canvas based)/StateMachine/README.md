
### State Machine (Alpha)

Реализация паттерна State Machine, основаная на Flow Canvas https://assetstore.unity.com/packages/tools/visual-scripting/flowcanvas-33903

State Node - Класс описывающий ноду состояния, наследуя её вы можете реализовавывать собственные ноды.
Содержит в себе Start Events и Disable Events - стандартные unity events, которые могут выполняться при входе в ноду, либо при выходе.
Также вы можете описать собственные Events, как в классе OnTriggerScenarioEvents.

Scenario Manager - синглтон, управляющий Графом
