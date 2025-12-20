# SnowmanBreaker 프로젝트 개요

이 프로젝트는 **SnowmanBreaker**라는 유니티 게임 프로젝트로, JDC 게임잼을 위해 개발된 것으로 보입니다. 플레이어 캐릭터가 팔과 다리 등 다양한 신체 부위를 사용하여 눈사람을 공격하는 게임으로, 눈사람에게 피해를 입히고 파괴하며, 난이도가 증가하면서 다시 생성되는 메커니즘을 포함하고 있습니다.

## 주요 기술 및 종속성

*   **엔진:** Unity.
*   **렌더 파이프라인:** Universal Render Pipeline (URP) (`com.unity.render-pipelines.universal`).
*   **입력 시스템:** Unity Input System (`com.unity.inputsystem`).
*   **기타 패키지:** TextMeshPro, Visual Scripting, AI Navigation.

## 시작하기

1.  **Unity에서 열기:**
    *   Unity Hub를 실행합니다.
    *   "Add"를 클릭하고 `SnowmanBreaker` 폴더(`Assets` 및 `Packages`가 포함된 상위 디렉토리)를 선택합니다.
    *   프로젝트를 엽니다. Unity 에디터가 에셋을 가져옵니다.

2.  **게임 실행:**
    *   메인 씬을 엽니다: `Assets/Scenes/SampleScene.unity`.
    *   Unity 에디터 툴바의 **Play** 버튼을 누릅니다.

## 핵심 게임 플레이 메커니즘

게임은 플레이어가 눈사람을 타격하는 것을 중심으로 진행됩니다.

*   **플레이어 조작 (`PlayerController.cs`):**
    *   **오른쪽 화살표:** 오른팔 공격.
    *   **왼쪽 화살표:** 왼팔 공격.
    *   **위쪽 화살표:** 왼쪽 다리 공격.
    *   공격은 빠른 "잽" 동작(위치를 앞뒤로 부드럽게 이동)을 포함합니다.

*   **눈사람 로직 (`Snowman.cs`):**
    *   각 눈사람은 체력(`hp`)을 가집니다.
    *   타격(`OnHit`)을 받으면 `hp`가 감소하고, 눈사람의 크기가 시각적으로 줄어듭니다(`localScale` y축 감소).
    *   어떤 신체 부위로 눈사람을 타격했는지(왼손, 오른손, 왼쪽 발)에 따라 다른 타격 파티클이 재생됩니다.

*   **게임 관리 (`GameManager.cs`):**
    *   `Snowman` 객체 리스트를 관리합니다.
    *   가장 앞에 있는 눈사람이 파괴되었는지 확인합니다 (hp < 0.2f).
    *   **순환 로직:** 눈사람이 파괴되면 남은 눈사람들이 앞으로 미끄러지듯 이동합니다 (`SmoothMoveRoutine`). 파괴된 눈사람은 대기열의 맨 뒤로 이동하며, 체력이 완전히 회복되고 레벨이 높아진 상태로 "리스폰"되어 무한 루프를 생성합니다.

## 프로젝트 구조

*   **`Assets/Scripts/`**: 핵심 C# 소스 코드가 포함되어 있습니다.
    *   `GameManager.cs`: 게임 흐름과 눈사람 대기열을 제어합니다.
    *   `PlayerController.cs`: 사용자 입력 및 캐릭터 애니메이션을 처리합니다.
    *   `Snowman.cs`: 개별 눈사람의 상태, 체력 및 충돌 반응을 처리합니다.
    *   `SnowmanCollisionChecker.cs`: 충돌 감지 로직을 처리하고 이벤트를 `Snowman`에 전달하는 것으로 보입니다.
    *   `StatusOverlay.cs`: 게임 상태에 대한 UI 오버레이를 관리합니다.
*   **`Assets/Scenes/`**: 게임 씬(예: `SampleScene.unity`)이 포함되어 있습니다.
*   **`Assets/Materials/`**: 게임 오브젝트(눈, 스카이박스, 플레이어 신체 부위)를 위한 머티리얼입니다.
*   **`Assets/Prefab/`**: 미리 구성된 게임 오브젝트(예: `Snowman.prefab`)입니다.

## 개발 규칙

*   **코드 스타일:** 표준 C# 명명 규칙을 따릅니다. `PlayerController`에서 개인 멤버 변수에는 `m_` 접두사가 사용됩니다.
*   **스크립팅:** `MonoBehaviour` 기반 아키텍처입니다. 애니메이션 및 시간 지정 시퀀스(예: `SmoothMoveRoutine`, `MovePlayerBodyRoutine`)에 코루틴이 많이 사용됩니다.
*   **물리:** 이 게임은 플레이어와 눈사람 간의 상호 작용을 위해 Collider(트리거)와 `OnHit` 이벤트에 의존합니다.

## 참고 사항

*   조작이 작동하지 않는 경우 프로젝트 설정(Project Settings)에서 Input System 패키지가 올바르게 구성되어 있는지 확인하세요.
*   이 프로젝트는 URP를 사용하므로 머티리얼과 쉐이더가 Universal Render Pipeline과 호환되어야 합니다.