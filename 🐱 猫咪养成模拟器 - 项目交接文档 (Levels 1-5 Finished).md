# 🐱 猫咪养成模拟器 - 项目交接文档 (Levels 1-5 Finished)



有一些以物件命名的脚本不用管，是场景自带的（比如时钟的脚本就是管时钟走动的）

## 1. 核心逻辑架构 (The "Brain")



这部分控制整个游戏的数值和流程，是项目的骨架。



### 🔹 **LevelSystem (游戏物体)**



- **挂载脚本：** `LevelManager.cs`
- **作用：** 游戏的**总指挥**。
  - 管理当前是第几关 (`currentLevelIndex`)。
  - 管理右上角的任务文本 (`Task Text`)。
  - **通关逻辑：**
    - **数值关卡（如第1关）：** 在 `Update` 中实时监测数值（如 `Hunger > 100`），自动触发升级。
    - **解谜关卡（如第2-5关）：** 目标设为不可能达成的数值（如 99999），等待子系统调用 `ForceLevelUp()` 手动强制升级。
- **配置方法：** 在 Inspector 的 `All Levels` 列表中配置每一关的描述和目标。



### 🔹 **PetState (游戏物体)**



- **挂载脚本：** `PetState.cs`
- **作用：** 猫咪的**数值管理器**。
  - 存储核心属性：`Health`, `Hunger`, `Anxiety`, `CatCoin`。
  - **CheckGameStatus()：** 每次数值变动后必须调用此方法，用于防止数值为负，并检测游戏是否结束（Game Over）。
  - **UpdateUI()：** 刷新左上角的数值显示。

------



## 2. 关卡逻辑详解 (Level Specifics)



为了保持代码整洁，我们采用了**“分包工头”**模式。特殊的关卡都有自己独立的管理物体。



### ✅ 第 1 关：新手喂食



- **逻辑：** 纯数值判定，由 `LevelManager` 自动接管。
- **交互：** 点击 UI 上的按钮（Kibble/Canned Food）调用 `PetState` 的加分函数。



### ✅ 第 2 关：猫砂盆谜题 (放置类)



- **管理物体：** `level2_litterboxlogic` (挂载 `LitterBoxPuzzle.cs`)
- **逻辑：**
  - 场景出现 3 个光圈 (`SpotA/B/C`)。
  - 点击光圈 -> 放置猫砂盆。
  - 点击猫砂盆 -> 撤销放置。
  - **通关：** 必须点击**正确的光圈**（代码中设定为 Index 0 的 SpotA），触发 `ForceLevelUp()`。
- **注意：** 光圈和猫砂盆身上挂有 `ClickTrigger.cs` 用于接收点击。



### ✅ 第 3 关：建立羁绊 (交互类)



- **管理物体：** `Level3_Logic` (挂载 `Level3Manager.cs`)
- **逻辑：**
  - 点击 **猫** -> 弹出头顶悬浮按钮 "Force Hug" -> 点击后焦虑上升。
  - 点击 **逗猫棒** -> 弹出旁边悬浮按钮 "Use Teaser" -> 点击后焦虑下降 -> **通关**。
- **技术点：** 使用了 **World Space Canvas**（世界空间 UI），按钮是漂浮在 3D 物体旁边的，而不是贴在屏幕上的。



### ✅ 第 4 关：找跳蚤 (找茬类)



- **管理物体：** `Level4_Logic` (挂载 `Level4Manager.cs`)
- **逻辑：**
  - 场景中生成 3 个黑色小球 (`Flea`)。
  - 点击跳蚤 -> 跳蚤消失 -> 计数器 +1。
  - **通关：** 计数器满 3 个，触发 `ForceLevelUp()`。
- **注意：** 跳蚤身上挂有 `FleaClick.cs` 脚本。



### ✅ 第 5 关：驱虫决策 (UI 选择类)



- **管理物体：** `Level5_Logic` (挂载 `Level5Manager.cs`)
- **逻辑：**
  - 进入关卡时，自动弹出一个大的 UI 面板 (`Treatment_Panel`)。
  - 面板上有 3 个选项按钮。
  - 点击正确选项 -> 修改数值 -> 关闭面板 -> **通关**。
- **UI排版：** 面板使用了 `Vertical Layout Group` 组件来实现自动竖向排版。

------



## 3. UI 系统与注意事项 (UI System)



项目中有两种完全不同的 UI，千万别混淆：



### 🖥️ 1. 屏幕 UI (Screen Space - Overlay)



- **物体：** `Main_HUD_Canvas`
- **内容：** 左上角数值、右上角任务栏、第1关喂食按钮、第5关决策面板。
- **特点：** 永远覆盖在画面最上层，跟随屏幕缩放。
- **配置：** `Canvas Scaler` 设置为 `Scale With Screen Size` (1920x1080)。



### 🌍 2. 世界 UI (World Space)



- **物体：** `Cat_UI_Canvas`, `Teaser_UI_Canvas`
- **内容：** 第3关的 "Force Hug" 和 "Use Teaser" 悬浮按钮。
- **特点：** 像 3D 物体一样漂浮在场景里，有近大远小的效果。
- **⚠️ 巨坑预警：**
  - **Scale 必须极小：** 通常设置为 `0.005, 0.005, 0.005`。如果设为 1，UI 会比房子还大，挡住所有射线！
  - **父级缩放：** 最好不要把 Canvas 设为变形物体（如被拉长的逗猫棒）的子物体，否则 UI 也会变形。建议建立一个 Scale 为 1,1,1 的父物体容器。